using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests;

[Binding]
public sealed class HealthCheckSteps(ScenarioApiContext api) : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private HttpResponseMessage? _response;

    [Given("the scenario SQL database has the required catalogue tables")]
    public async Task GivenRequiredTables(Table table)
    {
        var expected = Values(table);
        var state = await ObserveDatabase("Initial catalogue schema");
        state.DatabaseAvailable.ShouldBe(bool.Parse(expected["DatabaseAvailable"]));
        state.CurrentTablePresent.ShouldBe((bool?)bool.Parse(expected["CurrentTablePresent"]));
        state.HistoryTablePresent.ShouldBe((bool?)bool.Parse(expected["HistoryTablePresent"]));
    }

    [When("I request the health probe {string}")]
    public async Task WhenProbe(string path)
    {
        _response?.Dispose();
        _response = await api.Client!.GetAsync(path);
    }

    [When("the scenario SQL database is deleted")]
    public async Task WhenDeleteDatabase()
    {
        using (var scope = api.Factory!.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            (await db.Database.EnsureDeletedAsync()).ShouldBeTrue();
        }
        var state = await ObserveDatabase("Scenario database deleted");
        state.DatabaseAvailable.ShouldBeFalse();
    }

    [When("the scenario SQL database is recreated with migrations")]
    public async Task WhenRestoreDatabase()
    {
        await api.Factory!.MigrateAsync();
        var state = await ObserveDatabase("Scenario database recreated");
        state.DatabaseAvailable.ShouldBeTrue();
        state.CurrentTablePresent.ShouldBe((bool?)true);
        state.HistoryTablePresent.ShouldBe((bool?)true);
    }

    [When("the scenario SQL history table is removed")]
    public async Task WhenRemoveHistoryTable()
    {
        using (var scope = api.Factory!.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            await db.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].[TB_MobilePhones_History];");
        }
        var state = await ObserveDatabase("Scenario history table removed");
        state.DatabaseAvailable.ShouldBeTrue();
        state.CurrentTablePresent.ShouldBe((bool?)true);
        state.HistoryTablePresent.ShouldBe((bool?)false);
    }

    [Then("the health probe returns this text response")]
    public async Task ThenTextResponse(Table table)
    {
        var expected = Values(table);
        _response.ShouldNotBeNull();
        _response.StatusCode.ShouldBe((HttpStatusCode)int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture));
        _response.Content.Headers.ContentType?.MediaType.ShouldBe(expected["ContentType"]);
        (await _response.Content.ReadAsStringAsync()).ShouldBe(expected["Body"]);
    }

    private async Task<DatabaseObservation> ObserveDatabase(string phase)
    {
        using var scope = api.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var available = await db.Database.CanConnectAsync();
        bool? current = null;
        bool? history = null;
        if (available)
        {
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT OBJECT_ID(N'dbo.TB_MobilePhones', N'U'), OBJECT_ID(N'dbo.TB_MobilePhones_History', N'U');";
            await using var reader = await command.ExecuteReaderAsync();
            (await reader.ReadAsync()).ShouldBeTrue();
            current = !reader.IsDBNull(0);
            history = !reader.IsDBNull(1);
        }
        var observed = new DatabaseObservation(available, current, history);
        AllureJson.AttachObject("Health database observation", new { phase, observed }, JsonOptions);
        return observed;
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose() => _response?.Dispose();

    private sealed record DatabaseObservation(bool DatabaseAvailable, bool? CurrentTablePresent, bool? HistoryTablePresent);
}
