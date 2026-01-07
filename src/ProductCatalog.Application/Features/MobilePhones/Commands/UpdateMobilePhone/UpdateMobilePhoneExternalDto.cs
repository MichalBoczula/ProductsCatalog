using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;
using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateMobilePhoneExternalDto(
        [property: JsonRequired] CommonDescriptionExtrernalDto CommonDescription,
        [property: JsonRequired] UpdateElectronicDetailsExternalDto ElectronicDetails,
        [property: JsonRequired] UpdateConnectivityExternalDto Connectivity,
        [property: JsonRequired] UpdateSatelliteNavigationSystemExternalDto SatelliteNavigationSystems,
        [property: JsonRequired] UpdateSensorsExternalDto Sensors,
        bool FingerPrint,
        bool FaceId,
        Guid CategoryId,
        [property: JsonRequired] UpdateMoneyExternalDto Price);
}
