using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    internal class GetProductsByCategoryIdQueryHandler(IProductsQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductsByCategoryIdQuery, IReadOnlyList<ProductDto>?>
    {
        public async Task<IReadOnlyList<ProductDto>?> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetByCategoryIdAsync(request.categoryId, cancellationToken);
            return result.Adapt<List<ProductDto>>();
        }
    }
}
