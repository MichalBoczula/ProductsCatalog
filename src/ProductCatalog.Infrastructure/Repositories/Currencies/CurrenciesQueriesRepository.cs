using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Common;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories.Currencies
{
    internal class CurrenciesQueriesRepository : ICurrenciesQueriesRepository
    {
        private readonly string _connectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public CurrenciesQueriesRepository(IConfiguration configuration)
        {
            _connectionString = ConnectionStringExtensions.Initialize(configuration);
        }

        public async Task<IReadOnlyList<CurrencyReadModel>> GetCurrencies(CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                        Code,
                        Description,
                        IsActive
                FROM {SqlTableNames.Currencies}
                ";

            using var connection = CreateConnection();

            var result = await connection.QueryAsync<CurrencyReadModel>(
                new CommandDefinition(sql, ct));

            return result.ToList().AsReadOnly();
        }
    }
}
