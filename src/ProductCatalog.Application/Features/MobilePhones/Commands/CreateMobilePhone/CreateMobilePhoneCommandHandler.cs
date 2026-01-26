using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    internal class CreateMobilePhoneCommandHandler(
        IMobilePhonesCommandsRepository _mobilePhonesCommandsRepository,
        IValidationPolicy<MobilePhone> _validationPolicy,
        CreateMobilePhoneCommandFlowDescribtor _createMobilePhoneCommandFlowDescribtor
        ) : IRequestHandler<CreateMobilePhoneCommand, MobilePhoneDetailsDto>
    {
        public async Task<MobilePhoneDetailsDto> Handle(CreateMobilePhoneCommand request, CancellationToken cancellationToken)
        {
            var mobilePhone = _createMobilePhoneCommandFlowDescribtor.MapRequestToMobilePhoneAggregate(request);
            var validationResult = await _createMobilePhoneCommandFlowDescribtor
                .ValidateMobilePhoneAggregate(mobilePhone, _validationPolicy);
            _createMobilePhoneCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _createMobilePhoneCommandFlowDescribtor.AddMobilePhoneToRepository(mobilePhone, _mobilePhonesCommandsRepository);
            var mobilePhonesHistory = _createMobilePhoneCommandFlowDescribtor.CreateMobilePhoneHistoryEntry(mobilePhone);
            _createMobilePhoneCommandFlowDescribtor.WriteHistoryToRepository(_mobilePhonesCommandsRepository, mobilePhonesHistory);
            await _createMobilePhoneCommandFlowDescribtor.SaveChanges(_mobilePhonesCommandsRepository, cancellationToken);
            return _createMobilePhoneCommandFlowDescribtor.MapMobilePhoneToMobilePhoneDto(mobilePhone);
        }
    }
}
