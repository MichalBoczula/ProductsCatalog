using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;

namespace ProductCatalog.Application.Features.Products.Commands.RemoveProduct
{
    public sealed record class RemoveProductCommand(Guid ProductId) : IRequest<ProductDto>;
}
