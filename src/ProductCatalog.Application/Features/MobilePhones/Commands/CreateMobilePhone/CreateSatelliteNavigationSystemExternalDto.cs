namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateSatelliteNavigationSystemExternalDto(
        bool GPS,
        bool AGPS,
        bool Galileo,
        bool GLONASS,
        bool QZSS);
}
