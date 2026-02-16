using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
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
    public class DeleteCurrencySteps
    {
        private CreateCurrencyExternalDto _currencyToCreate = default!;
        private CurrencyDto? _createdCurrency;
        private CurrencyDto? _deleteResult;
        private Guid _missingCurrencyId;
        private ApiProblemDetails? _apiProblem;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing currency to delete")]
        public async Task GivenAnExistingCurrencyToDelete(Table table)
        {
            _currencyToCreate = BuildCurrencyRequest(table, "DEL", "Delete Currency");

            AllureJson.AttachObject(
                "Request JSON (create for delete)",
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

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _createdCurrency = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
            _createdCurrency.ShouldNotBeNull();
        }

        [When("I submit the delete currency request")]
        public async Task WhenISubmitTheDeleteCurrencyRequest()
        {
            _createdCurrency.ShouldNotBeNull();

            var deleteRequest = new
            {
                CurrencyId = _createdCurrency!.Id
            };

            AllureJson.AttachObject(
                "Request JSON (delete)",
                deleteRequest,
                _jsonOptions
            );

            _response = await TestRunHooks.Client.DeleteAsync($"/currencies/{_createdCurrency!.Id}");
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _deleteResult = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
        }

        [Then("the currency is deleted successfully")]
        public void ThenTheCurrencyIsDeletedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _deleteResult.ShouldNotBeNull();
            _deleteResult!.Id.ShouldBe(_createdCurrency!.Id);
            _deleteResult.Code.ShouldBe(GetExpectedValue(expected, "Code", _currencyToCreate.Code));
            _deleteResult.Description.ShouldBe(GetExpectedValue(expected, "Description", _currencyToCreate.Description));

            if (TryGetBool(expected, "HasId", out var hasId))
            {
                if (hasId)
                {
                    _deleteResult.Id.ShouldNotBe(Guid.Empty);
                }
                else
                {
                    _deleteResult.Id.ShouldBe(Guid.Empty);
                }
            }

            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                _deleteResult.IsActive.ShouldBe(isActive);
            }
        }

        [Given("a currency id that does not exist")]
        public void GivenACurrencyIdThatDoesNotExist()
        {
            _missingCurrencyId = Guid.NewGuid();
        }

        [When("I submit the delete currency request for non existing currency")]
        public async Task WhenISubmitTheDeleteCurrencyRequestForNonExistingCurrency()
        {
            var deleteRequest = new
            {
                CurrencyId = _missingCurrencyId
            };

            AllureJson.AttachObject(
                "Request JSON (delete missing)",
                deleteRequest,
                _jsonOptions
            );

            _response = await TestRunHooks.Client.DeleteAsync($"/currencies/{_missingCurrencyId}");
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the currency deletion fails with API error")]
        public void ThenTheCurrencyDeletionFailsWithAPIError(Table table)
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

        private static CreateCurrencyExternalDto BuildCurrencyRequest(
            Table? table,
            string defaultCode,
            string defaultDescription)
        {
            var values = MergeDefaultValues(table, defaultCode, defaultDescription);
            return new CreateCurrencyExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Description"));
        }

        private static Dictionary<string, string> MergeDefaultValues(
            Table? table,
            string defaultCode,
            string defaultDescription)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Code"] = defaultCode,
                ["Description"] = defaultDescription
            };

            if (table is null)
            {
                return values;
            }

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
