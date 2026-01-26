using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    internal sealed class UpdateMobilePhoneCommandHandler(
        IMobilePhonesCommandsRepository _mobilePhonesCommandsRepository,
        IValidationPolicy<MobilePhone> _validationPolicy,
        UpdateMobilePhoneCommandFlowDescribtor _updateMobilePhoneCommandFlowDescribtor)
        : IRequestHandler<UpdateMobilePhoneCommand, MobilePhoneDetailsDto>
    {
        public async Task<MobilePhoneDetailsDto> Handle(UpdateMobilePhoneCommand request, CancellationToken cancellationToken)
        {
            var incoming = _updateMobilePhoneCommandFlowDescribtor.MapRequestToMobilePhoneAggregate(request);
            var validationResultIncoming = await _updateMobilePhoneCommandFlowDescribtor
                .ValidateIncomingMobilePhone(incoming, _validationPolicy);
            _updateMobilePhoneCommandFlowDescribtor.ThrowValidationExceptionIfIncomingInvalid(validationResultIncoming);

            var mobilePhone = await _updateMobilePhoneCommandFlowDescribtor
                .LoadExistingMobilePhone(request.mobilePhoneId, _mobilePhonesCommandsRepository, cancellationToken);

            var validationResultExisting = await _updateMobilePhoneCommandFlowDescribtor
                .ValidateExistingMobilePhone(mobilePhone, _validationPolicy);
            _updateMobilePhoneCommandFlowDescribtor.ThrowValidationExceptionIfExistingInvalid(validationResultExisting);

            _updateMobilePhoneCommandFlowDescribtor.AssignNewMobilePhoneInformation(mobilePhone!, incoming);
            _updateMobilePhoneCommandFlowDescribtor.UpdateMobilePhoneInRepository(mobilePhone!, _mobilePhonesCommandsRepository);

            var mobilePhonesHistory = _updateMobilePhoneCommandFlowDescribtor.CreateMobilePhoneHistoryEntry(mobilePhone!);
            _updateMobilePhoneCommandFlowDescribtor.WriteHistoryToRepository(_mobilePhonesCommandsRepository, mobilePhonesHistory);

            await _updateMobilePhoneCommandFlowDescribtor.SaveChanges(_mobilePhonesCommandsRepository, cancellationToken);
            return _updateMobilePhoneCommandFlowDescribtor.MapMobilePhoneToMobilePhoneDto(mobilePhone!);
        }
    }
}
