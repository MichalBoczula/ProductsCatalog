using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure.Extensions
{
    public static class DataSeedExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            SeedCategoriesAsync(context);
            SeedCurrenciesAsync(context);
            SeedMobilePhonesAsync(context);

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static void SeedCategoriesAsync(ProductsContext context)
        {
            if (context.Categories.Any())
                return;

            var categories = new[]
            {
                new Category("MOBILE", "Mobile"),
                new Category("PC", "Personal Computer"),
                new Category("TABLET", "Tablet")
            };

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }
        }

        private static void SeedCurrenciesAsync(ProductsContext context)
        {
            if (context.Currencies.Any())
                return;

            var currencies = new[]
            {
                new Currency("USD", "US Dollar"),
                new Currency("PLN", "Polish Złoty"),
                new Currency("EUR", "Euro")
            };

            foreach (var currency in currencies)
            {
                context.Currencies.Add(currency);
            }
        }

        private static void SeedMobilePhonesAsync(ProductsContext context)
        {
            if (context.MobilePhones.Any())
                return;

            var mobileCategory = context.Categories.Local.FirstOrDefault(category => category.Code == "MOBILE")
                ?? context.Categories.FirstOrDefault(category => category.Code == "MOBILE");

            if (mobileCategory is null)
                return;

            var samsungGalaxyA56 = new MobilePhone(
                new CommonDescription(
                    "Samsung Galaxy A56 5G 8/256GB Czarny",
                    "Nagrywaj niesamowite wideo selfie w Super HDR; Filmuj tętniące życiem noce; Rób niesamowite selfie aparatem 12 MP.",
                    "samsung-galaxy-a56-5g-black-main.jpg",
                    new[]
                    {
                        "samsung-galaxy-a56-5g-black-1.jpg",
                        "samsung-galaxy-a56-5g-black-2.jpg"
                    }),
                new ElectronicDetails(
                    "Samsung Exynos 1580 (1x 2.9 GHz, A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)",
                    "Brak danych",
                    "8 GB",
                    "256 GB",
                    "Super AMOLED",
                    120,
                    6.7m,
                    2340,
                    1080,
                    "Li-Ion",
                    5000),
                new Connectivity(true, true, true, true),
                new SatelliteNavigationSystem(true, true, true, true, true),
                new Sensors(true, true, true, true, false, true, true),
                true,
                false,
                mobileCategory.Id,
                new Money(1999.00m, "PLN"));

            context.MobilePhones.Add(samsungGalaxyA56);
        }
    }
}
