using ProductCatalog.Application.Features.Products.Commands.CreateProduct;

namespace ProductCatalog.Acceptance.Tests.Features.Products;

public sealed class ProductScenarioContext
{
    public string CurrencyCode { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public HttpResponseMessage? Response { get; set; }
    public CreateProductExternalDto? Request { get; set; }
}
