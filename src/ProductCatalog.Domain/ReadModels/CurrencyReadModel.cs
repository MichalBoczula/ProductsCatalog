namespace ProductCatalog.Domain.ReadModels
{
    public sealed record CurrencyReadModel
    {
        public required Guid Id { get; init; }
        public required string Code { get; init; }
        public required string Description { get; init; }
        public required bool IsActive { get; init; }
    }
}
