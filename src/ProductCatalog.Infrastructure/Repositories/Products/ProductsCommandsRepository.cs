using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories.Products
{
    internal sealed class ProductsCommandsRepository : IProductCommandsRepository
    {
        private readonly ProductsContext _context;

        public ProductsCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
            return product;
        }
    }
}
