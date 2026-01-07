using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateConnectivityExternalDto(
        [property: JsonRequired] bool Has5G,
        [property: JsonRequired] bool WiFi,
        [property: JsonRequired] bool NFC,
        [property: JsonRequired] bool Bluetooth);
}
