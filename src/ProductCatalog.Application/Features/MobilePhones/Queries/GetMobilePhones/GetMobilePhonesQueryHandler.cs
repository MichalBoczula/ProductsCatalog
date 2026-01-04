using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones
{
    internal sealed class GetMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetMobilePhonesQueryFlowDescribtor _getMobilePhonesQueryFlowDescribtor)
        : IRequestHandler<GetMobilePhonesQuery, IReadOnlyList<MobilePhoneDto>>
    {
        public async Task<IReadOnlyList<MobilePhoneDto>> Handle(GetMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var mobilePhones = await _getMobilePhonesQueryFlowDescribtor
                .GetMobilePhones(_mobilePhonesQueriesRepository, request.amount, cancellationToken);

            var existingMobilePhones = _getMobilePhonesQueryFlowDescribtor
                .EnsureMobilePhonesFound(mobilePhones);

            return _getMobilePhonesQueryFlowDescribtor.MapMobilePhonesToDto(existingMobilePhones);
        }
    }
}
