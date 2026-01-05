namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateConnectivityExternalDto(
        bool Has5G,
        bool WiFi,
        bool NFC,
        bool Bluetooth);
}
