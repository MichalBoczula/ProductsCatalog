using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds
{
    [Documentation("products.flow.by-ids", "Read the distinct requested active phones; reject empty input or any missing/inactive ID.")]
    [FlowOutcome("products.branch.by-ids.found", "Every distinct requested ID is present and active.", "Return the matching phones ordered by Id as 200; no writes.")]
    [FlowOutcome("products.branch.by-ids.empty-ids", "The supplied ID array is empty.", "Return validation_failed 400 from the inline guard before SQL.", "products.guard.selected-ids")]
    [FlowOutcome("products.branch.by-ids.missing-ids", "One or more distinct requested IDs are missing or inactive.", "Return resource_not_found 404 for the request; no partial successful array or writes.")]
    [FlowOutcome("products.branch.by-ids.repository-failure", "The selected-ID repository read throws.", "Return safe internal_error 500 without writes.")]
    internal sealed class GetMobilePhoneByIdsQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneByIdsQuery>
    {
        [FlowStep(1, "require-ids", "Reject an empty ID array with an inline validation error before querying SQL.", RuleId = "products.guard.selected-ids")]
        public void EnsureIdsProvided(IReadOnlyCollection<Guid> mobilePhoneIds)
        {
            if (mobilePhoneIds.Count == 0)
            {
                var validationResult = new ValidationResult();
                validationResult.AddValidationError(new ValidationError
                {
                    Name = nameof(GetMobilePhoneByIdsQuery),
                    Entity = nameof(mobilePhoneIds),
                    Message = "At least one mobile phone Id must be provided."
                });

                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(2, "load", "Query active phones matching the supplied IDs; SQL orders by Id and returns each phone once.")]
        public Task<IReadOnlyList<MobilePhoneReadModel>> GetMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            IReadOnlyCollection<Guid> mobilePhoneIds,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetByIds(mobilePhoneIds, cancellationToken);
        }

        [FlowStep(3, "require-all", "Compare distinct requested IDs with found IDs; any missing or inactive ID causes resource_not_found for the whole request.")]
        public IReadOnlyList<MobilePhoneReadModel> EnsureAllMobilePhonesFound(
            IReadOnlyList<MobilePhoneReadModel> mobilePhones,
            IReadOnlyCollection<Guid> requestedIds)
        {
            var foundIds = mobilePhones.Select(mobilePhone => mobilePhone.Id).ToHashSet();
            var missingIds = requestedIds.Distinct().Where(id => !foundIds.Contains(id)).ToList().AsReadOnly();

            if (missingIds.Count > 0)
            {
                throw new ResourceNotFoundException(
                    nameof(GetMobilePhoneByIdsQuery),
                    missingIds,
                    nameof(MobilePhoneDto));
            }

            return mobilePhones;
        }

        [FlowStep(4, "map-response", "Map the matching active phones, including mainPhoto and price, to a 200 array without writes.")]
        public IReadOnlyList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<MobilePhoneDto>>().AsReadOnly();
        }
    }
}
