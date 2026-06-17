using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Infrastructure.Configuration.Aggregates;
using ProductCatalog.Infrastructure.Configuration.DataSeed;
using ProductCatalog.Infrastructure.Configuration.Histories;

namespace ProductCatalog.Infrastructure.Contexts.Commands
{
    internal class ProductsContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<MobilePhone> MobilePhones { get; set; }
        public DbSet<CategoriesHistory> CategoriesHistories { get; set; }
        public DbSet<CurrenciesHistory> CurrenciesHistories { get; set; }
        public DbSet<MobilePhonesHistory> MobilePhonesHistories { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new CurrenciesConfiguration());
            modelBuilder.ApplyConfiguration(new MobilePhonesConfiguration());

            modelBuilder.ApplyConfiguration(new CurrenciesHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new MobilePhonesHistoryConfiguration());

            modelBuilder.SeedData();
        }
    }
}
