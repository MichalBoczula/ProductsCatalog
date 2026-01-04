using MediatR;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone
{
    public sealed record DeleteMobilePhoneCommand(Guid mobilePhoneId) : IRequest<MobilePhoneDto>;
}
