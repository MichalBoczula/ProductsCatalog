using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Infrastructure.Common;
using ProductCatalog.Infrastructure.Extensions.Methods;
using System.Text;

namespace ProductCatalog.Infrastructure.Repositories.MobilePhones
{
    internal sealed class MobilePhonesQueriesRepository : IMobilePhonesQueriesRepository
    {
        private readonly string _connectionString;
        private const int ReadTimeoutSeconds = 12;
        private const int CommandTimeoutSeconds = 5;
        private const int MaxOpenAttempts = 3;

        public MobilePhonesQueriesRepository(IConfiguration configuration)
        {
            var connectionString = new SqlConnectionStringBuilder(ConnectionStringExtensions.Initialize(configuration));
            connectionString.ConnectTimeout = Math.Min(connectionString.ConnectTimeout, 3);
            _connectionString = connectionString.ConnectionString;
        }

        private async Task<T> ExecuteReadAsync<T>(Func<SqlConnection, CancellationToken, Task<T>> read, CancellationToken cancellationToken)
        {
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            deadline.CancelAfter(TimeSpan.FromSeconds(ReadTimeoutSeconds));

            try
            {
                await using var connection = await OpenConnectionAsync(deadline.Token);
                return await read(connection, deadline.Token);
            }
            catch (SqlException) when (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw;
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested && deadline.IsCancellationRequested)
            {
                throw new TimeoutException("The catalog read exceeded its time limit.");
            }
        }

        private async Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken)
        {
            for (var attempt = 1; ; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var connection = new SqlConnection(_connectionString);
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    return connection;
                }
                catch (SqlException exception) when (attempt < MaxOpenAttempts && IsTransientOpenFailure(exception) && !cancellationToken.IsCancellationRequested)
                {
                    await connection.DisposeAsync();
                    await Task.Delay(TimeSpan.FromMilliseconds(500 * attempt), cancellationToken);
                }
                catch
                {
                    await connection.DisposeAsync();
                    throw;
                }
            }
        }

        private static bool IsTransientOpenFailure(SqlException exception) =>
            exception.Errors.Cast<SqlError>().Any(error => error.Number is
                53 or 4060 or 927 or 942 or 10054 or 10060 or 10928 or 10929 or 40197 or 40501 or 40613);

        public async Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                       Name,
                       Brand,
                       Description,
                       MainPhoto,
                       OtherPhotos,
                       CPU,
                       GPU,
                       Ram,
                       Storage,
                       DisplayType,
                       RefreshRateHz,
                       ScreenSizeInches,
                       Width,
                       Height,
                       BatteryType,
                       BatteryCapacity,
                       GPS,
                       AGPS,
                       Galileo,
                       GLONASS,
                       QZSS,
                       Accelerometer,
                       Gyroscope,
                       Proximity,
                       Compass,
                       Barometer,
                       Halla,
                       AmbientLight,
                       [5G] AS Has5G,
                       WiFi,
                       NFC,
                       Bluetooth,
                       Camera,
                       FingerPrint,
                       FaceId,
                       PriceAmount,
                       PriceCurrency,
                       Description2,
                       Description3,
                       IsActive
                FROM {SqlTableNames.MobilePhones}
                WHERE Id = @Id;
                ";

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QuerySingleOrDefaultAsync<MobilePhoneReadModel?>(
                    new CommandDefinition(sql, new { Id = id }, commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result;
            }, ct);
        }

        public async Task<IReadOnlyList<MobilePhoneReadModel>> GetByIds(IReadOnlyCollection<Guid> ids, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                       Name,
                       Brand,
                       Camera,
                       DisplayType,
                       ScreenSizeInches,
                       PriceAmount,
                       PriceCurrency,
                       IsActive
                FROM {SqlTableNames.MobilePhones}
                WHERE Id IN @Ids
                  AND IsActive = 1
                ORDER BY Id;
                ";

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QueryAsync<MobilePhoneReadModel>(
                    new CommandDefinition(sql, new { Ids = ids }, commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result.ToList().AsReadOnly();
            }, ct);
        }

        public async Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct)
        {
            var sql = $@"
                SELECT TOP (@Amount)
                       Id,
                       Name,
                       Brand,
                       Camera,
                       DisplayType,
                       ScreenSizeInches,
                       PriceAmount,
                       PriceCurrency,
                       IsActive
                FROM {SqlTableNames.MobilePhones}
                WHERE IsActive = 1
                ORDER BY Name, Id;
                ";

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QueryAsync<MobilePhoneReadModel>(
                    new CommandDefinition(sql, new { Amount = amount }, commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result.ToList().AsReadOnly();
            }, ct);
        }

        public async Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken ct)
        {
            var offset = pageNumber - 1;
            var size = pageSize;

            var sql = $@"
                SELECT Id,
                       MobilePhoneId,
                       Name,
                       Brand,
                       Description,
                       MainPhoto,
                       OtherPhotos,
                       CPU,
                       GPU,
                       Ram,
                       Storage,
                       DisplayType,
                       RefreshRateHz,
                       ScreenSizeInches,
                       Width,
                       Height,
                       BatteryType,
                       BatteryCapacity,
                       GPS,
                       AGPS,
                       Galileo,
                       GLONASS,
                       QZSS,
                       Accelerometer,
                       Gyroscope,
                       Proximity,
                       Compass,
                       Barometer,
                       Halla,
                       AmbientLight,
                       Has5G,
                       WiFi,
                       NFC,
                       Bluetooth,
                       Camera,
                       FingerPrint,
                       FaceId,
                       PriceAmount,
                       PriceCurrency,
                       Description2,
                       Description3,
                       IsActive,
                       ChangedAt,
                       Operation
                FROM {SqlTableNames.MobilePhonesHistory}
                WHERE MobilePhoneId = @MobilePhoneId
                ORDER BY ChangedAt DESC, Id DESC
                OFFSET (@Offset * @PageSize) ROWS
                FETCH NEXT @PageSize ROWS ONLY;
                ";

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QueryAsync<MobilePhonesHistory>(
                    new CommandDefinition(
                        sql,
                        new
                        {
                            MobilePhoneId = mobilePhoneId,
                            Offset = offset,
                            PageSize = size
                        },
                        commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result.ToList().AsReadOnly();
            }, ct);
        }

        public async Task<IReadOnlyList<MobilePhoneReadModel>> GetTop(CancellationToken ct)
        {
            var sql = $@"
                SELECT TOP 3
                       Id,
                       Name,
                       Brand,
                       MainPhoto,
                       PriceAmount,
                       PriceCurrency
                FROM {SqlTableNames.MobilePhones}
                WHERE IsActive = 1
                ORDER BY ChangedAt DESC, Id DESC;
                ";

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QueryAsync<MobilePhoneReadModel>(
                    new CommandDefinition(sql, commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result.ToList().AsReadOnly();
            }, ct);
        }

        public async Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredPhones(
             MobilePhoneReadFilterDto mobilePhoneFilter,
             CancellationToken ct)
        {
            var query = new StringBuilder($@"
                SELECT
                    Id,
                    Name,
                    Brand,
                    Camera,
                    DisplayType,
                    ScreenSizeInches,
                    PriceAmount,
                    PriceCurrency,
                    IsActive
                FROM {SqlTableNames.MobilePhones}
                WHERE IsActive = 1
            ");

            var @params = MobilePhoneFilterDtoExtensions.FilterQueryBuilder(mobilePhoneFilter, query);
            query.Append(" ORDER BY Name, Id;");

            return await ExecuteReadAsync(async (connection, token) =>
            {
                var result = await connection.QueryAsync<MobilePhoneReadModel>(
                    new CommandDefinition(query.ToString(), @params, commandTimeout: CommandTimeoutSeconds, cancellationToken: token));

                return result.AsList().AsReadOnly();
            }, ct);
        }
    }
}
