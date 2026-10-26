using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhonesByAmountSteps
    {
        private readonly ScenarioApiContext _apiContext;

        public GetMobilePhonesByAmountSteps(ScenarioApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        private readonly List<MobilePhoneDetailsDto> _createdMobilePhones = new();
        private List<MobilePhoneDto> _result = new();
        private HttpResponseMessage? _response;
        private int _amount;

        [Given("the following phones are available in the catalogue")]
        public async Task GivenCataloguePhones(Table table)
        {
            foreach (var row in table.Rows)
            {
                var details = new Table("Field", "Value");
                foreach (var column in table.Header)
                    details.AddRow(column, row[column]);
                var request = CreateMobilePhoneSteps.BuildMobilePhoneRequest(details);
                using var response = await _apiContext.Client!.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
                response.StatusCode.ShouldBe(HttpStatusCode.Created);
                var created = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
                created.ShouldNotBeNull();
                _createdMobilePhones.Add(created);
            }
        }

        [Given("no mobile phones exist in the database")]
        public async Task GivenNoMobilePhonesExistInTheDatabase()
        {
            using var scope = _apiContext.Factory!.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            context.MobilePhonesHistories.RemoveRange(context.MobilePhonesHistories);
            context.MobilePhones.RemoveRange(context.MobilePhones);

            await context.SaveChangesAsync();
        }

        [When("I request mobile phones with amount {int}")]
        public async Task WhenIRequestMobilePhonesWithAmount(int amount)
        {
            _amount = amount;
            _response = await _apiContext.Client!.GetAsync($"/mobile-phones?amount={amount}");

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the catalogue page contains exactly these phones in order")]
        public async Task ThenExactCataloguePage(Table table)
        {
            _response.ShouldNotBeNull();
            _response.StatusCode.ShouldBe((HttpStatusCode)int.Parse(table.Rows[0]["StatusCode"], CultureInfo.InvariantCulture));
            var result = await _response.Content.ReadFromJsonAsync<List<MobilePhoneDto>>(_jsonOptions);
            result.ShouldNotBeNull();
            result.Count.ShouldBe(table.Rows.Count);
            result.Count.ShouldBe(_amount);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var expected = table.Rows[i];
                var phone = result[i];
                phone.Id.ShouldBe(_createdMobilePhones.Single(created => created.CommonDescription.Name == expected["Name"]).Id);
                phone.Name.ShouldBe(expected["Name"]);
                phone.MainPhoto.ShouldBe(expected["MainPhoto"]);
                phone.Price.Amount.ShouldBe(decimal.Parse(expected["PriceAmount"], CultureInfo.InvariantCulture));
                phone.Price.Currency.ShouldBe(expected["PriceCurrency"]);
            }
        }

        [Then("an empty mobile phone list is returned")]
        public async Task ThenAnEmptyMobilePhoneListIsReturned(Table table)
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(ParseExpectedTable(table), "StatusCode"));

            var result = await DeserializeResponse<List<MobilePhoneDto>>(_response);
            result.ShouldNotBeNull();
            _result = result;
            _result.Count.ShouldBe(int.Parse(ParseExpectedTable(table)["Amount"], CultureInfo.InvariantCulture));
            _result.ShouldBeEmpty();
        }

        [Then("the mobile phone request fails with validation error")]
        public async Task ThenTheMobilePhoneRequestFailsWithValidationError(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var problem = await DeserializeResponse<ApiProblemDetails>(_response);
            problem.ShouldNotBeNull();
            problem.Status.ShouldBe((int)_response.StatusCode);
            problem.Errors.ShouldContain(error =>
                error.Message == GetRequiredValue(expected, "ErrorMessage")
                && error.Entity == GetRequiredValue(expected, "ErrorEntity")
                && error.Name == GetRequiredValue(expected, "ErrorName"));
        }

        private static Dictionary<string, string> ParseExpectedTable(Table table)
        {
            var expected = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                expected[row["Field"]] = row["Value"];
            }

            return expected;
        }

        private static HttpStatusCode ParseStatusCode(IReadOnlyDictionary<string, string> expected, string key)
        {
            return (HttpStatusCode)int.Parse(GetRequiredValue(expected, key), CultureInfo.InvariantCulture);
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> expected, string key)
        {
            if (!expected.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in expected mobile phone response table.");
            }

            return value;
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
