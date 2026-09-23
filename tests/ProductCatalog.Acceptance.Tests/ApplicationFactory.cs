using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Api;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Acceptance.Tests
{
    public class ApplicationFactory(string connectionString, bool? applyMigrations = false) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = connectionString
                };
                if (applyMigrations.HasValue)
                {
                    settings["Database:ApplyMigrations"] = applyMigrations.Value.ToString();
                }

                config.AddInMemoryCollection(settings);
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ProductsContext>>();
                services.RemoveAll<ProductsContext>();

                services.AddDbContext<ProductsContext>(options =>
                    options.UseSqlServer(connectionString, sql =>
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory");
                    }));
            });
        }

        public async Task MigrateAsync()
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
