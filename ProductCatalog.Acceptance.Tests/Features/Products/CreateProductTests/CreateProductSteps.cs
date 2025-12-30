using System.Net.Mime;
using System.Text;
using System.Text.Json;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Acceptance.Tests.Features.Products.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products.CreateProductTests;

[Binding]
public class CreateProductSteps
{
    private readonly ProductScenarioContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public CreateProductSteps(ProductScenarioContext context)
    {
        _context = context;
    }

    [Given(@"an existing category with code ""(.*)""")]
    public async Task GivenAnExistingCategoryWithCode(string code)
    {
        var existingCategory = await FindCategoryByCode(code);
        if (existingCategory is not null)
        {
            _context.CategoryId = existingCategory.Id;
            return;
        }

        var payload = new CreateCategoryExternalDto(code, $"{code} name");
        var content = new StringContent(JsonSerializer.Serialize(payload, _jsonOptions),
                                        Encoding.UTF8,
                                        MediaTypeNames.Application.Json);

        var response = await TestRunHooks.Client.PostAsync("/categories", content);
        response.EnsureSuccessStatusCode();

        var category = await DeserializeResponse<CategoryDto>(response);
        category.ShouldNotBeNull();
        category.Code.ShouldBe(code);

        _context.CategoryId = category.Id;
    }

    [Given(@"I have valid product details")]
    public void GivenIHaveValidProductDetails()
    {
        _context.CategoryId.ShouldNotBe(Guid.Empty);
        _context.CurrencyCode.ShouldNotBeNullOrWhiteSpace();

        _context.Request = new CreateProductExternalDto(
            "Test Product",
            "A product created via acceptance test",
            new CreateMoneyExternalDto(199.99m, _context.CurrencyCode),
            _context.CategoryId);
    }

    [Then(@"the product is created successfully")]
    public async Task ThenTheProductIsCreatedSuccessfully()
    {
        _context.Response.ShouldNotBeNull();
        _context.Response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        var product = await DeserializeResponse<ProductDto>(_context.Response);
        product.ShouldNotBeNull();
        product.Id.ShouldNotBe(Guid.Empty);
        product.Name.ShouldBe(_context.Request!.Name);
        product.Description.ShouldBe(_context.Request.Description);
        product.CategoryId.ShouldBe(_context.Request.CategoryId);
        product.Price.Amount.ShouldBe(_context.Request.Price.Amount);
        product.Price.Currency.ShouldBe(_context.Request.Price.Currency);
    }

    private async Task<CategoryDto?> FindCategoryByCode(string code)
    {
        var response = await TestRunHooks.Client.GetAsync("/categories");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var categories = await DeserializeResponse<IReadOnlyList<CategoryDto>>(response);
        return categories?.FirstOrDefault(x => x.Code == code);
    }

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}
