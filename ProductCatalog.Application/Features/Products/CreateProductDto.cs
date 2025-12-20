using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductExternalDto(string Name, string Description, Money Price, Guid CategoryId);
}