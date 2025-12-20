using MediatR;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    internal class GetProductsByCategoryIdQueryHandler(IProductQueriesRepository _productQueriesRepository)
        :IRequestHandler<GetProductsByCategoryIdQuery, IReadOnlyList<ProductDto>?>
    {
        public async Task<IReadOnlyList<ProductDto>?> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetByCategoryIdAsync(request.categoryId, cancellationToken);
            return result;
        }
    }
}
