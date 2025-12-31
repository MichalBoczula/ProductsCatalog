using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using Reqnroll;
using Shouldly;
using System;
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
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing category to delete")]
        public async Task GivenAnExistingCategoryToDelete()
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
        }

        [When("I submit the delete category request")]
        public async Task WhenISubmitTheDeleteCategoryRequest()
        {
            _createdCategory.ShouldNotBeNull();

            var response = await TestRunHooks.Client.DeleteAsync($"/categories/{_createdCategory!.Id}");
            var json = await response.Content.ReadAsStringAsync();
            _deleteResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("the category is deleted successfully")]
        public void ThenTheCategoryIsDeletedSuccessfully()
        {
            _deleteResult.ShouldNotBeNull();
            _deleteResult!.Id.ShouldBe(_createdCategory!.Id);
            _deleteResult.Code.ShouldBe(_categoryToCreate.Code);
            _deleteResult.Name.ShouldBe(_categoryToCreate.Name);
            _deleteResult.IsActive.ShouldBeFalse();
        }

        [Given("existing list of categories")]
        public void GivenACategoryWithNotExistingIdentifier()
        {
        }

        [When("I submit the delete category request for non existing category")]
        public async Task WhenISubmitTheDeleteCategoryRequestForNonExistingCategory()
        {
            var response = await TestRunHooks.Client.DeleteAsync($"/categories/{_missingCategoryId}");
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the category deletion fails with API error")]
        public void ThenTheCategoryDeletionFailsWithAPIError()
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
