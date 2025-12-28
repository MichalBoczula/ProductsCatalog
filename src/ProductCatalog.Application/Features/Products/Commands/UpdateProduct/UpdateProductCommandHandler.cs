using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    internal sealed class UpdateProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validationPolicy,
         UpdateProductCommandFlowDescribtor _updateProductCommandFlowDescribtor)
        : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var incoming = _updateProductCommandFlowDescribtor.MapRequestToProductAggregate(request);
            var validationResultIncoming = await _updateProductCommandFlowDescribtor
                .ValidateIncomingProduct(incoming, _validationPolicy);
            _updateProductCommandFlowDescribtor.ThrowValidationExceptionIfIncomingInvalid(validationResultIncoming);

            var product = await _updateProductCommandFlowDescribtor
                .LoadExistingProduct(request.productId, _productCommandsRepository, cancellationToken);

            var validationResultExisting = await _updateProductCommandFlowDescribtor
                .ValidateExistingProduct(product, _validationPolicy);
            _updateProductCommandFlowDescribtor.ThrowValidationExceptionIfExistingInvalid(validationResultExisting);

            _updateProductCommandFlowDescribtor.AssignNewProductInformation(product, incoming);
            _updateProductCommandFlowDescribtor.UpdateProductInRepository(product, _productCommandsRepository);

            var productsHistory = _updateProductCommandFlowDescribtor.CreateProductHistoryEntry(product);

            _updateProductCommandFlowDescribtor.WriteHistoryToRepository(_productCommandsRepository, productsHistory);

            await _updateProductCommandFlowDescribtor.SaveChanges(_productCommandsRepository, cancellationToken);
            return _updateProductCommandFlowDescribtor.MapProductToProductDto(product);
        }
    }
}
