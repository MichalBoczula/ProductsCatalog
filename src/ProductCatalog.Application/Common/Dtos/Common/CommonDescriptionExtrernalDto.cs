namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record CommonDescriptionExtrernalDto(
        string Name,
        string Description,
        string MainPhoto,
        IReadOnlyList<string> OtherPhotos);
}
