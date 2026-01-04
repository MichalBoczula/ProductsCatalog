using ProductCatalog.Application.Common.Dtos.Common;

namespace ProductCatalog.Application.Common.Dtos.Products
{
    public sealed record ProductDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required MoneyDto Price { get; init; }
        public required bool IsActive { get; init; }
        public required Guid CategoryId { get; init; }
    }
}
