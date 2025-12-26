using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
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
            _productCommandsRepository.Update(product);

            var productsHistory = new ProductsHistory
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                PriceAmount = product.Price.Amount,
                PriceCurrency = product.Price.Currency,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                ChangedAt = product.ChangedAt,
                Operation = Operation.Updated
            };

            _productCommandsRepository.WriteHistory(productsHistory);
            
            await _productCommandsRepository.SaveChanges(cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
