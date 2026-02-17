using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
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
            AllureJson.AttachObject(
                "Request JSON (get currencies)",
                new { Path = "/currencies" },
                _jsonOptions
            );

            _response = await TestRunHooks.Client.GetAsync("/currencies");

            var json = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _currencies = JsonSerializer.Deserialize<IReadOnlyList<CurrencyDto>>(json, _jsonOptions);
        }

        [Then("the currency list is returned")]
        public void ThenTheCurrencyListIsReturned(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _currencies.ShouldNotBeNull();
            _currencies!.ShouldNotBeEmpty();
            _currencies.ShouldContain(c => c.Code == GetRequiredValue(expected, "Code"));
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

        private static HttpStatusCode ParseStatusCode(IReadOnlyDictionary<string, string> values, string key)
        {
            var value = GetRequiredValue(values, key);
            return (HttpStatusCode)int.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
