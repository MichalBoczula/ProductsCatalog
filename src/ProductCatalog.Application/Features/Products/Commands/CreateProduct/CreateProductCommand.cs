using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand(CreateProductExternalDto product) : IRequest<ProductDto>;
}
