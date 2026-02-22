using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Domain.Common.Filters
{
    public sealed record MobilePhoneFilterDto
    {
        public MobilePhonesBrand? Brand { get; set; }
        public decimal? MinimalPrice { get; set; }
        public decimal? MaximalPrice { get; set; }
    }
}
