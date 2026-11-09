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

public sealed class ProductsPactProviderTests
{
    private const string StatePath = "/pact-provider-state";

    [Theory]
    [InlineData("invoice")]
    [InlineData("web")]
    public async Task VerifyPinnedPactAgainstProductsApi(string consumer)
    {
        Environment.SetEnvironmentVariable("PACT_DO_NOT_TRACK", "true");
        var directory = Path.Combine(AppContext.BaseDirectory, "pact");
        var pactPath = Path.Combine(directory, $"{consumer}-products.json");
        var pactBytes = await File.ReadAllBytesAsync(pactPath);
        using var source = JsonDocument.Parse(await File.ReadAllBytesAsync(Path.Combine(directory, $"{consumer}-source.json")));
        using var pact = JsonDocument.Parse(pactBytes);
        var hash = Convert.ToHexStringLower(SHA256.HashData(pactBytes));
        var interactions = pact.RootElement.GetProperty("interactions").GetArrayLength();
        Assert.Equal(source.RootElement.GetProperty("contractSha256").GetString(), hash);
        Assert.Equal(source.RootElement.GetProperty("interactionCount").GetInt32(), interactions);
        Assert.True(interactions > 0);
        Assert.Equal(consumer, pact.RootElement.GetProperty("consumer").GetProperty("name").GetString());
        Assert.Equal("products", pact.RootElement.GetProperty("provider").GetProperty("name").GetString());
        Assert.Equal("4.0", pact.RootElement.GetProperty("metadata").GetProperty("pactSpecification").GetProperty("version").GetString());

        var (startedAt, finishedAt) = await VerifyAsync(pactPath);

        var reportDirectory = Environment.GetEnvironmentVariable("PRODUCTS_PACT_REPORT_DIR");
        if (!string.IsNullOrWhiteSpace(reportDirectory))
        {
            Directory.CreateDirectory(reportDirectory);
            var report = Path.Combine(reportDirectory, $"{consumer}-pact-verification.json");
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
            var (productId, price, photo, displayType, screenSize, camera) = request.GetProperty("state").GetString() switch
            {
                "one priced phone exists and another phone is missing" => (
                    parameters.GetProperty("productId").GetGuid(), parameters, "fixture.jpg", "OLED", 6.1m, "48 MP"),
                "a priced catalog phone exists and another ID is absent" => (
                    parameters.GetProperty("id").GetGuid(), parameters.GetProperty("price"),
                    parameters.GetProperty("mainPhoto").GetString()!, parameters.GetProperty("displayType").GetString()!,
                    parameters.GetProperty("screenSizeInches").GetDecimal(), parameters.GetProperty("camera").GetString()!),
                _ => throw new InvalidOperationException("Unsupported Products Pact provider state.")
            };

            var db = context.RequestServices.GetRequiredService<ProductsContext>();
            var missingId = parameters.GetProperty("missingId").GetGuid();
            if (productId == missingId || await db.MobilePhones.AnyAsync(phone => phone.Id == missingId, context.RequestAborted))
                throw new InvalidOperationException("The Pact's missing phone unexpectedly exists.");

            if (!await db.MobilePhones.AnyAsync(phone => phone.Id == productId, context.RequestAborted))
            {
                var phone = new MobilePhone(
                    new CommonDescription(parameters.GetProperty("name").GetString()!, parameters.GetProperty("brand").GetString()!,
                        "Pact fixture", photo, []),
                    new ElectronicDetails("CPU", "GPU", "8 GB", "128 GB", displayType, 120, screenSize, 71, 146, "Li-Ion", 4000),
                    new Connectivity(true, true, true, true),
                    new SatelliteNavigationSystem(true, true, true, false, false),
                    new Sensors(true, true, true, true, false, false, true),
                    camera, true, false,
                    new Money(price.GetProperty("amount").GetDecimal(), price.GetProperty("currency").GetString()!),
                    "Pact fixture", "Pact fixture");
                db.Entry(phone).Property(entity => entity.Id).CurrentValue = productId;
                db.MobilePhones.Add(phone);
                await db.SaveChangesAsync(context.RequestAborted);
            }

            await context.Response.WriteAsJsonAsync(new { }, context.RequestAborted);
        }
    }
}
