using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductQueriesRepository
    {
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ProductDto>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct);
    }
}
