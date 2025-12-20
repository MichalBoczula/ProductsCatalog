using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler(IProductQueriesRepository _productQueriesRepository)
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueriesRepository.GetByIdAsync(request.id, cancellationToken);
            return result.Adapt<ProductDto>();
        }
    }
}
