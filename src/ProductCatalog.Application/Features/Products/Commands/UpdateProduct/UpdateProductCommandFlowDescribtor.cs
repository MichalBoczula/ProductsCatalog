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

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    internal sealed class UpdateProductCommandFlowDescribtor : FlowDescriberBase<UpdateProductCommand>
    {
        [FlowStep(1)]
        public Product MapRequestToProductAggregate(UpdateProductCommand command)
        {
            var product = command.product.Adapt<Product>();
            return product;
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateIncomingProduct(Product product, IValidationPolicy<Product> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(product);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfIncomingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public Task<Product> LoadExistingProduct(Guid productId, IProductsCommandsRepository productCommandsRepository, CancellationToken cancellationToken)
        {
            return productCommandsRepository.GetProductById(productId, cancellationToken);
        }

        [FlowStep(5)]
        public async Task<ValidationResult> ValidateExistingProduct(Product product, IValidationPolicy<Product> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(product);
            return validationResult;
        }

        [FlowStep(6)]
        public void ThrowValidationExceptionIfExistingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(7)]
        public void AssignNewProductInformation(Product product, Product incomingProduct)
        {
            product.AssigneNewProductInformation(incomingProduct);
        }

        [FlowStep(8)]
        public void UpdateProductInRepository(Product product, IProductsCommandsRepository productCommandsRepository)
        {
            productCommandsRepository.Update(product);
        }

        [FlowStep(9)]
        public ProductsHistory CreateProductHistoryEntry(Product product)
        {
            var productsHistory = product.BuildAdapter()
                .AddParameters("operation", Operation.Updated)
                .AdaptToType<ProductsHistory>();

            return productsHistory;
        }

        [FlowStep(10)]
        public void WriteHistoryToRepository(IProductsCommandsRepository productCommandsRepository, ProductsHistory productsHistory)
        {
            productCommandsRepository.WriteHistory(productsHistory);
        }

        [FlowStep(11)]
        public async Task SaveChanges(IProductsCommandsRepository productCommandsRepository, CancellationToken cancellationToken)
        {
            await productCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(12)]
        public ProductDto MapProductToProductDto(Product product)
        {
            var productDto = product.Adapt<ProductDto>();
            return productDto;
        }
    }
}
