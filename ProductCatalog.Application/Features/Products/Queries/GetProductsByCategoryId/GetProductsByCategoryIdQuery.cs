using MediatR;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    public sealed record GetProductsByCategoryIdQuery(Guid categoryId) : IRequest<IReadOnlyList<ProductDto>?>;
}
