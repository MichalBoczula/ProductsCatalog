using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain
{
    internal static class MobilePhonesValidationDataFactory
    {
        public static readonly Guid ValidCategoryId = Guid.Parse("587480bb-c126-4f9b-b531-b0244daa4ba4");
        public static readonly Guid InvalidCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000000");

        public static MobilePhone CreateValid()
        {
            var commonDescription = new CommonDescription(
                name: "iPhone 15 Pro",
                brand: "Apple",
                description: "Flagowy smartfon z procesorem A17 Pro i tytanową obudową.",
                mainPhoto: "https://store.com/photos/iphone15pro_main.jpg",
                otherPhotos: new List<string> { "https://store.com/photos/iphone15pro_side.jpg" });

            var electronicDetails = new ElectronicDetails(
                cPU: "A17 Pro",
                gPU: "Apple GPU 6-core",
                ram: "8 GB",
                storage: "128 GB",
                displayType: "OLED Super Retina XDR",
                refreshRateHz: 120,
                screenSizeInches: 6.1m,
                width: 1179,
                height: 2556,
                batteryType: "Li-Ion",
                batteryCapacity: 3274);

            var connectivity = new Connectivity(has5G: true, wiFi: true, nFC: true, bluetooth: true);
            var sensors = new Sensors(accelerometer: true, gyroscope: true, proximity: true, compass: true, barometer: true, halla: false, ambientLight: true);
            var price = new Money(999.99m, "USD");

            return new MobilePhone(
                commonDescription: commonDescription,
                electronicDetails: electronicDetails,
                connectivity: connectivity,
                satelliteNavigationSystems: default,
                sensors: sensors,
                camera: "48 MP, f/1.8",
                fingerPrint: false,
                faceId: true,
                categoryId: ValidCategoryId,
                price: price,
                description2: "Dodatkowy opis produktu w katalogu",
                description3: "Szczegółowe warunki gwarancji producenta");
        }

        public static MobilePhone CreateInvalidSingle()
        {
            var valid = CreateValid();

            return new MobilePhone(
                commonDescription: valid.CommonDescription,
                electronicDetails: valid.ElectronicDetails,
                connectivity: valid.Connectivity,
                satelliteNavigationSystems: valid.SatelliteNavigationSystems,
                sensors: valid.Sensors,
                camera: valid.Camera,
                fingerPrint: valid.FingerPrint,
                faceId: valid.FaceId,
                categoryId: InvalidCategoryId,
                price: valid.Price,
                description2: valid.Description2,
                description3: valid.Description3);
        }

        public static MobilePhone CreateAllInvalid()
        {
            var invalidDescription = new CommonDescription("", "", "", "", new List<string>());
            var invalidElectronic = new ElectronicDetails("", "", "", "", "", -1, -5.0m, -10, -10, "", -500);
            var invalidPrice = new Money(-1500.00m, "PLN");

            return new MobilePhone(
                commonDescription: invalidDescription,
                electronicDetails: invalidElectronic,
                connectivity: default,
                satelliteNavigationSystems: default,
                sensors: default,
                camera: "",
                fingerPrint: false,
                faceId: false,
                categoryId: InvalidCategoryId,
                price: invalidPrice,
                description2: "",
                description3: "");
        }
    }
}