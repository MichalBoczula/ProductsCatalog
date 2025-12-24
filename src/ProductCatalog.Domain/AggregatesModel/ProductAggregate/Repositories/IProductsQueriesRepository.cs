using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductsQueriesRepository
    {
        Task<ProductReadModel?> GetById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ProductReadModel>> GetByCategoryId(Guid categoryId, CancellationToken ct);
    }
}
