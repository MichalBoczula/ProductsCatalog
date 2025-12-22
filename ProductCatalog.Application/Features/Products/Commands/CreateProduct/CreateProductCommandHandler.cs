using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    internal class CreateProductCommandHandler
        (IProductCommandsRepository _productCommandsRepository,
         IValidationPolicy<Product> _validationPolicy)
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.product.Adapt<Product>();
            var validationResult = _validationPolicy.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            await _productCommandsRepository.AddAsync(product, cancellationToken);
            return product.Adapt<ProductDto>();
        }
    }
}
