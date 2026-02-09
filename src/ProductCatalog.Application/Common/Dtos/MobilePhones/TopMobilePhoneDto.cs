using ProductCatalog.Application.Common.Dtos.Common;

namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record TopMobilePhoneDto
    {
        public required TopMobilePhonesCommonDescriptionDto CommonDescription { get; init; }
        public required MoneyDto Price { get; init; }
        public required Guid Id { get; init; }
    }
}
