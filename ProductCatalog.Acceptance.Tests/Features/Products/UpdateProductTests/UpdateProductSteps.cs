using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Domain.Validation.Common;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Products.UpdateProductTests;

[Binding]
public class UpdateProductSteps
{
    private const string CategoryId = "587480bb-c126-4f9b-b531-b0244daa4ba4";

    private readonly HttpClient _client;
    private ProductDto? _existingProduct;
    private HttpResponseMessage? _updateResponse;
    private UpdateProductExternalDto? _updateRequest;

    public UpdateProductSteps()
    {
        _client = TestRunHooks.Client;
    }

    [Given("an existing product")]
    public async Task GivenAnExistingProduct()
    {
        var createRequest = new CreateProductExternalDto(
            "Phone",
            "Nice phone",
            new CreateMoneyExternalDto(99.99m, "USD"),
            Guid.Parse(CategoryId));

        var createResponse = await _client.PostAsJsonAsync("/products", createRequest);
        createResponse.EnsureSuccessStatusCode();

        _existingProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
        _existingProduct.ShouldNotBeNull();
    }

    [When("I update the product with:")]
    public async Task WhenIUpdateTheProductWith(Table table)
    {
        var row = table.Rows.Single();
        _updateRequest = new UpdateProductExternalDto(
            row["name"],
            row["description"],
            new UpdateMoneyExternalDto(
                decimal.Parse(row["amount"], CultureInfo.InvariantCulture),
                row["currency"]),
            Guid.Parse(CategoryId));

        _updateResponse = await _client.PutAsJsonAsync($"/products/{_existingProduct!.Id}", _updateRequest);
    }

    [Then("the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int statusCode)
    {
        _updateResponse.ShouldNotBeNull();
        ((int)_updateResponse.StatusCode).ShouldBe(statusCode);
    }

    [Then("the updated product contract matches the request")]
    public async Task ThenTheUpdatedProductContractMatchesTheRequest()
    {
        _updateResponse.ShouldNotBeNull();
        _updateResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedProduct = await _updateResponse.Content.ReadFromJsonAsync<ProductDto>();
        updatedProduct.ShouldNotBeNull();

        updatedProduct.Id.ShouldBe(_existingProduct!.Id);
        updatedProduct.Name.ShouldBe(_updateRequest!.Name);
        updatedProduct.Description.ShouldBe(_updateRequest.Description);
        updatedProduct.CategoryId.ShouldBe(_updateRequest.CategoryId);
        updatedProduct.Price.Amount.ShouldBe(_updateRequest.Price.Amount);
        updatedProduct.Price.Currency.ShouldBe(_updateRequest.Price.Currency.ToUpperInvariant());
        updatedProduct.IsActive.ShouldBeTrue();
    }

    [Then("the response should match the validation problem details contract")]
    public async Task ThenTheResponseShouldMatchTheValidationProblemDetailsContract()
    {
        _updateResponse.ShouldNotBeNull();
        _updateResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await _updateResponse.Content.ReadFromJsonAsync<JsonElement>();

        problemDetails.GetProperty("status").GetInt32().ShouldBe(StatusCodes.Status400BadRequest);
        problemDetails.GetProperty("title").GetString().ShouldBe("Validation failed");
        problemDetails.GetProperty("detail").GetString().ShouldBe("One or more validation errors occurred.");

        var errors = problemDetails.GetProperty("errors").EnumerateArray().ToList();
        errors.ShouldNotBeEmpty();
        errors.Any(error =>
                error.GetProperty(nameof(ValidationError.Name)).GetString() == "ProductsNameValidationRule")
            .ShouldBeTrue();

        problemDetails.GetProperty("traceId").GetString().ShouldNotBeNullOrWhiteSpace();
    }
}
