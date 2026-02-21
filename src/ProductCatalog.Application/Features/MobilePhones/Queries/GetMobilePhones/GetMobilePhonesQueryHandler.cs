using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones
{
    internal sealed class GetMobilePhonesQueryHandler(
        IMobilePhonesQueriesRepository _mobilePhonesQueriesRepository,
        IValidationPolicy<int> _amountValidationPolicy,
        GetMobilePhonesQueryFlowDescribtor _getMobilePhonesQueryFlowDescribtor)
        : IRequestHandler<GetMobilePhonesQuery, IReadOnlyList<MobilePhoneDto>>
    {
        public async Task<IReadOnlyList<MobilePhoneDto>> Handle(GetMobilePhonesQuery request, CancellationToken cancellationToken)
        {
            var amountValidationResult = await _getMobilePhonesQueryFlowDescribtor
                .ValidateAmount(request.amount, _amountValidationPolicy);

            _getMobilePhonesQueryFlowDescribtor.ThrowValidationExceptionIfAmountInvalid(amountValidationResult);

            var mobilePhones = await _getMobilePhonesQueryFlowDescribtor
                .GetMobilePhones(_mobilePhonesQueriesRepository, request.amount, cancellationToken);

            var existingMobilePhones = _getMobilePhonesQueryFlowDescribtor
                .EnsureMobilePhonesFound(mobilePhones);

            return _getMobilePhonesQueryFlowDescribtor.MapMobilePhonesToDto(existingMobilePhones);
        }
    }
}
