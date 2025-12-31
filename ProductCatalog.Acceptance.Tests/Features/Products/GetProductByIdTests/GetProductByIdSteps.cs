using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Acceptance.Tests.Features.Products.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Products.GetProductByIdTests
{
    [Binding]
    public class GetProductByIdSteps
    {
        private readonly ProductScenarioContext _context;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public GetProductByIdSteps(ProductScenarioContext context)
        {
            _context = context;
        }

        [Given(@"an active product")]
        public async Task GivenAnActiveProduct()
        {
            _context.CategoryId.ShouldNotBe(Guid.Empty);
            _context.CurrencyCode.ShouldNotBeNullOrWhiteSpace();

            _context.Request = new CreateProductExternalDto(
                "Product for get by id",
                "A product created for get by id scenario",
                new CreateMoneyExternalDto(123.45m, _context.CurrencyCode),
                _context.CategoryId);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/products", _context.Request);
            response.EnsureSuccessStatusCode();

            var created = await DeserializeResponse<ProductDto>(response);
            created.ShouldNotBeNull();

            _context.ProductId = created.Id;
        }

        [When(@"I request the product by id")]
        public async Task WhenIRequestTheProductById()
        {
            _context.ProductId.ShouldNotBe(Guid.Empty);

            _context.Response = await TestRunHooks.Client.GetAsync($"/products/{_context.ProductId}");
        }

        [Given(@"a non existing product id")]
        public void GivenANonExistingProductId()
        {
            _context.ProductId = Guid.NewGuid();
        }

        [Then(@"the product details are returned successfully")]
        public async Task ThenTheProductDetailsAreReturnedSuccessfully()
        {
            _context.Response.ShouldNotBeNull();
            _context.Response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var product = await DeserializeResponse<ProductDto>(_context.Response);
            product.ShouldNotBeNull();

            product.Id.ShouldBe(_context.ProductId);
            product.Name.ShouldBe(_context.Request!.Name);
            product.Description.ShouldBe(_context.Request.Description);
            product.CategoryId.ShouldBe(_context.Request.CategoryId);
            product.Price.Amount.ShouldBe(_context.Request.Price.Amount);
            product.Price.Currency.ShouldBe(_context.Request.Price.Currency);
        }

        [Then(@"a resource not found problem details is returned")]
        public async Task ThenAResourceNotFoundProblemDetailsIsReturned()
        {
            _context.Response.ShouldNotBeNull();
            _context.Response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_context.Response);
            problem.ShouldNotBeNull();

            problem.Title.ShouldBe("Resource not found.");
            problem.Status.ShouldBe(StatusCodes.Status404NotFound);
            problem.Detail.ShouldBe($"Resource {nameof(ProductDto)} identify by id {_context.ProductId} cannot be found in databese during action {nameof(GetProductByIdQuery)}.");
            problem.Instance.ShouldBe($"/products/{_context.ProductId}");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
