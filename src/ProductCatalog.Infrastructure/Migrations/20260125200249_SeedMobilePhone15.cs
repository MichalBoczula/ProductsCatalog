using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone15 : Migration
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
           ('a5f1c3d7-2b9e-4f6a-8c1d-0e9b7a6c5d4f',
            '50 MP (f/1.88) rear with OIS + 8 MP ultrawide (f/2.2), 32 MP front (f/2.2); video up to UHD 4K',
            1, 1,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            'Immerse yourself in vivid visuals with a 6.67-inch AMOLED display featuring HDR10+ and a 120 Hz refresh rate. With impressive peak brightness, it delivers outstanding image quality—from vibrant colors to deep blacks—regardless of lighting conditions. Dual stereo speakers with Dolby Atmos® complete the cinematic experience with spacious, rich sound.',
            '',
            'Motorola moto g86 power 5G 12/256GB Spellbound 120Hz',
            '[]',
            1, 1, 1, 1,
            6720,
            'Li-Ion',
            'MediaTek Dimensity 7300',
            'AMOLED',
            '',
            162,
            '12 GB',
            120,
            6.67,
            '256 GB',
            75,
            0.00,
            'PLN',
            1, 1, 1, 1, 1,
            1, 1, 0, 1, 1, 0, 0,
            1,
            '2026-01-25T00:00:00',
            'Motorola moto g86 power 5G is built for exceptional durability, ready to handle everyday challenges. A plastic frame with Corning® Gorilla® Glass 7i on the front provides solidity and comfort. IP68/IP69 and MIL-STD-810H certifications confirm resistance to water, dust and extreme conditions.',
            'Forget compromises on memory and performance. Android™ 15 delivers an intuitive experience and access to the latest innovations. Photography fans will appreciate the versatile rear camera system with AI features such as automatic smile detection and Night mode, plus a 32 MP front camera for perfect selfies. With a 6,720 mAh battery that lasts up to 41 hours and TurboPower fast charging.',
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
           ('c1b2a3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5e',
            'a5f1c3d7-2b9e-4f6a-8c1d-0e9b7a6c5d4f',
            'Motorola moto g86 power 5G 12/256GB Spellbound 120Hz',
            'Immerse yourself in vivid visuals with a 6.67-inch AMOLED display featuring HDR10+ and a 120 Hz refresh rate. With impressive peak brightness, it delivers outstanding image quality—from vibrant colors to deep blacks—regardless of lighting conditions. Dual stereo speakers with Dolby Atmos® complete the cinematic experience with spacious, rich sound.',
            '',
            '[]',
            'MediaTek Dimensity 7300',
            '',
            '12 GB',
            '256 GB',
            'AMOLED',
            120,
            6.67,
            75,
            162,
            'Li-Ion',
            6720,
            1, 1, 1, 1, 1,
            1, 1, 0, 1, 0, 0, 1,
            1, 1, 1, 1,
            '50 MP (f/1.88) rear with OIS + 8 MP ultrawide (f/2.2), 32 MP front (f/2.2); video up to UHD 4K',
            1, 1,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            0.00,
            'PLN',
            1,
            '2026-01-25T00:00:00',
            0,
            'Motorola moto g86 power 5G is built for exceptional durability, ready to handle everyday challenges. A plastic frame with Corning® Gorilla® Glass 7i on the front provides solidity and comfort. IP68/IP69 and MIL-STD-810H certifications confirm resistance to water, dust and extreme conditions.',
            'Forget compromises on memory and performance. Android™ 15 delivers an intuitive experience and access to the latest innovations. Photography fans will appreciate the versatile rear camera system with AI features such as automatic smile detection and Night mode, plus a 32 MP front camera for perfect selfies. With a 6,720 mAh battery that lasts up to 41 hours and TurboPower fast charging.',
            'Motorola');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
