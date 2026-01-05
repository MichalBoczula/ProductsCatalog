namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record SatelliteNavigationSystemDto
    {
        public required bool GPS { get; init; }
        public required bool AGPS { get; init; }
        public required bool Galileo { get; init; }
        public required bool GLONASS { get; init; }
        public required bool QZSS { get; init; }
    }
}
