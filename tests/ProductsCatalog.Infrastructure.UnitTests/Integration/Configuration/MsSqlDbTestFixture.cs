using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Testcontainers.MsSql;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration
{
    public sealed class MsSqlDbTestFixture : IAsyncLifetime
    {
        private const string Database = "IntegrationTestDb";
        private const string Username = "sa";
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1433;
        private readonly MsSqlContainer _msSqlContainer;
        private string _connectionString = string.Empty;
        public string ConnectionString => _connectionString;

        public MsSqlDbTestFixture()
        {
            _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(MsSqlPort, true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SQLCMDUSER", Username)
                .WithEnvironment("SQLCMDPASSWORD", Password)
                .WithEnvironment("MSSQL_SA_PASSWORD", Password)
                .WithWaitStrategy(Wait.ForUnixContainer()
                        .UntilExternalTcpPortIsAvailable(MsSqlPort))
                .Build();
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

            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(_connectionString, sql =>
            {
                sql.MigrationsHistoryTable("__EFMigrationsHistory");
            });

            using var dbContext = new ProductsContext(optionsBuilder.Options);
            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
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