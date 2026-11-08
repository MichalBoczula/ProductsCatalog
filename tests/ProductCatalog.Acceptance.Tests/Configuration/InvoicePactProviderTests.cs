using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PactNet.Verifier;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class InvoicePactProviderTests
{
    private const string StatePath = "/pact-provider-state";

    [Fact]
    public async Task VerifyPinnedInvoicePactAgainstProductsApi()
    {
        Environment.SetEnvironmentVariable("PACT_DO_NOT_TRACK", "true");
        var directory = Path.Combine(AppContext.BaseDirectory, "pact");
        var pactPath = Path.Combine(directory, "invoice-products.json");
        var pactBytes = await File.ReadAllBytesAsync(pactPath);
        using var source = JsonDocument.Parse(await File.ReadAllBytesAsync(Path.Combine(directory, "invoice-source.json")));
        using var pact = JsonDocument.Parse(pactBytes);
        var hash = Convert.ToHexStringLower(SHA256.HashData(pactBytes));
        var interactions = pact.RootElement.GetProperty("interactions").GetArrayLength();
        Assert.Equal(source.RootElement.GetProperty("contractSha256").GetString(), hash);
        Assert.Equal(source.RootElement.GetProperty("interactionCount").GetInt32(), interactions);
        Assert.True(interactions > 0);
        Assert.Equal("invoice", pact.RootElement.GetProperty("consumer").GetProperty("name").GetString());
        Assert.Equal("products", pact.RootElement.GetProperty("provider").GetProperty("name").GetString());

        var (startedAt, finishedAt) = await VerifyAsync(pactPath);

        var report = Environment.GetEnvironmentVariable("PRODUCTS_PACT_REPORT");
        if (!string.IsNullOrWhiteSpace(report))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(report))!);
            await File.WriteAllTextAsync(report, JsonSerializer.Serialize(new
            {
                consumerCommitSha = source.RootElement.GetProperty("commitSha").GetString(),
                providerCommitSha = Environment.GetEnvironmentVariable("GITHUB_SHA"),
                contractSha256 = hash,
                verifierName = "PactNet",
                verifierVersion = "5.0.1",
                status = "Passed",
                verifiedInteractions = interactions,
                failedInteractions = 0,
                summary = "Products HTTP provider Pact verification passed.",
                issues = Array.Empty<string>(),
                startedAt,
                finishedAt
            }));
        }
    }

    private static async Task<(DateTimeOffset Started, DateTimeOffset Finished)> VerifyAsync(string pactPath)
    {
        var databaseName = $"acceptance_{Guid.NewGuid():N}";
        try
        {
            await using var factory = new PactApplicationFactory(AcceptanceSqlServer.ForDatabase(databaseName));
            factory.UseKestrel(options => options.Listen(IPAddress.Loopback, 0));
            await factory.MigrateAsync();
            using var client = factory.CreateClient();
            var endpoint = client.BaseAddress ?? throw new InvalidOperationException("Expected a Products HTTP listener.");
            using var verifier = new PactVerifier("products");
            var started = DateTimeOffset.UtcNow;
            verifier.WithHttpEndpoint(endpoint)
                .WithFileSource(new FileInfo(pactPath))
                .WithProviderStateUrl(new Uri(endpoint, StatePath))
                .Verify();
            return (started, DateTimeOffset.UtcNow);
        }
        finally
        {
            await AcceptanceSqlServer.DropDatabaseAsync(databaseName);
        }
    }

    private sealed class PactApplicationFactory(string connectionString) : ApplicationFactory(connectionString)
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services => services.AddTransient<IStartupFilter, PactStates>());
        }
    }

    private sealed class PactStates : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            app.Map(StatePath, state => state.Run(SetupAsync));
            next(app);
        };

        private static async Task SetupAsync(HttpContext context)
        {
            var request = await context.Request.ReadFromJsonAsync<JsonElement>(context.RequestAborted);
            var parameters = request.GetProperty("params");
            var db = context.RequestServices.GetRequiredService<ProductsContext>();
            var missingId = parameters.GetProperty("missingId").GetGuid();
            if (await db.MobilePhones.AnyAsync(phone => phone.Id == missingId, context.RequestAborted))
                throw new InvalidOperationException("The Pact's missing phone unexpectedly exists.");

            switch (request.GetProperty("state").GetString())
            {
                case "one priced phone exists and the other requested phone is missing":
                    var phone = new MobilePhone(
                        new CommonDescription(parameters.GetProperty("name").GetString()!, parameters.GetProperty("brand").GetString()!,
                            "Pact fixture", "fixture.jpg", []),
                        new ElectronicDetails("CPU", "GPU", "8 GB", "128 GB", "OLED", 120, 6.1m, 71, 146, "Li-Ion", 4000),
                        new Connectivity(true, true, true, true),
                        new SatelliteNavigationSystem(true, true, true, false, false),
                        new Sensors(true, true, true, true, false, false, true),
                        "48 MP", true, false,
                        new Money(parameters.GetProperty("amount").GetDecimal(), parameters.GetProperty("currency").GetString()!),
                        "Pact fixture", "Pact fixture");
                    db.Entry(phone).Property(entity => entity.Id).CurrentValue = parameters.GetProperty("productId").GetGuid();
                    db.MobilePhones.Add(phone);
                    await db.SaveChangesAsync(context.RequestAborted);
                    break;
                case "the requested phone does not exist":
                    break;
                default:
                    throw new InvalidOperationException("Unsupported Invoice Pact provider state.");
            }

            await context.Response.WriteAsJsonAsync(new { }, context.RequestAborted);
        }
    }
}
