using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using Reqnroll;
using Shouldly;
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
        public void ThenTheCategoryListIsReturned()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            _categories.ShouldNotBeNull();
            _categories!.ShouldNotBeEmpty();
            _categories.ShouldContain(c => c.Code == "MOBILE");
        }
    }
}
