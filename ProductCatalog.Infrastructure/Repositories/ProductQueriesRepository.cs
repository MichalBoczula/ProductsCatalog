using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Common;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories
{
    internal class ProductQueriesRepository : IProductQueriesRepository
    {
        private readonly string _connectionString;

        public ProductQueriesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProductCatalogDb")
                ?? throw new InvalidOperationException("Connection string 'ProductCatalogDb' is not configured.");
        }
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                       Name,
                       Description,
                       PriceAmount,
                       PriceCurrency,
                       IsActive,
                       CategoryId
                FROM {SqlTableNames.Products}
                WHERE Id = @Id;
                ";

            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<ProductDto>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        }

        public async Task<IReadOnlyList<ProductDto>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                       Name,
                       Description,
                       PriceAmount,
                       PriceCurrency,
                       IsActive,
                       CategoryId
                FROM {SqlTableNames.Products}
                WHERE CategoryId = @CategoryId
                ORDER BY Name;
                ";

            using var connection = CreateConnection();
            var results = await connection.QueryAsync<ProductDto>(
                new CommandDefinition(sql, new { CategoryId = categoryId }, cancellationToken: ct));
            return results.AsList();
        }
    }
}
