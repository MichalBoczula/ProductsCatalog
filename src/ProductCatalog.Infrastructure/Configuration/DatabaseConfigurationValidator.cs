using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Infrastructure.Configuration;

public static class DatabaseConfigurationValidator
{
    public static bool Validate(IConfiguration configuration)
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
        if (applyMigrations is null)
        {
            return false;
        }

        if (!bool.TryParse(applyMigrations, out var enabled))
        {
            throw new InvalidOperationException("Database:ApplyMigrations must be true or false.");
        }

        return enabled;
    }
}
