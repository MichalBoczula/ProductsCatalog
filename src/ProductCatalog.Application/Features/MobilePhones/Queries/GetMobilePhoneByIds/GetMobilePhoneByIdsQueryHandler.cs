using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds
{
    internal sealed class GetMobilePhoneByIdsQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetMobilePhoneByIdsQueryFlowDescribtor _getMobilePhoneByIdsQueryFlowDescribtor)
        : IRequestHandler<GetMobilePhoneByIdsQuery, IReadOnlyList<MobilePhoneDto>>
    {
        public async Task<IReadOnlyList<MobilePhoneDto>> Handle(GetMobilePhoneByIdsQuery request, CancellationToken cancellationToken)
        {
            var mobilePhones = await _getMobilePhoneByIdsQueryFlowDescribtor
                .GetMobilePhones(_mobilePhonesQueriesRepository, request.ids, cancellationToken);

            var existingMobilePhones = _getMobilePhoneByIdsQueryFlowDescribtor
                .EnsureAllMobilePhonesFound(mobilePhones, request.ids);

            return _getMobilePhoneByIdsQueryFlowDescribtor.MapMobilePhonesToDto(existingMobilePhones);
        }
    }
}
