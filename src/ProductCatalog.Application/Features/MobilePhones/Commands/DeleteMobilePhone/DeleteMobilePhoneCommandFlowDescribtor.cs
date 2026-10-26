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

namespace ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone
{
    [Documentation("products.flow.delete", "Soft delete an active phone with conditional concurrency protection; repeated delete is a no-op.")]
    [FlowOutcome("products.branch.delete.deleted", "The phone is active and the conditional save succeeds.", "Set IsActive false, advance ChangedAt and append one Deleted history row; return 200.")]
    [FlowOutcome("products.branch.delete.missing-before-read", "The initial lookup has no phone.", "Return validation_failed 400; do not save.", "products.rule.phone-exists")]
    [FlowOutcome("products.branch.delete.repeated", "The loaded phone is already inactive.", "Return 200 with its existing state; no additional save, timestamp change or history.")]
    [FlowOutcome("products.branch.delete.conflict", "Another writer commits after the original state was loaded.", "Return concurrency_conflict 409, preserve the winner and reject losing delete history.")]
    [FlowOutcome("products.branch.delete.removed-after-read", "The loaded row is physically removed before conditional save.", "Return resource_not_found 404 without adding delete history.")]
    [FlowOutcome("products.branch.delete.save-failure", "The repository save throws.", "Return safe internal_error 500; the controlled BDD failure occurs before EF SaveChanges, not during an actual SQL transaction.")]
    internal sealed class DeleteMobilePhoneCommandFlowDescribtor : FlowDescriberBase<DeleteMobilePhoneCommand>
    {
        [FlowStep(1, "load", "Load the tracked phone and original ChangedAt token; no physical deletion is performed by this flow.")]
        public async Task<MobilePhone> LoadMobilePhone(Guid mobilePhoneId, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return (await mobilePhonesCommandsRepository.GetById(mobilePhoneId, cancellationToken))!;
        }

        [FlowStep(2, "validate", "Validate current phone state; a missing initial ID fails the phone policy.")]
        public Task<ValidationResult> ValidateMobilePhone(MobilePhone mobilePhone, IValidationPolicy<MobilePhone> validationPolicy)
        {
            var validationResult = validationPolicy.Validate(mobilePhone);
            return validationResult;
        }

        [FlowStep(3, "reject-invalid", "Return validation_failed 400 when the initial phone is missing or invalid, before any save.")]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4, "already-inactive", "If phone is already inactive, return it without writing history or calling SaveChanges.")]
        public bool IsAlreadyInactive(MobilePhone mobilePhone)
        {
            return !mobilePhone.IsActive;
        }

        [FlowStep(5, "deactivate", "Set IsActive false and advance ChangedAt monotonically.")]
        public void DeactivateMobilePhone(MobilePhone mobilePhone)
        {
            mobilePhone.Deactivate();
        }

        [FlowStep(6, "stage-current", "Stage the conditional soft-delete update using the original ChangedAt token.")]
        public void UpdateMobilePhoneInRepository(MobilePhone mobilePhone, IMobilePhonesCommandsRepository mobilePhonesCommandsRepository)
        {
            mobilePhonesCommandsRepository.Update(mobilePhone);
        }

        [FlowStep(7, "build-history", "Build a Deleted history snapshot of the inactive phone.")]
        public MobilePhonesHistory CreateMobilePhoneHistoryEntry(MobilePhone mobilePhone)
        {
            var mobilePhonesHistory = mobilePhone.BuildAdapter()
                .AddParameters("operation", Operation.Deleted)
                .AdaptToType<MobilePhonesHistory>();

            return mobilePhonesHistory;
        }

        [FlowStep(8, "stage-history", "Stage Deleted history in the same context as the current-row update.")]
        public void WriteHistoryToRepository(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, MobilePhonesHistory mobilePhonesHistory)
        {
            mobilePhonesCommandsRepository.WriteHistory(mobilePhonesHistory);
        }

        [FlowStep(9, "save", "Save current and history together; reject stale writes as 409 or physical removal as 404 without losing history.")]
        public Task SaveChanges(IMobilePhonesCommandsRepository mobilePhonesCommandsRepository, CancellationToken cancellationToken)
        {
            return mobilePhonesCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(10, "map-response", "Return the inactive phone details as 200; it remains readable by ID.")]
        public MobilePhoneDetailsDto MapMobilePhoneToMobilePhoneDto(MobilePhone mobilePhone)
        {
            return mobilePhone.Adapt<MobilePhoneDetailsDto>();
        }
    }
}
