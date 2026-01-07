using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateSatelliteNavigationSystemExternalDto(
        [property: JsonRequired] bool GPS,
        [property: JsonRequired] bool AGPS,
        [property: JsonRequired] bool Galileo,
        [property: JsonRequired] bool GLONASS,
        [property: JsonRequired] bool QZSS);
}
