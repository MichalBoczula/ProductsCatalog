namespace ProductCatalog.Application.Common.Dtos.Categories
{
    public sealed record CategoryDto()
    {
        public required Guid Id { get; init; }
        public required string Code { get; init; }
        public required string Name { get; init; }
        public required bool IsActive { get; init; }
    }
}
