using MediatR;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    public sealed record CreateMobilePhoneCommand(CreateMobilePhoneExternalDto mobilePhoneExternalDto) : IRequest<MobilePhoneDto>;
}
