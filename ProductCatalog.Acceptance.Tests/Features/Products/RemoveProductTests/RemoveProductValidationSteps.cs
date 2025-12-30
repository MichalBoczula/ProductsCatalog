using ProductCatalog.Acceptance.Tests.Features.Products.Common;
using ProductCatalog.Api.Configuration.Common;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Products.RemoveProductTests;

[Binding]
public class RemoveProductValidationSteps
{
    private readonly ProductScenarioContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public RemoveProductValidationSteps(ProductScenarioContext context)
    {
        _context = context;
    }

    [Given(@"a product id that does not exist")]
    public void GivenAProductIdThatDoesNotExist()
    {
        _context.ProductId = Guid.NewGuid();
    }

    [Then(@"the product removal fails with validation errors")]
    public async Task ThenTheProductRemovalFailsWithValidationErrors()
    {
        _context.Response.ShouldNotBeNull();
        _context.Response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

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
            e.Message == "Product cannot be null."
            && e.Entity == "Product"
            && e.Name == "ProductsIsNullValidationRule");
    }

    private static void AssertFailWithContent(string content)
    {
        throw new ShouldAssertException($"Expected validation errors in response, but none were found. Response content: {content}");
    }
}
