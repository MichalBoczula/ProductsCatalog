using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class RejectedRequestStateSteps(ScenarioApiContext api)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private JsonElement _beforePhones;
    private JsonElement _beforeHistory;

    [Given("the catalogue and history are recorded before the rejected request")]
    public async Task GivenSavedState()
    {
        (_beforePhones, _beforeHistory) = await ReadState();
    }

    [Then("the rejected request leaves the catalogue and history unchanged")]
    public async Task ThenUnchangedState(Table table)
    {
        var expected = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        var (phones, history) = await ReadState();
        var currentUnchanged = JsonElement.DeepEquals(_beforePhones, phones);
        var historyUnchanged = JsonElement.DeepEquals(_beforeHistory, history);
        var currentRowsChange = phones.GetArrayLength() - _beforePhones.GetArrayLength();
        var historyRowsChange = history.GetArrayLength() - _beforeHistory.GetArrayLength();
        AllureJson.AttachObject("State after rejected request", new
        {
            beforeCurrentRows = _beforePhones.GetArrayLength(),
            afterCurrentRows = phones.GetArrayLength(),
            beforeHistoryRows = _beforeHistory.GetArrayLength(),
            afterHistoryRows = history.GetArrayLength(),
            currentRowsChange,
            historyRowsChange,
            currentDataUnchanged = currentUnchanged,
            historyDataUnchanged = historyUnchanged
        }, JsonOptions);
        if (!currentUnchanged || !historyUnchanged)
            AllureJson.AttachObject("Changed state after rejected request", new
            {
                beforePhones = _beforePhones,
                afterPhones = phones,
                beforeHistory = _beforeHistory,
                afterHistory = history
            }, JsonOptions);
        currentRowsChange.ShouldBe(int.Parse(expected["CurrentRowsChange"], CultureInfo.InvariantCulture));
        historyRowsChange.ShouldBe(int.Parse(expected["HistoryRowsChange"], CultureInfo.InvariantCulture));
        currentUnchanged.ShouldBe(bool.Parse(expected["CurrentDataUnchanged"]));
        historyUnchanged.ShouldBe(bool.Parse(expected["HistoryDataUnchanged"]));
    }

    private async Task<(JsonElement Phones, JsonElement History)> ReadState()
    {
        using var scope = api.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var phones = await db.MobilePhones.AsNoTracking().OrderBy(phone => phone.Id).ToListAsync();
        var history = await db.MobilePhonesHistories.AsNoTracking().OrderBy(entry => entry.Id).ToListAsync();
        return (JsonSerializer.SerializeToElement(phones, JsonOptions), JsonSerializer.SerializeToElement(history, JsonOptions));
    }
}
