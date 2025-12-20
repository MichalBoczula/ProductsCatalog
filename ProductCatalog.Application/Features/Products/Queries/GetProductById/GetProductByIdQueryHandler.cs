using MediatR;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler(IProductQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetByIdAsync(request.id, cancellationToken);
            return result;
        }
    }
}
