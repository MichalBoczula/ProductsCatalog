using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory
{
    public sealed record GetMobilePhoneHistoryQuery(Guid mobilePhoneId, int pageNumber, int pageSize)
        : IRequest<IReadOnlyList<MobilePhoneHistoryDto>>;
}
