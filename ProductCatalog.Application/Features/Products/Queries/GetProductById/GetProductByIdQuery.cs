using MediatR;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(Guid id) : IRequest<ProductDto?>;
}
