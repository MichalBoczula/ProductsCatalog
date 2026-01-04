using MediatR;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    public sealed record UpdateMobilePhoneCommand(Guid mobilePhoneId, UpdateMobilePhoneExternalDto MobilePhone) : IRequest<MobilePhoneDto>;
}
