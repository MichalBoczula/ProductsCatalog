using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace ProductCatalog.Acceptance.Tests
{
    internal static class AcceptanceSqlServer
    {
        private const string Username = "sa";
        private const string Password = "yourStrong(!)Password";
        private const ushort MsSqlPort = 1433;

        private static MsSqlContainer? _container;
        private static string _connectionString = string.Empty;

        public static async Task StartAsync()
        {
            if (_container is not null)
            {
                throw new InvalidOperationException("The acceptance SQL container is already running.");
            }

            var container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(MsSqlPort, true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SQLCMDUSER", Username)
                .WithEnvironment("SQLCMDPASSWORD", Password)
                .WithEnvironment("MSSQL_SA_PASSWORD", Password)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilExternalTcpPortIsAvailable(MsSqlPort))
                .Build();

            try
            {
                await container.StartAsync();
                var baseConnectionString =
                    $"Server={container.Hostname},{container.GetMappedPublicPort(MsSqlPort)};" +
                    $"User Id={Username};Password={Password};Database=master;" +
                    "TrustServerCertificate=True;Encrypt=False;Connection Timeout=5;";

                await WaitUntilSqlIsReady(baseConnectionString);
                _connectionString = baseConnectionString;
                _container = container;
            }
            catch
            {
                await container.DisposeAsync();
                throw;
            }
        }

        public static string ForDatabase(string databaseName)
        {
            ValidateDatabaseName(databaseName);
            if (_container is null)
            {
                throw new InvalidOperationException("The acceptance SQL container has not been started.");
            }

            return new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = databaseName
            }.ConnectionString;
        }

        public static async Task DropDatabaseAsync(string databaseName)
        {
            ValidateDatabaseName(databaseName);
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = $"""
                IF DB_ID(N'{databaseName}') IS NOT NULL
                BEGIN
                    ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{databaseName}];
                END
                """;
            command.CommandTimeout = 30;
            await command.ExecuteNonQueryAsync();
        }

        public static async Task DisposeAsync()
        {
            var container = _container;
            _container = null;
            _connectionString = string.Empty;
            if (container is not null)
            {
                await container.DisposeAsync();
            }
        }

        private static void ValidateDatabaseName(string databaseName)
        {
            if (databaseName.Length != 43 ||
                !databaseName.StartsWith("acceptance_", StringComparison.Ordinal) ||
                !Guid.TryParseExact(databaseName[11..], "N", out _))
            {
                throw new ArgumentException("Invalid acceptance database name.", nameof(databaseName));
            }
        }

        private static async Task WaitUntilSqlIsReady(string connectionString)
        {
            for (var attempt = 0; attempt < 30; attempt++)
            {
                try
                {
                    await using var connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();
                    return;
                }
                catch (SqlException) when (attempt < 29)
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}
