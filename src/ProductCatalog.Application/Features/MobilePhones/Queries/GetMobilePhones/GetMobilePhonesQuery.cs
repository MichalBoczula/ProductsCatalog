using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones
{
    public sealed record GetMobilePhonesQuery(int amount) : IRequest<IReadOnlyList<MobilePhoneDto>>;
}
