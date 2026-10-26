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

namespace ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone
{
    [Documentation("products.flow.create", "Create one active phone and its Inserted history snapshot in one save.")]
    [FlowOutcome("products.branch.create.saved", "Valid details and SaveChanges succeeds.", "Create one active current row and one Inserted history row; return 201.")]
    [FlowOutcome("products.branch.create.validation", "The phone policy rejects the mapped aggregate.", "Return validation_failed 400 before writes.", "products.rule.phone-description", "products.rule.phone-strings", "products.rule.phone-exists")]
    [FlowOutcome("products.branch.create.save-failure", "The repository save throws.", "Return safe internal_error 500. The controlled BDD fault throws before EF SaveChanges; it does not prove SQL transaction rollback.")]
    internal sealed class CreateMobilePhoneCommandFlowDescribtor : FlowDescriberBase<CreateMobilePhoneCommand>
    {
        [FlowStep(1, "map-input", "Map request details to a new phone aggregate and normalize currency text.")]
        public MobilePhone MapRequestToMobilePhoneAggregate(CreateMobilePhoneCommand command)
        {
            return command.mobilePhoneExternalDto.Adapt<MobilePhone>();
        }

        [FlowStep(2, "validate", "Run the phone policy; no hardware or money rules are added by this flow.")]
        public Task<ValidationResult> ValidateMobilePhoneAggregate(MobilePhone mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone);
        }

        [FlowStep(3, "reject-invalid", "If phone validation fails, throw validation_failed before staging current or history rows.")]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4, "stage-current", "Stage the new phone in EF; this does not commit it.")]
        public void AddMobilePhoneToRepository(MobilePhone mobilePhone, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository)
        {
            mobilePhonesCommandsRepository.Add(mobilePhone);
        }

        [FlowStep(5, "build-history", "Build the first history snapshot with operation Inserted and the phone change time.")]
        public MobilePhonesHistory CreateMobilePhoneHistoryEntry(MobilePhone mobilePhone)
        {
            var mobilePhonesHistory = mobilePhone.BuildAdapter()
                .AddParameters("operation", Operation.Inserted)
                .AdaptToType<MobilePhonesHistory>();

            return mobilePhonesHistory;
        }

        [FlowStep(6, "stage-history", "Stage the Inserted history row in the same EF context as the phone.")]
        public void WriteHistoryToRepository(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, MobilePhonesHistory mobilePhonesHistory)
        {
            mobilePhonesCommandsRepository.WriteHistory(mobilePhonesHistory);
        }

        [FlowStep(7, "save", "Save current and history together; a successful SQL save commits both, while a thrown save does not return success.")]
        public Task SaveChanges(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(8, "map-response", "Map the saved phone to details; the endpoint returns 201 and its relative Location.")]
        public MobilePhoneDetailsDto MapMobilePhoneToMobilePhoneDto(MobilePhone mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDetailsDto>();
        }
    }
}
