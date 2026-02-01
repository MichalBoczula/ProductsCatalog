using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Categories
{
    [Binding]
    public class GetCategoriesSteps
    {
        private HttpResponseMessage? _response;
        private IReadOnlyList<CategoryDto>? _categories;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [When("I request the list of categories")]
        public async Task WhenIRequestTheListOfCategories()
        {
            _response = await TestRunHooks.Client.GetAsync("/categories");
            var json = await _response.Content.ReadAsStringAsync();
            _categories = JsonSerializer.Deserialize<IReadOnlyList<CategoryDto>>(json, _jsonOptions);
        }

        [Then("the category list is returned")]
        public void ThenTheCategoryListIsReturned(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _categories.ShouldNotBeNull();
            _categories!.ShouldNotBeEmpty();
            _categories.ShouldContain(c => c.Code == GetRequiredValue(expected, "Code"));
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
                throw new InvalidOperationException($"Missing '{key}' value in category expected result table.");
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
