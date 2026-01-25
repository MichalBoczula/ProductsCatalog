using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone12 : Migration
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
           ('f2c7d5b1-4a9e-4a1a-8e3c-9b2f1d7c6a55',
            '50 MP (f/1.8) rear + 12 MP ultrawide (f/2.2) + 5 MP macro (f/2.2), 12 MP front (f/2.2)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            'Record stunning videos from your adventures and everyday life in Super HDR. The Galaxy A56 5G front camera has been upgraded to deliver optimized color and contrast for high-resolution selfie videos.',
            '',
            'Samsung Galaxy A56 5G 8/128GB Black',
            '[]',
            0, 1, 1, 1,
            5000,
            'Li-Ion',
            'Samsung Exynos 1580 (1x 2.9 GHz A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)',
            'Super AMOLED',
            '',
            162,
            '8 GB',
            120,
            6.70,
            '128 GB',
            78,
            0.00,
            'PLN',
            1, 1, 1, 1, 1,
            1, 1, 0, 1, 1, 1, 1,
            1,
            '2026-01-25T00:00:00',
            'Experience powerful performance from the upgraded octa-core processor. Plus, with an improved cooling system and storage up to 256 GB, you can multitask, stream and game more smoothly than ever.',
            'Immerse yourself in your favorite entertainment on a large 6.7-inch FHD+ Super AMOLED display. Vision Booster with up to 1,200 nits boosts clarity for an incredible viewing experience in any lighting.',
            'Samsung');

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
           ('5e1f7c9a-3d4b-4a1e-9c2f-8a7b6c5d4e33',
            'f2c7d5b1-4a9e-4a1a-8e3c-9b2f1d7c6a55',
            'Samsung Galaxy A56 5G 8/128GB Black',
            'Record stunning videos from your adventures and everyday life in Super HDR. The Galaxy A56 5G front camera has been upgraded to deliver optimized color and contrast for high-resolution selfie videos.',
            '',
            '[]',
            'Samsung Exynos 1580 (1x 2.9 GHz A720 + 3x 2.6 GHz A700 + 4x 1.95 GHz A500)',
            '',
            '8 GB',
            '128 GB',
            'Super AMOLED',
            120,
            6.70,
            78,
            162,
            'Li-Ion',
            5000,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 1, 1,
            1, 1, 1, 0,
            '50 MP (f/1.8) rear + 12 MP ultrawide (f/2.2) + 5 MP macro (f/2.2), 12 MP front (f/2.2)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            0.00,
            'PLN',
            1,
            '2026-01-25T00:00:00',
            0,
            'Experience powerful performance from the upgraded octa-core processor. Plus, with an improved cooling system and storage up to 256 GB, you can multitask, stream and game more smoothly than ever.',
            'Immerse yourself in your favorite entertainment on a large 6.7-inch FHD+ Super AMOLED display. Vision Booster with up to 1,200 nits boosts clarity for an incredible viewing experience in any lighting.',
            'Samsung');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
