using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductQueriesRepository
    {
        Task<ProductReadModel?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ProductReadModel>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct);
    }
}
