using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Pagination;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    internal sealed class GetMobilePhoneHistoryQueryHandler(
        IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
        IValidationPolicy<PaginationParameters> paginationValidationPolicy,
        GetMobilePhoneHistoryQueryFlowDescribtor flowDescribtor)
        : IRequestHandler<GetMobilePhoneHistoryQuery, IReadOnlyList<MobilePhoneHistoryDto>>
    {
        public GetMobilePhoneHistoryQueryHandler(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            GetMobilePhoneHistoryQueryFlowDescribtor flowDescribtor)
            : this(mobilePhonesQueriesRepository, new PaginationParametersValidationPolicy(), flowDescribtor)
        {
        }

        public async Task<IReadOnlyList<MobilePhoneHistoryDto>> Handle(GetMobilePhoneHistoryQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await flowDescribtor.ValidatePagination(request.pagination, paginationValidationPolicy);
            flowDescribtor.ThrowValidationExceptionIfPaginationInvalid(validationResult);

            var mobilePhone = await flowDescribtor.GetMobilePhone(mobilePhonesQueriesRepository, request.mobilePhoneId, cancellationToken);
            flowDescribtor.EnsureMobilePhoneFound(mobilePhone, request.mobilePhoneId);

            var historyEntries = await flowDescribtor.LoadHistory(
                mobilePhonesQueriesRepository,
                request.mobilePhoneId,
                request.pagination.PageNumber,
                request.pagination.PageSize,
                cancellationToken);

            return flowDescribtor.MapHistoryToDto(historyEntries);
        }
    }
}
