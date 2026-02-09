using ProductCatalog.Application.Common.Dtos.Common;

namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record class TopMobilePhoneDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public required string DisplayType { get; init; }
        public required decimal ScreenSizeInches { get; init; }
        public required string Camera { get; init; }
        public required MoneyDto Price { get; init; }
    }
}
