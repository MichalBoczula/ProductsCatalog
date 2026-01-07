using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateSatelliteNavigationSystemExternalDto(
        [property: JsonRequired] bool GPS,
        [property: JsonRequired] bool AGPS,
        [property: JsonRequired] bool Galileo,
        [property: JsonRequired] bool GLONASS,
        [property: JsonRequired] bool QZSS);
}
