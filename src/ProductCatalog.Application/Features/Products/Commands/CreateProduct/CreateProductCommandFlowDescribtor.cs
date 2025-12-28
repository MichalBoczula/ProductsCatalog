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


namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    internal sealed class CreateProductCommandFlowDescribtor : FlowDescriberBase<CreateProductCommand>
    {
        [FlowStep(1)]
        public Product MapRequestToProductAggregate(CreateProductCommand _command)
        {
            var product = _command.product.Adapt<Product>();
            return product;
        }

        [FlowStep(2)]
        public Task<ValidationResult> ValidateProductAggregate(Product product, IValidationPolicy<Product> validationPolicy)
        {
            var validationResult = validationPolicy.Validate(product);
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
        public void AddProductToRepository(Product product, IProductsCommandsRepository _productCommandsRepository)
        {
            _productCommandsRepository.Add(product);
        }

        [FlowStep(5)]
        public ProductsHistory CreateProductHistoryEntry(Product product)
        {
            var productsHistory = product.BuildAdapter()
                .AddParameters("operation", Operation.Inserted)
                .AdaptToType<ProductsHistory>();

            return productsHistory;
        }

        [FlowStep(6)]
        public void WriteHistoryToRepository(IProductsCommandsRepository _productCommandsRepository, ProductsHistory productsHistory)
        {
            _productCommandsRepository.WriteHistory(productsHistory);
        }

        [FlowStep(7)]
        public Task SaveChanges(IProductsCommandsRepository _productCommandsRepository, CancellationToken cancellationToken)
        {
            return _productCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(8)]
        public ProductDto MapProductToProductDto(Product product)
        {
            var productDto = product.Adapt<ProductDto>();
            return productDto;
        }
    }
}