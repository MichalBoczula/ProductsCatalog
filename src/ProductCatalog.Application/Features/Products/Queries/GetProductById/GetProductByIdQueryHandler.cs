using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal sealed class GetProductByIdQueryHandler
        (IProductsQueriesRepository _productQueriesRepository,
         GetProductByIdQueryFlowDescribtor _getProductByIdQueryFlowDescribtor)
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _getProductByIdQueryFlowDescribtor.GetProduct(_productQueriesRepository, request.id, cancellationToken);
            var existingProduct = _getProductByIdQueryFlowDescribtor.EnsureProductFound(product, request.id);
            return _getProductByIdQueryFlowDescribtor.MapProductToDto(existingProduct);
        }
    }
}
