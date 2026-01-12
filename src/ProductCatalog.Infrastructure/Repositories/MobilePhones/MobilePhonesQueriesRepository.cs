using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Infrastructure.Common;
using System;
using System.Data;

namespace ProductCatalog.Infrastructure.Repositories.MobilePhones
{
    internal sealed class MobilePhonesQueriesRepository : IMobilePhonesQueriesRepository
    {
        private readonly string _connectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public MobilePhonesQueriesRepository(IConfiguration configuration)
        {
            _connectionString = ConnectionStringExtensions.Initialize(configuration);
        }

        public async Task<MobilePhoneReadModel?> GetById(Guid id, CancellationToken ct)
        {
            var sql = $@"
                SELECT Id,
                       Name,
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
                       FingerPrint,
                       FaceId,
                       CategoryId,
                       PriceAmount,
                       PriceCurrency,
                       IsActive
                FROM {SqlTableNames.MobilePhones}
                WHERE Id = @Id;
                ";

            using var connection = CreateConnection();

            var result = await connection.QuerySingleOrDefaultAsync<MobilePhoneReadModel?>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return result;
        }

        public async Task<IReadOnlyList<MobilePhoneReadModel>> GetPhones(int amount, CancellationToken ct)
        {
            var sql = $@"
                SELECT TOP (@Amount)
                       Id,
                       Name,
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
                       FingerPrint,
                       FaceId,
                       CategoryId,
                       PriceAmount,
                       PriceCurrency,
                       IsActive
                FROM {SqlTableNames.MobilePhones}
                ";

            using var connection = CreateConnection();

            var result = await connection.QueryAsync<MobilePhoneReadModel>(
                new CommandDefinition(sql, new { Amount = amount }, cancellationToken: ct));

            return result.ToList().AsReadOnly();
        }

        public Task<IReadOnlyList<MobilePhoneReadModel>> GetByFilter(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<MobilePhonesHistory>> GetHistoryOfChanges(Guid mobilePhoneId, int pageNumber, int pageSize, CancellationToken ct)
        {
            var offset = Math.Max(pageNumber, 1) - 1;
            var size = Math.Max(pageSize, 1);

            var sql = $@"
                SELECT Id,
                       MobilePhoneId,
                       Name,
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
                       FingerPrint,
                       FaceId,
                       CategoryId,
                       PriceAmount,
                       PriceCurrency,
                       IsActive,
                       ChangedAt,
                       Operation
                FROM {SqlTableNames.MobilePhonesHistory}
                WHERE MobilePhoneId = @MobilePhoneId
                ORDER BY ChangedAt DESC
                OFFSET (@Offset * @PageSize) ROWS
                FETCH NEXT @PageSize ROWS ONLY;
                ";

            using var connection = CreateConnection();

            var result = await connection.QueryAsync<MobilePhonesHistory>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        MobilePhoneId = mobilePhoneId,
                        Offset = offset,
                        PageSize = size
                    },
                    cancellationToken: ct));

            return result.ToList().AsReadOnly();
        }
    }
}
