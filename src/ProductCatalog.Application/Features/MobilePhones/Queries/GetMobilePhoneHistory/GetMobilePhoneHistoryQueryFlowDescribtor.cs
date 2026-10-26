using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Pagination;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    [Documentation("products.flow.history", "Validate pagination before existence lookup, then read stable newest-first snapshots.")]
    [FlowOutcome("products.branch.history.found", "Valid pagination and an existing phone yield history rows.", "Return the requested newest-first page as 200 without writes.")]
    [FlowOutcome("products.branch.history.empty", "Valid pagination and an existing phone yield no history rows for that page.", "Return an empty array as 200 without writes.")]
    [FlowOutcome("products.branch.history.invalid-pagination", "Page number or size is zero or negative.", "Return validation_failed 400 before the phone lookup.", "products.rule.pagination-positive")]
    [FlowOutcome("products.branch.history.missing", "Valid pagination reaches the lookup, but the phone does not exist.", "Return resource_not_found 404 without history read or writes.")]
    [FlowOutcome("products.branch.history.repository-failure", "The initial GetById lookup throws.", "Return safe internal_error 500 without writes; this is the existing controlled BDD read fault.")]
    [FlowOutcome("products.branch.history.history-read-failure", "The history query throws after successful existence lookup.", "Return safe internal_error 500 without writes; a controlled history-query fault follows a successful real phone lookup.")]
    internal sealed class GetMobilePhoneHistoryQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneHistoryQuery>
    {
        [FlowStep(1, "validate-pagination", "Validate pageNumber and pageSize greater than zero before looking up the phone.")]
        public Task<ValidationResult> ValidatePagination(PaginationParameters pagination, IValidationPolicy<PaginationParameters> validationPolicy)
            => validationPolicy.Validate(pagination);

        [FlowStep(2, "reject-pagination", "Return validation_failed 400 for invalid pagination even if the phone ID is unknown.")]
        public void ThrowValidationExceptionIfPaginationInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3, "load-phone", "Look up the current phone, including inactive phones, before reading its history.")]
        public Task<MobilePhoneReadModel?> GetMobilePhone(IMobilePhonesQueriesRepository repository, Guid mobilePhoneId, CancellationToken cancellationToken)
            => repository.GetById(mobilePhoneId, cancellationToken);

        [FlowStep(4, "require-found", "If the current phone is absent, return resource_not_found 404 without reading history.")]
        public void EnsureMobilePhoneFound(MobilePhoneReadModel? mobilePhone, Guid mobilePhoneId)
        {
            if (mobilePhone is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhoneHistoryQuery), mobilePhoneId, nameof(MobilePhoneHistoryDto));
            }
        }

        [FlowStep(5, "load-history", "Load the requested page ordered by ChangedAt descending, then history Id descending; an empty page is allowed.")]
        public Task<IReadOnlyList<MobilePhonesHistory>> LoadHistory(IMobilePhonesQueriesRepository repository, Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken cancellationToken)
            => repository.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, cancellationToken);

        [FlowStep(6, "map-response", "Return full history snapshots with operation values as a 200 array; do not change current/history.")]
        public IReadOnlyList<MobilePhoneHistoryDto> MapHistoryToDto(IReadOnlyList<MobilePhonesHistory> historyEntries)
            => historyEntries.Adapt<List<MobilePhoneHistoryDto>>().AsReadOnly();
    }
}

