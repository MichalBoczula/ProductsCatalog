namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateSensorsExternalDto(
        bool Accelerometer,
        bool Gyroscope,
        bool Proximity,
        bool Compass,
        bool Barometer,
        bool Halla,
        bool AmbientLight);
}
