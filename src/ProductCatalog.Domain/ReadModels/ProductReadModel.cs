namespace ProductCatalog.Domain.ReadModels
{
    public sealed record ProductReadModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal PriceAmount { get; init; }
        public required string PriceCurrency { get; init; }
        public required bool IsActive { get; init; }
        public required Guid CategoryId { get; init; }
    }
}
