using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.Common.Pagination;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    public sealed record GetMobilePhoneHistoryQuery(Guid mobilePhoneId, PaginationParameters pagination)
        : IRequest<IReadOnlyList<MobilePhoneHistoryDto>>
    {
        public GetMobilePhoneHistoryQuery(Guid mobilePhoneId, int pageNumber, int pageSize)
            : this(mobilePhoneId, new PaginationParameters(pageNumber, pageSize))
        {
        }
    }
}
