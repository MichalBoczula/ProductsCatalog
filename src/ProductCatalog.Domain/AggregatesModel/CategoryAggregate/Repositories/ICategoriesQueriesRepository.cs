using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories
{
    public interface ICategoriesQueriesRepository
    {
        Task<CategoryReadModel?> GetById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<CategoryReadModel>> GetCategories(CancellationToken ct);
    }
}
