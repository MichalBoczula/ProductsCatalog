using MediatR;
using ProductCatalog.Application.Common.Dtos;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    public sealed record GetProductsByCategoryIdQuery(Guid categoryId) : IRequest<IReadOnlyList<ProductDto>?>;
}
