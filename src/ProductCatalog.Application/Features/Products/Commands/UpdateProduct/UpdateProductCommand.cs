using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(Guid productId, UpdateProductExternalDto product) : IRequest<ProductDto>;
}
