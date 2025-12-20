using MediatR;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(UpdateProductExternalDto product) : IRequest<ProductDto>;
}
