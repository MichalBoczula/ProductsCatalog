using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Categories
{
    [Binding]
    public class GetCategoryByIdSteps
    {
        private CategoryDto? _createdCategory;
        private HttpResponseMessage? _response;
        private HttpResponseMessage? _responseFailure;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        private Guid _categoryId;

        [Given("an existing category id")]
        public async Task GivenAnExistingCategoryId(Table table)
        {
            var values = MergeDefaultValues(table);
            var request = new CreateCategoryExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Name"));
            var response = await TestRunHooks.Client.PostAsJsonAsync("/categories", request);
            response.EnsureSuccessStatusCode();

            _createdCategory = await response.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            _createdCategory.ShouldNotBeNull();

            _categoryId = _createdCategory!.Id;
        }

        [When("I request the category by id")]
        public async Task WhenIRequestTheCategoryById()
        {
            _response = await TestRunHooks.Client.GetAsync($"/categories/{_categoryId}");
        }

        [Then("the category details are returned successfully")]
        public async Task ThenTheCategoryDetailsAreReturnedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var category = await DeserializeResponse<CategoryDto>(_response);
            category.ShouldNotBeNull();

            category.Id.ShouldBe(_categoryId);
            category.Code.ShouldBe(GetExpectedValue(expected, "Code", _createdCategory!.Code));
            category.Name.ShouldBe(GetExpectedValue(expected, "Name", _createdCategory!.Name));
            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                category.IsActive.ShouldBe(isActive);
            }
        }

        [Given("a category without specific id doesnt exists")]
        public void GivenACategoryWithoutSpecificIdDoesntExists()
        {
            _categoryId = Guid.NewGuid();
        }

        [When("I send request for category by not existed id")]
        public async Task WhenIRequestTheCategoryByNotExistedId()
        {
            _responseFailure = await TestRunHooks.Client.GetAsync($"/categories/{_categoryId}");
        }

        [Then("response show not found error")]
        public async Task ThenResponseShowNotFoundError(Table table)
        {
            var expected = ParseExpectedTable(table);
            _responseFailure.ShouldNotBeNull();
            _responseFailure!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_responseFailure);
            problem.ShouldNotBeNull();

            if (expected.TryGetValue("Title", out var title))
            {
                problem.Title.ShouldBe(title);
            }

            if (TryGetInt(expected, "Status", out var status))
            {
                problem.Status.ShouldBe(status);
            }

            if (expected.TryGetValue("Detail", out var detail))
            {
                problem.Detail.ShouldBe(ReplacePlaceholders(detail));
            }

            if (expected.TryGetValue("Instance", out var instance))
            {
                problem.Instance.ShouldBe(ReplacePlaceholders(instance));
            }

            if (expected.TryGetValue("TraceId", out var traceId))
            {
                problem.TraceId.ShouldBe(ReplacePlaceholders(traceId));
            }
            else
            {
                problem.TraceId.ShouldNotBeNullOrWhiteSpace();
            }
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        private static Dictionary<string, string> MergeDefaultValues(Table table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Code"] = $"BOOKS-{Guid.NewGuid():N}",
                ["Name"] = "Books category"
            };

            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
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
            return (HttpStatusCode)ParseRequiredInt(expected, key);
        }

        private static int ParseRequiredInt(IReadOnlyDictionary<string, string> expected, string key)
        {
            return int.Parse(GetRequiredValue(expected, key), CultureInfo.InvariantCulture);
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> expected, string key)
        {
            if (!expected.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in expected category response table.");
            }

            return value;
        }

        private static string GetExpectedValue(IReadOnlyDictionary<string, string> expected, string key, string fallback)
        {
            return expected.TryGetValue(key, out var value) ? value : fallback;
        }

        private static bool TryGetBool(IReadOnlyDictionary<string, string> expected, string key, out bool value)
        {
            value = default;
            if (!expected.TryGetValue(key, out var raw))
            {
                return false;
            }

            value = bool.Parse(raw);
            return true;
        }

        private static bool TryGetInt(IReadOnlyDictionary<string, string> expected, string key, out int value)
        {
            value = default;
            if (!expected.TryGetValue(key, out var raw))
            {
                return false;
            }

            value = int.Parse(raw, CultureInfo.InvariantCulture);
            return true;
        }

        private string ReplacePlaceholders(string value)
        {
            return value.Replace("{CategoryId}", _categoryId.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in category contract table.");
            }

            return value;
        }
    }
}
