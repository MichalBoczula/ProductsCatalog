using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.History
{
    public sealed record ProductsHistory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required Guid ProductId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal PriceAmount { get; init; }
        public required string PriceCurrency { get; init; }
        public required bool IsActive { get; init; }
        public required Guid CategoryId { get; init; }
        public required DateTime ChangedAt { get; init; }
        public required Operation Operation { get; init; }
    }
}
