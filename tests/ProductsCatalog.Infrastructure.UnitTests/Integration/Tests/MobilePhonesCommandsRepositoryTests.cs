using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

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
    }
}
