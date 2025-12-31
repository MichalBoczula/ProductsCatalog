using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories.Categories
{
    internal sealed class CategoriesCommandsRepository : ICategoriesCommandsRepository
    {
        private readonly ProductsContext _context;

        public CategoriesCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task<Category?> GetCategoryById(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
            return category;
        }

        public void WriteHistory(CategoriesHistory entity)
        {
            _context.CategoriesHistories.Add(entity);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
