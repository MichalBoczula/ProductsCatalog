using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.Common.Enums;

namespace ProductCatalog.Infrastructure.Configuration.DataSeed
{
    internal static class DateSeedExtension
    {
        private static readonly Guid MobileCategoryId = Guid.Parse("587480bb-c126-4f9b-b531-b0244daa4ba4");
        private static readonly Guid PcCategoryId = Guid.Parse("9656c5c5-8ed9-46e1-a5df-025f5d7885d4");
        private static readonly Guid TabletCategoryId = Guid.Parse("f5fd7b52-275e-4710-a578-40a522ac139c");

        private static readonly Guid MobileCategoryHistoryId = Guid.Parse("24daf035-652f-404a-8fd7-785fae68b341");
        private static readonly Guid PcCategoryHistoryId = Guid.Parse("b3d060f4-81f4-4d64-b4ac-77b3f39b1e29");
        private static readonly Guid TabletCategoryHistoryId = Guid.Parse("fc500d67-46a9-492a-a861-77fcd73f1bfc");

        private static readonly Guid UsdCurrencyId = Guid.Parse("1a017544-890c-4219-891f-cd5549473d4e");
        private static readonly Guid PlnCurrencyId = Guid.Parse("e73b3ef4-ec2c-4262-81ef-0ac21fbc1ec3");
        private static readonly Guid EurCurrencyId = Guid.Parse("12da255e-6408-4b28-a5b1-84758f889348");

        private static readonly Guid UsdCurrencyHistoryId = Guid.Parse("1b0d0b8c-2926-415b-a1b8-1843fc189747");
        private static readonly Guid PlnCurrencyHistoryId = Guid.Parse("58ad897f-178c-45c9-a8f0-3302a265a9aa");
        private static readonly Guid EurCurrencyHistoryId = Guid.Parse("5bbe9f3d-6299-40b6-a9e6-e851de397563");

        private static readonly Guid SamsungGalaxyA56Id = Guid.Parse("fe523b04-4ed4-4b6d-824a-e5b6b2aa4f63");
        private static readonly Guid SamsungGalaxyA56HistoryId = Guid.Parse("f4ed88ac-6e5b-45a0-998b-490138f6c87c");

        private static readonly DateTime MobileCategoryChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2867);
        private static readonly DateTime PcCategoryChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2870);
        private static readonly DateTime TabletCategoryChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2871);

        private static readonly DateTime UsdCurrencyChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3042);
        private static readonly DateTime PlnCurrencyChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3043);
        private static readonly DateTime EurCurrencyChangedAt = new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3044);
        private static readonly DateTime SamsungGalaxyA56ChangedAt = new DateTime(2026, 1, 12, 10, 15, 42, 120, DateTimeKind.Utc).AddTicks(1730);

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedCurrencies(modelBuilder);
            SeedMobilePhones(modelBuilder);
        }

        private static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new
                {
                    Id = MobileCategoryId,
                    Code = "MOBILE",
                    Name = "Mobile",
                    IsActive = true,
                    ChangedAt = MobileCategoryChangedAt
                },
                new
                {
                    Id = PcCategoryId,
                    Code = "PC",
                    Name = "Personal Computer",
                    IsActive = true,
                    ChangedAt = PcCategoryChangedAt
                },
                new
                {
                    Id = TabletCategoryId,
                    Code = "TABLET",
                    Name = "Tablet",
                    IsActive = true,
                    ChangedAt = TabletCategoryChangedAt
                }
            );

            modelBuilder.Entity<CategoriesHistory>().HasData(
                new
                {
                    Id = MobileCategoryHistoryId,
                    CategoryId = MobileCategoryId,
                    Code = "MOBILE",
                    Name = "Mobile",
                    IsActive = true,
                    ChangedAt = MobileCategoryChangedAt,
                    Operation = Operation.Inserted
                },
                new
                {
                    Id = PcCategoryHistoryId,
                    CategoryId = PcCategoryId,
                    Code = "PC",
                    Name = "Personal Computer",
                    IsActive = true,
                    ChangedAt = PcCategoryChangedAt,
                    Operation = Operation.Inserted
                },
                new
                {
                    Id = TabletCategoryHistoryId,
                    CategoryId = TabletCategoryId,
                    Code = "TABLET",
                    Name = "Tablet",
                    IsActive = true,
                    ChangedAt = TabletCategoryChangedAt,
                    Operation = Operation.Inserted
                }
            );
        }

        private static void SeedCurrencies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new
                {
                    Id = UsdCurrencyId,
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true,
                    ChangedAt = UsdCurrencyChangedAt
                },
                new
                {
                    Id = PlnCurrencyId,
                    Code = "PLN",
                    Description = "Polish Złoty",
                    IsActive = true,
                    ChangedAt = PlnCurrencyChangedAt
                },
                new
                {
                    Id = EurCurrencyId,
                    Code = "EUR",
                    Description = "Euro",
                    IsActive = true,
                    ChangedAt = EurCurrencyChangedAt
                }
            );

            modelBuilder.Entity<CurrenciesHistory>().HasData(
                new
                {
                    Id = UsdCurrencyHistoryId,
                    CurrencyId = UsdCurrencyId,
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true,
                    ChangedAt = UsdCurrencyChangedAt,
                    Operation = Operation.Inserted
                },
                new
                {
                    Id = PlnCurrencyHistoryId,
                    CurrencyId = PlnCurrencyId,
                    Code = "PLN",
                    Description = "Polish Złoty",
                    IsActive = true,
                    ChangedAt = PlnCurrencyChangedAt,
                    Operation = Operation.Inserted
                },
                new
                {
                    Id = EurCurrencyHistoryId,
                    CurrencyId = EurCurrencyId,
                    Code = "EUR",
                    Description = "Euro",
                    IsActive = true,
                    ChangedAt = EurCurrencyChangedAt,
                    Operation = Operation.Inserted
                }
            );
        }

        private static void SeedMobilePhones(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MobilePhone>().HasData(
                new
                {
                    Id = SamsungGalaxyA56Id,
                    CategoryId = MobileCategoryId,
                    FingerPrint = true,
                    FaceId = false,
                    IsActive = true,
                    ChangedAt = SamsungGalaxyA56ChangedAt,
                    Name = "Samsung Galaxy A56 5G 8/256GB Czarny",
                    Description = "Nagrywaj niesamowite wideo selfie w Super HDR; Filmuj tętniące życiem noce; Rób niesamowite selfie aparatem 12 MP.",
                    MainPhoto = "samsung-galaxy-a56-5g-black-main.jpg",
                    OtherPhotos = new[]
                    {
                        "samsung-galaxy-a56-5g-black-1.jpg",
                        "samsung-galaxy-a56-5g-black-2.jpg"
                    },
                    CPU = "Samsung Exynos 1580 (1x 2.9 GHz, A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)",
                    GPU = "Brak danych",
                    Ram = "8 GB",
                    Storage = "256 GB",
                    DisplayType = "Super AMOLED",
                    RefreshRateHz = 120,
                    ScreenSizeInches = 6.7m,
                    Width = 2340,
                    Height = 1080,
                    BatteryType = "Li-Ion",
                    BatteryCapacity = 5000,
                    Has5G = true,
                    WiFi = true,
                    NFC = true,
                    Bluetooth = true,
                    Camera = "12 MP",
                    GPS = true,
                    AGPS = true,
                    Galileo = true,
                    GLONASS = true,
                    QZSS = true,
                    Sensors_Accelerometer = true,
                    Sensors_Gyroscope = true,
                    Proximity = true,
                    Compass = true,
                    Barometer = false,
                    Halla = true,
                    AmbientLight = true,
                    Amount = 1999.00m,
                    Currency = "PLN"
                }
            );

            modelBuilder.Entity<MobilePhonesHistory>().HasData(
                new
                {
                    Id = SamsungGalaxyA56HistoryId,
                    MobilePhoneId = SamsungGalaxyA56Id,
                    Name = "Samsung Galaxy A56 5G 8/256GB Czarny",
                    Description = "Nagrywaj niesamowite wideo selfie w Super HDR; Filmuj tętniące życiem noce; Rób niesamowite selfie aparatem 12 MP.",
                    MainPhoto = "samsung-galaxy-a56-5g-black-main.jpg",
                    OtherPhotos = "[\"samsung-galaxy-a56-5g-black-1.jpg\",\"samsung-galaxy-a56-5g-black-2.jpg\"]",
                    CPU = "Samsung Exynos 1580 (1x 2.9 GHz, A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)",
                    GPU = "Brak danych",
                    Ram = "8 GB",
                    Storage = "256 GB",
                    DisplayType = "Super AMOLED",
                    RefreshRateHz = 120,
                    ScreenSizeInches = 6.7m,
                    Width = 2340,
                    Height = 1080,
                    BatteryType = "Li-Ion",
                    BatteryCapacity = 5000,
                    GPS = true,
                    AGPS = true,
                    Galileo = true,
                    GLONASS = true,
                    QZSS = true,
                    Accelerometer = true,
                    Gyroscope = true,
                    Proximity = true,
                    Compass = true,
                    Barometer = false,
                    Halla = true,
                    AmbientLight = true,
                    Has5G = true,
                    WiFi = true,
                    NFC = true,
                    Bluetooth = true,
                    Camera = "12 MP",
                    FingerPrint = true,
                    FaceId = false,
                    CategoryId = MobileCategoryId,
                    PriceAmount = 1999.00m,
                    PriceCurrency = "PLN",
                    IsActive = true,
                    ChangedAt = SamsungGalaxyA56ChangedAt,
                    Operation = Operation.Inserted
                }
            );
        }
    }
}
