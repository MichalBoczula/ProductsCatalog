using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateMobilePhoneExternalDto(
        CommonDescriptionExtrernalDto CommonDescription,
        UpdateElectronicDetailsExternalDto ElectronicDetails,
        UpdateConnectivityExternalDto Connectivity,
        UpdateSatelliteNavigationSystemExternalDto SatelliteNavigationSystems,
        UpdateSensorsExternalDto Sensors,
        bool FingerPrint,
        bool FaceId,
        Guid CategoryId,
        UpdateMoneyExternalDto Price);
}
