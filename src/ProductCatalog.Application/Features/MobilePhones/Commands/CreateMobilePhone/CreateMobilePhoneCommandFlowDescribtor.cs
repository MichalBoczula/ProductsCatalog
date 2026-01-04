using Mapster;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    internal sealed class CreateMobilePhoneCommandFlowDescribtor : FlowDescriberBase<CreateMobilePhoneCommand>
    {
        [FlowStep(1)]
        public MobilePhone MapRequestToMobilePhoneAggregate(CreateMobilePhoneCommand command)
        {
            return command.mobilePhoneExternalDto.Adapt<MobilePhone>();
        }

        [FlowStep(2)]
        public Task<ValidationResult> ValidateMobilePhoneAggregate(MobilePhone mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone);
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public void AddMobilePhoneToRepository(MobilePhone mobilePhone, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository)
        {
            mobilePhonesCommandsRepository.Add(mobilePhone);
        }

        [FlowStep(5)]
        public Task SaveChanges(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(6)]
        public MobilePhoneDto MapMobilePhoneToMobilePhoneDto(MobilePhone mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDto>();
        }
    }
}
