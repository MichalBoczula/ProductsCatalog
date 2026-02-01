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
    public class CreateCategorySteps
    {
        private CreateCategoryExternalDto _categoryCorrect = default!;
        private CategoryDto? _successResult;
        private CreateCategoryExternalDto _categoryIncorrect = default!;
        private ApiProblemDetails? _apiProblem;
        private HttpResponseMessage? _response;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("I have valid category details")]
        public void GivenIHaveValidCategoryDetails(Table? table)
        {
            _categoryCorrect = BuildCategoryRequest(table, "HOME", "Home goods");
        }

        [When("I submit the create category request")]
        public async Task WhenISubmitTheCreateCategoryRequest()
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(_categoryCorrect, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);
            _response = await TestRunHooks.Client.PostAsync("/categories", content);
            var json = await _response.Content.ReadAsStringAsync();
            _successResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("the category is created successfully")]
        public void ThenTheCategoryIsCreatedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));
            _successResult.ShouldNotBeNull();
            _successResult!.Code.ShouldBe(GetExpectedValue(expected, "Code", _categoryCorrect.Code));
            _successResult.Name.ShouldBe(GetExpectedValue(expected, "Name", _categoryCorrect.Name));

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

        [Given("I have invalid category details")]
        public void GivenIHaveInvalidCategoryDetails(Table? table)
        {
            _categoryIncorrect = BuildCategoryRequest(table, string.Empty, string.Empty);
        }

        [When("I submit the create invalid category request")]
        public async Task WhenISubmitTheCreateInvalidCategoryRequest()
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(_categoryIncorrect, _jsonOptions),
                  Encoding.UTF8,
                  MediaTypeNames.Application.Json);
            _response = await TestRunHooks.Client.PostAsync("/categories", content);
            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the category creation fails with API error")]
        public void ThenTheCategoryCreationFailsWithAPIError(Table table)
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

        private static CreateCategoryExternalDto BuildCategoryRequest(Table? table, string defaultCode, string defaultName)
        {
            var values = MergeDefaultValues(table, defaultCode, defaultName);
            return new CreateCategoryExternalDto(
                GetValue(values, "Code"),
                GetValue(values, "Name"));
        }

        private static Dictionary<string, string> MergeDefaultValues(Table? table, string defaultCode, string defaultName)
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
