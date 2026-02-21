namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record MobilePhoneFilterDto
    {
        public string Brand { get; set; }
        public decimal MinimalPrice { get; set; }
        public decimal MaximalPrice { get; set; }
    }
}
