using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.RemoveProduct
{
    internal sealed class RemoveProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validation)
        : IRequestHandler<RemoveProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productCommandsRepository.GetProductById(request.productId, cancellationToken);
            var validationResult = _validation.Validate(product);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            product.Deactivate();
            _productCommandsRepository.Update(product);

            var productsHistory = product.BuildAdapter()
                           .AddParameters("operation", Operation.Deleted)
                           .AdaptToType<ProductsHistory>();

            _productCommandsRepository.WriteHistory(productsHistory);

            await _productCommandsRepository.SaveChanges(cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
