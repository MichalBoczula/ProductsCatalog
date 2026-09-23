using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class MobilePhonesQueriesRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private static readonly Guid AppleIphone16Id = Guid.Parse("5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2");
        private static readonly Guid XiaomiPocoF7Id = Guid.Parse("0f62c3e1-8e3e-4b1f-9d74-3d6e2ff2c6d2");
        private static readonly Guid AppleIphone16HistoryId = Guid.Parse("4ee1f28f-4cfe-4a38-9d13-122f5c2c1f12");

        private readonly MsSqlDbTestFixture _fixture;

        public MobilePhonesQueriesRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetTop_RetriesOpeningAnOfflineDatabaseAfterItComesOnline()
        {
            var (database, masterConnectionString, readConnectionString) = await CreateReadTestDatabaseAsync();
            try
            {
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET OFFLINE WITH ROLLBACK IMMEDIATE");
                var repository = new MobilePhonesQueriesRepository(new CustomTestConfiguration(readConnectionString));
                var read = repository.GetTop(CancellationToken.None);

                await Task.Delay(200);
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET ONLINE");

                (await read).ShouldBeEmpty();
            }
            finally
            {
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET ONLINE");
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{database}]");
            }
        }

        [Fact]
        public async Task GetTop_DoesNotRetryQueryErrors()
        {
            var (database, masterConnectionString, readConnectionString) = await CreateReadTestDatabaseAsync(createTable: false);
            try
            {
                var repository = new MobilePhonesQueriesRepository(new CustomTestConfiguration(readConnectionString));
                var exception = await Should.ThrowAsync<SqlException>(() => repository.GetTop(CancellationToken.None));
                exception.Number.ShouldBe(208);
            }
            finally
            {
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{database}]");
            }
        }

        [Fact]
        public async Task GetTop_CancelsWhileWaitingForSqlLock()
        {
            var (database, masterConnectionString, readConnectionString) = await CreateReadTestDatabaseAsync();
            try
            {
                await using var blocker = new SqlConnection(readConnectionString);
                await blocker.OpenAsync();
                await using var lockCommand = new SqlCommand("BEGIN TRAN; INSERT INTO dbo.TB_MobilePhones (Id, IsActive, ChangedAt) VALUES (NEWID(), 1, SYSUTCDATETIME()); UPDATE dbo.TB_MobilePhones WITH (TABLOCKX) SET IsActive = 1;", blocker);
                await lockCommand.ExecuteNonQueryAsync();

                using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
                var repository = new MobilePhonesQueriesRepository(new CustomTestConfiguration(readConnectionString));
                await Should.ThrowAsync<OperationCanceledException>(() => repository.GetTop(cancellation.Token));

                var timeout = await Should.ThrowAsync<SqlException>(() => repository.GetTop(CancellationToken.None));
                timeout.Number.ShouldBe(-2);

                await using var rollback = new SqlCommand("ROLLBACK TRAN", blocker);
                await rollback.ExecuteNonQueryAsync();
            }
            finally
            {
                await ExecuteMasterSqlAsync(masterConnectionString, $"ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{database}]");
            }
        }

        private async Task<(string Database, string Master, string Read)> CreateReadTestDatabaseAsync(bool createTable = true)
        {
            var database = "ReadTest" + Guid.NewGuid().ToString("N");
            var builder = new SqlConnectionStringBuilder(_fixture.ConnectionString)
            {
                InitialCatalog = "master",
                Pooling = false
            };
            var master = builder.ConnectionString;
            await ExecuteMasterSqlAsync(master, $"CREATE DATABASE [{database}]");
            builder.InitialCatalog = database;
            var read = builder.ConnectionString;
            if (createTable)
            {
                await using var connection = new SqlConnection(read);
                await connection.OpenAsync();
                await using var command = new SqlCommand("CREATE TABLE dbo.TB_MobilePhones (Id uniqueidentifier NOT NULL, Name nvarchar(100) NULL, Brand nvarchar(100) NULL, MainPhoto nvarchar(100) NULL, PriceAmount decimal(18, 2) NULL, PriceCurrency nvarchar(3) NULL, IsActive bit NOT NULL, ChangedAt datetime2 NOT NULL)", connection);
                await command.ExecuteNonQueryAsync();
            }

            return (database, master, read);
        }

        private static async Task ExecuteMasterSqlAsync(string connectionString, string sql)
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        [Fact]
        public async Task GetById_ShouldReturnSeededAppleIphoneWithValidReadModel()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetById(AppleIphone16Id, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(AppleIphone16Id);
            result.Name.ShouldBe("Apple iPhone 16 128GB White");
            result.Brand.ShouldBe("Apple");
            result.Description.ShouldBe("Top-class display The 6.1-inch Super Retina XDR OLED screen makes watching movies and photos incredibly enjoyable. The 2556 x 1179 resolution delivers impressive sharpness and realism. True Tone and Haptic Touch improve everyday comfort. It is a display that raises the standard of mobile entertainment.");
            result.Description2.ShouldBe("Professional photography The 48 MP camera captures detailed photos in any lighting. The 12 MP ultrawide lens lets you capture a wider perspective. Modern imaging algorithms make videos and photos look professional. It is a solution for those who want more than standard photos.");
            result.Description3.ShouldBe("Top performance The Apple A18 processor delivers outstanding speed and stability. The smartphone runs iOS 18 with new features and improvements. The built-in battery supports fast and wireless charging for daily convenience. It is a phone that performs well in every situation.");
            result.MainPhoto.ShouldBe("apple-iphone-16-white-main.jpg");
            result.OtherPhotos.ShouldBe("[\"apple-iphone-16-white-1.jpg\",\"apple-iphone-16-white-2.jpg\"]");
            result.CPU.ShouldBe("Apple A18");
            result.GPU.ShouldBe("Apple GPU");
            result.Ram.ShouldBe("6 GB");
            result.Storage.ShouldBe("128 GB");
            result.DisplayType.ShouldBe("OLED");
            result.RefreshRateHz.ShouldBe(60);
            result.ScreenSizeInches.ShouldBe(6.10m);
            result.Width.ShouldBe(72);
            result.Height.ShouldBe(148);
            result.BatteryType.ShouldBe("Li-Ion");
            result.BatteryCapacity.ShouldBe(3000);
            result.GPS.ShouldBeTrue();
            result.AGPS.ShouldBeTrue();
            result.Galileo.ShouldBeTrue();
            result.GLONASS.ShouldBeTrue();
            result.QZSS.ShouldBeTrue();
            result.Accelerometer.ShouldBeTrue();
            result.Gyroscope.ShouldBeTrue();
            result.Proximity.ShouldBeTrue();
            result.Compass.ShouldBeTrue();
            result.Barometer.ShouldBeTrue();
            result.Halla.ShouldBeFalse();
            result.AmbientLight.ShouldBeTrue();
            result.Has5G.ShouldBeTrue();
            result.WiFi.ShouldBeTrue();
            result.NFC.ShouldBeTrue();
            result.Bluetooth.ShouldBeTrue();
            result.Camera.ShouldBe("48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front");
            result.FingerPrint.ShouldBeFalse();
            result.FaceId.ShouldBeTrue();
            result.PriceAmount.ShouldBe(0.00m);
            result.PriceCurrency.ShouldBe("PLN");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task GetByIds_ShouldReturnRequestedActiveSeededMobilePhonesWithValidReadModels()
        {
            // Arrange
            var repository = CreateRepository();
            var mobilePhoneIds = new[] { AppleIphone16Id, XiaomiPocoF7Id };

            // Act
            var result = await repository.GetByIds(mobilePhoneIds, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.Select(mobilePhone => mobilePhone.Id).ShouldBe(mobilePhoneIds, ignoreOrder: true);

            var appleIphone = result.Single(mobilePhone => mobilePhone.Id == AppleIphone16Id);
            appleIphone.Name.ShouldBe("Apple iPhone 16 128GB White");
            appleIphone.Brand.ShouldBe("Apple");
            appleIphone.Camera.ShouldBe("48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front");
            appleIphone.DisplayType.ShouldBe("OLED");
            appleIphone.ScreenSizeInches.ShouldBe(6.10m);
            appleIphone.PriceAmount.ShouldBe(0.00m);
            appleIphone.PriceCurrency.ShouldBe("PLN");
            appleIphone.IsActive.ShouldBeTrue();

            var xiaomiPoco = result.Single(mobilePhone => mobilePhone.Id == XiaomiPocoF7Id);
            xiaomiPoco.Name.ShouldBe("Xiaomi POCO F7 12/512GB Black");
            xiaomiPoco.Brand.ShouldBe("Xiaomi");
            xiaomiPoco.Camera.ShouldBe("50 MP (Sony LYT-600, OIS) + 8 MP ultrawide, 20 MP front");
            xiaomiPoco.DisplayType.ShouldBe("AMOLED");
            xiaomiPoco.ScreenSizeInches.ShouldBe(6.83m);
            xiaomiPoco.PriceAmount.ShouldBe(2499.00m);
            xiaomiPoco.PriceCurrency.ShouldBe("PLN");
            xiaomiPoco.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task GetPhones_ShouldReturnRequestedAmountOfActiveSeededMobilePhonesWithValidReadModels()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetPhones(2, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);

            foreach (var mobilePhone in result)
            {
                mobilePhone.Id.ShouldNotBe(Guid.Empty);
                mobilePhone.Name.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.Brand.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.Camera.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.DisplayType.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.ScreenSizeInches.ShouldBeGreaterThan(0);
                mobilePhone.PriceCurrency.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.IsActive.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task GetPhones_ShouldApplyNameThenIdBeforeLimitingResults()
        {
            var repository = CreateRepository();
            var all = await repository.GetPhones(100, CancellationToken.None);
            var limited = await repository.GetPhones(3, CancellationToken.None);

            all.Count.ShouldBeGreaterThan(3);
            var expected = all.OrderBy(phone => phone.Name, StringComparer.OrdinalIgnoreCase)
                .ThenBy(phone => phone.Id).Take(3).Select(phone => phone.Id);
            limited.Select(phone => phone.Id).ShouldBe(expected);
        }

        [Fact]
        public async Task GetHistoryOfChanges_ShouldKeepEqualTimestampsOnStablePages()
        {
            var repository = CreateRepository();
            var firstId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var secondId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var options = new DbContextOptionsBuilder<ProductsContext>()
                .UseSqlServer(_fixture.ConnectionString).Options;

            await using var context = new ProductsContext(options);
            var seeded = await context.MobilePhonesHistories.AsNoTracking()
                .SingleAsync(history => history.Id == AppleIphone16HistoryId);
            var tiedTime = seeded.ChangedAt.AddDays(1);

            try
            {
                foreach (var id in new[] { firstId, secondId })
                {
                    var copy = seeded with { ChangedAt = tiedTime };
                    context.MobilePhonesHistories.Add(copy);
                    context.Entry(copy).Property(history => history.Id).CurrentValue = id;
                }

                await context.SaveChangesAsync();

                var firstPage = await repository.GetHistoryOfChanges(AppleIphone16Id, 1, 1, CancellationToken.None);
                var secondPage = await repository.GetHistoryOfChanges(AppleIphone16Id, 2, 1, CancellationToken.None);
                var thirdPage = await repository.GetHistoryOfChanges(AppleIphone16Id, 3, 1, CancellationToken.None);

                firstPage.Single().Id.ShouldBe(secondId);
                secondPage.Single().Id.ShouldBe(firstId);
                thirdPage.Single().Id.ShouldBe(AppleIphone16HistoryId);
            }
            finally
            {
                await context.MobilePhonesHistories.Where(history => history.Id == firstId || history.Id == secondId)
                    .ExecuteDeleteAsync();
            }
        }


        [Fact]
        public async Task GetHistoryOfChanges_ShouldReturnWholeSeededHistoryForAppleIphoneWithValidReadModel()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetHistoryOfChanges(AppleIphone16Id, 1, 10, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);

            var history = result.Single();
            history.Id.ShouldBe(AppleIphone16HistoryId);
            history.MobilePhoneId.ShouldBe(AppleIphone16Id);
            history.Name.ShouldBe("Apple iPhone 16 128GB White");
            history.Brand.ShouldBe("Apple");
            history.MainPhoto.ShouldBe("apple-iphone-16-white-main.jpg");
            history.OtherPhotos.ShouldBe("[\"apple-iphone-16-white-1.jpg\",\"apple-iphone-16-white-2.jpg\"]");
            history.CPU.ShouldBe("Apple A18");
            history.GPU.ShouldBe("Apple GPU");
            history.Ram.ShouldBe("6 GB");
            history.Storage.ShouldBe("128 GB");
            history.DisplayType.ShouldBe("OLED");
            history.RefreshRateHz.ShouldBe(60);
            history.ScreenSizeInches.ShouldBe(6.10m);
            history.Width.ShouldBe(72);
            history.Height.ShouldBe(148);
            history.BatteryType.ShouldBe("Li-Ion");
            history.BatteryCapacity.ShouldBe(3000);
            history.Camera.ShouldBe("48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front");
            history.PriceAmount.ShouldBe(0.00m);
            history.PriceCurrency.ShouldBe("PLN");
            history.IsActive.ShouldBeTrue();
            history.Operation.ShouldBe(Operation.Inserted);
            history.ChangedAt.ShouldBe(new DateTime(2025, 12, 26, 20, 40, 00));
        }

        [Fact]
        public async Task GetFilteredPhones_WithMinimalPrice_ShouldReturnOnlyPhonesWithPriceGreaterThanOrEqualToMinimalPrice()
        {
            // Arrange
            var repository = CreateRepository();
            var filter = new MobilePhoneReadFilterDto { MinimalPrice = 2499.00m };

            // Act
            var result = await repository.GetFilteredPhones(filter, CancellationToken.None);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldAllBe(x => x.PriceAmount >= filter.MinimalPrice);
            result.ShouldContain(x => x.Name == "Xiaomi POCO F7 12/512GB Black" && x.PriceAmount == 2499.00m);
            AssertValidFilteredPhones(result);
        }

        [Fact]
        public async Task GetFilteredPhones_WithMaximalPrice_ShouldReturnOnlyPhonesWithPriceLessThanOrEqualToMaximalPrice()
        {
            // Arrange
            var repository = CreateRepository();
            var filter = new MobilePhoneReadFilterDto { MaximalPrice = 999.00m };

            // Act
            var result = await repository.GetFilteredPhones(filter, CancellationToken.None);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldAllBe(x => x.PriceAmount <= filter.MaximalPrice);
            result.ShouldContain(x => x.Name == "Xiaomi Redmi 15 5G 4/128GB Midnight Black" && x.PriceAmount == 999.00m);
            AssertValidFilteredPhones(result);
        }

        [Fact]
        public async Task GetFilteredPhones_WithBrand_ShouldReturnOnlyPhonesForSelectedBrand()
        {
            // Arrange
            var repository = CreateRepository();
            var filter = new MobilePhoneReadFilterDto { BrandName = MobilePhonesBrand.Apple.ToString() };

            // Act
            var result = await repository.GetFilteredPhones(filter, CancellationToken.None);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldAllBe(x => x.Brand == filter.BrandName);
            result.ShouldContain(x => x.Id == AppleIphone16Id && x.Name == "Apple iPhone 16 128GB White");
            AssertValidFilteredPhones(result);
        }

        [Fact]
        public async Task GetFilteredPhones_WithMinimalAndMaximalPrice_ShouldReturnOnlyPhonesWithinPriceRange()
        {
            // Arrange
            var repository = CreateRepository();
            var filter = new MobilePhoneReadFilterDto { MinimalPrice = 999.00m, MaximalPrice = 1999.00m };

            // Act
            var result = await repository.GetFilteredPhones(filter, CancellationToken.None);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldAllBe(x => x.PriceAmount >= filter.MinimalPrice && x.PriceAmount <= filter.MaximalPrice);
            result.ShouldContain(x => x.Name == "Xiaomi Redmi 15 5G 4/128GB Midnight Black" && x.PriceAmount == 999.00m);
            result.ShouldContain(x => x.Name == "Xiaomi Redmi Note 15 Pro 8/256GB Black" && x.PriceAmount == 1999.00m);
            AssertValidFilteredPhones(result);
        }

        [Fact]
        public async Task GetFilteredPhones_WithBrandAndPriceRange_ShouldReturnOnlyPhonesMatchingAllFilters()
        {
            // Arrange
            var repository = CreateRepository();
            var filter = new MobilePhoneReadFilterDto
            {
                BrandName = MobilePhonesBrand.Xiaomi.ToString(),
                MinimalPrice = 999.00m,
                MaximalPrice = 1999.00m
            };

            // Act
            var result = await repository.GetFilteredPhones(filter, CancellationToken.None);

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldAllBe(x => x.Brand == filter.BrandName && x.PriceAmount >= filter.MinimalPrice && x.PriceAmount <= filter.MaximalPrice);
            result.ShouldContain(x => x.Name == "Xiaomi Redmi 15 5G 4/128GB Midnight Black" && x.PriceAmount == 999.00m);
            result.ShouldContain(x => x.Name == "Xiaomi Redmi Note 15 Pro 8/256GB Black" && x.PriceAmount == 1999.00m);
            AssertValidFilteredPhones(result);
        }

        [Fact]
        public async Task GetTop_ShouldReturnTopThreeActiveSeededMobilePhonesWithValidReadModels()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetTop(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);

            foreach (var mobilePhone in result)
            {
                mobilePhone.Id.ShouldNotBe(Guid.Empty);
                mobilePhone.Name.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.Brand.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.PriceCurrency.ShouldNotBeNullOrWhiteSpace();
            }
        }

        private MobilePhonesQueriesRepository CreateRepository()
        {
            var configuration = new CustomTestConfiguration(_fixture.ConnectionString);

            return new MobilePhonesQueriesRepository(configuration);
        }

        private static void AssertValidFilteredPhones(IReadOnlyList<ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel.MobilePhoneReadModel> mobilePhones)
        {
            foreach (var mobilePhone in mobilePhones)
            {
                mobilePhone.Id.ShouldNotBe(Guid.Empty);
                mobilePhone.Name.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.Brand.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.Camera.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.DisplayType.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.ScreenSizeInches.ShouldBeGreaterThan(0);
                mobilePhone.PriceCurrency.ShouldNotBeNullOrWhiteSpace();
                mobilePhone.IsActive.ShouldBeTrue();
            }
        }
    }
}
