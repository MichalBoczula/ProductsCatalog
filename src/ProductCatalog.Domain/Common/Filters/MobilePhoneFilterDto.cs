namespace ProductCatalog.Domain.Common.Filters
{
    public sealed record MobilePhoneFilterDto
    {
        public string? Brand { get; set; }
        public decimal? MinimalPrice { get; set; }
        public decimal? MaximalPrice { get; set; }
    }
}
