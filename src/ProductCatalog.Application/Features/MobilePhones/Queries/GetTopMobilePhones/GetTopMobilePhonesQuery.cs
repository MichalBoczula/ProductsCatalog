using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones
{
    public sealed record GetTopMobilePhonesQuery : IRequest<IReadOnlyList<TopMobilePhoneDto>>;
}
