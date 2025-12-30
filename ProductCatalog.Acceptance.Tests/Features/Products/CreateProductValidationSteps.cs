using System.Text.Json;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products;

[Binding]
public class CreateProductValidationSteps
{
    private readonly ProductScenarioContext _context;

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
        using var document = JsonDocument.Parse(content);

        if (!document.RootElement.TryGetProperty("errors", out var errorsElement))
        {
            AssertFailWithContent(content);
        }

        errorsElement.ValueKind.ShouldBe(JsonValueKind.Array);
        var messages = errorsElement.EnumerateArray()
            .Select(error => TryGetStringProperty(error, "Message"))
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .ToList();

        messages.ShouldContain("CategoryId does not exist.");
    }

    private static string? TryGetStringProperty(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var property))
        {
            return property.GetString();
        }

        var matching = element.EnumerateObject()
            .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        return matching.Value.ValueKind == JsonValueKind.Undefined ? null : matching.Value.GetString();
    }

    private static void AssertFailWithContent(string content)
    {
        throw new ShouldAssertException($"Expected validation errors in response, but none were found. Response content: {content}");
    }
}
