using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    internal sealed class CreateProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validationPolicy,
         CreateProductCommandFlowDescribtor _createProductCommandFlowDescribtor)
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _createProductCommandFlowDescribtor.MapRequestToProductAggregate(request);
            var validationResult = await _createProductCommandFlowDescribtor.ValidateProductAggregate(product, _validationPolicy);
            _createProductCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);
            _createProductCommandFlowDescribtor.AddProductToRepository(product, _productCommandsRepository);
            var productsHistory = _createProductCommandFlowDescribtor.CreateProductHistoryEntry(product);
            _createProductCommandFlowDescribtor.WriteHistoryToRepository(_productCommandsRepository, productsHistory);
            await _createProductCommandFlowDescribtor.SaveChanges(_productCommandsRepository, cancellationToken);
            var result = _createProductCommandFlowDescribtor.MapProductToProductDto(product);
            return result;
        }
    }
}