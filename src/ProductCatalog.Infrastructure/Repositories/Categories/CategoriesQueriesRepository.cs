using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Common;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories.Categories
{
    internal sealed class CategoriesQueriesRepository : ICategoriesQueriesRepository
    {
        private readonly string _connectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public CategoriesQueriesRepository(IConfiguration configuration)
        {
            ConnectionStringExtensions.Initialize(configuration);
        }

        public Task<CategoryReadModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<CategoryReadModel>> GetCategories(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
