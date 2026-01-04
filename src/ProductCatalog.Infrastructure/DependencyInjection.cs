using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.Categories;
using ProductCatalog.Infrastructure.Repositories.Currencies;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductCatalog.Infrastructure.Repositories.Products;

namespace ProductCatalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var cs = configuration.GetConnectionString("ProductCatalogDb");

            services.AddDbContext<ProductsContext>(options =>
                options.UseSqlServer(cs, sql =>
                {
                    sql.MigrationsHistoryTable("__EFMigrationsHistory");
                }));

            services.AddScoped<IProductsCommandsRepository, ProductsCommandsRepository>();
            services.AddScoped<IProductsQueriesRepository, ProductsQueriesRepository>();

            services.AddScoped<ICategoriesCommandsRepository, CategoriesCommandsRepository>();
            services.AddScoped<ICategoriesQueriesRepository, CategoriesQueriesRepository>();

            services.AddScoped<ICurrenciesQueriesRepository, CurrenciesQueriesRepository>();
            services.AddScoped<ICurrenciesCommandsRepository, CurrenciesCommandsRepository>();

            services.AddScoped<IMobilePhonesCommandsRepository, MobilePhonesCommandsRepository>();

            return services;
        }
    }
}
