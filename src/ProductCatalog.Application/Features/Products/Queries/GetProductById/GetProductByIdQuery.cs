using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(Guid id) : IRequest<ProductDto?>;
}
