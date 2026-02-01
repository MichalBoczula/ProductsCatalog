using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Categories
{
    [Binding]
    public class DeleteCategorySteps
    {
        private CreateCategoryExternalDto _categoryToCreate = default!;
        private CategoryDto? _createdCategory;
        private CategoryDto? _deleteResult;
        private Guid _missingCategoryId;
        private ApiProblemDetails? _apiProblem;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing category to delete")]
        public async Task GivenAnExistingCategoryToDelete(Table table)
        {
            _categoryToCreate = BuildCategoryRequest(table, "DELETE-CATEGORY", "Delete Category");

            var content = new StringContent(
                 JsonSerializer.Serialize(_categoryToCreate, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PostAsync("/categories", content);
            _response.EnsureSuccessStatusCode();
            var json = await _response.Content.ReadAsStringAsync();
            _createdCategory = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
            _createdCategory.ShouldNotBeNull();
        }

        [When("I submit the delete category request")]
        public async Task WhenISubmitTheDeleteCategoryRequest()
        {
            _createdCategory.ShouldNotBeNull();

            _response = await TestRunHooks.Client.DeleteAsync($"/categories/{_createdCategory!.Id}");
            var json = await _response.Content.ReadAsStringAsync();
            _deleteResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("the category is deleted successfully")]
        public void ThenTheCategoryIsDeletedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _deleteResult.ShouldNotBeNull();
            _deleteResult!.Id.ShouldBe(_createdCategory!.Id);
            _deleteResult.Code.ShouldBe(GetExpectedValue(expected, "Code", _categoryToCreate.Code));
            _deleteResult.Name.ShouldBe(GetExpectedValue(expected, "Name", _categoryToCreate.Name));

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

        [Given("a category with id that does not exist")]
        public void GivenACategoryIdThatDoesNotExist()
        {
            _missingCategoryId = Guid.NewGuid();
        }

        [When("I submit the delete category request for non existing category")]
        public async Task WhenISubmitTheDeleteCategoryRequestForNonExistingCategory()
        {
            _response = await TestRunHooks.Client.DeleteAsync($"/categories/{_missingCategoryId}");
            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the category deletion fails with API error")]
        public void ThenTheCategoryDeletionFailsWithAPIError(Table table)
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

        private static CreateCategoryExternalDto BuildCategoryRequest(Table? table, string defaultCode, string defaultName)
        {
            var values = MergeDefaultValues(table, defaultCode, defaultName);
            return new CreateCategoryExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Name"));
        }

        private static Dictionary<string, string> MergeDefaultValues(
            Table? table,
            string defaultCode,
            string defaultName)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Code"] = defaultCode,
                ["Name"] = defaultName
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
                throw new InvalidOperationException($"Missing '{key}' value in category contract table.");
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
                throw new InvalidOperationException($"Missing '{key}' value in category expected result table.");
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
