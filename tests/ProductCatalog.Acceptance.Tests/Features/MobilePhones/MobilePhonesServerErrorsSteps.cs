using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

[Binding]
public sealed class MobilePhonesServerErrorsSteps(ScenarioApiContext apiContext) : IDisposable
{
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;
    private HttpRequestMessage? _request;
    private HttpResponseMessage? _response;
    private FailingMobilePhonesRepository? _repository;
    private IServiceScope? _lookupScope;
    private Guid _historyPhoneId;

    [Given("mobile phone reads fail unexpectedly")]
    public void GivenMobilePhoneReadsFailUnexpectedly()
    {
        _repository = new FailingMobilePhonesRepository();
        CreateFailureClient();
    }

    [Given("the history query fails after a successful phone lookup")]
    public async Task GivenLaterHistoryFailure()
    {
        using var response = await apiContext.Client!.PostAsJsonAsync(
            "/mobile-phones", CreateMobilePhoneSteps.BuildMobilePhoneRequest(null), _json);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var phone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_json);
        phone.ShouldNotBeNull();
        _historyPhoneId = phone.Id;
        _lookupScope = apiContext.Factory!.Services.CreateScope();
        _repository = new FailingMobilePhonesRepository(
            _lookupScope.ServiceProvider.GetRequiredService<IMobilePhonesQueriesRepository>());
        CreateFailureClient();
    }

    private void CreateFailureClient()
    {
        _factory = apiContext.Factory!.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IMobilePhonesQueriesRepository>();
            services.AddSingleton<IMobilePhonesQueriesRepository>(_repository);
        }));
        _client = apiContext.CreateClient(_factory);
    }

    [When("I send this request during the read failure")]
    public async Task WhenReadRequest(Table table)
    {
        var values = Values(table);
        _request = new HttpRequestMessage(new HttpMethod(values["Method"]),
            values["Path"].Replace("{existingPhoneId}", _historyPhoneId.ToString(), StringComparison.Ordinal));
        if (values.TryGetValue("JsonBody", out var body))
            _request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        _response = await _client!.SendAsync(_request);
    }

    [Then("the read failure returns this safe problem")]
    public async Task ThenSafeError(Table table)
    {
        var expected = Values(table);
        _response.ShouldNotBeNull();
        _repository.ShouldNotBeNull();
        AllureJson.AttachObject("Read failure observation", new
        {
            repositoryCalls = _repository.Methods.Count,
            repositoryMethods = _repository.Methods
        }, _json);
        _repository.Methods.Count.ShouldBe(int.Parse(expected["RepositoryCalls"], CultureInfo.InvariantCulture));
        _repository.Methods.ShouldBe(expected["RepositoryMethod"].Split(',', StringSplitOptions.TrimEntries));
        var problem = await ApiProblemAssertions.Assert(_response, table);
        problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
        problem.GetProperty("detail").GetString().ShouldBe(expected["Detail"]);
        var body = await _response.Content.ReadAsStringAsync();
        body.ShouldNotContain(FailingMobilePhonesRepository.SensitiveDetail);
        body.ShouldNotContain(nameof(InvalidOperationException));
        body.ShouldNotContain(nameof(FailingMobilePhonesRepository));
        problem.TryGetProperty("exception", out _).ShouldBeFalse();
    }

    private static Dictionary<string, string> Values(Table table)
        => table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);

    public void Dispose()
    {
        _response?.Dispose();
        _request?.Dispose();
        _client?.Dispose();
        _factory?.Dispose();
        _lookupScope?.Dispose();
    }

    private sealed class FailingMobilePhonesRepository(IMobilePhonesQueriesRepository? historyLookup = null) : IMobilePhonesQueriesRepository
    {
        public const string SensitiveDetail = "INTERNAL_FAILURE_MARKER_DO_NOT_EXPOSE";
        public List<string> Methods { get; } = [];

        private Task<T> Fail<T>(string method)
        {
            Methods.Add(method);
            return Task.FromException<T>(new InvalidOperationException(SensitiveDetail));
        }

        public Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct)
        {
            if (historyLookup is null)
                return Fail<MobilePhoneReadModel?>(nameof(GetById));
            Methods.Add(nameof(GetById));
            return historyLookup.GetById(id, ct);
        }
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetByIds(IReadOnlyCollection<Guid> ids, CancellationToken ct)
            => Fail<IReadOnlyList<MobilePhoneReadModel>>(nameof(GetByIds));
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct)
            => Fail<IReadOnlyList<MobilePhoneReadModel>>(nameof(GetPhones));
        public Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken ct)
            => Fail<IReadOnlyList<MobilePhonesHistory>>(nameof(GetHistoryOfChanges));
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetTop(CancellationToken ct)
            => Fail<IReadOnlyList<MobilePhoneReadModel>>(nameof(GetTop));
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredPhones(MobilePhoneReadFilterDto filter, CancellationToken ct)
            => Fail<IReadOnlyList<MobilePhoneReadModel>>(nameof(GetFilteredPhones));
    }
}

