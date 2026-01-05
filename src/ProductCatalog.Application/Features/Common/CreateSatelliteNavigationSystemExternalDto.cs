namespace ProductCatalog.Application.Features.Common
{
    public sealed record CreateSatelliteNavigationSystemExternalDto(
        bool GPS,
        bool AGPS,
        bool Galileo,
        bool GLONASS,
        bool QZSS);
}
