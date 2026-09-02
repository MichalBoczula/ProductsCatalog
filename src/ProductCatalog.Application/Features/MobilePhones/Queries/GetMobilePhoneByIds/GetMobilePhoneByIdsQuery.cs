using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds
{
    public sealed record GetMobilePhoneByIdsQuery(IReadOnlyCollection<Guid> ids) : IRequest<IReadOnlyList<MobilePhoneDto>>;
}
