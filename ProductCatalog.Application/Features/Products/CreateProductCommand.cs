using MediatR;
using ProductCatalog.Application.Common.Dtos;

namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductCommand(CreateProductExternalDto product) : IRequest<ProductDto>;
}
