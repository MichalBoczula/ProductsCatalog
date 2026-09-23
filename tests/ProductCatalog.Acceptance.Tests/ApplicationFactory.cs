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
    public class ApplicationFactory(string connectionString) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = connectionString,
                    ["Database:ApplyMigrations"] = "false"
                });
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
