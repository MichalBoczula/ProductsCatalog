using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Api;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Testcontainers.MsSql;

namespace ProductCatalog.Acceptance.Tests
{
    public class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string Database = "IntegrationTestDb";
        private const string Username = "sa";
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1433;

        private readonly MsSqlContainer _msSqlContainer;
        private string _connectionString = string.Empty;

        public ApplicationFactory()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(MsSqlPort, true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SQLCMDUSER", Username)
                .WithEnvironment("SQLCMDPASSWORD", Password)
                .WithEnvironment("MSSQL_SA_PASSWORD", Password)
                .WithWaitStrategy(Wait.ForUnixContainer()
                        .UntilExternalTcpPortIsAvailable(MsSqlPort))
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<ProductsContext>>();
                services.RemoveAll<ProductsContext>();

                services.AddDbContext<ProductsContext>(options =>
                    options.UseSqlServer(_connectionString, sql =>
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory");
                    }));
            });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            var host = _msSqlContainer.Hostname;
            var port = _msSqlContainer.GetMappedPublicPort(MsSqlPort);

            var baseCs =
                $"Server={host},{port};User Id={Username};Password={Password};" +
                $"TrustServerCertificate=True;Encrypt=False;Connection Timeout=5;";

            await WaitUntilSqlIsReady(baseCs); 

            _connectionString = baseCs + $"Database={Database};";

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            await dbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
        }

        private static async Task WaitUntilSqlIsReady(string cs)
        {
            for (var i = 0; i < 30; i++)
            {
                try
                {
                    using var conn = new SqlConnection(cs);
                    await conn.OpenAsync();
                    using var cmd = new SqlCommand("SELECT 1", conn);
                    await cmd.ExecuteScalarAsync();
                    return;
                }
                catch (SqlException)
                {
                    await Task.Delay(1000);
                }
            }

            throw new InvalidOperationException("SQL Server did not become ready in time.");
        }
    }
}