using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories
{
    internal sealed class ProductCommandsRepository : IProductCommandsRepository
    {
        private readonly ProductsContext _context;

        public ProductCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(ct);
        }

        public Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Update(product);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
