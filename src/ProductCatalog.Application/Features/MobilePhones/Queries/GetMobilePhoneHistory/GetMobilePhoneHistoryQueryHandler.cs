using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    internal sealed class GetMobilePhoneHistoryQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetMobilePhoneHistoryQueryFlowDescribtor _getMobilePhoneHistoryQueryFlowDescribtor)
        : IRequestHandler<GetMobilePhoneHistoryQuery, IReadOnlyList<MobilePhoneHistoryDto>>
    {
        public async Task<IReadOnlyList<MobilePhoneHistoryDto>> Handle(
            GetMobilePhoneHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var historyEntries = await _getMobilePhoneHistoryQueryFlowDescribtor
                .LoadHistory(_mobilePhonesQueriesRepository, request.mobilePhoneId, request.pageNumber, request.pageSize, cancellationToken);

            var existingHistoryEntries = _getMobilePhoneHistoryQueryFlowDescribtor
                .EnsureHistoryFound(historyEntries, request.mobilePhoneId);

            return _getMobilePhoneHistoryQueryFlowDescribtor.MapHistoryToDto(existingHistoryEntries);
        }
    }
}
