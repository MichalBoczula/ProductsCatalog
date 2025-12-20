using MediatR;

namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductCommand(CreateProductDto product) : IRequest<Guid>;
}
