using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products;

[Binding]
public class RemoveProductSteps
{
    private readonly ProductScenarioContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public RemoveProductSteps(ProductScenarioContext context)
    {
        _context = context;
    }

    [Given(@"an existing active product")]
    public async Task GivenAnExistingActiveProduct()
    {
        _context.CategoryId.ShouldNotBe(Guid.Empty);
        _context.CurrencyCode.ShouldNotBeNullOrWhiteSpace();

        _context.Request = new CreateProductExternalDto(
            "Product To Remove",
            "A product to be removed via acceptance test",
            new CreateMoneyExternalDto(49.99m, _context.CurrencyCode),
            _context.CategoryId);

        var content = new StringContent(
            JsonSerializer.Serialize(_context.Request, _jsonOptions),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        var response = await TestRunHooks.Client.PostAsync("/products", content);
        response.EnsureSuccessStatusCode();

        var product = await DeserializeResponse<ProductDto>(response);
        product.ShouldNotBeNull();

        _context.ProductId = product.Id;
    }

    [When(@"I submit the remove product request")]
    public async Task WhenISubmitTheRemoveProductRequest()
    {
        _context.ProductId.ShouldNotBe(Guid.Empty);

        _context.Response = await TestRunHooks.Client.DeleteAsync($"/products/{_context.ProductId}");
    }

    [Then(@"the product is removed successfully")]
    public async Task ThenTheProductIsRemovedSuccessfully()
    {
        _context.Response.ShouldNotBeNull();
        _context.Response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var product = await DeserializeResponse<ProductDto>(_context.Response);
        product.ShouldNotBeNull();

        product.Id.ShouldBe(_context.ProductId);
        product.IsActive.ShouldBeFalse();
        product.CategoryId.ShouldBe(_context.Request!.CategoryId);
        product.Name.ShouldBe(_context.Request!.Name);
        product.Description.ShouldBe(_context.Request!.Description);
        product.Price.Amount.ShouldBe(_context.Request!.Price.Amount);
        product.Price.Currency.ShouldBe(_context.Request!.Price.Currency);
    }

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
