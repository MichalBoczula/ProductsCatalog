using Mapster;
using MediatR;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    internal class CreateMobilePhoneCommandHandler(
        IMobilePhonesCommandsRepository _mobilePhonesCommandsRepository
        ) : IRequestHandler<CreateMobilePhoneCommand, MobilePhoneDto>
    {
        public async Task<MobilePhoneDto> Handle(CreateMobilePhoneCommand request, CancellationToken cancellationToken)
        {
            var product = request.mobilePhoneExternalDto.Adapt<MobilePhone>();
            _mobilePhonesCommandsRepository.Add(product);
            await _mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
            var result = product.Adapt<MobilePhoneDto>();
            return result;
        }
    }
}