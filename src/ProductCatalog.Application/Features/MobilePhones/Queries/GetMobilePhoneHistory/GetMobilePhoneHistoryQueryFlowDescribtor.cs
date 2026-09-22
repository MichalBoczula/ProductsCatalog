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
    internal sealed class GetMobilePhoneHistoryQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneHistoryQuery>
    {
        [FlowStep(1)]
        public Task<ValidationResult> ValidatePagination(PaginationParameters pagination, IValidationPolicy<PaginationParameters> validationPolicy)
            => validationPolicy.Validate(pagination);

        [FlowStep(2)]
        public void ThrowValidationExceptionIfPaginationInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3)]
        public Task<MobilePhoneReadModel?> GetMobilePhone(IMobilePhonesQueriesRepository repository, Guid mobilePhoneId, CancellationToken cancellationToken)
            => repository.GetById(mobilePhoneId, cancellationToken);

        [FlowStep(4)]
        public void EnsureMobilePhoneFound(MobilePhoneReadModel? mobilePhone, Guid mobilePhoneId)
        {
            if (mobilePhone is null)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhoneHistoryQuery), mobilePhoneId, nameof(MobilePhoneHistoryDto));
            }
        }

        [FlowStep(5)]
        public Task<IReadOnlyList<MobilePhonesHistory>> LoadHistory(IMobilePhonesQueriesRepository repository, Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken cancellationToken)
            => repository.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, cancellationToken);

        [FlowStep(6)]
        public IReadOnlyList<MobilePhoneHistoryDto> MapHistoryToDto(IReadOnlyList<MobilePhonesHistory> historyEntries)
            => historyEntries.Adapt<List<MobilePhoneHistoryDto>>().AsReadOnly();
    }
}
