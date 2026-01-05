namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateConnectivityExternalDto(
        bool Has5G,
        bool WiFi,
        bool NFC,
        bool Bluetooth);
}
