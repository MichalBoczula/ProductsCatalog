using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal sealed class GetProductByIdQueryHandler
        (IProductsQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetById(request.id, cancellationToken);
            if(result is null)
            {
                throw new ResourceNotFoundException(nameof(GetProductByIdQuery), request.id, nameof(ProductDto));
            }
            return result.Adapt<ProductDto>();
        }
    }
}
