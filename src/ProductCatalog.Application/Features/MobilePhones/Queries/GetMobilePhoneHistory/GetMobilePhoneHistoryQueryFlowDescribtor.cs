using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    internal sealed class GetMobilePhoneHistoryQueryFlowDescribtor : FlowDescriberBase<GetMobilePhoneHistoryQuery>
    {
        [FlowStep(1)]
        public Task<IReadOnlyList<MobilePhonesHistory>> LoadHistory(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            Guid mobilePhoneId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, cancellationToken);
        }

        [FlowStep(2)]
        public IReadOnlyList<MobilePhonesHistory> EnsureHistoryFound(
            IReadOnlyList<MobilePhonesHistory> historyEntries,
            Guid mobilePhoneId)
        {
            if (historyEntries is null || historyEntries.Count == 0)
            {
                throw new ResourceNotFoundException(nameof(GetMobilePhoneHistoryQuery), mobilePhoneId, nameof(List<MobilePhoneHistoryDto>));
            }

            return historyEntries;
        }

        [FlowStep(3)]
        public IReadOnlyList<MobilePhoneHistoryDto> MapHistoryToDto(IReadOnlyList<MobilePhonesHistory> historyEntries)
        {
            return historyEntries.Adapt<List<MobilePhoneHistoryDto>>().AsReadOnly();
        }
    }
}
