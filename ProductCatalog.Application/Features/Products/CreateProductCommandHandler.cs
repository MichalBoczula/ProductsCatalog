using MediatR;
using ProductCatalog.Application.Common.Dtos.Internal;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        IProductCommandsRepository _productCommandsRepository;

        public Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
