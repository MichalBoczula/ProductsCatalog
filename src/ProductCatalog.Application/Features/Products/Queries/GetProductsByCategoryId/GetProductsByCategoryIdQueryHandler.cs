using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    internal sealed class GetProductsByCategoryIdQueryHandler
        (IProductsQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductsByCategoryIdQuery, IReadOnlyList<ProductDto>?>
    {
        public async Task<IReadOnlyList<ProductDto>?> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetByCategoryId(request.categoryId, cancellationToken);
            if (result is null)
            {
                throw new ResourceNotFoundException(nameof(GetProductsByCategoryIdQuery), request.categoryId, nameof(List<ProductDto>));
            }
            return result.Adapt<List<ProductDto>>().AsReadOnly();
        }
    }
}
