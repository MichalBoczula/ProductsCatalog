using MediatR;
using ProductCatalog.Application.Common.Dtos.External;
using ProductCatalog.Application.Common.Dtos.Internal;

namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductCommand(ProductExternalDto product) : IRequest<ProductDto>;
}
