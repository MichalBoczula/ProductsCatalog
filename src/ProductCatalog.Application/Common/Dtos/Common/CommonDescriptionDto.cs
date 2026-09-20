namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record CommonDescriptionDto()
    {
        public string Name { get; init; } = null!;
        public string Brand { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string MainPhoto { get; init; } = null!;
        public IReadOnlyList<string> OtherPhotos { get; init; } = [];
    }
}
