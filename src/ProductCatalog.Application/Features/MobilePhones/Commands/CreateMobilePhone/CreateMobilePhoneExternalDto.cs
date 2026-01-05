using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateMobilePhoneExternalDto(
        CommonDescriptionExtrernalDto CommonDescription,
        CreateElectronicDetailsExternalDto ElectronicDetails,
        CreateConnectivityExternalDto Connectivity,
        bool FingerPrint,
        bool FaceId,
        Guid CategoryId,
        CreateMoneyExternalDto Price
    );
}
