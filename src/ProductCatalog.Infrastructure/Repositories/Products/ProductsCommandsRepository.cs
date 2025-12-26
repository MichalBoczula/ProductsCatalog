using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Repositories.Products
{
    internal sealed class ProductsCommandsRepository : IProductsCommandsRepository
    {
        private readonly ProductsContext _context;

        public ProductsCommandsRepository(ProductsContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public void WriteHistory(ProductsHistory entity)
        {
            _context.ProductsHistories.Add(entity);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
