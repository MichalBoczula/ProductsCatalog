using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History
{
    public sealed record CurrenciesHistory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required Guid CurrencyId { get; init; }
        public required string Code { get; init; }
        public required string Description { get; init; }
        public required bool IsActive { get; init; }
        public required DateTime ChangedAt { get; init; }
        public required Operation Operation { get; init; }
    }
}
