namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateSensorsExternalDto(
        bool Accelerometer,
        bool Gyroscope,
        bool Proximity,
        bool Compass,
        bool Barometer,
        bool Halla,
        bool AmbientLight);
}
