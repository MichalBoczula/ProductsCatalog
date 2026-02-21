using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    internal sealed class GetFilteredMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository
        ) : IRequestHandler<GetFilteredMobilePhonesQuery, IList<MobilePhoneDto>>
    {
        public async Task<IList<MobilePhoneDto>> Handle(GetFilteredMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var phones = await _mobilePhonesQueriesRepository.GetFilteredPhones(request.filter, cancellationToken);
            return phones.Adapt<IList<MobilePhoneDto>>();
        }
    }
}
