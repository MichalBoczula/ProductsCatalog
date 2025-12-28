using Mapster;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.RemoveProduct
{
    internal sealed class RemoveProductCommandFlowDescribtor : FlowDescriberBase<RemoveProductCommand>
    {
        [FlowStep(1)]
        public Task<Product> LoadProduct(Guid productId, IProductsCommandsRepository productCommandsRepository, CancellationToken cancellationToken)
        {
            return productCommandsRepository.GetProductById(productId, cancellationToken);
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateProduct(Product product, IValidationPolicy<Product> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(product);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public void DeactivateProduct(Product product)
        {
            product.Deactivate();
        }

        [FlowStep(5)]
        public void UpdateProductInRepository(Product product, IProductsCommandsRepository productCommandsRepository)
        {
            productCommandsRepository.Update(product);
        }

        [FlowStep(6)]
        public ProductsHistory CreateProductHistoryEntry(Product product)
        {
            var productsHistory = product.BuildAdapter()
                .AddParameters("operation", Operation.Deleted)
                .AdaptToType<ProductsHistory>();

            return productsHistory;
        }

        [FlowStep(7)]
        public void WriteHistoryToRepository(IProductsCommandsRepository productCommandsRepository, ProductsHistory productsHistory)
        {
            productCommandsRepository.WriteHistory(productsHistory);
        }

        [FlowStep(8)]
        public async Task SaveChanges(IProductsCommandsRepository productCommandsRepository, CancellationToken cancellationToken)
        {
            await productCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(9)]
        public ProductDto MapProductToProductDto(Product product)
        {
            var productDto = product.Adapt<ProductDto>();
            return productDto;
        }
    }
}
