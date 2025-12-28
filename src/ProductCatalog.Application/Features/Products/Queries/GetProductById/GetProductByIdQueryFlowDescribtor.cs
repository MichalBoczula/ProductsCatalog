using Mapster;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Products.Queries.GetProductById
{
    internal sealed class GetProductByIdQueryFlowDescribtor : FlowDescriberBase<GetProductByIdQuery>
    {
        [FlowStep(1)]
        public Task<ProductReadModel?> GetProduct(IProductsQueriesRepository productQueriesRepository, Guid productId, CancellationToken cancellationToken)
        {
            return productQueriesRepository.GetById(productId, cancellationToken);
        }

        [FlowStep(2)]
        public ProductReadModel EnsureProductFound(ProductReadModel? product, Guid productId)
        {
            if (product is null)
            {
                throw new ResourceNotFoundException(nameof(GetProductByIdQuery), productId, nameof(ProductDto));
            }

            return product;
        }

        [FlowStep(3)]
        public ProductDto MapProductToDto(ProductReadModel product)
        {
            return product.Adapt<ProductDto>();
        }
    }
}
