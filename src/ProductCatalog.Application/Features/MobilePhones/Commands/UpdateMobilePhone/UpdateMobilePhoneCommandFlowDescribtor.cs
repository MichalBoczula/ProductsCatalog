using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone
{
    internal sealed class UpdateMobilePhoneCommandFlowDescribtor : FlowDescriberBase<UpdateMobilePhoneCommand>
    {
        [FlowStep(1)]
        public MobilePhone MapRequestToMobilePhoneAggregate(UpdateMobilePhoneCommand command)
        {
            var mobilePhone = command.MobilePhone.Adapt<MobilePhone>();
            return mobilePhone;
        }

        [FlowStep(2)]
        public Task<ValidationResult> ValidateIncomingMobilePhone(MobilePhone mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone!);
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfIncomingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public Task<MobilePhone?> LoadExistingMobilePhone(Guid mobilePhoneId, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.GetById(mobilePhoneId, cancellationToken);
        }

        [FlowStep(5)]
        public Task<ValidationResult> ValidateExistingMobilePhone(MobilePhone? mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone!);
        }

        [FlowStep(6)]
        public void ThrowValidationExceptionIfExistingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(7, "If information is unchanged, return the existing phone without writing history")]
        public bool HasSameInformation(MobilePhone mobilePhone, MobilePhone incomingMobilePhone)
        {
            return mobilePhone.HasSameInformation(incomingMobilePhone);
        }

        [FlowStep(8)]
        public void AssignNewMobilePhoneInformation(MobilePhone mobilePhone, MobilePhone incomingMobilePhone)
        {
            mobilePhone.AssigneNewMobilePhoneInformation(incomingMobilePhone);
        }

        [FlowStep(9)]
        public void UpdateMobilePhoneInRepository(MobilePhone mobilePhone, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository)
        {
            mobilePhonesCommandsRepository.Update(mobilePhone);
        }

        [FlowStep(10)]
        public MobilePhonesHistory CreateMobilePhoneHistoryEntry(MobilePhone mobilePhone)
        {
            var mobilePhonesHistory = mobilePhone.BuildAdapter()
                .AddParameters("operation", Operation.Updated)
                .AdaptToType<MobilePhonesHistory>();

            return mobilePhonesHistory;
        }

        [FlowStep(11)]
        public void WriteHistoryToRepository(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, MobilePhonesHistory mobilePhonesHistory)
        {
            mobilePhonesCommandsRepository.WriteHistory(mobilePhonesHistory);
        }

        [FlowStep(12)]
        public Task SaveChanges(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(13)]
        public MobilePhoneDetailsDto MapMobilePhoneToMobilePhoneDto(MobilePhone mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDetailsDto>();
        }
    }
}
