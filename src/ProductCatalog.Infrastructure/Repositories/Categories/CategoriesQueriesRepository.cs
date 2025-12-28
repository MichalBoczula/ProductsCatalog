using Dapper;
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
            _connectionString = ConnectionStringExtensions.Initialize(configuration);
        }

        public Task<CategoryReadModel?> GetById(Guid id, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                        Code,
                        Name,
                        IsActive
                FROM {SqlTableNames.Categories}
                WHERE Id = @Id;
                ";

            using var connection = CreateConnection();

            var result = connection.QuerySingleOrDefaultAsync<CategoryReadModel?>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return result;
        }

        public async Task<IReadOnlyList<CategoryReadModel>> GetCategories(CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                        Code,
                        Name,
                        IsActive
                FROM {SqlTableNames.Categories}
                ";

            using var connection = CreateConnection();

            var result = await connection.QueryAsync<CategoryReadModel>(
                new CommandDefinition(sql, ct));

            return result.ToList().AsReadOnly();
        }
    }
}
