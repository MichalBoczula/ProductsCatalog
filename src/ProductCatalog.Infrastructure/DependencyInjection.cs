using Microsoft.Data.SqlClient;
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
            var cs = ValidateDatabaseConfiguration(configuration);

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

        private static string ValidateDatabaseConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ProductCatalogDb");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("ConnectionStrings:ProductCatalogDb must be configured.");
            }

            try
            {
                var parsed = new SqlConnectionStringBuilder(connectionString);
                if (string.IsNullOrWhiteSpace(parsed.DataSource) || string.IsNullOrWhiteSpace(parsed.InitialCatalog))
                {
                    throw new InvalidOperationException(
                        "ConnectionStrings:ProductCatalogDb must specify a SQL Server and database.");
                }
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException(
                    "ConnectionStrings:ProductCatalogDb must be a valid SQL Server connection string.");
            }

            var applyMigrations = configuration["Database:ApplyMigrations"];
            if (applyMigrations is not null && !bool.TryParse(applyMigrations, out _))
            {
                throw new InvalidOperationException("Database:ApplyMigrations must be true or false.");
            }

            return connectionString;
        }
    }
}
