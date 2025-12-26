using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History
{
    public sealed record CategoriesHistory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required Guid CategoryId { get; init; }
        public required string Code { get; init; }
        public required string Name { get; init; }
        public required bool IsActive { get; init; }
        public required DateTime ChangedAt { get; init; }
        public required Operation Operation { get; init; }
    }
}
