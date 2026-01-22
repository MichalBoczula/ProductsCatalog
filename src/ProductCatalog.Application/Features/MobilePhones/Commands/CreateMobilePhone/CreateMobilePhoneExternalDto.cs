using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;
using System.Text.Json.Serialization;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateMobilePhoneExternalDto(
        [property: JsonRequired] CommonDescriptionExtrernalDto CommonDescription,
        [property: JsonRequired] CreateElectronicDetailsExternalDto ElectronicDetails,
        [property: JsonRequired] CreateConnectivityExternalDto Connectivity,
        [property: JsonRequired] CreateSatelliteNavigationSystemExternalDto SatelliteNavigationSystems,
        [property: JsonRequired] CreateSensorsExternalDto Sensors,
        [property: JsonRequired] string Camera,
        [property: JsonRequired] bool FingerPrint,
        [property: JsonRequired] bool FaceId,
        [property: JsonRequired] Guid CategoryId,
        [property: JsonRequired] CreateMoneyExternalDto Price,
        [property: JsonRequired] string Description2,
        [property: JsonRequired] string Description3
    );
}
