namespace ProductCatalog.Domain.Common.Filters
{
    public sealed record MobilePhoneReadFilterDto
    {
        public string? BrandName { get; init; }
        public decimal? MinimalPrice { get; init; }
        public decimal? MaximalPrice { get; init; }
    }
}
