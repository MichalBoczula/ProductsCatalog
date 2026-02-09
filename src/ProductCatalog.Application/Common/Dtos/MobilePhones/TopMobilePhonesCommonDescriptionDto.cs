namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record TopMobilePhonesCommonDescriptionDto
    {
        public string Name { get; init; }
        public string Brand { get; init; }
        public string Description { get; init; }
        public string MainPhoto { get; init; }
    }
}
