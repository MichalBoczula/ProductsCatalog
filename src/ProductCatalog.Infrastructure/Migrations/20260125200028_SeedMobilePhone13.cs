using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone13 : Migration
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
           ('c9b7d8e1-0f4a-4d3b-9a1c-2e6f7b8c9d10',
            '50 MP (f/1.8) main with 10x digital zoom + 5 MP ultrawide (f/2.2) + 2 MP macro (f/2.4), 13 MP front (f/2.0)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            'Take detail-rich photos with the advanced Galaxy A16 LTE camera system. Use the high-resolution 50 MP main camera with 10x digital zoom to capture precious memories, the 5 MP ultrawide camera to record wide panoramas, and focus on close-up detail with the 2 MP macro camera. The 13 MP front camera lets you take sharp selfies that highlight key details.',
            '',
            'Samsung Galaxy A16 4/128GB Black',
            '[]',
            1, 0, 1, 1,
            5000,
            'Li-Ion',
            'MediaTek Helio G99 (2x 2.2 GHz Cortex-A76 + 6x 2.0 GHz A55)',
            'Super AMOLED',
            '',
            164,
            '4 GB',
            90,
            6.70,
            '128 GB',
            78,
            0.00,
            'PLN',
            1, 1, 1, 1, 1,
            1, 1, 0, 1, 1, 0, 1,
            1,
            '2026-01-25T00:00:00',
            'Enjoy what you love without worrying about battery life. With a 5,000 mAh battery, Galaxy A16 LTE lasts a long time on a single charge. And with 25 W Super Fast Charging, you’ll quickly get back to your favorite entertainment.',
            'Samsung Knox Vault is a safe-like security solution. It protects your sensitive data—such as PIN codes, passwords and other private information—preventing hackers from gaining access.',
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
           ('1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b4c5d',
            'c9b7d8e1-0f4a-4d3b-9a1c-2e6f7b8c9d10',
            'Samsung Galaxy A16 4/128GB Black',
            'Take detail-rich photos with the advanced Galaxy A16 LTE camera system. Use the high-resolution 50 MP main camera with 10x digital zoom to capture precious memories, the 5 MP ultrawide camera to record wide panoramas, and focus on close-up detail with the 2 MP macro camera. The 13 MP front camera lets you take sharp selfies that highlight key details.',
            '',
            '[]',
            'MediaTek Helio G99 (2x 2.2 GHz Cortex-A76 + 6x 2.0 GHz A55)',
            '',
            '4 GB',
            '128 GB',
            'Super AMOLED',
            90,
            6.70,
            78,
            164,
            'Li-Ion',
            5000,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 0, 0, 1,
            0, 1, 1, 1,
            '50 MP (f/1.8) main with 10x digital zoom + 5 MP ultrawide (f/2.2) + 2 MP macro (f/2.4), 13 MP front (f/2.0)',
            1, 0,
            '587480bb-c126-4f9b-b531-b0244daa4ba4',
            0.00,
            'PLN',
            1,
            '2026-01-25T00:00:00',
            0,
            'Enjoy what you love without worrying about battery life. With a 5,000 mAh battery, Galaxy A16 LTE lasts a long time on a single charge. And with 25 W Super Fast Charging, you’ll quickly get back to your favorite entertainment.',
            'Samsung Knox Vault is a safe-like security solution. It protects your sensitive data—such as PIN codes, passwords and other private information—preventing hackers from gaining access.',
            'Samsung');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
