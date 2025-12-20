using Mapster;
using MediatR;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler(IProductCommandsRepository _productCommandsRepository)
        :IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.product.Adapt<Product>();
            await _productCommandsRepository.UpdateAsync(product, cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
