namespace ProductCatalog.Application.Features.Common
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
