using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone
{
    public sealed record DeleteMobilePhoneCommand(Guid mobilePhoneId) : IRequest<MobilePhoneDetailsDto>;
}
