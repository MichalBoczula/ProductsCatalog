using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;

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
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));

            services.AddHealthChecks()
                .AddCheck<SqlServerHealthCheck>(
                    "sql-server",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["ready"]);



            services.AddScoped<IMobilePhonesCommandsRepository, MobilePhonesCommandsRepository>();
            services.AddScoped<IMobilePhonesQueriesRepository, MobilePhonesQueriesRepository>();

            return services;
        }
    }
}
