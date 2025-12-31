using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;
using Reqnroll;
using Shouldly;
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
        public async Task GivenAnExistingCategoryId()
        {
            var request = new CreateCategoryExternalDto("BOOKS", "Books category");
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
        public async Task ThenTheCategoryDetailsAreReturnedSuccessfully()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            var category = await DeserializeResponse<CategoryDto>(_response);
            category.ShouldNotBeNull();

            category.Id.ShouldBe(_categoryId);
            category.Code.ShouldBe(_createdCategory!.Code);
            category.Name.ShouldBe(_createdCategory!.Name);
            category.IsActive.ShouldBeTrue();
        }

        [Given("a category without specific id doesnt exists")]
        public void GivenACategoryWithoutSpecificIdDoesntExists()
        {
        }

        [When("I send request for category by not existed id")]
        public async Task WhenIRequestTheCategoryByNotExistedId()
        {
            _responseFailure = await TestRunHooks.Client.GetAsync($"/categories/{_categoryId}");
        }

        [Then("response show not found error")]
        public async Task ThenResponseShowNotFoundError()
        {
            _responseFailure.ShouldNotBeNull();
            _responseFailure!.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_responseFailure);
            problem.ShouldNotBeNull();

            problem.Title.ShouldBe("Resource not found.");
            problem.Status.ShouldBe(StatusCodes.Status404NotFound);
            problem.Detail.ShouldBe($"Resource {nameof(CategoryDto)} identify by id {_categoryId} cannot be found in databese during action {nameof(GetCategoryByIdQuery)}.");
            problem.Instance.ShouldBe($"/categories/{_categoryId}");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
