using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[TB_MobilePhones]
           ([Id],[Camera],[FingerPrint],[FaceId],[CategoryId],[Description],[MainPhoto],[Name],[OtherPhotos],
            [Bluetooth],[5G],[NFC],[WiFi],[BatteryCapacity],[BatteryType],[CPU],[DisplayType],[GPU],[Height],
            [Ram],[RefreshRateHz],[ScreenSizeInches],[Storage],[Width],[PriceAmount],[PriceCurrency],
            [AGPS],[GLONASS],[GPS],[Galileo],[QZSS],
            [Accelerometer],[AmbientLight],[Barometer],[Compass],[Gyroscope],[Halla],[Proximity],
            [IsActive],[ChangedAt],[Description2],[Description3],[Brand])
     VALUES
           ('d7b0f3c2-4a6f-4b2d-8c1e-1f2a3b4c5d6e',
            '50 MP rear (f/1.6) + 50 MP ultrawide (f/2.0) + 50 MP telephoto, 50 MP front (f/2.0); 3x optical zoom, 100x digital zoom; OIS; video up to 8K',
            1, 1,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            'Motorola Signature 5G features a huge 6.78-inch AMOLED display with a 165 Hz refresh rate. With a 2780 x 1264 resolution, every movie and photo gains incredible sharpness and breathtaking colors. Enjoy smoothness and intensity in every view for an even more satisfying multimedia experience.',
            '',
            'Motorola Signature 5G 16/512GB Carbon 165Hz',
            '[]',
            1, 1, 1, 1,
            5200,
            'Carbon-silicon',
            'Qualcomm Snapdragon 8 Gen 5',
            'AMOLED',
            'Adreno',
            162,
            '16 GB',
            165,
            6.78,
            '512 GB',
            76,
            0.00,
            'PLN',
            1, 1, 1, 1, 0,
            1, 1, 0, 1, 1, 0, 1,
            1,
            '2026-01-25T00:00:00',
            'Powered by the Qualcomm Snapdragon 8 Gen 5 and 16 GB of RAM, Motorola Signature 5G delivers exceptional performance. Whether you play demanding games, browse the web or multitask, it handles everything smoothly—keeping up with your pace of life.',
            'Motorola Signature 5G impresses with a triple-camera system featuring a 50 MP ultrawide lens and a telephoto camera with 3x optical zoom. With 8K video recording and features like optical image stabilization, every moment can be captured in top quality. Step into a more professional level of mobile photography and capture unforgettable moments.',
            'Motorola');

GO

INSERT INTO [dbo].[TB_MobilePhones_History]
           ([Id],[MobilePhoneId],[Name],[Description],[MainPhoto],[OtherPhotos],[CPU],[GPU],[Ram],[Storage],
            [DisplayType],[RefreshRateHz],[ScreenSizeInches],[Width],[Height],[BatteryType],[BatteryCapacity],
            [GPS],[AGPS],[Galileo],[GLONASS],[QZSS],
            [Accelerometer],[Gyroscope],[Proximity],[Compass],[Barometer],[Halla],[AmbientLight],
            [Has5G],[WiFi],[NFC],[Bluetooth],
            [Camera],[FingerPrint],[FaceId],[CategoryId],[PriceAmount],[PriceCurrency],[IsActive],[ChangedAt],
            [Operation],[Description2],[Description3],[Brand])
     VALUES
           ('3f6a9c1b-2d3e-4f5a-8b9c-1d2e3f4a5b6c',
            'd7b0f3c2-4a6f-4b2d-8c1e-1f2a3b4c5d6e',
            'Motorola Signature 5G 16/512GB Carbon 165Hz',
            'Motorola Signature 5G features a huge 6.78-inch AMOLED display with a 165 Hz refresh rate. With a 2780 x 1264 resolution, every movie and photo gains incredible sharpness and breathtaking colors. Enjoy smoothness and intensity in every view for an even more satisfying multimedia experience.',
            '',
            '[]',
            'Qualcomm Snapdragon 8 Gen 5',
            'Adreno',
            '16 GB',
            '512 GB',
            'AMOLED',
            165,
            6.78,
            76,
            162,
            'Carbon-silicon',
            5200,
            1, 1, 1, 1, 0,
            1, 1, 1, 1, 0, 0, 1,
            1, 1, 1, 1,
            '50 MP rear (f/1.6) + 50 MP ultrawide (f/2.0) + 50 MP telephoto, 50 MP front (f/2.0); 3x optical zoom, 100x digital zoom; OIS; video up to 8K',
            1, 1,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            0.00,
            'PLN',
            1,
            '2026-01-25T00:00:00',
            0,
            'Powered by the Qualcomm Snapdragon 8 Gen 5 and 16 GB of RAM, Motorola Signature 5G delivers exceptional performance. Whether you play demanding games, browse the web or multitask, it handles everything smoothly—keeping up with your pace of life.',
            'Motorola Signature 5G impresses with a triple-camera system featuring a 50 MP ultrawide lens and a telephoto camera with 3x optical zoom. With 8K video recording and features like optical image stabilization, every moment can be captured in top quality. Step into a more professional level of mobile photography and capture unforgettable moments.',
            'Motorola');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
