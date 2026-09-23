using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class DatabaseConfigurationStartupTests
{
    [Theory]
    [InlineData("", "ConnectionStrings:ProductCatalogDb must be configured.")]
    [InlineData("Server=localhost;Database=ProductsDb;Password=example-secret;UnknownOption=private-value", "ConnectionStrings:ProductCatalogDb must be a valid SQL Server connection string.")]
    [InlineData("Server=localhost;Password=example-secret", "ConnectionStrings:ProductCatalogDb must specify a SQL Server and database.")]
    [InlineData("Database=ProductsDb;Password=example-secret", "ConnectionStrings:ProductCatalogDb must specify a SQL Server and database.")]
    public void Startup_WithInvalidConnectionString_FailsWithoutExposingTheValue(
        string connectionString,
        string expectedMessage)
    {
        using var factory = CreateFactory(connectionString, "false");

        var error = Record.Exception(() => factory.CreateClient());

        error.ShouldNotBeNull();
        error.ToString().ShouldContain(expectedMessage);
        error.ToString().ShouldNotContain("example-secret");
        error.ToString().ShouldNotContain("private-value");
    }

    [Fact]
    public void Startup_WithInvalidApplyMigrationsValue_FailsBeforeConnecting()
    {
        using var factory = CreateFactory(DisconnectedSql, "invalid-private-value");

        var error = Record.Exception(() => factory.CreateClient());

        error.ShouldNotBeNull();
        error.ToString().ShouldContain("Database:ApplyMigrations must be true or false.");
        error.ToString().ShouldNotContain("invalid-private-value");
    }

    [Fact]
    public async Task Swagger_WithValidConfigurationAndUnavailableSql_ReturnsOpenApi()
    {
        using var factory = CreateFactory(DisconnectedSql, "false");
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/swagger/v1/swagger.json");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        document.RootElement.GetProperty("openapi").GetString().ShouldStartWith("3.");
    }

    private const string DisconnectedSql =
        "Server=127.0.0.1,1;Database=ProductsDb;User Id=sa;Password=NotUsed123!;TrustServerCertificate=True;Connect Timeout=1";

    private static WebApplicationFactory<Program> CreateFactory(string connectionString, string applyMigrations)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = connectionString,
                    ["Database:ApplyMigrations"] = applyMigrations
                })));
}
