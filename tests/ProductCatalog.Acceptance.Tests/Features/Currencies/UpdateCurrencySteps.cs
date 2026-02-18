using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Currencies
{
    [Binding]
    public class UpdateCurrencySteps
    {
        private CreateCurrencyExternalDto _currencyToCreate = default!;
        private UpdateCurrencyExternalDto _updatePayload = default!;
        private CurrencyDto? _createdCurrency;
        private CurrencyDto? _successResult;
        private ApiProblemDetails? _apiProblem;
        private Guid _missingCurrencyId;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing currency which will be updated")]
        public async Task GivenAnExistingCurrencyWhichWillBeUpdated(Table table)
        {
            _currencyToCreate = BuildCreateRequest(table);

            AllureJson.AttachObject(
                "Request JSON (create for update)",
                _currencyToCreate,
                _jsonOptions
            );

            var content = new StringContent(
                JsonSerializer.Serialize(_currencyToCreate, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PostAsync("/currencies", content);
            _response.EnsureSuccessStatusCode();
            var json = await _response.Content.ReadAsStringAsync();
            _createdCurrency = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
            _createdCurrency.ShouldNotBeNull();
        }

        [Given("I have updated currency details")]
        public void GivenIHaveUpdatedCurrencyDetails(Table table)
        {
            _updatePayload = BuildUpdateRequest(table);

            AllureJson.AttachObject(
                "Request JSON (update)",
                _updatePayload,
                _jsonOptions
            );
        }

        [When("I submit the request to update currency")]
        public async Task WhenISubmitTheRequestToUpdateCurrency()
        {
            _createdCurrency.ShouldNotBeNull();

            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PutAsync($"/currencies/{_createdCurrency!.Id}", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _successResult = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
        }

        [Then("response return succesfully updated currency")]
        public void ThenResponseReturnSuccesfullyUpdatedCurrency(Table table)
        {
            var expected = ParseExpectedTable(table);
            _successResult.ShouldNotBeNull();
            _successResult!.Id.ShouldBe(_createdCurrency!.Id);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _successResult.Code.ShouldBe(GetExpectedValue(expected, "Code", _updatePayload.Code));
            _successResult.Description.ShouldBe(GetExpectedValue(expected, "Description", _updatePayload.Description));

            if (TryGetBool(expected, "HasId", out var hasId))
            {
                if (hasId)
                {
                    _successResult.Id.ShouldNotBe(Guid.Empty);
                }
                else
                {
                    _successResult.Id.ShouldBe(Guid.Empty);
                }
            }

            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                _successResult.IsActive.ShouldBe(isActive);
            }
        }

        [Given("currency does not exist in the database")]
        public void GivenCurrencyDoesNotExistInTheDatabase()
        {
            _missingCurrencyId = Guid.NewGuid();
        }

        [When("I send a request to update the currency")]
        public async Task WhenISendARequestToUpdateTheCurrency()
        {
            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PutAsync($"/currencies/{_missingCurrencyId}", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("response returns an error indicating currency not found")]
        public void ThenResponseReturnsAnErrorIndicatingCurrencyNotFound(Table table)
        {
            var expected = ParseExpectedTable(table);
            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(ParseRequiredInt(expected, "Status"));
            _apiProblem.Title.ShouldBe(GetRequiredValue(expected, "Title"));
            _apiProblem.Detail.ShouldBe(GetRequiredValue(expected, "Detail"));
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(e =>
                e.Message == GetRequiredValue(expected, "ErrorMessage")
                && e.Entity == GetRequiredValue(expected, "ErrorEntity")
                && e.Name == GetRequiredValue(expected, "ErrorName"));
        }

        private static CreateCurrencyExternalDto BuildCreateRequest(Table table)
        {
            var values = ParseContractTable(table);
            return new CreateCurrencyExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Description"));
        }

        private static UpdateCurrencyExternalDto BuildUpdateRequest(Table table)
        {
            var values = ParseContractTable(table);
            return new UpdateCurrencyExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Description"));
        }

        private static Dictionary<string, string> ParseContractTable(Table table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in currency contract table.");
            }

            return value;
        }

        private static Dictionary<string, string> ParseExpectedTable(Table table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in currency expected result table.");
            }

            return value;
        }

        private static string GetExpectedValue(IReadOnlyDictionary<string, string> values, string key, string fallback)
        {
            return values.TryGetValue(key, out var value) ? value : fallback;
        }

        private static HttpStatusCode ParseStatusCode(IReadOnlyDictionary<string, string> values, string key)
        {
            var value = GetRequiredValue(values, key);
            return (HttpStatusCode)int.Parse(value, CultureInfo.InvariantCulture);
        }

        private static int ParseRequiredInt(IReadOnlyDictionary<string, string> values, string key)
        {
            var value = GetRequiredValue(values, key);
            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        private static bool TryGetBool(IReadOnlyDictionary<string, string> values, string key, out bool result)
        {
            if (!values.TryGetValue(key, out var value))
            {
                result = false;
                return false;
            }

            result = bool.Parse(value);
            return true;
        }
    }
}
