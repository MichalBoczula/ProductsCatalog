using MediatR;
using ProductCatalog.Application.Common.Dtos;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(UpdateProductExternalDto product) : IRequest<ProductDto>;
}
