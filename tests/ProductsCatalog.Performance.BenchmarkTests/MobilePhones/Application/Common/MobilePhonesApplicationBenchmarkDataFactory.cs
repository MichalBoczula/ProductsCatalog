using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common
{
    internal static class MobilePhonesApplicationBenchmarkDataFactory
    {
        public static readonly Guid MobileCategoryId = Guid.Parse("587480bb-c126-4f9b-b531-b0244daa4ba4");


        public static GetTopMobilePhonesQuery CreateTopQuery()
        {
            return new GetTopMobilePhonesQuery();
        }

        public static IReadOnlyList<MobilePhoneReadModel> CreateTopReadModels()
        {
            return new List<MobilePhoneReadModel>
            {
                CreateReadModel(Guid.Parse("26400545-81c4-4e50-95c7-c723006a83dd")),
                CreateReadModel(Guid.Parse("deae902e-9732-4f95-8b8d-764994685195")),
                CreateReadModel(Guid.Parse("1fa43da3-86b1-4e35-bc3c-551c2d19a920"))
            };
        }

        public static CreateMobilePhoneExternalDto CreateExternalDto()
        {
            return new CreateMobilePhoneExternalDto(
                CommonDescription: new CommonDescriptionExtrernalDto("iPhone 15", "Apple", "Flagship phone", "main.jpg", ["side1.jpg", "side2.jpg"]),
                ElectronicDetails: new CreateElectronicDetailsExternalDto("A16 Bionic", "Apple GPU", "6GB", "128GB", "OLED", 60, 6.1m, 1179, 2556, "Li-Ion", 3349),
                Connectivity: new CreateConnectivityExternalDto(true, true, true, true),
                SatelliteNavigationSystems: new CreateSatelliteNavigationSystemExternalDto(true, true, true, true, false),
                Sensors: new CreateSensorsExternalDto(true, true, true, true, false, false, true),
                Camera: "48MP",
                FingerPrint: false,
                FaceId: true,
                CategoryId: MobileCategoryId,
                Price: new CreateMoneyExternalDto(799.00m, "USD"),
                Description2: "Text line 2",
                Description3: "Text line 3"
            );
        }

        public static UpdateMobilePhoneExternalDto UpdateExternalDto()
        {
            return new UpdateMobilePhoneExternalDto(
                CommonDescription: new CommonDescriptionExtrernalDto("iPhone 15 Updated", "Apple", "Updated text", "main_updated.jpg", ["side1.jpg"]),
                ElectronicDetails: new UpdateElectronicDetailsExternalDto("A16 Bionic", "Apple GPU", "6GB", "256GB", "OLED", 60, 6.1m, 1179, 2556, "Li-Ion", 3349),
                Connectivity: new UpdateConnectivityExternalDto(true, true, true, true),
                SatelliteNavigationSystems: new UpdateSatelliteNavigationSystemExternalDto(true, true, true, true, true),
                Sensors: new UpdateSensorsExternalDto(true, true, true, true, true, false, true),
                Camera: "48MP, f/1.6",
                FingerPrint: false,
                FaceId: true,
                CategoryId: MobileCategoryId,
                Price: new UpdateMoneyExternalDto(849.00m, "USD"),
                Description2: "Updated line 2",
                Description3: "Updated line 3"
            );
        }

        public static MobilePhone CreateDomainPhone(Guid id)
        {
            var phone = new MobilePhone(
                commonDescription: new ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.CommonDescription("iPhone 15", "Apple", "Flagship phone", "main.jpg", ["side1.jpg"]),
                electronicDetails: new ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails("A16", "GPU", "6GB", "128GB", "OLED", 60, 6.1m, 1179, 2556, "Li-Ion", 3349),
                connectivity: new Connectivity(true, true, true, true),
                satelliteNavigationSystems: default,
                sensors: new Sensors(true, true, true, true, false, false, true),
                camera: "48MP",
                fingerPrint: false,
                faceId: true,
                categoryId: MobileCategoryId,
                price: new Money(799.00m, "USD"),
                description2: "Text 2",
                description3: "Text 3"
            );

            var idProperty = typeof(MobilePhone).GetProperty("Id") ?? typeof(MobilePhone).BaseType?.GetProperty("Id");
            idProperty?.SetValue(phone, id);

            return phone;
        }

        public static MobilePhoneReadModel CreateReadModel(Guid id)
        {
            return new MobilePhoneReadModel
            {
                Id = id,
                Name = "iPhone 15 Pro",
                Brand = "Apple",
                Description = "Flagowy smartfon z procesorem A17 Pro.",
                MainPhoto = "main.jpg",
                OtherPhotos = "side1.jpg,side2.jpg",
                CPU = "A17 Pro",
                GPU = "Apple GPU 6-core",
                Ram = "8 GB",
                Storage = "128 GB",
                DisplayType = "OLED",
                RefreshRateHz = 120,
                ScreenSizeInches = 6.1m,
                Width = 1179,
                Height = 2556,
                BatteryType = "Li-Ion",
                BatteryCapacity = 3274,
                GPS = true,
                AGPS = true,
                Galileo = true,
                GLONASS = true,
                QZSS = false,
                Accelerometer = true,
                Gyroscope = true,
                Proximity = true,
                Compass = true,
                Barometer = true,
                Halla = false,
                AmbientLight = true,
                Has5G = true,
                WiFi = true,
                NFC = true,
                Bluetooth = true,
                Camera = "48 MP",
                FingerPrint = false,
                FaceId = true,
                CategoryId = MobileCategoryId,
                PriceAmount = 999.99m,
                PriceCurrency = "USD",
                Description2 = "Dodatkowy opis produktu",
                Description3 = "Warunki gwarancji",
                IsActive = true
            };
        }

        public static MobilePhoneFilterDto CreateFilterDto()
        {
            return new MobilePhoneFilterDto
            {
                Brand = MobilePhonesBrand.Apple,
                MinimalPrice = 500,
                MaximalPrice = 1500
            };
        }
    }
}