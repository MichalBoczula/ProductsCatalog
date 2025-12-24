using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal sealed class GetProductByIdQueryHandler
        (IProductsQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetById(request.id, cancellationToken);
            return result.Adapt<ProductDto>();
        }
    }
}
