using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
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
    public class UpdateCategorySteps
    {
        private CreateCategoryExternalDto _categoryToCreate = default!;
        private UpdateCategoryExternalDto _updatePayload = default!;
        private CategoryDto? _createdCategory;
        private CategoryDto? _successResult;
        private ApiProblemDetails? _apiProblem;
        private Guid _missingCategoryId;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing category which will be updated")]
        public async Task GivenAnExistingCategoryWhichWillBeUpdated(Table table)
        {
            _categoryToCreate = BuildCreateRequest(table, "UPDATE-CATEGORY", "Update Category");

            var content = new StringContent(
                JsonSerializer.Serialize(_categoryToCreate, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            AllureJson.AttachObject(
                "Request JSON (create for update)",
                _categoryToCreate,
                _jsonOptions
            );

            _response = await TestRunHooks.Client.PostAsync("/categories", content);
            _response.EnsureSuccessStatusCode();
            var json = await _response.Content.ReadAsStringAsync();
            _createdCategory = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
            _createdCategory.ShouldNotBeNull();
        }

        [Given("I have updated category details")]
        public void GivenIHaveUpdatedCategoryDetails(Table table)
        {
            _updatePayload = BuildUpdateRequest(table, "UPDATED-CATEGORY", "Updated Category");

            AllureJson.AttachObject(
                "Request JSON (update)",
                _updatePayload,
                _jsonOptions
            );
        }

        [When("I submit the request to category update category")]
        public async Task WhenISubmitTheRequestToCategoryUpdateCategory()
        {
            _createdCategory.ShouldNotBeNull();

            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PutAsync($"/categories/{_createdCategory!.Id}", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _successResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("response return succesfully updated category")]
        public void ThenResponseReturnSuccesfullyUpdatedCategory(Table table)
        {
            var expected = ParseExpectedTable(table);
            _successResult.ShouldNotBeNull();
            _successResult!.Id.ShouldBe(_createdCategory!.Id);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _successResult.Code.ShouldBe(GetExpectedValue(expected, "Code", _updatePayload.Code));
            _successResult.Name.ShouldBe(GetExpectedValue(expected, "Name", _updatePayload.Name));

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

        [Given("Category does not exist in the database")]
        public void GivenCategoryDoesNotExistInTheDatabase()
        {
            _missingCategoryId = Guid.NewGuid();
        }

        [When("I send a request to update the category")]
        public async Task WhenISendARequestToUpdateTheCategory()
        {
            var content = new StringContent(
              JsonSerializer.Serialize(_updatePayload, _jsonOptions),
              Encoding.UTF8,
              MediaTypeNames.Application.Json);

            _response = await TestRunHooks.Client.PutAsync($"/categories/{_missingCategoryId}", content);
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("response returns an error indicating category not found")]
        public void ThenResponseReturnsAnErrorIndicatingCategoryNotFound(Table table)
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

        private static CreateCategoryExternalDto BuildCreateRequest(Table? table, string defaultCode, string defaultName)
        {
            var values = MergeDefaultValues(table, defaultCode, defaultName);
            return new CreateCategoryExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Name"));
        }

        private static UpdateCategoryExternalDto BuildUpdateRequest(Table? table, string defaultCode, string defaultName)
        {
            var values = MergeDefaultValues(table, defaultCode, defaultName);
            return new UpdateCategoryExternalDto(
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
