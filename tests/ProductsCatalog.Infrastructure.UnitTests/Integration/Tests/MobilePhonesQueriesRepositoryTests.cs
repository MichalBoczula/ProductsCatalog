using Microsoft.Extensions.Configuration;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class MobilePhonesQueriesRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private static readonly Guid AppleIphone16Id = Guid.Parse("5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2");
        private static readonly Guid MobileCategoryId = Guid.Parse("587480bb-c126-4f9b-b531-b0244daa4ba4");

        private readonly MsSqlDbTestFixture _fixture;

        public MobilePhonesQueriesRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetById_ShouldReturnSeededAppleIphoneWithValidReadModel()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = _fixture.ConnectionString
                })
                .Build();

            var repository = new MobilePhonesQueriesRepository(configuration);

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
            result.CategoryId.ShouldBe(MobileCategoryId);
            result.PriceAmount.ShouldBe(0.00m);
            result.PriceCurrency.ShouldBe("PLN");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task GetPhones_ShouldReturnRequestedAmountOfActiveSeededMobilePhonesWithValidReadModels()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = _fixture.ConnectionString
                })
                .Build();

            var repository = new MobilePhonesQueriesRepository(configuration);

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
        public async Task GetTop_ShouldReturnTopThreeActiveSeededMobilePhonesWithValidReadModels()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = _fixture.ConnectionString
                })
                .Build();

            var repository = new MobilePhonesQueriesRepository(configuration);

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
    }
}
