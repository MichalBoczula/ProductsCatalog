using ProductCatalog.Application.Common.Dtos.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record MobilePhoneDto
    {
        public CommonDescriptionDto CommonDescription { get; init; }
        public MoneyDto Price { get; init; }
        public bool FingerPrint { get; init; }
        public bool FaceId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid Id { get; init; }
    }
}