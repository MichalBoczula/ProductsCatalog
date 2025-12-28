using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Products.Commands.RemoveProduct
{
    internal sealed class RemoveProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validation,
         RemoveProductCommandFlowDescribtor _removeProductCommandFlowDescribtor)
        : IRequestHandler<RemoveProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _removeProductCommandFlowDescribtor
                .LoadProduct(request.productId, _productCommandsRepository, cancellationToken);

            var validationResult = await _removeProductCommandFlowDescribtor
                .ValidateProduct(product, _validation);
            _removeProductCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _removeProductCommandFlowDescribtor.DeactivateProduct(product);
            _removeProductCommandFlowDescribtor.UpdateProductInRepository(product, _productCommandsRepository);

            var productsHistory = _removeProductCommandFlowDescribtor.CreateProductHistoryEntry(product);

            _removeProductCommandFlowDescribtor.WriteHistoryToRepository(_productCommandsRepository, productsHistory);

            await _removeProductCommandFlowDescribtor.SaveChanges(_productCommandsRepository, cancellationToken);
            return _removeProductCommandFlowDescribtor.MapProductToProductDto(product);
        }
    }
}
