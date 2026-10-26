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
    [Documentation("products.flow.update", "Replace changed phone details with conditional concurrency protection; unchanged information is a no-op.")]
    [FlowOutcome("products.branch.update.changed", "Valid replacement differs from the existing phone and the conditional save succeeds.", "Replace current details, advance ChangedAt and append one Updated snapshot; return 200.")]
    [FlowOutcome("products.branch.update.invalid-input", "Incoming details fail the phone policy.", "Return validation_failed 400 before loading or writing.", "products.rule.phone-description", "products.rule.phone-strings", "products.rule.phone-exists")]
    [FlowOutcome("products.branch.update.missing-before-read", "The initial tracked lookup returns no phone.", "Return validation_failed 400 through the phone-exists rule; preserve current/history.", "products.rule.phone-exists")]
    [FlowOutcome("products.branch.update.no-change", "The incoming profile equals the loaded profile.", "Return 200 with existing details; preserve ChangedAt and all history; do not save.")]
    [FlowOutcome("products.branch.update.conflict", "Another writer commits after this request loaded its original token.", "Return concurrency_conflict 409; preserve the winning state and reject losing history.")]
    [FlowOutcome("products.branch.update.removed-after-read", "The loaded current row is physically removed before conditional save.", "Return resource_not_found 404; do not append update history.")]
    [FlowOutcome("products.branch.update.save-failure", "The repository save throws.", "Return safe internal_error 500; the controlled fault is before EF SaveChanges and does not verify rollback of an actual SQL failure.")]
    internal sealed class UpdateMobilePhoneCommandFlowDescribtor : FlowDescriberBase<UpdateMobilePhoneCommand>
    {
        [FlowStep(1, "map-input", "Map the replacement profile to an incoming phone aggregate.")]
        public MobilePhone MapRequestToMobilePhoneAggregate(UpdateMobilePhoneCommand command)
        {
            var mobilePhone = command.MobilePhone.Adapt<MobilePhone>();
            return mobilePhone;
        }

        [FlowStep(2, "validate-input", "Validate incoming phone details with the phone policy before loading current state.")]
        public Task<ValidationResult> ValidateIncomingMobilePhone(MobilePhone mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone!);
        }

        [FlowStep(3, "reject-input", "If incoming details are invalid, return validation_failed 400 before loading or saving.")]
        public void ThrowValidationExceptionIfIncomingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4, "load", "Load the tracked current phone and its original ChangedAt concurrency token.")]
        public Task<MobilePhone?> LoadExistingMobilePhone(Guid mobilePhoneId, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.GetById(mobilePhoneId, cancellationToken);
        }

        [FlowStep(5, "validate-current", "Run the phone policy on the loaded aggregate; a missing ID is a validation rejection.")]
        public Task<ValidationResult> ValidateExistingMobilePhone(MobilePhone? mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhone!);
        }

        [FlowStep(6, "reject-current", "If current state is missing or invalid, return validation_failed 400 without saving.")]
        public void ThrowValidationExceptionIfExistingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(7, "same-information", "If information is unchanged, return the existing phone without writing history or calling SaveChanges.")]
        public bool HasSameInformation(MobilePhone mobilePhone, MobilePhone incomingMobilePhone)
        {
            return mobilePhone.HasSameInformation(incomingMobilePhone);
        }

        [FlowStep(8, "apply", "Assign changed information and advance ChangedAt monotonically.")]
        public void AssignNewMobilePhoneInformation(MobilePhone mobilePhone, MobilePhone incomingMobilePhone)
        {
            mobilePhone.AssigneNewMobilePhoneInformation(incomingMobilePhone);
        }

        [FlowStep(9, "stage-current", "Stage a conditional update using the originally loaded ChangedAt token.")]
        public void UpdateMobilePhoneInRepository(MobilePhone mobilePhone, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository)
        {
            mobilePhonesCommandsRepository.Update(mobilePhone);
        }

        [FlowStep(10, "build-history", "Build an Updated snapshot of the replacement state.")]
        public MobilePhonesHistory CreateMobilePhoneHistoryEntry(MobilePhone mobilePhone)
        {
            var mobilePhonesHistory = mobilePhone.BuildAdapter()
                .AddParameters("operation", Operation.Updated)
                .AdaptToType<MobilePhonesHistory>();

            return mobilePhonesHistory;
        }

        [FlowStep(11, "stage-history", "Stage Updated history in the same EF context as the conditional current-row update.")]
        public void WriteHistoryToRepository(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, MobilePhonesHistory mobilePhonesHistory)
        {
            mobilePhonesCommandsRepository.WriteHistory(mobilePhonesHistory);
        }

        [FlowStep(12, "save", "Save current and history together. A stale token produces 409, or 404 after physical removal, without extra losing history.")]
        public Task SaveChanges(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(13, "map-response", "Return details of the updated phone or the unchanged existing phone as 200.")]
        public MobilePhoneDetailsDto MapMobilePhoneToMobilePhoneDto(MobilePhone mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDetailsDto>();
        }
    }
}
