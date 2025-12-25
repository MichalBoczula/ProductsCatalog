using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    internal sealed class UpdateProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validationPolicy)
        : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var incoming = request.product.Adapt<Product>();
            var validationResultIncoming = _validationPolicy.Validate(incoming);
            if (!validationResultIncoming.IsValid)
            {
                throw new ValidationException(validationResultIncoming);
            }

            var product = await _productCommandsRepository.GetProductById(request.productId, cancellationToken);
            
            var validationResultExisting = _validationPolicy.Validate(product);
            if (!validationResultExisting.IsValid)
            {
                throw new ValidationException(validationResultExisting);
            }

            product.AssigneNewProductInformation(incoming);

            await _productCommandsRepository.Update(product, cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
