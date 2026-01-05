namespace ProductCatalog.Application.Features.Common
{
    public sealed record CreateConnectivityExternalDto(
        bool Has5G,
        bool WiFi,
        bool NFC,
        bool Bluetooth);
}
