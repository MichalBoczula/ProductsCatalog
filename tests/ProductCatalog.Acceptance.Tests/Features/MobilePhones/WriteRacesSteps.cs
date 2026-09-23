using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

[Binding]
public sealed class WriteRacesSteps(ScenarioApiContext context) : IDisposable
{
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private Guid _id;
    private Interruption _interruption;

    [Given("a phone write is interrupted by a concurrent update")]
    public Task GivenConcurrentUpdate() => Arrange(Interruption.Update);

    [Given("a phone write is interrupted by a concurrent delete")]
    public Task GivenConcurrentDelete() => Arrange(Interruption.Delete);

    [Given("a phone write is interrupted by a physical removal")]
    public Task GivenPhysicalRemoval() => Arrange(Interruption.Remove);

    [Given("a phone exists for an unchanged update")]
    public Task GivenUnchangedUpdate() => Arrange(Interruption.None);

    [Given("a phone exists for a repeated delete")]
    public Task GivenRepeatedDelete() => Arrange(Interruption.None);

    private async Task Arrange(Interruption interruption)
    {
        _interruption = interruption;
        using var seed = await context.Client!.PostAsJsonAsync("/mobile-phones",
            CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _json);
        seed.StatusCode.ShouldBe(HttpStatusCode.Created);
        _id = (await seed.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json))!.Id;
        if (interruption == Interruption.None)
            return;

        _factory = context.Factory!.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            var registration = services.Single(x => x.ServiceType == typeof(IMobilePhonesCommandsRepository));
            services.RemoveAll<IMobilePhonesCommandsRepository>();
            services.AddScoped<IMobilePhonesCommandsRepository>(provider => new RacingRepository(
                (IMobilePhonesCommandsRepository)ActivatorUtilities.CreateInstance(provider, registration.ImplementationType!),
                () => Interrupt(interruption)));
        }));
        _client = _factory.CreateClient();
    }

    private async Task Interrupt(Interruption interruption)
    {
        if (interruption == Interruption.Remove)
        {
            using var scope = context.Factory!.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            (await db.MobilePhones.Where(x => x.Id == _id).ExecuteDeleteAsync()).ShouldBe(1);
            return;
        }

        using var response = interruption == Interruption.Delete
            ? await context.Client!.DeleteAsync($"/mobile-phones/{_id}")
            : await context.Client!.PutAsJsonAsync($"/mobile-phones/{_id}", UpdateRequest("Winner Phone"), _json);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [When("the pending update is submitted")]
    public async Task WhenUpdate() => _response = await _client!.PutAsJsonAsync(
        $"/mobile-phones/{_id}", UpdateRequest("Losing Phone"), _json);

    [When("the pending delete is submitted")]
    public async Task WhenDelete() => _response = await _client!.DeleteAsync($"/mobile-phones/{_id}");

    [When("the unchanged update is submitted")]
    public async Task WhenUnchangedUpdate() => _response = await context.Client!.PutAsJsonAsync(
        $"/mobile-phones/{_id}", UpdateRequest("Test Mobile Phone"), _json);

    [When("the repeated delete is submitted")]
    public async Task WhenRepeatedDelete()
    {
        using var first = await context.Client!.DeleteAsync($"/mobile-phones/{_id}");
        first.StatusCode.ShouldBe(HttpStatusCode.OK);
        _response = await context.Client.DeleteAsync($"/mobile-phones/{_id}");
    }

    [Then("the conditional phone write returns {int} and preserves only the winning history")]
    public async Task ThenStatusAndHistory(int status)
    {
        _response.ShouldNotBeNull();
        _response.StatusCode.ShouldBe((HttpStatusCode)status);
        if (status != 200)
        {
            _response.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");
            var problem = await _response.Content.ReadFromJsonAsync<JsonElement>(_json);
            problem.GetProperty("code").GetString().ShouldBe(status == 404
                ? "resource_not_found" : "concurrency_conflict");
        }

        using var scope = context.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var history = await db.MobilePhonesHistories.AsNoTracking()
            .Where(x => x.MobilePhoneId == _id).ToListAsync();
        history.Count.ShouldBe(_interruption == Interruption.Update || _interruption == Interruption.Delete
            ? 2 : status == 200 && _response.RequestMessage?.Method == HttpMethod.Delete ? 2 : 1);
        var phone = await db.MobilePhones.AsNoTracking().SingleOrDefaultAsync(x => x.Id == _id);
        if (_interruption == Interruption.Remove)
            phone.ShouldBeNull();
        else
        {
            phone.ShouldNotBeNull();
            if (_interruption == Interruption.Update)
                phone.CommonDescription.Name.ShouldBe("Winner Phone");
            if (_interruption == Interruption.Delete || _response.RequestMessage?.Method == HttpMethod.Delete)
                phone.IsActive.ShouldBeFalse();
            if (_interruption == Interruption.None && _response.RequestMessage?.Method == HttpMethod.Put)
                phone.CommonDescription.Name.ShouldBe("Test Mobile Phone");
        }
    }

    private UpdateMobilePhoneExternalDto UpdateRequest(string name)
    {
        var create = CreateMobilePhoneSteps.BuildMobilePhoneRequest(null);
        var update = JsonSerializer.Deserialize<UpdateMobilePhoneExternalDto>(
            JsonSerializer.Serialize(create, _json), _json)!;
        return update with { CommonDescription = update.CommonDescription with { Name = name } };
    }

    public void Dispose()
    {
        _response?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
    }

    private enum Interruption { None, Update, Delete, Remove }

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
