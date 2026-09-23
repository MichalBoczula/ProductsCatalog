using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public sealed class MobilePhonesServerErrorsSteps : IDisposable
    {
        private readonly ScenarioApiContext _apiContext;

        public MobilePhonesServerErrorsSteps(ScenarioApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        private WebApplicationFactory<Program>? _factory;
        private HttpClient? _client;
        private HttpResponseMessage? _response;
        private FailingMobilePhonesRepository? _repository;

        [Given("mobile phone reads fail unexpectedly")]
        public void GivenMobilePhoneReadsFailUnexpectedly()
        {
            _repository = new FailingMobilePhonesRepository();
            _factory = _apiContext.Factory!.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IMobilePhonesQueriesRepository>();
                    services.AddSingleton<IMobilePhonesQueriesRepository>(_repository);
                }));
            _client = _factory.CreateClient();
        }

        [When("I request {string} during the read failure")]
        public async Task WhenIRequestDuringTheReadFailure(string operation)
        {
            _client.ShouldNotBeNull();
            var id = Guid.NewGuid();
            _response = operation switch
            {
                "Get by ID" => await _client.GetAsync($"/mobile-phones/{id}"),
                "Get by amount" => await _client.GetAsync("/mobile-phones?amount=1"),
                "Get history" => await _client.GetAsync($"/mobile-phones/{id}/history?pageNumber=1&pageSize=10"),
                "Get top" => await _client.GetAsync("/mobile-phones/top"),
                "Filter" => await _client.PostAsJsonAsync("/mobile-phones/filter", new { Brand = "Motorola" }),
                "Get by IDs" => await _client.PostAsJsonAsync("/mobile-phones/by-ids", new[] { id }),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unknown operation.")
            };
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})",
                await _response.Content.ReadAsStringAsync());
        }

        [Then("a safe mobile phone server error is returned")]
        public async Task ThenASafeMobilePhoneServerErrorIsReturned(Table table)
        {
            var expected = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
            _response.ShouldNotBeNull();
            _repository.ShouldNotBeNull();
            _repository.Calls.ShouldBe(1);
            _response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

            var body = await _response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(body);
            var problem = document.RootElement;
            problem.ValueKind.ShouldBe(JsonValueKind.Object);
            problem.GetProperty("status").GetInt32().ShouldBe(int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture));
            problem.GetProperty("title").GetString().ShouldBe(expected["Title"]);
            problem.GetProperty("detail").GetString().ShouldBe(expected["Detail"]);
            problem.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
            problem.GetProperty("code").GetString().ShouldBe("internal_error");
            body.ShouldNotContain(FailingMobilePhonesRepository.SensitiveDetail);
            body.ShouldNotContain(nameof(InvalidOperationException));
            body.ShouldNotContain(nameof(FailingMobilePhonesRepository));
            problem.TryGetProperty("stackTrace", out _).ShouldBeFalse();
            problem.TryGetProperty("exception", out _).ShouldBeFalse();
        }

        public void Dispose()
        {
            _response?.Dispose();
            _client?.Dispose();
            _factory?.Dispose();
        }

        private sealed class FailingMobilePhonesRepository : IMobilePhonesQueriesRepository
        {
            public const string SensitiveDetail = "INTERNAL_FAILURE_MARKER_DO_NOT_EXPOSE";
            public int Calls { get; private set; }

            private Task<T> Fail<T>()
            {
                Calls++;
                return Task.FromException<T>(new InvalidOperationException(SensitiveDetail));
            }

            public Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct)
                => Fail<MobilePhoneReadModel?>();

            public Task<IReadOnlyList<MobilePhoneReadModel>> GetByIds(IReadOnlyCollection<Guid> ids, CancellationToken ct)
                => Fail<IReadOnlyList<MobilePhoneReadModel>>();

            public Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct)
                => Fail<IReadOnlyList<MobilePhoneReadModel>>();

            public Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken ct)
                => Fail<IReadOnlyList<MobilePhonesHistory>>();

            public Task<IReadOnlyList<MobilePhoneReadModel>> GetTop(CancellationToken ct)
                => Fail<IReadOnlyList<MobilePhoneReadModel>>();

            public Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredPhones(MobilePhoneReadFilterDto mobilePhoneFilter, CancellationToken ct)
                => Fail<IReadOnlyList<MobilePhoneReadModel>>();
        }
    }
}
