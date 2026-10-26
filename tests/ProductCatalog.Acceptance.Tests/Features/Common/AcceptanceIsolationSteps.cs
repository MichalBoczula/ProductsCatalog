using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class AcceptanceIsolationSteps(ScenarioApiContext api)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private string? _marker;

    [Given("the isolated scenario starts with this marker state")]
    public async Task GivenInitialState(Table table)
    {
        var expected = Values(table);
        _marker = expected["Name"];
        using var scope = api.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var current = await db.MobilePhones.AsNoTracking().Where(phone => phone.CommonDescription.Name == _marker).ToListAsync();
        var history = await db.MobilePhonesHistories.AsNoTracking().Where(entry => entry.Name == _marker).ToListAsync();
        AllureJson.AttachObject("Isolation marker before creation", new
        {
            database = db.Database.GetDbConnection().Database,
            marker = _marker,
            current,
            history
        }, JsonOptions);
        current.Count.ShouldBe(int.Parse(expected["CurrentRows"], CultureInfo.InvariantCulture));
        history.Count.ShouldBe(int.Parse(expected["HistoryRows"], CultureInfo.InvariantCulture));
    }

    [Then("the isolated scenario contains this linked marker state")]
    public async Task ThenLinkedState(Table table)
    {
        var expected = Values(table);
        expected["Name"].ShouldBe(_marker);
        using var scope = api.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var current = await db.MobilePhones.AsNoTracking().Where(phone => phone.CommonDescription.Name == _marker).ToListAsync();
        var history = await db.MobilePhonesHistories.AsNoTracking().Where(entry => entry.Name == _marker).ToListAsync();
        AllureJson.AttachObject("Isolation marker after creation", new
        {
            database = db.Database.GetDbConnection().Database,
            marker = _marker,
            current,
            history
        }, JsonOptions);
        current.Count.ShouldBe(int.Parse(expected["CurrentRows"], CultureInfo.InvariantCulture));
        history.Count.ShouldBe(int.Parse(expected["HistoryRows"], CultureInfo.InvariantCulture));
        var phone = current.Single();
        var entry = history.Single();
        phone.Id.ShouldNotBe(Guid.Empty);
        entry.Id.ShouldNotBe(Guid.Empty);
        phone.CommonDescription.Name.ShouldBe(expected["Name"]);
        phone.IsActive.ShouldBe(bool.Parse(expected["IsActive"]));
        entry.MobilePhoneId.ShouldBe(phone.Id);
        entry.Name.ShouldBe(expected["Name"]);
        entry.IsActive.ShouldBe(phone.IsActive);
        entry.Operation.ToString().ShouldBe(expected["HistoryOperation"]);
        entry.ChangedAt.ShouldBe(phone.ChangedAt);
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
}
