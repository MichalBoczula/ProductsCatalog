namespace ProductCatalog.Application.Common.Dtos.Categories
{
    internal sealed record CategoryDto
    {
        public required Guid Id { get; init; }
        public required string Code { get; init; }
        public required string Name { get; init; }
    }
}
