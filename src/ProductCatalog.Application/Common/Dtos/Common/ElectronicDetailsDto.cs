namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record ElectronicDetailsDto
    {
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
    }
}
