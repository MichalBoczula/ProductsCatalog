using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using Reqnroll;
using Shouldly;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Products;

[Binding]
public class CreateProductValidationSteps
{
    private readonly ProductScenarioContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public CreateProductValidationSteps(ProductScenarioContext context)
    {
        _context = context;
    }

    [Given(@"I have invalid product details")]
    public void GivenIHaveInvalidProductDetails()
    {
        _context.CurrencyCode.ShouldNotBeNullOrWhiteSpace();

        _context.Request = new CreateProductExternalDto(
            "Invalid Product",
            "Invalid category reference",
            new CreateMoneyExternalDto(10m, _context.CurrencyCode),
            Guid.NewGuid());
    }

    [Then(@"the product creation fails with validation errors")]
    public async Task ThenTheProductCreationFailsWithValidationErrors()
    {
        _context.Response.ShouldNotBeNull();
        _context.Response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        var content = await _context.Response.Content.ReadAsStringAsync();
        var problem = JsonSerializer.Deserialize<ApiProblemDetails>(content, _jsonOptions);

        problem.ShouldNotBeNull();

        if (problem is null)
        {
            AssertFailWithContent(content);
        }

        var errors = problem!.Errors.ToList();
        errors.ShouldNotBeEmpty();

        errors.ShouldContain(e =>
            e.Message == "CategoryId does not exist."
            && e.Entity == "Product"
            && e.Name == "ProductsCategoryIdValidationRule");
    }

    private static void AssertFailWithContent(string content)
    {
        throw new ShouldAssertException($"Expected validation errors in response, but none were found. Response content: {content}");
    }
}
