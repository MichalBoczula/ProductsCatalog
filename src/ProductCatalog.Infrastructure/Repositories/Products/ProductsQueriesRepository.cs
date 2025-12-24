using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Common;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories.Products
{
    internal class ProductsQueriesRepository : IProductsQueriesRepository
    {
        private readonly string _connectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public ProductsQueriesRepository(IConfiguration configuration)
        {
            _connectionString = ConnectionStringExtensions.Initialize(configuration);
        }

        public async Task<ProductReadModel?> GetById(Guid id, CancellationToken ct)
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

        public async Task<IReadOnlyList<ProductReadModel>?> GetByCategoryId(Guid categoryId, CancellationToken ct)
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

            return result.ToList().AsReadOnly();
        }
    }
}
