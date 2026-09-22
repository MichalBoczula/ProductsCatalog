using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds
{
    internal sealed class GetMobilePhoneByIdsQueryHandler(
        IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
        GetMobilePhoneByIdsQueryFlowDescribtor flowDescribtor)
        : IRequestHandler<GetMobilePhoneByIdsQuery, IReadOnlyList<MobilePhoneDto>>
    {
        public async Task<IReadOnlyList<MobilePhoneDto>> Handle(GetMobilePhoneByIdsQuery request, CancellationToken cancellationToken)
        {
            flowDescribtor.EnsureIdsProvided(request.ids);

            var mobilePhones = await flowDescribtor.GetMobilePhones(mobilePhonesQueriesRepository, request.ids, cancellationToken);
            var existingMobilePhones = flowDescribtor.EnsureAllMobilePhonesFound(mobilePhones, request.ids);

            return flowDescribtor.MapMobilePhonesToDto(existingMobilePhones);
        }
    }
}
