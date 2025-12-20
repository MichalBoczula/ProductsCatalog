using MediatR;

namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductCommand(CreateProductExternalDto product) : IRequest<Guid>;
}
