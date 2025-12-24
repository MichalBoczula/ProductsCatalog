namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories
{
    public interface ICategoriesCommandsRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task Update(Category category, CancellationToken cancellationToken);
        Task<Category?> GetCategoryById(Guid categoryId, CancellationToken cancellationToken);
    }
}
