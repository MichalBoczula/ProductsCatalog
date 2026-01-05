namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel
{
    public sealed record MobilePhoneReadModel
    {
        public required Guid Id { get; init; }
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
        public required bool FingerPrint { get; init; }
        public required bool FaceId { get; init; }
        public required Guid CategoryId { get; init; }
        public required decimal PriceAmount { get; init; }
        public required string PriceCurrency { get; init; }
        public required bool IsActive { get; init; }
    }
}
