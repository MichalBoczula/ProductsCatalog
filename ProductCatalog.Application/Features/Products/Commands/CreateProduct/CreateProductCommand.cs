using MediatR;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand(CreateProductExternalDto product) : IRequest<ProductDto>;
}
