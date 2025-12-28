using Mapster;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId
{
    internal sealed class GetProductsByCategoryIdQueryFlowDescribtor : FlowDescriberBase<GetProductsByCategoryIdQuery>
    {
        [FlowStep(1)]
        public Task<IReadOnlyList<ProductReadModel>> GetProducts(IProductsQueriesRepository productQueriesRepository, Guid categoryId, CancellationToken cancellationToken)
        {
            return productQueriesRepository.GetByCategoryId(categoryId, cancellationToken);
        }

        [FlowStep(2)]
        public IReadOnlyList<ProductReadModel> EnsureProductsFound(IReadOnlyList<ProductReadModel> products, Guid categoryId)
        {
            if (products is null)
            {
                throw new ResourceNotFoundException(nameof(GetProductsByCategoryIdQuery), categoryId, nameof(List<ProductDto>));
            }

            return products;
        }

        [FlowStep(3)]
        public IReadOnlyList<ProductDto> MapProductsToDto(IReadOnlyList<ProductReadModel> products)
        {
            return products.Adapt<List<ProductDto>>().AsReadOnly();
        }
    }
}
