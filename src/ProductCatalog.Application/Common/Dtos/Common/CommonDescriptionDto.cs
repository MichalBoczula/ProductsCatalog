namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record CommonDescriptionDto()
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string MainPhoto { get; init; }
        public IReadOnlyList<string> OtherPhotos { get; init; }
    }
}
