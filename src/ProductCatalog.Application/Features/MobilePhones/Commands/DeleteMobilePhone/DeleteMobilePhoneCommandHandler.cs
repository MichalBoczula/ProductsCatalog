using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone
{
    internal sealed class DeleteMobilePhoneCommandHandler(
        IMobilePhonesCommandsRepository _mobilePhonesCommandsRepository,
        IValidationPolicy<MobilePhone> _validationPolicy,
        DeleteMobilePhoneCommandFlowDescribtor _deleteMobilePhoneCommandFlowDescribtor)
        : IRequestHandler<DeleteMobilePhoneCommand, MobilePhoneDetailsDto>
    {
        public async Task<MobilePhoneDetailsDto> Handle(DeleteMobilePhoneCommand request, CancellationToken cancellationToken)
        {
            var mobilePhone = await _deleteMobilePhoneCommandFlowDescribtor
                .LoadMobilePhone(request.mobilePhoneId, _mobilePhonesCommandsRepository, cancellationToken);

            var validationResult = await _deleteMobilePhoneCommandFlowDescribtor
                .ValidateMobilePhone(mobilePhone, _validationPolicy);
            _deleteMobilePhoneCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _deleteMobilePhoneCommandFlowDescribtor.DeactivateMobilePhone(mobilePhone);
            _deleteMobilePhoneCommandFlowDescribtor.UpdateMobilePhoneInRepository(mobilePhone, _mobilePhonesCommandsRepository);

            var mobilePhonesHistory = _deleteMobilePhoneCommandFlowDescribtor.CreateMobilePhoneHistoryEntry(mobilePhone);
            _deleteMobilePhoneCommandFlowDescribtor.WriteHistoryToRepository(_mobilePhonesCommandsRepository, mobilePhonesHistory);

            await _deleteMobilePhoneCommandFlowDescribtor.SaveChanges(_mobilePhonesCommandsRepository, cancellationToken);
            return _deleteMobilePhoneCommandFlowDescribtor.MapMobilePhoneToMobilePhoneDto(mobilePhone);
        }
    }
}
