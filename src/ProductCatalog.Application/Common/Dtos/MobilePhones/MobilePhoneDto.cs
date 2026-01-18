using ProductCatalog.Application.Common.Dtos.Common;

namespace ProductCatalog.Application.Common.Dtos.MobilePhones
{
    public sealed record MobilePhoneDto
    {
        public required CommonDescriptionDto CommonDescription { get; init; }
        public required ElectronicDetailsDto ElectronicDetails { get; init; }
        public required ConnectivityDto Connectivity { get; init; }
        public required SatelliteNavigationSystemDto SatelliteNavigationSystems { get; init; }
        public required SensorsDto Sensors { get; init; }
        public required string Camera { get; init; }
        public required MoneyDto Price { get; init; }
        public required bool FingerPrint { get; init; }
        public required bool FaceId { get; init; }
        public required Guid CategoryId { get; init; }
        public required Guid Id { get; init; }
        public required bool IsActive { get; init; }
    }
}
