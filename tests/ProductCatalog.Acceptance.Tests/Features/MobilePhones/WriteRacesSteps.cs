using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

[Binding]
public sealed class WriteRacesSteps(ScenarioApiContext context) : IDisposable
{
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private readonly List<string> _timeline = [];
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private CreateMobilePhoneExternalDto _create = null!;
    private MobilePhoneDetailsDto _created = null!;
    private SavedState _initial = null!;
    private SavedState _protected = null!;
    private SavedState _after = null!;
    private JsonElement _expectedResponse;
    private Interruption _interruption;
    private string? _winnerName;
    private int _competingActions;
    private int _physicallyRemovedRows;

    [Given("an active phone exists for a conditional write")]
    public async Task GivenAnActivePhone(Table table)
    {
        _create = CreateMobilePhoneSteps.BuildMobilePhoneRequest(table);
        using var seed = await context.Client!.PostAsJsonAsync("/mobile-phones", _create, _json);
        seed.StatusCode.ShouldBe(HttpStatusCode.Created);
        _created = (await seed.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json))!;
        _created.ShouldNotBeNull();
        _created.IsActive.ShouldBeTrue();
        _initial = await ReadState();
        _protected = _initial;
        _expectedResponse = JsonSerializer.SerializeToElement(_created, _json);
        _timeline.Add("Initial active phone and inserted history saved");
    }

    [Given("a competing update will commit after the pending write loads the phone")]
    public void GivenCompetingUpdate(Table table)
    {
        _winnerName = Values(table)["Name"];
        ArrangeInterruption(Interruption.Update);
    }

    [Given("a competing soft delete will commit after the pending write loads the phone")]
    public void GivenCompetingDelete() => ArrangeInterruption(Interruption.Delete);

    [Given("the phone row will be physically removed after the pending write loads it")]
    public void GivenPhysicalRemoval() => ArrangeInterruption(Interruption.Remove);

    private void ArrangeInterruption(Interruption interruption)
    {
        _interruption = interruption;
        _factory = context.Factory!.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            var registration = services.Single(x => x.ServiceType == typeof(IMobilePhonesCommandsRepository));
            services.RemoveAll<IMobilePhonesCommandsRepository>();
            services.AddScoped<IMobilePhonesCommandsRepository>(provider => new RacingRepository(
                (IMobilePhonesCommandsRepository)ActivatorUtilities.CreateInstance(provider, registration.ImplementationType!),
                Interrupt));
        }));
        _client = context.CreateClient(_factory);
    }

    private async Task Interrupt()
    {
        // The decorator calls this only after the real repository has loaded the stale entity.
        // Buffer observations here: the server callback has no caller Allure context.
        _timeline.Add("Pending write loaded the original phone");
        _competingActions++;
        if (_interruption == Interruption.Remove)
        {
            using var scope = context.Factory!.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            _physicallyRemovedRows = await db.MobilePhones.Where(phone => phone.Id == _created.Id).ExecuteDeleteAsync();
            _physicallyRemovedRows.ShouldBe(1);
            _timeline.Add("Physical removal committed");
        }
        else
        {
            using var response = _interruption == Interruption.Delete
                ? await context.Client!.DeleteAsync($"/mobile-phones/{_created.Id}")
                : await context.Client!.PutAsJsonAsync($"/mobile-phones/{_created.Id}", UpdateRequest(_winnerName!), _json);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var winner = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json);
            winner.ShouldNotBeNull();
            var expected = _interruption == Interruption.Delete
                ? _created with { IsActive = false }
                : _created with { CommonDescription = _created.CommonDescription with { Name = _winnerName! } };
            JsonElement.DeepEquals(JsonSerializer.SerializeToElement(winner, _json),
                JsonSerializer.SerializeToElement(expected, _json)).ShouldBeTrue();
            _timeline.Add(_interruption == Interruption.Delete ? "Competing soft delete committed" : "Competing update committed");
        }
        _protected = await ReadState();
    }

    [When("I submit the pending phone update")]
    public async Task WhenUpdate(Table table)
    {
        _timeline.Add("Pending update submitted");
        _response = await _client!.PutAsJsonAsync($"/mobile-phones/{_created.Id}", UpdateRequest(Values(table)["Name"]), _json);
        await RecordResult();
    }

    [When("I submit the pending phone delete")]
    public async Task WhenDelete()
    {
        _timeline.Add("Pending delete submitted");
        _response = await _client!.DeleteAsync($"/mobile-phones/{_created.Id}");
        await RecordResult();
    }

    [When("I submit the saved phone profile unchanged")]
    public async Task WhenUnchangedUpdate()
    {
        _timeline.Add("Unchanged profile submitted");
        _response = await context.Client!.PutAsJsonAsync($"/mobile-phones/{_created.Id}", UpdateRequest(_create.CommonDescription.Name), _json);
        await RecordResult();
    }

    [When("I delete the phone and repeat the delete")]
    public async Task WhenRepeatedDelete()
    {
        using var firstRequest = new HttpRequestMessage(HttpMethod.Delete, $"/mobile-phones/{_created.Id}");
        firstRequest.Options.Set(HttpEvidenceHandler.SupportingExchangeKey, true);
        using var first = await context.Client!.SendAsync(firstRequest);
        first.StatusCode.ShouldBe(HttpStatusCode.OK);
        var deleted = await first.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json);
        deleted.ShouldNotBeNull();
        JsonElement.DeepEquals(JsonSerializer.SerializeToElement(deleted, _json),
            JsonSerializer.SerializeToElement(_created with { IsActive = false }, _json)).ShouldBeTrue();
        _expectedResponse = JsonSerializer.SerializeToElement(deleted, _json);
        _protected = await ReadState();
        _timeline.Add("First soft delete committed");
        _timeline.Add("Repeated delete submitted");
        _response = await context.Client.DeleteAsync($"/mobile-phones/{_created.Id}");
        await RecordResult();
    }

    private async Task RecordResult()
    {
        _timeline.Add($"Pending response returned {(int)_response!.StatusCode}");
        _after = await ReadState();
        AllureJson.AttachObject("Conditional write timeline", _timeline, _json);
        AllureJson.AttachObject("Conditional write saved state", new
        {
            initial = _initial,
            protectedBeforePendingResult = _protected,
            after = _after,
            competingActions = _competingActions,
            physicallyRemovedRows = _physicallyRemovedRows,
            pendingCurrentUnchanged = Same(_protected.Phone, _after.Phone),
            pendingHistoryUnchanged = Same(_protected.History, _after.History)
        }, _json);
    }

    [Then("the pending phone write returns this response")]
    public async Task ThenResponse(Table table)
    {
        var expected = Values(table);
        _response.ShouldNotBeNull();
        var status = int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture);
        _response.StatusCode.ShouldBe((HttpStatusCode)status);
        _response.Content.Headers.ContentType?.MediaType.ShouldBe(expected["ContentType"]);
        if (status >= 400)
        {
            var problem = await ApiProblemAssertions.Assert(_response, table);
            problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
            if (expected.TryGetValue("Detail", out var detail))
                problem.GetProperty("detail").GetString().ShouldBe(detail);
            if (expected.TryGetValue("DetailContains", out var fragment))
                problem.GetProperty("detail").GetString()!.ShouldContain(fragment.Replace("{PhoneId}", _created.Id.ToString()));
        }
        else
        {
            var result = await _response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json);
            result.ShouldNotBeNull();
            result.Id.ShouldBe(_created.Id);
            result.CommonDescription.Name.ShouldBe(expected["Name"]);
            result.IsActive.ShouldBe(bool.Parse(expected["IsActive"]));
            JsonElement.DeepEquals(JsonSerializer.SerializeToElement(result, _json), _expectedResponse).ShouldBeTrue();
        }
    }

    [Then("the conditional write leaves this saved state and history")]
    public void ThenSavedState(Table table)
    {
        var expected = Values(table);
        (_after.Phone is null ? 0 : 1).ShouldBe(int.Parse(expected["CurrentRows"], CultureInfo.InvariantCulture));
        if (_after.Phone is not null)
        {
            _after.Phone.CommonDescription.Name.ShouldBe(expected["CurrentName"]);
            _after.Phone.IsActive.ShouldBe(bool.Parse(expected["CurrentIsActive"]));
        }
        _after.History.Count.ShouldBe(int.Parse(expected["HistoryRows"], CultureInfo.InvariantCulture));
        _after.History.Select(entry => entry.Operation.ToString()).ShouldBe(
            expected["HistoryOperations"].Split(',', StringSplitOptions.TrimEntries));
        _after.History.Last().Name.ShouldBe(expected["LatestHistoryName"]);
        _after.History.Last().IsActive.ShouldBe(bool.Parse(expected["LatestHistoryIsActive"]));
        _competingActions.ShouldBe(int.Parse(expected["CompetingActions"], CultureInfo.InvariantCulture));
        Same(_protected.Phone, _after.Phone).ShouldBe(bool.Parse(expected["PendingCurrentUnchanged"]));
        Same(_protected.History, _after.History).ShouldBe(bool.Parse(expected["PendingHistoryUnchanged"]));
        var inserted = _initial.History.Single();
        Same(inserted, _after.History.Single(entry => entry.Id == inserted.Id)).ShouldBeTrue();
    }

    private UpdateMobilePhoneExternalDto UpdateRequest(string name)
    {
        var update = JsonSerializer.Deserialize<UpdateMobilePhoneExternalDto>(JsonSerializer.Serialize(_create, _json), _json)!;
        return update with { CommonDescription = update.CommonDescription with { Name = name } };
    }

    private bool Same<T>(T before, T after) => JsonElement.DeepEquals(
        JsonSerializer.SerializeToElement(before, _json), JsonSerializer.SerializeToElement(after, _json));

    private async Task<SavedState> ReadState()
    {
        using var scope = context.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var phone = await db.MobilePhones.AsNoTracking().SingleOrDefaultAsync(item => item.Id == _created.Id);
        var history = await db.MobilePhonesHistories.AsNoTracking().Where(entry => entry.MobilePhoneId == _created.Id)
            .OrderBy(entry => entry.ChangedAt).ThenBy(entry => entry.Id).ToListAsync();
        return new SavedState(phone, history);
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose()
    {
        _response?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
    }

    private enum Interruption { None, Update, Delete, Remove }
    private sealed record SavedState(MobilePhone? Phone, List<MobilePhonesHistory> History);

    private sealed class RacingRepository(IMobilePhonesCommandsRepository inner, Func<Task> interrupt)
        : IMobilePhonesCommandsRepository
    {
        private bool _triggered;
        public void Add(MobilePhone phone) => inner.Add(phone);
        public void Update(MobilePhone phone) => inner.Update(phone);
        public async Task<MobilePhone?> GetById(Guid id, CancellationToken token)
        {
            var loaded = await inner.GetById(id, token);
            if (loaded is not null && !_triggered)
            {
                _triggered = true;
                await interrupt();
            }
            return loaded;
        }
        public void WriteHistory(MobilePhonesHistory history) => inner.WriteHistory(history);
        public Task SaveChanges(CancellationToken token) => inner.SaveChanges(token);
    }
}
