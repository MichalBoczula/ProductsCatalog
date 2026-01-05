namespace ProductCatalog.Application.Features.Common
{
    public sealed record UpdateSatelliteNavigationSystemExternalDto(
        bool GPS,
        bool AGPS,
        bool Galileo,
        bool GLONASS,
        bool QZSS);
}
