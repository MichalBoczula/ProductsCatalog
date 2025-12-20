using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Infrastructure.Configuration;

namespace ProductCatalog.Infrastructure.Contexts.Commands
{
    internal class ProductsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
        }
    }
}
