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
        [property: JsonRequired] string Camera,
        [property: JsonRequired] bool FingerPrint,
        [property: JsonRequired] bool FaceId,
        [property: JsonRequired] Guid CategoryId,
        [property: JsonRequired] UpdateMoneyExternalDto Price,
        [property: JsonRequired] string Description2,
        [property: JsonRequired] string Description3);
}
