namespace ProductCatalog.Application.Features.Common
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
