using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Infrastructure.Configuration.DataSeed
{
    internal static class DateSeedExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedCurrencies(modelBuilder);
        }

        private static void SeedCategories(ModelBuilder modelBuilder)
        {
            var category1 = new Category("MOBILE", "Mobile");
            var category2 = new Category("PC", "Personal Computer");
            var category3 = new Category("TABLET", "Tablet");

            modelBuilder.Entity<Category>().HasData(
                category1,
                category2,
                category3
            );

            var categoriesHistory1 = new CategoriesHistory
            {
                CategoryId = category1.Id,
                Code = category1.Code,
                Name = category1.Name,
                ChangedAt = category1.ChangedAt,
                IsActive = category1.IsActive,
                Operation = Operation.Inserted
            };

            var categoriesHistory2 = new CategoriesHistory
            {
                CategoryId = category2.Id,
                Code = category2.Code,
                Name = category2.Name,
                ChangedAt = category2.ChangedAt,
                IsActive = category2.IsActive,
                Operation = Operation.Inserted
            };

            var categoriesHistory3 = new CategoriesHistory
            {
                CategoryId = category3.Id,
                Code = category3.Code,
                Name = category3.Name,
                ChangedAt = category3.ChangedAt,
                IsActive = category3.IsActive,
                Operation = Operation.Inserted
            };

            modelBuilder.Entity<CategoriesHistory>().HasData(
                categoriesHistory1,
                categoriesHistory2,
                categoriesHistory3
            );
        }

        private static void SeedCurrencies(ModelBuilder modelBuilder)
        {
            var currency1 = new Currency("USD", "US Dollar");
            var currency2 = new Currency("PLN", "Polish Złoty");
            var currency3 = new Currency("EUR", "Euro");

            modelBuilder.Entity<Currency>().HasData(
                currency1,
                currency2,
                currency3
            );

            var currenciesHistory1 = new CurrenciesHistory
            {
                CurrencyId = currency1.Id,
                Code = currency1.Code,
                Description = currency1.Description,
                ChangedAt = currency1.ChangedAt,
                IsActive = currency1.IsActive,
                Operation = Operation.Inserted
            };

            var currenciesHistory2 = new CurrenciesHistory
            {
                CurrencyId = currency2.Id,
                Code = currency2.Code,
                Description = currency2.Description,
                ChangedAt = currency2.ChangedAt,
                IsActive = currency2.IsActive,
                Operation = Operation.Inserted
            };

            var currenciesHistory3 = new CurrenciesHistory
            {
                CurrencyId = currency3.Id,
                Code = currency3.Code,
                Description = currency3.Description,
                ChangedAt = currency3.ChangedAt,
                IsActive = currency3.IsActive,
                Operation = Operation.Inserted
            };

            modelBuilder.Entity<CurrenciesHistory>().HasData(
                currenciesHistory1,
                currenciesHistory2,
                currenciesHistory3
            );
        }
    }
}
