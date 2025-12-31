using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using Reqnroll;
using Shouldly;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Currencies
{
    [Binding]
    public class UpdateCategorySteps
    {
        private CreateCategoryExternalDto _categoryToCreate = default!;
        private UpdateCategoryExternalDto _updatePayload = default!;
        private CategoryDto? _createdCategory;
        private CategoryDto? _updateResult;
        private Guid _missingCategoryId;
        private ApiProblemDetails? _apiProblem;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing category to update")]
        public async Task GivenAnExistingCategoryToUpdate()
        {
            var code = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
            _categoryToCreate = new CreateCategoryExternalDto(code, $"{code} name");

            var content = new StringContent(
                JsonSerializer.Serialize(_categoryToCreate, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PostAsync("/categories", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            _createdCategory = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
            _createdCategory.ShouldNotBeNull();

            _updatePayload = new UpdateCategoryExternalDto($"{code}-UPD", $"{code} updated name");
        }

        [When("I submit the update category request")]
        public async Task WhenISubmitTheUpdateCategoryRequest()
        {
            _createdCategory.ShouldNotBeNull();

            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PutAsync($"/categories/{_createdCategory!.Id}", content);
            var json = await response.Content.ReadAsStringAsync();
            _updateResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("the category is updated successfully")]
        public void ThenTheCategoryIsUpdatedSuccessfully()
        {
            _updateResult.ShouldNotBeNull();
            _updateResult!.Id.ShouldBe(_createdCategory!.Id);
            _updateResult.Code.ShouldBe(_updatePayload.Code);
            _updateResult.Name.ShouldBe(_updatePayload.Name);
            _updateResult.IsActive.ShouldBeTrue();
        }

        [Given("a category id that does not exist")]
        public void GivenACategoryIdThatDoesNotExist()
        {
            _missingCategoryId = Guid.NewGuid();
            _updatePayload = new UpdateCategoryExternalDto("MISSING", "Missing category");
        }

        [When("I submit the update category request for non existing category")]
        public async Task WhenISubmitTheUpdateCategoryRequestForNonExistingCategory()
        {
            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PutAsync($"/categories/{_missingCategoryId}", content);
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the category update fails with API error")]
        public void ThenTheCategoryUpdateFailsWithAPIError()
        {
            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(e =>
                e.Message == "Category cannot be null."
                && e.Entity == "Category"
                && e.Name == "CategoryIsNullValidationRule");
        }
    }
}
