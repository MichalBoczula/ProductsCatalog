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

        public async Task<ProductReadModel?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                        Name,
                        Description,
                        IsActive,
                        CategoryId,
                        PriceAmount,
                        PriceCurrency
                FROM {SqlTableNames.Products}
                WHERE Id = @Id;
                ";

            using var connection = CreateConnection();

            var result = await connection.QuerySingleOrDefaultAsync<ProductReadModel?>(
                sql, new { Id = id });

            return result;
        }

        public async Task<IReadOnlyList<ProductReadModel>?> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct)
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
            var result = await connection.QueryAsync<ProductReadModel>(
                new CommandDefinition(sql, new { CategoryId = categoryId }, cancellationToken: ct)
             );

            return result.ToList();
        }
    }
}
