using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductCommandsRepository _productCommandsRepository;

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.product.Adapt<Product>();
            await _productCommandsRepository.AddAsync(product, cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
