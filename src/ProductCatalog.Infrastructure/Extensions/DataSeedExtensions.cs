using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
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

    }
}
