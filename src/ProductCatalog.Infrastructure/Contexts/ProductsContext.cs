using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Infrastructure.Configuration.Aggregates;
using ProductCatalog.Infrastructure.Configuration.DataSeed;
using ProductCatalog.Infrastructure.Configuration.Histories;

namespace ProductCatalog.Infrastructure.Contexts.Commands
{
    internal class ProductsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ProductsHistory> ProductsHistories { get; set; }
        public DbSet<CategoriesHistory> CategoriesHistories { get; set; }
        public DbSet<CurrenciesHistory> CurrenciesHistories { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new CurrenciesConfiguration());

            modelBuilder.ApplyConfiguration(new ProductsHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CurrenciesHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesHistoryConfiguration());

            modelBuilder.SeedData();
        }
    }
}
