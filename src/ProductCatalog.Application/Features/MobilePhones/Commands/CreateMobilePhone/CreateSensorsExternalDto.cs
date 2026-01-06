using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateSensorsExternalDto(
        [property: JsonRequired] bool Accelerometer,
        [property: JsonRequired] bool Gyroscope,
        [property: JsonRequired] bool Proximity,
        [property: JsonRequired] bool Compass,
        [property: JsonRequired] bool Barometer,
        [property: JsonRequired] bool Halla,
        [property: JsonRequired] bool AmbientLight);
}
