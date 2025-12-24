using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Infrastructure.Common;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories.Categories
{
    internal sealed class CategoriesQueriesRepository
    {
        private readonly string _connectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public CategoriesQueriesRepository(IConfiguration configuration)
        {
            ConnectionStringExtensions.Initialize(configuration);
        }

    }
}
