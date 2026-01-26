using MediatR;
using ProductCatalog.Application.Common.Dtos.MobilePhones;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById
{
    public sealed record GetMobilePhoneByIdQuery(Guid id) : IRequest<MobilePhoneDetailsDto?>;
}
