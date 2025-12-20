using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;

namespace ProductCatalog.Infrastructure.Repositories
{
    internal class ProductQueriesRepository : IProductQueriesRepository
    {
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetTopTen(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByCategoryId(Guid CategoryId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
