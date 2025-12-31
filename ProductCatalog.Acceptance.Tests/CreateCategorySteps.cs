using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using Reqnroll;
using Shouldly;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests
{
    [Binding]
    public class CreateCategorySteps
    {
        private CreateCategoryExternalDto _categoryCorrect = default!;
        private CategoryDto? _successResult;
        private CreateCategoryExternalDto _categoryIncorrect = default!;
        private ApiProblemDetails? _apiProblem;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("I have valid category details")]
        public void GivenIHaveValidCategoryDetails()
        {
            _categoryCorrect = new CreateCategoryExternalDto("HOME", "Home goods");
        }

        [When("I submit the create category request")]
        public async Task WhenISubmitTheCreateCategoryRequest()
        {
            var content = new StringContent(
                 JsonSerializer.Serialize(_categoryCorrect, _jsonOptions),
                 Encoding.UTF8,
                 MediaTypeNames.Application.Json);
            var response = await TestRunHooks.Client.PostAsync("/categories", content);
            var json = await response.Content.ReadAsStringAsync();
            _successResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("the category is created successfully")]
        public void ThenTheCategoryIsCreatedSuccessfully()
        {
            _successResult.ShouldNotBeNull();
            _successResult!.Id.ShouldNotBe(Guid.Empty);
            _successResult.Code.ShouldBe(_categoryCorrect.Code);
            _successResult.Name.ShouldBe(_categoryCorrect.Name);
            _successResult.IsActive.ShouldBeTrue();
        }

        [Given("I have invalid category details")]
        public void GivenIHaveInvalidCategoryDetails()
        {
            _categoryIncorrect = new CreateCategoryExternalDto(string.Empty, string.Empty);
        }

        [When("I submit the create invalid category request")]
        public async Task WhenISubmitTheCreateInvalidCategoryRequest()
        {
            var content = new StringContent(
                  JsonSerializer.Serialize(_categoryIncorrect, _jsonOptions),
                  Encoding.UTF8,
                  MediaTypeNames.Application.Json);
            var response = await TestRunHooks.Client.PostAsync("/categories", content);
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the category creation fails with API error")]
        public void ThenTheCategoryCreationFailsWithAPIError()
        {
            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(e =>
                e.Message == "Code cannot be null or whitespace."
                && e.Entity == "Category"
                && e.Name == "CategoriesCodeValidationRule");
            _apiProblem.Errors.ShouldContain(e =>
                e.Message == "Name cannot be null or whitespace."
                && e.Entity == "Category"
                && e.Name == "CategoriesNameValidationRule");
        }
    }
}
