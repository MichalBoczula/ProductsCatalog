using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Infrastructure.Common
{
    internal static class MobilePhonesInfrastructureDataFactory
    {
        public static readonly Guid MobileCategoryId = Guid.Parse("587480bb-c126-4f9b-b531-b0244daa4ba4");

        public static MobilePhone Create(Guid id)
        {
            var commonDescription = new CommonDescription(
                name: "Benchmark Phone",
                brand: "TestBrand",
                description: "Description for performance testing.",
                mainPhoto: "https://store.com/photos/main.jpg",
                otherPhotos: new List<string> { "https://store.com/photos/side.jpg" });

            var electronicDetails = new ElectronicDetails(
                cPU: "Octa-Core",
                gPU: "HighPerformance GPU",
                ram: "12 GB",
                storage: "256 GB",
                displayType: "AMOLED",
                refreshRateHz: 144,
                screenSizeInches: 6.7m,
                width: 1440,
                height: 3200,
                batteryType: "Li-Po",
                batteryCapacity: 5000);

            var connectivity = new Connectivity(has5G: true, wiFi: true, nFC: true, bluetooth: true);
            var sensors = new Sensors(accelerometer: true, gyroscope: true, proximity: true, compass: true, barometer: true, halla: true, ambientLight: true);
            var price = new Money(1199.99m, "USD");

            var phone = new MobilePhone(
                commonDescription: commonDescription,
                electronicDetails: electronicDetails,
                connectivity: connectivity,
                satelliteNavigationSystems: default,
                sensors: sensors,
                camera: "108 MP",
                fingerPrint: true,
                faceId: true,
                categoryId: MobileCategoryId,
                price: price,
                description2: "Secondary text block",
                description3: "Tertiary text block");

            var idProperty = typeof(MobilePhone).GetProperty("Id")
                             ?? typeof(MobilePhone).BaseType?.GetProperty("Id");
            idProperty?.SetValue(phone, id);

            return phone;
        }

        public static MobilePhone Create() => Create(Guid.NewGuid());
    }
}