using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById
{
    internal sealed class GetMobilePhoneByIdQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        GetMobilePhoneByIdQueryFlowDescribtor _getMobilePhoneByIdQueryFlowDescribtor)
        : IRequestHandler<GetMobilePhoneByIdQuery, MobilePhoneDetailsDto?>
    {
        public async Task<MobilePhoneDetailsDto?> Handle(GetMobilePhoneByIdQuery request, CancellationToken cancellationToken)
        {
            var mobilePhone = await _getMobilePhoneByIdQueryFlowDescribtor
                .GetMobilePhone(_mobilePhonesQueriesRepository, request.id, cancellationToken);

            var existingMobilePhone = _getMobilePhoneByIdQueryFlowDescribtor
                .EnsureMobilePhoneFound(mobilePhone, request.id);

            return _getMobilePhoneByIdQueryFlowDescribtor.MapMobilePhoneToDto(existingMobilePhone);
        }
    }
}
