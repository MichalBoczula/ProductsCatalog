using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[TB_MobilePhones]
           ([Id],[Camera],[FingerPrint],[FaceId],[CategoryId],[Description],[MainPhoto],[Name],[OtherPhotos],
            [Bluetooth],[5G],[NFC],[WiFi],[BatteryCapacity],[BatteryType],[CPU],[DisplayType],[GPU],[Height],
            [Ram],[RefreshRateHz],[ScreenSizeInches],[Storage],[Width],[PriceAmount],[PriceCurrency],
            [AGPS],[GLONASS],[GPS],[Galileo],[QZSS],
            [Accelerometer],[AmbientLight],[Barometer],[Compass],[Gyroscope],[Halla],[Proximity],
            [IsActive],[ChangedAt],[Description2],[Description3],[Brand])
     VALUES
           ('2c1d2f69-3a9f-4c51-8a6b-8f2c5d6b7a10',
            '50 MP (f/1.8) wide + 8 MP ultrawide (f/2.2) + 5 MP macro (f/2.4), 12 MP front (f/2.2)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            'Watch the latest shows on a larger 6.7-inch FHD+ Super AMOLED FHD+ display. With a 120 Hz refresh rate and boosted brightness up to 1,200 nits, enjoy smooth scrolling and better visibility even outdoors.',
            '',
            'Samsung Galaxy A36 5G 8/256GB Black',
            '[]',
            1, 1, 1, 1,
            5000,
            'Li-Ion',
            'Qualcomm Snapdragon 6 Gen 3',
            'Super AMOLED',
            '',
            163,
            '8 GB',
            120,
            6.70,
            '256 GB',
            79,
            0.00,
            'PLN',
            1, 1, 1, 1, 1,
            1, 1, 0, 0, 1, 0, 0,
            1,
            '2026-01-25T00:00:00',
            'Unlock the full power of an octa-core processor with an efficient CPU, GPU and NPU built for multitasking, gaming and streaming. Galaxy A36 5G has improved cooling (up to 15% vs. the previous model), so it stays cool even under load.',
            'Power up every day. Stay connected all day with a 5,000 mAh (typical) battery that delivers up to 29 hours of continuous video playback. When it’s time to recharge, quickly top up with 45 W Super Fast Charging.',
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
           ('7a4a3c1f-9f2d-4f6a-9b11-1c2d3e4f5a66',
            '2c1d2f69-3a9f-4c51-8a6b-8f2c5d6b7a10',
            'Samsung Galaxy A36 5G 8/256GB Black',
            'Watch the latest shows on a larger 6.7-inch FHD+ Super AMOLED FHD+ display. With a 120 Hz refresh rate and boosted brightness up to 1,200 nits, enjoy smooth scrolling and better visibility even outdoors.',
            '',
            '[]',
            'Qualcomm Snapdragon 6 Gen 3',
            '',
            '8 GB',
            '256 GB',
            'Super AMOLED',
            120,
            6.70,
            79,
            163,
            'Li-Ion',
            5000,
            1, 1, 1, 1, 1,
            1, 1, 0, 0, 0, 0, 1,
            1, 1, 1, 1,
            '50 MP (f/1.8) wide + 8 MP ultrawide (f/2.2) + 5 MP macro (f/2.4), 12 MP front (f/2.2)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            0.00,
            'PLN',
            1,
            '2026-01-25T00:00:00',
            0,
            'Unlock the full power of an octa-core processor with an efficient CPU, GPU and NPU built for multitasking, gaming and streaming. Galaxy A36 5G has improved cooling (up to 15% vs. the previous model), so it stays cool even under load.',
            'Power up every day. Stay connected all day with a 5,000 mAh (typical) battery that delivers up to 29 hours of continuous video playback. When it’s time to recharge, quickly top up with 45 W Super Fast Charging.',
            'Samsung');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
