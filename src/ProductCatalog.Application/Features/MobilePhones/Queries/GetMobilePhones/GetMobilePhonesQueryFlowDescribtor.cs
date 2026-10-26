using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones
{
    [Documentation("products.flow.list", "Return up to a positive requested amount of active phones in stable name/Id order.")]
    [FlowOutcome("products.branch.list.found", "The valid list query finds active phones.", "Return up to amount phones ordered by Name and Id as 200 without writes.")]
    [FlowOutcome("products.branch.list.empty", "The valid list query finds no active phones.", "Return an empty array as 200, not 404; no writes.")]
    [FlowOutcome("products.branch.list.invalid-amount", "Amount is zero or negative.", "Return validation_failed 400 before SQL.", "products.rule.amount-positive")]
    [FlowOutcome("products.branch.list.repository-failure", "The list repository read throws.", "Return safe internal_error 500 without writes.")]
    [FlowOutcome("products.branch.list.null-result", "A substituted repository returns null instead of its normal non-null list.", "Defensive resource_not_found branch; not a declared HTTP response or covered BDD path. Do not treat it as normal empty-list behavior.")]
    internal sealed class GetMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetMobilePhonesQuery>
    {
        [FlowStep(1, "validate-amount", "Validate amount greater than zero before querying SQL.")]
        public Task<ValidationResult> ValidateAmount(int amount, IValidationPolicy<int> validationPolicy)
        {
            return validationPolicy.Validate(amount);
        }

        [FlowStep(2, "reject-amount", "If amount is not positive, return validation_failed 400 without querying or writing.")]
        public void ThrowValidationExceptionIfAmountInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3, "load", "Load at most amount active phones, ordered by Name then Id, with mainPhoto and price.")]
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetMobilePhones(IMobilePhonesQueriesRepository mobilePhonesQueriesRepository, int amount, CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetPhones(amount, cancellationToken);
        }

        [FlowStep(4, "check-result", "An empty list is valid. A defensive null-result guard throws resource_not_found; the real repository returns a non-null list.")]
        public IReadOnlyList<MobilePhoneReadModel> EnsureMobilePhonesFound(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            if (mobilePhones is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhonesQuery), Guid.Empty, nameof(List<MobilePhoneDetailsDto>));
            }

            return mobilePhones;
        }

        [FlowStep(5, "map-response", "Return the mapped list, including an empty array, as 200 with no writes.")]
        public IReadOnlyList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<MobilePhoneDto>>().AsReadOnly();
        }
    }
}
