using MediatR;
using ProductCatalog.Application.Common.Dtos;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand(CreateProductExternalDto product) : IRequest<ProductDto>;
}
