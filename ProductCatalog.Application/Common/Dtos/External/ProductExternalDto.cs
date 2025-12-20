using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Application.Common.Dtos.External
{
    public sealed record ProductExternalDto(string Name, string Description, Money Price, Guid CategoryId);
}