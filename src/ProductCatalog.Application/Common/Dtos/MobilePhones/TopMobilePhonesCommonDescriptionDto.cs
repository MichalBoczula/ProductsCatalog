namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record TopMobilePhonesCommonDescriptionDto
    {
        public string Name { get; init; } = null!;
        public string Brand { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string MainPhoto { get; init; } = null!;
    }
}
