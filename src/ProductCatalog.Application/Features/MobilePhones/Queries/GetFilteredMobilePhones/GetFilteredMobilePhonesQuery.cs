using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    public sealed record GetFilteredMobilePhonesQuery(MobilePhoneFilterDto filter) : IRequest<IList<MobilePhoneDto>>;
}
