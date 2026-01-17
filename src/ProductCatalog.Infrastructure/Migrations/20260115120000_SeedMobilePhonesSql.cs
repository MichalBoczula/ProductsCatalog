using System;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductCatalog.Infrastructure.Contexts.Commands;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    [DbContext(typeof(ProductsContext))]
    [Migration("20260115120000_SeedMobilePhonesSql")]
    public partial class SeedMobilePhonesSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "INSERT INTO TB_MobilePhones " +
                "(Id, FingerPrint, FaceId, CategoryId, Description, MainPhoto, Name, OtherPhotos, " +
                "CPU, GPU, Ram, Storage, DisplayType, RefreshRateHz, ScreenSizeInches, Width, Height, " +
                "BatteryType, BatteryCapacity, [5G], Bluetooth, WiFi, NFC, GPS, AGPS, Galileo, GLONASS, QZSS, " +
                "Accelerometer, Gyroscope, Proximity, Compass, Barometer, Halla, AmbientLight, PriceAmount, PriceCurrency, IsActive, ChangedAt) " +
                "VALUES " +
                "('fe523b04-4ed4-4b6d-824a-e5b6b2aa4f63', 1, 0, '587480bb-c126-4f9b-b531-b0244daa4ba4', " +
                "'Nagrywaj niesamowite wideo selfie w Super HDR; Filmuj tętniące życiem noce; Rób niesamowite selfie aparatem 12 MP.', " +
                "'samsung-galaxy-a56-5g-black-main.jpg', " +
                "'Samsung Galaxy A56 5G 8/256GB Czarny', " +
                "'[\"samsung-galaxy-a56-5g-black-1.jpg\",\"samsung-galaxy-a56-5g-black-2.jpg\"]', " +
                "'Samsung Exynos 1580 (1x 2.9 GHz, A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)', " +
                "'Brak danych', " +
                "'8 GB', " +
                "'256 GB', " +
                "'Super AMOLED', " +
                "120, 6.7, 2340, 1080, " +
                "'Li-Ion', 5000, " +
                "1, 1, 1, 1, 1, 1, 1, 1, 1, " +
                "1, 1, 1, 1, 0, 1, 1, " +
                "1999.00, 'PLN', 1, '2026-01-12T10:15:42.1201730Z')");

            migrationBuilder.Sql(
                "INSERT INTO TB_MobilePhones_History " +
                "(Id, MobilePhoneId, Name, Description, MainPhoto, OtherPhotos, CPU, GPU, Ram, Storage, DisplayType, RefreshRateHz, ScreenSizeInches, Width, Height, " +
                "BatteryType, BatteryCapacity, GPS, AGPS, Galileo, GLONASS, QZSS, Accelerometer, Gyroscope, Proximity, Compass, Barometer, Halla, AmbientLight, " +
                "Has5G, WiFi, NFC, Bluetooth, FingerPrint, FaceId, CategoryId, PriceAmount, PriceCurrency, IsActive, ChangedAt, Operation) " +
                "VALUES " +
                "('f4ed88ac-6e5b-45a0-998b-490138f6c87c', 'fe523b04-4ed4-4b6d-824a-e5b6b2aa4f63', " +
                "'Samsung Galaxy A56 5G 8/256GB Czarny', " +
                "'Nagrywaj niesamowite wideo selfie w Super HDR; Filmuj tętniące życiem noce; Rób niesamowite selfie aparatem 12 MP.', " +
                "'samsung-galaxy-a56-5g-black-main.jpg', " +
                "'[\"samsung-galaxy-a56-5g-black-1.jpg\",\"samsung-galaxy-a56-5g-black-2.jpg\"]', " +
                "'Samsung Exynos 1580 (1x 2.9 GHz, A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)', " +
                "'Brak danych', " +
                "'8 GB', " +
                "'256 GB', " +
                "'Super AMOLED', " +
                "120, 6.7, 2340, 1080, " +
                "'Li-Ion', 5000, " +
                "1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, " +
                "1, 1, 1, 1, 1, 0, " +
                "'587480bb-c126-4f9b-b531-b0244daa4ba4', 1999.00, 'PLN', 1, '2026-01-12T10:15:42.1201730Z', 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE FROM TB_MobilePhones_History WHERE Id = 'f4ed88ac-6e5b-45a0-998b-490138f6c87c';");
            migrationBuilder.Sql(
                "DELETE FROM TB_MobilePhones WHERE Id = 'fe523b04-4ed4-4b6d-824a-e5b6b2aa4f63';");
        }
    }
}
