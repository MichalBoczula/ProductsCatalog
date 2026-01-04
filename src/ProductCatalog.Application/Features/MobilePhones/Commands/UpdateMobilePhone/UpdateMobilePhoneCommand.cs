using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateMobilePhoneCommand(Guid mobilePhoneId, UpdateMobilePhoneExternalDto MobilePhone) : IRequest<MobilePhoneDto>;
}
