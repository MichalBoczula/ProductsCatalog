namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateSatelliteNavigationSystemExternalDto(
        bool GPS,
        bool AGPS,
        bool Galileo,
        bool GLONASS,
        bool QZSS);
}
