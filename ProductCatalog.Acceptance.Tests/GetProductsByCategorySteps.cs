using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests
{
    [Binding]
    public class GetProductsByCategorySteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        private Guid _otherCategoryId = Guid.Parse("9656c5c5-8ed9-46e1-a5df-025f5d7885d4");
        private Guid _categoryId;
        private string _currencyCode = "USD";
        private NotFoundProblemDetails? _notFound;
        private List<ProductDto> _result = new();

        [Given(@"an existing list of products with code ""(.*)""")]
        public async Task GivenAnExistingListOfProductsWithCode(string categoryCode)
        {
            var categoryRequest = new CreateCategoryExternalDto("Test", "Test");
                var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>();
            _categoryId = category.Id;

            var requests = new List<CreateProductExternalDto>
            {
                new("Category product 1",
                    "First product for category filter",
                    new CreateMoneyExternalDto(25.50m, _currencyCode),
                    _categoryId),
                new("Category product 2",
                    "Second product for category filter",
                    new CreateMoneyExternalDto(30.00m, _currencyCode),
                    _categoryId),
                new("Other category product",
                    "Product that should be filtered out",
                    new CreateMoneyExternalDto(15.00m, _currencyCode),
                    _otherCategoryId)
            };

            foreach (var request in requests)
            {
                var response = await TestRunHooks.Client.PostAsJsonAsync("/products", request);
                response.EnsureSuccessStatusCode();
            }
        }

        [When(@"I request products for the category ""(.*)""")]
        public async Task WhenIRequestProductsForTheCategory(string mobileCategoryId)
        {
            var response = await TestRunHooks.Client.GetAsync($"/products/categories/{_categoryId}");
            response.EnsureSuccessStatusCode();
            _result = await DeserializeResponse<List<ProductDto>>(response) ?? new List<ProductDto>();
        }

        [Then(@"only products from the category are returned")]
        public void ThenOnlyProductsFromTheCategoryAreReturned()
        {
            _result.ShouldNotBeNull();
            _result.ForEach(p => p.CategoryId.ShouldBe(_categoryId));
            _result.Count.ShouldBe(2);
        }

        [Given("an list of products with code {string}")]
        public void GivenAnListOfProductsWithCode(string mobileCategoryId)
        {
        }

        [When(@"I request tablets for the category ""(.*)""")]
        public async Task WhenIRequestTabletsForTheCategory(string tabletCategoryId)
        {
            var response = await TestRunHooks.Client.GetAsync($"/products/categories/{Guid.NewGuid()}");
            _result = await DeserializeResponse<List<ProductDto>>(response);
        }

        [Then(@"an empty product list is returned")]
        public void ThenAnEmptyProductListIsReturned()
        {
            _result.ShouldBeEmpty();
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}