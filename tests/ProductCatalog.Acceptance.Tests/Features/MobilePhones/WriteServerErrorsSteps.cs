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
public sealed class WriteServerErrorsSteps(ScenarioApiContext context) : IDisposable
{
    private const string SensitiveDetail = "REF07_INTERNAL_WRITE_FAILURE_DO_NOT_EXPOSE";
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private FailingSaveRepository? _repository;
    private string? _operation;
    private string? _path;
    private Guid _phoneId;
    private string? _before;

    [Given("a {string} mobile phone write will fail at SaveChanges")]
    public async Task GivenAWriteWillFail(string operationId)
    {
        _operation = operationId;
        if (operationId is "UpdateMobilePhone" or "DeleteMobilePhone")
        {
            using var seed = await context.Client!.PostAsJsonAsync("/mobile-phones",
                CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _options);
            seed.EnsureSuccessStatusCode();
            var created = await seed.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_options);
            created.ShouldNotBeNull();
            _phoneId = created.Id;
        }

        _before = await Snapshot();
        _factory = context.Factory!.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            var registration = services.Single(x => x.ServiceType == typeof(IMobilePhonesCommandsRepository));
            services.RemoveAll<IMobilePhonesCommandsRepository>();
            services.AddScoped<IMobilePhonesCommandsRepository>(provider =>
            {
                _repository = new FailingSaveRepository(
                    (IMobilePhonesCommandsRepository)ActivatorUtilities.CreateInstance(provider, registration.ImplementationType!));
                return _repository;
            });
        }));
        _client = _factory.CreateClient();
    }

    [When("I perform the failing {string} mobile phone write")]
    public async Task WhenIPerformTheFailingWrite(string operationId)
    {
        operationId.ShouldBe(_operation);
        _client.ShouldNotBeNull();
        _path = operationId == "CreateMobilePhone" ? "/mobile-phones" : $"/mobile-phones/{_phoneId}";
        _response = operationId switch
        {
            "CreateMobilePhone" => await _client.PostAsJsonAsync(_path,
                CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _options),
            "UpdateMobilePhone" => await _client.PutAsJsonAsync(_path, UpdateRequest(), _options),
            "DeleteMobilePhone" => await _client.DeleteAsync(_path),
            _ => throw new ArgumentOutOfRangeException(nameof(operationId))
        };
    }

    [Then("a safe error is returned and the phone and history are unchanged")]
    public async Task ThenTheWriteIsRejectedWithoutChanges()
    {
        _response.ShouldNotBeNull();
        _repository.ShouldNotBeNull();
        _repository.SaveCalls.ShouldBe(1);
        _repository.HistoryCalls.ShouldBe(1);
        _response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        _response.Content.Headers.ContentType?.MediaType.ShouldBe("application/problem+json");
        var body = await _response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(body);
        json.RootElement.GetProperty("status").GetInt32().ShouldBe(500);
        json.RootElement.GetProperty("code").GetString().ShouldBe("internal_error");
        json.RootElement.GetProperty("instance").GetString().ShouldBe(_path);
        json.RootElement.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
        body.ShouldNotContain(SensitiveDetail);
        body.ShouldNotContain("stackTrace");
        (await Snapshot()).ShouldBe(_before);
    }

    private UpdateMobilePhoneExternalDto UpdateRequest()
    {
        var create = CreateMobilePhoneSteps.BuildMobilePhoneRequest(null);
        var update = JsonSerializer.Deserialize<UpdateMobilePhoneExternalDto>(
            JsonSerializer.Serialize(create, _options), _options)!;
        return update with { CommonDescription = update.CommonDescription with { Name = "REF07 changed name" } };
    }

    private async Task<string> Snapshot()
    {
        using var scope = context.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var phones = await db.MobilePhones.AsNoTracking()
            .OrderBy(x => x.Id).Select(x => new { x.Id, x.CommonDescription.Name, x.IsActive }).ToListAsync();
        var history = await db.MobilePhonesHistories.AsNoTracking()
            .OrderBy(x => x.Id).Select(x => new { x.Id, x.MobilePhoneId, x.Name, x.IsActive }).ToListAsync();
        return JsonSerializer.Serialize(new { phones, history });
    }

    public void Dispose()
    {
        _response?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
    }

    private sealed class FailingSaveRepository(IMobilePhonesCommandsRepository inner) : IMobilePhonesCommandsRepository
    {
        public int SaveCalls { get; private set; }
        public int HistoryCalls { get; private set; }
        public void Add(MobilePhone phone) => inner.Add(phone);
        public void Update(MobilePhone phone) => inner.Update(phone);
        public Task<MobilePhone?> GetById(Guid id, CancellationToken ct) => inner.GetById(id, ct);
        public void WriteHistory(MobilePhonesHistory history)
        {
            HistoryCalls++;
            inner.WriteHistory(history);
        }
        public Task SaveChanges(CancellationToken ct)
        {
            SaveCalls++;
            return Task.FromException(new InvalidOperationException(SensitiveDetail));
        }
    }
}
