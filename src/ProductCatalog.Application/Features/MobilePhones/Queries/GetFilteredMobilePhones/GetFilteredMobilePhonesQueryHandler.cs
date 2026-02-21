using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    internal sealed class GetFilteredMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetFilteredMobilePhonesQueryFlowDescribtor _getFilteredMobilePhonesQueryFlowDescribtor
        ) : IRequestHandler<GetFilteredMobilePhonesQuery, IList<MobilePhoneDto>>
    {
        public async Task<IList<MobilePhoneDto>> Handle(GetFilteredMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var phones = await _getFilteredMobilePhonesQueryFlowDescribtor
                .GetFilteredMobilePhones(_mobilePhonesQueriesRepository, request.filter, cancellationToken);

            return _getFilteredMobilePhonesQueryFlowDescribtor.MapMobilePhonesToDto(phones);
        }
    }
}
