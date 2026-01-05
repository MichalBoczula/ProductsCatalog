namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record SensorsDto
    {
        public required bool Accelerometer { get; init; }
        public required bool Gyroscope { get; init; }
        public required bool Proximity { get; init; }
        public required bool Compass { get; init; }
        public required bool Barometer { get; init; }
        public required bool Halla { get; init; }
        public required bool AmbientLight { get; init; }
    }
}
