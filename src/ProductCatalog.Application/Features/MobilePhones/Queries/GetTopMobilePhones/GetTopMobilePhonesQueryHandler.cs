using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones
{
    internal sealed class GetTopMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetTopMobilePhonesQueryFlowDescribtor _getTopMobilePhonesQueryFlowDescribtor)
        : IRequestHandler<GetTopMobilePhonesQuery, IReadOnlyList<TopMobilePhoneDto>>
    {
        public async Task<IReadOnlyList<TopMobilePhoneDto>> Handle(GetTopMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var mobilePhones = await _getTopMobilePhonesQueryFlowDescribtor
                .GetTopMobilePhones(_mobilePhonesQueriesRepository, request.amount, cancellationToken);

            var existingMobilePhones = _getTopMobilePhonesQueryFlowDescribtor
                .EnsureTopMobilePhonesFound(mobilePhones);

            return _getTopMobilePhonesQueryFlowDescribtor.MapTopMobilePhonesToDto(existingMobilePhones);
        }
    }
}
