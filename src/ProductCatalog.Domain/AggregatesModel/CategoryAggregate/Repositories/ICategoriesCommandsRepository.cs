using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;

namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories
{
    public interface ICategoriesCommandsRepository
    {
        void Add(Category category);
        void Update(Category category);
        Task<Category?> GetCategoryById(Guid categoryId, CancellationToken cancellationToken);
        void WriteHistory(CategoriesHistory entity);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
