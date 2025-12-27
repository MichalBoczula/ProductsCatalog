using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    internal sealed class CreateProductCommandHandler
        (IProductsCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validationPolicy)
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.product.Adapt<Product>();
            var validationResult = await _validationPolicy.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            _productCommandsRepository.Add(product);

            var productsHistory = product.BuildAdapter()
                            .AddParameters("operation", Operation.Inserted)
                            .AdaptToType<ProductsHistory>();

            _productCommandsRepository.WriteHistory(productsHistory);
            await _productCommandsRepository.SaveChanges(cancellationToken);
            
            return product.Adapt<ProductDto>();
        }
    }
}