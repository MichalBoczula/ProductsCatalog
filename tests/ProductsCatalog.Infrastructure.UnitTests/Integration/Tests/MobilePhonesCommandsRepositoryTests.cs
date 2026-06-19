using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;
using System.Text.Json;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class MobilePhonesCommandsRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public MobilePhonesCommandsRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        private ProductsContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(_fixture.ConnectionString);
            return new ProductsContext(optionsBuilder.Options);
        }

        [Fact]
        public async Task Add_ShouldPersistNewMobilePhoneInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new MobilePhonesCommandsRepository(context);

            var mobilePhone = CreateMobilePhone("Add Phone", "Add description", 999.99m);

            // Act
            repository.Add(mobilePhone);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhone.Id);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(mobilePhone.Id);
            result.CommonDescription.Name.ShouldBe("Add Phone");
            result.CommonDescription.Brand.ShouldBe("Test Brand");
            result.CommonDescription.Description.ShouldBe("Add description");
            result.CommonDescription.MainPhoto.ShouldBe("main-photo.jpg");
            result.CommonDescription.OtherPhotos.ShouldBe(["photo-1.jpg", "photo-2.jpg"]);
            result.ElectronicDetails.CPU.ShouldBe("Test CPU");
            result.ElectronicDetails.GPU.ShouldBe("Test GPU");
            result.ElectronicDetails.Ram.ShouldBe("8 GB");
            result.ElectronicDetails.Storage.ShouldBe("128 GB");
            result.ElectronicDetails.DisplayType.ShouldBe("OLED");
            result.ElectronicDetails.RefreshRateHz.ShouldBe(120);
            result.ElectronicDetails.ScreenSizeInches.ShouldBe(6.1m);
            result.ElectronicDetails.Width.ShouldBe(71);
            result.ElectronicDetails.Height.ShouldBe(146);
            result.ElectronicDetails.BatteryType.ShouldBe("Li-Ion");
            result.ElectronicDetails.BatteryCapacity.ShouldBe(4000);
            result.Connectivity.Has5G.ShouldBeTrue();
            result.Connectivity.WiFi.ShouldBeTrue();
            result.Connectivity.NFC.ShouldBeTrue();
            result.Connectivity.Bluetooth.ShouldBeTrue();
            result.SatelliteNavigationSystems.GPS.ShouldBeTrue();
            result.SatelliteNavigationSystems.AGPS.ShouldBeTrue();
            result.SatelliteNavigationSystems.Galileo.ShouldBeTrue();
            result.SatelliteNavigationSystems.GLONASS.ShouldBeFalse();
            result.SatelliteNavigationSystems.QZSS.ShouldBeFalse();
            result.Sensors.Accelerometer.ShouldBeTrue();
            result.Sensors.Gyroscope.ShouldBeTrue();
            result.Sensors.Proximity.ShouldBeTrue();
            result.Sensors.Compass.ShouldBeTrue();
            result.Sensors.Barometer.ShouldBeFalse();
            result.Sensors.Halla.ShouldBeFalse();
            result.Sensors.AmbientLight.ShouldBeTrue();
            result.Camera.ShouldBe("48 MP");
            result.FingerPrint.ShouldBeTrue();
            result.FaceId.ShouldBeFalse();
            result.CategoryId.ShouldBe(mobilePhone.CategoryId);
            result.Price.Amount.ShouldBe(999.99m);
            result.Price.Currency.ShouldBe("USD");
            result.Description2.ShouldBe("Second description");
            result.Description3.ShouldBe("Third description");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Update_ShouldModifyExistingMobilePhoneInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            Guid mobilePhoneId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new MobilePhonesCommandsRepository(setupContext);
                var initialMobilePhone = CreateMobilePhone("Initial Phone", "Initial description", 500m);
                setupRepository.Add(initialMobilePhone);
                await setupRepository.SaveChanges(CancellationToken.None);
                mobilePhoneId = initialMobilePhone.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new MobilePhonesCommandsRepository(context);
            var mobilePhoneToUpdate = await repository.GetById(mobilePhoneId, CancellationToken.None);

            mobilePhoneToUpdate.ShouldNotBeNull();

            var newInfo = CreateMobilePhone("Updated Phone", "Updated description", 750m, camera: "108 MP", fingerPrint: false, faceId: true);
            mobilePhoneToUpdate.AssigneNewMobilePhoneInformation(newInfo);

            repository.Update(mobilePhoneToUpdate);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhoneId);

            result.ShouldNotBeNull();
            result.CommonDescription.Name.ShouldBe("Updated Phone");
            result.CommonDescription.Description.ShouldBe("Updated description");
            result.Camera.ShouldBe("108 MP");
            result.FingerPrint.ShouldBeFalse();
            result.FaceId.ShouldBeTrue();
            result.Price.Amount.ShouldBe(750m);
            result.ChangedAt.ShouldBeGreaterThan(DateTime.MinValue);
        }

        [Fact]
        public async Task GetById_ShouldReturnTrackedMobilePhone_WhenMobilePhoneExists()
        {
            // Arrange
            Guid mobilePhoneId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new MobilePhonesCommandsRepository(setupContext);
                var initialMobilePhone = CreateMobilePhone("Tracked Phone", "Tracked description", 600m);
                setupRepository.Add(initialMobilePhone);
                await setupRepository.SaveChanges(CancellationToken.None);
                mobilePhoneId = initialMobilePhone.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new MobilePhonesCommandsRepository(context);
            var trackedMobilePhone = await repository.GetById(mobilePhoneId, CancellationToken.None);

            // Assert
            trackedMobilePhone.ShouldNotBeNull();
            context.Entry(trackedMobilePhone).State.ShouldBe(EntityState.Unchanged);

            trackedMobilePhone.AssigneNewMobilePhoneInformation(CreateMobilePhone("Tracked Updated Phone", "Tracked updated description", 650m));
            await repository.SaveChanges(CancellationToken.None);

            using var assertContext = CreateContext();
            var result = await assertContext.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhoneId);

            result.ShouldNotBeNull();
            result.CommonDescription.Name.ShouldBe("Tracked Updated Phone");
            result.CommonDescription.Description.ShouldBe("Tracked updated description");
            result.Price.Amount.ShouldBe(650m);
        }

        [Fact]
        public async Task AddWithHistory_ShouldPersistBothRecordsWithMatchingData()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new MobilePhonesCommandsRepository(context);

            var mobilePhone = CreateMobilePhone("History Add Phone", "History add description", 1234.56m);
            var history = CreateTestHistory(mobilePhone, Operation.Inserted);

            // Act
            repository.Add(mobilePhone);
            repository.WriteHistory(history);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var mobilePhoneResult = await assertContext.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhone.Id);
            var historyResult = await assertContext.MobilePhonesHistories.FirstOrDefaultAsync(x => x.MobilePhoneId == mobilePhone.Id);

            mobilePhoneResult.ShouldNotBeNull();
            historyResult.ShouldNotBeNull();
            historyResult.Id.ShouldNotBe(Guid.Empty);
            AssertHistoryMatchesMobilePhone(historyResult, mobilePhoneResult, Operation.Inserted);
        }

        [Fact]
        public async Task UpdateWithHistory_ShouldPersistTwoHistoryRecordsAndNewestRecordShouldMatchUpdatedMobilePhone()
        {
            // Arrange
            Guid mobilePhoneId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new MobilePhonesCommandsRepository(setupContext);
                var initialMobilePhone = CreateMobilePhone("Initial History Phone", "Initial history description", 700m);
                setupRepository.Add(initialMobilePhone);
                setupRepository.WriteHistory(CreateTestHistory(initialMobilePhone, Operation.Inserted));
                await setupRepository.SaveChanges(CancellationToken.None);
                mobilePhoneId = initialMobilePhone.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new MobilePhonesCommandsRepository(context);
            var mobilePhoneToUpdate = await repository.GetById(mobilePhoneId, CancellationToken.None);

            mobilePhoneToUpdate.ShouldNotBeNull();

            var newInfo = CreateMobilePhone(
                "Updated History Phone",
                "Updated history description",
                850m,
                camera: "200 MP",
                fingerPrint: false,
                faceId: true);
            mobilePhoneToUpdate.AssigneNewMobilePhoneInformation(newInfo);

            repository.Update(mobilePhoneToUpdate);
            repository.WriteHistory(CreateTestHistory(mobilePhoneToUpdate, Operation.Updated));
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var mobilePhoneResult = await assertContext.MobilePhones.FirstOrDefaultAsync(x => x.Id == mobilePhoneId);
            var historyResults = await assertContext.MobilePhonesHistories
                .Where(x => x.MobilePhoneId == mobilePhoneId)
                .OrderByDescending(x => x.ChangedAt)
                .ThenByDescending(x => x.Id)
                .ToListAsync();

            mobilePhoneResult.ShouldNotBeNull();
            historyResults.Count.ShouldBe(2);

            var newestHistoryResult = historyResults.First();
            AssertHistoryMatchesMobilePhone(newestHistoryResult, mobilePhoneResult, Operation.Updated);
        }

        private static MobilePhone CreateMobilePhone(
            string name,
            string description,
            decimal price,
            string camera = "48 MP",
            bool fingerPrint = true,
            bool faceId = false)
        {
            return new MobilePhone(
                new CommonDescription(name, "Test Brand", description, "main-photo.jpg", ["photo-1.jpg", "photo-2.jpg"]),
                new ElectronicDetails("Test CPU", "Test GPU", "8 GB", "128 GB", "OLED", 120, 6.1m, 71, 146, "Li-Ion", 4000),
                new Connectivity(true, true, true, true),
                new SatelliteNavigationSystem(true, true, true, false, false),
                new Sensors(true, true, true, true, false, false, true),
                camera,
                fingerPrint,
                faceId,
                Guid.NewGuid(),
                new Money(price, "usd"),
                "Second description",
                "Third description");
        }

        private static MobilePhonesHistory CreateTestHistory(MobilePhone mobilePhone, Operation operation)
        {
            return new MobilePhonesHistory
            {
                MobilePhoneId = mobilePhone.Id,
                Name = mobilePhone.CommonDescription.Name,
                Brand = mobilePhone.CommonDescription.Brand,
                Description = mobilePhone.CommonDescription.Description,
                MainPhoto = mobilePhone.CommonDescription.MainPhoto,
                OtherPhotos = JsonSerializer.Serialize(mobilePhone.CommonDescription.OtherPhotos),
                CPU = mobilePhone.ElectronicDetails.CPU,
                GPU = mobilePhone.ElectronicDetails.GPU,
                Ram = mobilePhone.ElectronicDetails.Ram,
                Storage = mobilePhone.ElectronicDetails.Storage,
                DisplayType = mobilePhone.ElectronicDetails.DisplayType,
                RefreshRateHz = mobilePhone.ElectronicDetails.RefreshRateHz,
                ScreenSizeInches = mobilePhone.ElectronicDetails.ScreenSizeInches,
                Width = mobilePhone.ElectronicDetails.Width,
                Height = mobilePhone.ElectronicDetails.Height,
                BatteryType = mobilePhone.ElectronicDetails.BatteryType,
                BatteryCapacity = mobilePhone.ElectronicDetails.BatteryCapacity,
                GPS = mobilePhone.SatelliteNavigationSystems.GPS,
                AGPS = mobilePhone.SatelliteNavigationSystems.AGPS,
                Galileo = mobilePhone.SatelliteNavigationSystems.Galileo,
                GLONASS = mobilePhone.SatelliteNavigationSystems.GLONASS,
                QZSS = mobilePhone.SatelliteNavigationSystems.QZSS,
                Accelerometer = mobilePhone.Sensors.Accelerometer,
                Gyroscope = mobilePhone.Sensors.Gyroscope,
                Proximity = mobilePhone.Sensors.Proximity,
                Compass = mobilePhone.Sensors.Compass,
                Barometer = mobilePhone.Sensors.Barometer,
                Halla = mobilePhone.Sensors.Halla,
                AmbientLight = mobilePhone.Sensors.AmbientLight,
                Has5G = mobilePhone.Connectivity.Has5G,
                WiFi = mobilePhone.Connectivity.WiFi,
                NFC = mobilePhone.Connectivity.NFC,
                Bluetooth = mobilePhone.Connectivity.Bluetooth,
                Camera = mobilePhone.Camera,
                FingerPrint = mobilePhone.FingerPrint,
                FaceId = mobilePhone.FaceId,
                CategoryId = mobilePhone.CategoryId,
                PriceAmount = mobilePhone.Price.Amount,
                PriceCurrency = mobilePhone.Price.Currency,
                Description2 = mobilePhone.Description2,
                Description3 = mobilePhone.Description3,
                IsActive = mobilePhone.IsActive,
                ChangedAt = mobilePhone.ChangedAt,
                Operation = operation
            };
        }

        private static void AssertHistoryMatchesMobilePhone(
            MobilePhonesHistory history,
            MobilePhone mobilePhone,
            Operation operation)
        {
            history.MobilePhoneId.ShouldBe(mobilePhone.Id);
            history.Name.ShouldBe(mobilePhone.CommonDescription.Name);
            history.Brand.ShouldBe(mobilePhone.CommonDescription.Brand);
            history.Description.ShouldBe(mobilePhone.CommonDescription.Description);
            history.MainPhoto.ShouldBe(mobilePhone.CommonDescription.MainPhoto);
            history.OtherPhotos.ShouldBe(JsonSerializer.Serialize(mobilePhone.CommonDescription.OtherPhotos));
            history.CPU.ShouldBe(mobilePhone.ElectronicDetails.CPU);
            history.GPU.ShouldBe(mobilePhone.ElectronicDetails.GPU);
            history.Ram.ShouldBe(mobilePhone.ElectronicDetails.Ram);
            history.Storage.ShouldBe(mobilePhone.ElectronicDetails.Storage);
            history.DisplayType.ShouldBe(mobilePhone.ElectronicDetails.DisplayType);
            history.RefreshRateHz.ShouldBe(mobilePhone.ElectronicDetails.RefreshRateHz);
            history.ScreenSizeInches.ShouldBe(mobilePhone.ElectronicDetails.ScreenSizeInches);
            history.Width.ShouldBe(mobilePhone.ElectronicDetails.Width);
            history.Height.ShouldBe(mobilePhone.ElectronicDetails.Height);
            history.BatteryType.ShouldBe(mobilePhone.ElectronicDetails.BatteryType);
            history.BatteryCapacity.ShouldBe(mobilePhone.ElectronicDetails.BatteryCapacity);
            history.GPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GPS);
            history.AGPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.AGPS);
            history.Galileo.ShouldBe(mobilePhone.SatelliteNavigationSystems.Galileo);
            history.GLONASS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GLONASS);
            history.QZSS.ShouldBe(mobilePhone.SatelliteNavigationSystems.QZSS);
            history.Accelerometer.ShouldBe(mobilePhone.Sensors.Accelerometer);
            history.Gyroscope.ShouldBe(mobilePhone.Sensors.Gyroscope);
            history.Proximity.ShouldBe(mobilePhone.Sensors.Proximity);
            history.Compass.ShouldBe(mobilePhone.Sensors.Compass);
            history.Barometer.ShouldBe(mobilePhone.Sensors.Barometer);
            history.Halla.ShouldBe(mobilePhone.Sensors.Halla);
            history.AmbientLight.ShouldBe(mobilePhone.Sensors.AmbientLight);
            history.Has5G.ShouldBe(mobilePhone.Connectivity.Has5G);
            history.WiFi.ShouldBe(mobilePhone.Connectivity.WiFi);
            history.NFC.ShouldBe(mobilePhone.Connectivity.NFC);
            history.Bluetooth.ShouldBe(mobilePhone.Connectivity.Bluetooth);
            history.Camera.ShouldBe(mobilePhone.Camera);
            history.FingerPrint.ShouldBe(mobilePhone.FingerPrint);
            history.FaceId.ShouldBe(mobilePhone.FaceId);
            history.CategoryId.ShouldBe(mobilePhone.CategoryId);
            history.PriceAmount.ShouldBe(mobilePhone.Price.Amount);
            history.PriceCurrency.ShouldBe(mobilePhone.Price.Currency);
            history.Description2.ShouldBe(mobilePhone.Description2);
            history.Description3.ShouldBe(mobilePhone.Description3);
            history.IsActive.ShouldBe(mobilePhone.IsActive);
            history.ChangedAt.ShouldBe(mobilePhone.ChangedAt);
            history.Operation.ShouldBe(operation);
        }
    }
}
