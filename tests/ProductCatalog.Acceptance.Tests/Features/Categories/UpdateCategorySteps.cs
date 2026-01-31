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
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing category which will be updated")]
        public async Task GivenAnExistingCategoryWhichWillBeUpdated()
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

        [When("I submit the request to category update category")]
        public async Task WhenISubmitTheRequestToCategoryUpdateCategory()
        {
            _createdCategory.ShouldNotBeNull();

            var content = new StringContent(
                JsonSerializer.Serialize(_updatePayload, _jsonOptions),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PutAsync($"/categories/{_createdCategory!.Id}", content);
            var json = await response.Content.ReadAsStringAsync();
            _successResult = JsonSerializer.Deserialize<CategoryDto>(json, _jsonOptions);
        }

        [Then("response return succesfully updated category")]
        public void ThenResponseReturnSuccesfullyUpdatedCategory()
        {
            _successResult.ShouldNotBeNull();
            _successResult!.Id.ShouldBe(_createdCategory!.Id);
            _successResult.Code.ShouldBe(_updatePayload.Code);
            _successResult.Name.ShouldBe(_updatePayload.Name);
            _successResult.IsActive.ShouldBeTrue();
        }

        [Given("Category does not exist in the database")]
        public void GivenCategoryDoesNotExistInTheDatabase()
        {
            _missingCategoryId = Guid.NewGuid();
            _updatePayload = new UpdateCategoryExternalDto("MISSING", "Missing category");
        }

        [When("I send a request to update the category")]
        public async Task WhenISendARequestToUpdateTheCategory()
        {
            var content = new StringContent(
              JsonSerializer.Serialize(_updatePayload, _jsonOptions),
              Encoding.UTF8,
              MediaTypeNames.Application.Json);

            var response = await TestRunHooks.Client.PutAsync($"/categories/{_missingCategoryId}", content);
            var json = await response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("response returns an error indicating category not found")]
        public void ThenResponseReturnsAnErrorIndicatingCategoryNotFound()
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
