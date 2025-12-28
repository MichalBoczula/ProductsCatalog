using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    internal sealed class GetProductsByCategoryIdQueryHandler
        (IProductsQueriesRepository _productQueriesRepository,
         GetProductsByCategoryIdQueryFlowDescribtor _getProductsByCategoryIdQueryFlowDescribtor)
        : IRequestHandler<GetProductsByCategoryIdQuery, IReadOnlyList<ProductDto>?>
    {
        public async Task<IReadOnlyList<ProductDto>?> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _getProductsByCategoryIdQueryFlowDescribtor.GetProducts(_productQueriesRepository, request.categoryId, cancellationToken);
            var existingProducts = _getProductsByCategoryIdQueryFlowDescribtor.EnsureProductsFound(result, request.categoryId);
            return _getProductsByCategoryIdQueryFlowDescribtor.MapProductsToDto(existingProducts);
        }
    }
}
