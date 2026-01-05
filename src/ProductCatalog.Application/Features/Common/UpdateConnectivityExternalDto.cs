namespace ProductCatalog.Application.Features.Common
{
    public sealed record UpdateConnectivityExternalDto(
        bool Has5G,
        bool WiFi,
        bool NFC,
        bool Bluetooth);
}
