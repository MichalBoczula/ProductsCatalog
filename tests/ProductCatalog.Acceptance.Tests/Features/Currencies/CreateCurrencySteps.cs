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
    public class CreateCurrencySteps
    {
        private CreateCurrencyExternalDto _currencyCorrect = default!;
        private CurrencyDto? _successResult;
        private CreateCurrencyExternalDto _currencyIncorrect = default!;
        private ApiProblemDetails? _apiProblem;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("I have valid currency details")]
        public void GivenIHaveValidCurrencyDetails(Table? table)
        {
            _currencyCorrect = BuildCurrencyRequest(table, "JPY", "Japanese Yen");

            AllureJson.AttachObject(
                "Request JSON (valid)",
                _currencyCorrect,
                _jsonOptions
            );
        }

        [When("I submit the create currency request")]
        public async Task WhenISubmitTheCreateCurrencyRequest()
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(_currencyCorrect, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);
            _response = await TestRunHooks.Client.PostAsync("/currencies", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _successResult = JsonSerializer.Deserialize<CurrencyDto>(json, _jsonOptions);
        }

        [Then("the currency is created successfully")]
        public void ThenTheCurrencyIsCreatedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _successResult.ShouldNotBeNull();
            _successResult!.Code.ShouldBe(GetExpectedValue(expected, "Code", _currencyCorrect.Code));
            _successResult.Description.ShouldBe(GetExpectedValue(expected, "Description", _currencyCorrect.Description));

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

        [Given("I have invalid currency details")]
        public void GivenIHaveInvalidCurrencyDetails(Table? table)
        {
            _currencyIncorrect = BuildCurrencyRequest(table, string.Empty, string.Empty);

            AllureJson.AttachObject(
                "Request JSON (invalid)",
                _currencyIncorrect,
                _jsonOptions
            );
        }

        [When("I submit the create invalid currency request")]
        public async Task WhenISubmitTheCreateInvalidCurrencyRequest()
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(_currencyIncorrect, _jsonOptions),
                  Encoding.UTF8,
                  MediaTypeNames.Application.Json);
            _response = await TestRunHooks.Client.PostAsync("/currencies", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the currency creation fails with API error")]
        public void ThenTheCurrencyCreationFailsWithAPIError(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe((int)_response.StatusCode);
            _apiProblem.Title.ShouldBe(GetRequiredValue(expected, "Title"));
            _apiProblem.Detail.ShouldBe(GetRequiredValue(expected, "Detail"));
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            foreach (var expectedError in ParseExpectedErrors(expected))
            {
                _apiProblem.Errors.ShouldContain(e =>
                    e.Message == expectedError.Message
                    && e.Entity == expectedError.Entity
                    && e.Name == expectedError.Name);
            }
        }

        private static CreateCurrencyExternalDto BuildCurrencyRequest(Table? table, string defaultCode, string defaultDescription)
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

        private static IReadOnlyList<(string Message, string Entity, string Name)> ParseExpectedErrors(
            IReadOnlyDictionary<string, string> values)
        {
            var errors = new List<(string Message, string Entity, string Name)>();

            if (values.ContainsKey("ErrorMessage"))
            {
                errors.Add((
                    GetRequiredValue(values, "ErrorMessage"),
                    GetRequiredValue(values, "ErrorEntity"),
                    GetRequiredValue(values, "ErrorName")));
            }

            var index = 1;
            while (values.ContainsKey($"ErrorMessage{index}"))
            {
                errors.Add((
                    GetRequiredValue(values, $"ErrorMessage{index}"),
                    GetRequiredValue(values, $"ErrorEntity{index}"),
                    GetRequiredValue(values, $"ErrorName{index}")));
                index++;
            }

            return errors;
        }
    }
}
