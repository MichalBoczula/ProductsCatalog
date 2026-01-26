using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateMobilePhoneCommand(CreateMobilePhoneExternalDto mobilePhoneExternalDto) : IRequest<MobilePhoneDetailsDto>;
}
