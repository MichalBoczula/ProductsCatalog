using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using Reqnroll;
using Shouldly;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Currencies
{
    [Binding]
    public class GetCurrenciesSteps
    {
        private HttpResponseMessage? _response;
        private IReadOnlyList<CurrencyDto>? _currencies;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [When("I request the list of currencies")]
        public async Task WhenIRequestTheListOfCurrencies()
        {
            _response = await TestRunHooks.Client.GetAsync("/currencies");
            var json = await _response.Content.ReadAsStringAsync();
            _currencies = JsonSerializer.Deserialize<IReadOnlyList<CurrencyDto>>(json, _jsonOptions);
        }

        [Then("the currency list is returned")]
        public void ThenTheCurrencyListIsReturned()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            _currencies.ShouldNotBeNull();
            _currencies!.ShouldNotBeEmpty();
            _currencies.ShouldContain(c => c.Code == "USD");
        }
    }
}
