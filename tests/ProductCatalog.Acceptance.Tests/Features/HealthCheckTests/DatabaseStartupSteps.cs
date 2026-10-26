using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests;

[Binding]
public sealed class DatabaseStartupSteps(ScenarioApiContext api)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private string _connectionString = null!;
    private StartupSnapshot _disabled = null!;
    private StartupSnapshot _first = null!;
    private StartupSnapshot _second = null!;

    [Given("the scenario SQL database is absent before startup")]
    public async Task GivenAbsentDatabase()
    {
        using (var scope = api.Factory!.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            _connectionString = db.Database.GetConnectionString()!;
            (await db.Database.EnsureDeletedAsync()).ShouldBeTrue();
        }
        var exists = await DatabaseExists();
        AllureJson.AttachObject("Startup database precondition", new { databaseExists = exists }, JsonOptions);
        exists.ShouldBeFalse();
    }

    [When("the API starts with migrations disabled")]
    public async Task WhenDisabled() => _disabled = await StartAndCapture("Disabled", false, "/health/live");

    [Then("disabled startup has this observed state")]
    public void ThenDisabled(Table table)
    {
        var expected = Values(table);
        _disabled.EffectiveApplyMigrations.ShouldBe(bool.Parse(expected["EffectiveApplyMigrations"]));
        _disabled.DatabaseExists.ShouldBe(bool.Parse(expected["DatabaseExists"]));
        AssertProbe(_disabled.Probe, expected, "Live");
    }

    [When("the API starts with default migration settings")]
    public async Task WhenDefaults() => _first = await StartAndCapture("Application defaults", null, "/health/ready");

    [Then("the first migrated startup has this observed state")]
    public void ThenFirstStartup(Table table)
    {
        var expected = Values(table);
        _first.EffectiveApplyMigrations.ShouldBe(bool.Parse(expected["EffectiveApplyMigrations"]));
        _first.DatabaseExists.ShouldBe(bool.Parse(expected["DatabaseExists"]));
        var seed = _first.Seed;
        seed.ShouldNotBeNull();
        seed.Current.GetArrayLength().ShouldBe(int.Parse(expected["CurrentRows"], CultureInfo.InvariantCulture));
        seed.History.GetArrayLength().ShouldBe(int.Parse(expected["HistoryRows"], CultureInfo.InvariantCulture));
        seed.Applied.Length.ShouldBeGreaterThanOrEqualTo(int.Parse(expected["AppliedMigrationsAtLeast"], CultureInfo.InvariantCulture));
        seed.Pending.Length.ShouldBe(int.Parse(expected["PendingMigrations"], CultureInfo.InvariantCulture));
        seed.SeedLinksValid.ShouldBe(bool.Parse(expected["SeedLinksValid"]));
        AssertProbe(_first.Probe, expected, "Ready");
    }

    [When("the API starts again with migrations enabled")]
    public async Task WhenEnabledAgain() => _second = await StartAndCapture("Repeated enabled startup", true, "/health/ready");

    [Then("the repeated startup preserves this state")]
    public void ThenPreserved(Table table)
    {
        var expected = Values(table);
        var first = _first.Seed;
        var second = _second.Seed;
        first.ShouldNotBeNull();
        second.ShouldNotBeNull();
        var currentUnchanged = JsonElement.DeepEquals(first.Current, second.Current);
        var historyUnchanged = JsonElement.DeepEquals(first.History, second.History);
        var appliedUnchanged = first.Applied.SequenceEqual(second.Applied);
        AllureJson.AttachObject("Repeated startup comparison", new { currentUnchanged, historyUnchanged, appliedMigrationsUnchanged = appliedUnchanged }, JsonOptions);
        _second.EffectiveApplyMigrations.ShouldBe(bool.Parse(expected["EffectiveApplyMigrations"]));
        currentUnchanged.ShouldBe(bool.Parse(expected["CurrentUnchanged"]));
        historyUnchanged.ShouldBe(bool.Parse(expected["HistoryUnchanged"]));
        appliedUnchanged.ShouldBe(bool.Parse(expected["AppliedMigrationsUnchanged"]));
        second.Pending.Length.ShouldBe(int.Parse(expected["PendingMigrations"], CultureInfo.InvariantCulture));
        AssertProbe(_second.Probe, expected, "Ready");
    }

    private async Task<StartupSnapshot> StartAndCapture(string phase, bool? applyMigrations, string path)
    {
        using var factory = new ApplicationFactory(_connectionString, applyMigrations: applyMigrations);
        using var client = api.CreateClient(factory);
        using var response = await client.GetAsync(path);
        var effective = factory.Services.GetRequiredService<IConfiguration>().GetValue<bool>("Database:ApplyMigrations");
        var probe = new ProbeObservation((int)response.StatusCode,
            response.Content.Headers.ContentType?.MediaType, await response.Content.ReadAsStringAsync());
        var exists = await DatabaseExists();
        SeedSnapshot? seed = null;
        if (exists)
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            var phones = await db.MobilePhones.AsNoTracking().OrderBy(phone => phone.Id).ToListAsync();
            var entries = await db.MobilePhonesHistories.AsNoTracking().OrderBy(entry => entry.Id).ToListAsync();
            var current = JsonSerializer.SerializeToElement(phones, JsonOptions);
            var history = JsonSerializer.SerializeToElement(entries, JsonOptions);
            var applied = (await db.Database.GetAppliedMigrationsAsync()).ToArray();
            var pending = (await db.Database.GetPendingMigrationsAsync()).ToArray();
            var ids = phones.Select(phone => phone.Id).ToHashSet();
            var linksValid = entries.Count == phones.Count && entries.Select(entry => entry.MobilePhoneId).Distinct().Count() == ids.Count
                && entries.All(entry => ids.Contains(entry.MobilePhoneId) && entry.Operation == Operation.Inserted);
            seed = new SeedSnapshot(current, history, applied, pending, linksValid);
        }
        var snapshot = new StartupSnapshot(phase, applyMigrations, effective, exists, probe, seed);
        AllureJson.AttachObject("Startup observation", snapshot, JsonOptions);
        return snapshot;
    }

    private async Task<bool> DatabaseExists()
    {
        var builder = new SqlConnectionStringBuilder(_connectionString);
        var database = builder.InitialCatalog;
        builder.InitialCatalog = "master";
        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT DB_ID(@DatabaseName)", connection);
        command.Parameters.AddWithValue("@DatabaseName", database);
        return await command.ExecuteScalarAsync() is not DBNull;
    }

    private static void AssertProbe(ProbeObservation probe, IReadOnlyDictionary<string, string> expected, string prefix)
    {
        probe.StatusCode.ShouldBe(int.Parse(expected[prefix + "StatusCode"], CultureInfo.InvariantCulture));
        probe.ContentType.ShouldBe(expected[prefix + "ContentType"]);
        probe.Body.ShouldBe(expected[prefix + "Body"]);
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    private sealed record ProbeObservation(int StatusCode, string? ContentType, string Body);
    private sealed record StartupSnapshot(string Phase, bool? RequestedApplyMigrations, bool EffectiveApplyMigrations,
        bool DatabaseExists, ProbeObservation Probe, SeedSnapshot? Seed);
    private sealed record SeedSnapshot(JsonElement Current, JsonElement History, string[] Applied, string[] Pending, bool SeedLinksValid);
}
