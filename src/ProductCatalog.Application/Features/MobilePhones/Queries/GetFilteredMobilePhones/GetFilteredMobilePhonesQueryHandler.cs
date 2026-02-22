using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    internal sealed class GetFilteredMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        IValidationPolicy<MobilePhoneFilterDto> _mobilePhoneFilterValidationPolicy,
        GetFilteredMobilePhonesQueryFlowDescribtor _getFilteredMobilePhonesQueryFlowDescribtor
        ) : IRequestHandler<GetFilteredMobilePhonesQuery, IList<MobilePhoneDto>>
    {
        public async Task<IList<MobilePhoneDto>> Handle(GetFilteredMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _getFilteredMobilePhonesQueryFlowDescribtor
                .ValidateFilter(request.filter, _mobilePhoneFilterValidationPolicy);

            _getFilteredMobilePhonesQueryFlowDescribtor.ThrowValidationExceptionIfFilterInvalid(validationResult);
            var brandNameFilter = _getFilteredMobilePhonesQueryFlowDescribtor
                .CastBrandToString(request.filter);

            var phones = await _getFilteredMobilePhonesQueryFlowDescribtor
                .GetFilteredMobilePhones(_mobilePhonesQueriesRepository, brandNameFilter, cancellationToken);

            return _getFilteredMobilePhonesQueryFlowDescribtor.MapMobilePhonesToDto(phones);
        }
    }
}
