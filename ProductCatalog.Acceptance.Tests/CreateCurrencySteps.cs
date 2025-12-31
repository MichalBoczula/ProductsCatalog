using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using Reqnroll;
using Shouldly;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests
{
    [Binding]
    public class CreateCurrencySteps
    {
        private CreateCurrencyExternalDto _currencyCorrect;
        private CurrencyDto _successResult;
        private CreateCurrencyExternalDto _currencyIncorrect;
        private ApiProblemDetails _apiProblem;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("I have valid currency details")]
        public void GivenIHaveValidCurrencyDetails()
        {
            _currencyCorrect = new CreateCurrencyExternalDto("JPY", "Japanese Yen");
        }

        [When("I submit the create currency request")]
        public async Task WhenISubmitTheCreateCurrencyRequest()
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(_currencyCorrect, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);
            var response = await TestRunHooks.Client.PostAsync("/currencies", content);
            var json = await response.Content.ReadAsStringAsync();
            _successResult = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
        }

        [Then("the currency is created successfully")]
        public void ThenTheCurrencyIsCreatedSuccessfully()
        {
            _successResult.Id.ShouldNotBe(Guid.Empty);
            _successResult.Code.ShouldBe(_currencyCorrect.Code);
            _successResult.Description.ShouldBe(_currencyCorrect.Description);
            _successResult.IsActive.ShouldBeTrue();
        }

        [Given("I have invalid currency details")]
        public void GivenIHaveInvalidCurrencyDetails()
        {
            _currencyIncorrect = new CreateCurrencyExternalDto("", "");
        }

        [When("I submit the create invalid currency request")]
        public async Task WhenISubmitTheCreateInvalidCurrencyRequest()
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(_currencyIncorrect, _jsonOptions),
                  Encoding.UTF8,
                  MediaTypeNames.Application.Json);
            var response = await TestRunHooks.Client.PostAsync("/currencies", content);
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the currency creation fails with API error")]
        public async Task ThenTheCurrencyCreationFailsWithAPIError()
        {
            _apiProblem.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
        }
    }
}
