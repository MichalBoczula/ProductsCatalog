namespace ProductCatalog.Application.Common.Dtos.Common
{
    public sealed record ConnectivityDto
    {
        public required bool Has5G { get; init; }
        public required bool WiFi { get; init; }
        public required bool NFC { get; init; }
        public required bool Bluetooth { get; init; }
    }
}
