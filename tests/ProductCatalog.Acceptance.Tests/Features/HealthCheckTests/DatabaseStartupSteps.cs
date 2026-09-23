using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Net;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests;

[Binding]
public sealed class DatabaseStartupSteps(ScenarioApiContext apiContext)
{
    private string? _connectionString;
    private HttpStatusCode _defaultLiveStatus;
    private StartupSnapshot? _firstStartup;
    private StartupSnapshot? _secondStartup;

    [When("the scenario SQL database is removed before startup")]
    public async Task WhenTheScenarioSqlDatabaseIsRemovedBeforeStartup()
    {
        using var scope = apiContext.Factory!.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        _connectionString = context.Database.GetConnectionString();
        await context.Database.EnsureDeletedAsync();
    }

    [When("the API starts with default migration settings")]
    public async Task WhenTheApiStartsWithDefaultMigrationSettings()
    {
        using var factory = new ApplicationFactory(_connectionString!, applyMigrations: null);
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/health/live");
        _defaultLiveStatus = response.StatusCode;
    }

    [Then("startup leaves the database absent and liveness healthy")]
    public async Task ThenStartupLeavesTheDatabaseAbsentAndLivenessHealthy()
    {
        _defaultLiveStatus.ShouldBe(HttpStatusCode.OK);

        var builder = new SqlConnectionStringBuilder(_connectionString);
        var database = builder.InitialCatalog;
        builder.InitialCatalog = "master";
        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT DB_ID(@DatabaseName)", connection);
        command.Parameters.AddWithValue("@DatabaseName", database);
        (await command.ExecuteScalarAsync()).ShouldBe(DBNull.Value);
    }

    [When("the API starts with migrations enabled")]
    public async Task WhenTheApiStartsWithMigrationsEnabled()
        => _firstStartup = await StartAndCaptureAsync();

    [Then("the database contains the initial seed and no pending migrations")]
    public void ThenTheDatabaseContainsTheInitialSeedAndNoPendingMigrations()
    {
        _firstStartup.ShouldNotBeNull();
        _firstStartup.Current.Length.ShouldBe(15);
        _firstStartup.History.Length.ShouldBe(15);
        _firstStartup.Applied.ShouldNotBeEmpty();
        _firstStartup.Pending.ShouldBeEmpty();
    }

    [When("the API starts again with migrations enabled")]
    public async Task WhenTheApiStartsAgainWithMigrationsEnabled()
        => _secondStartup = await StartAndCaptureAsync();

    [Then("the seeded phones, history and applied migrations are unchanged")]
    public void ThenTheSeededPhonesHistoryAndAppliedMigrationsAreUnchanged()
    {
        _firstStartup.ShouldNotBeNull();
        _secondStartup.ShouldNotBeNull();
        _secondStartup.Pending.ShouldBeEmpty();
        _secondStartup.Current.SequenceEqual(_firstStartup.Current).ShouldBeTrue();
        _secondStartup.History.SequenceEqual(_firstStartup.History).ShouldBeTrue();
        _secondStartup.Applied.SequenceEqual(_firstStartup.Applied).ShouldBeTrue();
    }

    private async Task<StartupSnapshot> StartAndCaptureAsync()
    {
        using var factory = new ApplicationFactory(_connectionString!, applyMigrations: true);
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/health/ready");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var current = (await context.MobilePhones.AsNoTracking().ToListAsync())
            .OrderBy(phone => phone.Id)
            .Select(phone => $"{phone.Id}|{phone.CommonDescription.Name}|{phone.ChangedAt:O}")
            .ToArray();
        var history = (await context.MobilePhonesHistories.AsNoTracking().ToListAsync())
            .OrderBy(entry => entry.Id)
            .Select(entry => $"{entry.Id}|{entry.MobilePhoneId}|{entry.ChangedAt:O}|{entry.Operation}")
            .ToArray();
        var applied = (await context.Database.GetAppliedMigrationsAsync()).ToArray();
        var pending = (await context.Database.GetPendingMigrationsAsync()).ToArray();
        return new StartupSnapshot(current, history, applied, pending);
    }

    private sealed record StartupSnapshot(string[] Current, string[] History, string[] Applied, string[] Pending);
}
