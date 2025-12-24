using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.RemoveProduct
{
    internal sealed class RemoveProductCommandHandler
        (IProductCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validation)
        : IRequestHandler<RemoveProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productCommandsRepository.GetProductById(request.ProductId, cancellationToken);
            var validationResult = _validation.Validate(product);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            product.Deactivate();
            await _productCommandsRepository.UpdateAsync(product, cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
