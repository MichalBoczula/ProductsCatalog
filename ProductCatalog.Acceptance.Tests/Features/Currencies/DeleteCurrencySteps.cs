using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using Reqnroll;
using Shouldly;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Currencies
{
    [Binding]
    public class DeleteCurrencySteps
    {
        private CreateCurrencyExternalDto _currencyToCreate = default!;
        private CurrencyDto? _createdCurrency;
        private CurrencyDto? _deleteResult;
        private Guid _missingCurrencyId;
        private ApiProblemDetails? _apiProblem;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing currency to delete")]
        public async Task GivenAnExistingCurrencyToDelete()
        {
            var code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant();
            _currencyToCreate = new CreateCurrencyExternalDto(code, $"{code} description");

            var content = new StringContent(
                 JsonSerializer.Serialize(_currencyToCreate, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PostAsync("/currencies", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            _createdCurrency = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
            _createdCurrency.ShouldNotBeNull();
        }

        [When("I submit the delete currency request")]
        public async Task WhenISubmitTheDeleteCurrencyRequest()
        {
            _createdCurrency.ShouldNotBeNull();

            var response = await TestRunHooks.Client.DeleteAsync($"/currencies/{_createdCurrency!.Id}");
            var json = await response.Content.ReadAsStringAsync();
            _deleteResult = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
        }

        [Then("the currency is deleted successfully")]
        public void ThenTheCurrencyIsDeletedSuccessfully()
        {
            _deleteResult.ShouldNotBeNull();
            _deleteResult!.Id.ShouldBe(_createdCurrency!.Id);
            _deleteResult.Code.ShouldBe(_currencyToCreate.Code);
            _deleteResult.Description.ShouldBe(_currencyToCreate.Description);
            _deleteResult.IsActive.ShouldBeFalse();
        }

        [Given("a currency id that does not exist")]
        public void GivenACurrencyIdThatDoesNotExist()
        {
            _missingCurrencyId = Guid.NewGuid();
        }

        [When("I submit the delete currency request for non existing currency")]
        public async Task WhenISubmitTheDeleteCurrencyRequestForNonExistingCurrency()
        {
            var response = await TestRunHooks.Client.DeleteAsync($"/currencies/{_missingCurrencyId}");
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the currency deletion fails with API error")]
        public void ThenTheCurrencyDeletionFailsWithAPIError()
        {
            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(e =>
                e.Message == "Currency cannot be null."
                && e.Entity == "Currency"
                && e.Name == "CurrencyIsNullValidationRule");
        }
    }
}
