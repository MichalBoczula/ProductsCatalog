using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History
{
    public sealed record MobilePhonesHistory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required Guid MobilePhoneId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string MainPhoto { get; init; }
        public required string OtherPhotos { get; init; }
        public required string CPU { get; init; }
        public required string GPU { get; init; }
        public required string Ram { get; init; }
        public required string Storage { get; init; }
        public required string DisplayType { get; init; }
        public required int RefreshRateHz { get; init; }
        public required decimal ScreenSizeInches { get; init; }
        public required int Width { get; init; }
        public required int Height { get; init; }
        public required string BatteryType { get; init; }
        public required int BatteryCapacity { get; init; }
        public required bool GPS { get; init; }
        public required bool AGPS { get; init; }
        public required bool Galileo { get; init; }
        public required bool GLONASS { get; init; }
        public required bool QZSS { get; init; }
        public required bool Accelerometer { get; init; }
        public required bool Gyroscope { get; init; }
        public required bool Proximity { get; init; }
        public required bool Compass { get; init; }
        public required bool Barometer { get; init; }
        public required bool Halla { get; init; }
        public required bool AmbientLight { get; init; }
        public required bool Has5G { get; init; }
        public required bool WiFi { get; init; }
        public required bool NFC { get; init; }
        public required bool Bluetooth { get; init; }
        public required string Camera { get; init; }
        public required bool FingerPrint { get; init; }
        public required bool FaceId { get; init; }
        public required Guid CategoryId { get; init; }
        public required decimal PriceAmount { get; init; }
        public required string PriceCurrency { get; init; }
        public required string Description2 { get; init; }
        public required string Description3 { get; init; }
        public required bool IsActive { get; init; }
        public required DateTime ChangedAt { get; init; }
        public required Operation Operation { get; init; }
    }
}
