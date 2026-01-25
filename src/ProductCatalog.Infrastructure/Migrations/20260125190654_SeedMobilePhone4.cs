using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[TB_MobilePhones]
           ([Id]
           ,[Camera]
           ,[FingerPrint]
           ,[FaceId]
           ,[CategoryId]
           ,[Description]
           ,[MainPhoto]
           ,[Name]
           ,[OtherPhotos]
           ,[Bluetooth]
           ,[5G]
           ,[NFC]
           ,[WiFi]
           ,[BatteryCapacity]
           ,[BatteryType]
           ,[CPU]
           ,[DisplayType]
           ,[GPU]
           ,[Height]
           ,[Ram]
           ,[RefreshRateHz]
           ,[ScreenSizeInches]
           ,[Storage]
           ,[Width]
           ,[PriceAmount]
           ,[PriceCurrency]
           ,[AGPS]
           ,[GLONASS]
           ,[GPS]
           ,[Galileo]
           ,[QZSS]
           ,[Accelerometer]
           ,[AmbientLight]
           ,[Barometer]
           ,[Compass]
           ,[Gyroscope]
           ,[Halla]
           ,[Proximity]
           ,[IsActive]
           ,[ChangedAt]
           ,[Description2]
           ,[Description3]
           ,[Brand])
     VALUES
           ('07d39209-33b8-4274-b82d-2f0c4f341a9a'
           ,'50 MP (f/1.7) wide + 12 MP ultrawide + 50 MP telephoto, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'The Leica camera system in Xiaomi 15T is designed for users who expect professional results. Wide, ultrawide, and telephoto lenses capture a broad range of scenes, while Light Fusion 800 technology improves image quality in low light.'
           ,'xiaomi-15t-5g-black-main.jpg'
           ,'Xiaomi 15T 5G 12/512 Black 120Hz'
           ,'[""xiaomi-15t-5g-black-1.jpg"",""xiaomi-15t-5g-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,5500
           ,'Li-Ion'
           ,'MediaTek Dimensity 8400 Ultra'
           ,'AMOLED'
           ,'Mali-G720'
           ,164
           ,'12 GB'
           ,120
           ,6.83
           ,'512 GB'
           ,78
           ,2999.00
           ,'PLN'
           ,0
           ,0
           ,1
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,1
           ,'2025-12-26T20:30:00'
           ,'The 6.83-inch AMOLED screen in Xiaomi 15T delivers excellent visuals thanks to 1.5K resolution and a 120 Hz refresh rate. Brightness up to 3200 nits ensures readability in full sun, while eye-protection certifications help during long use. Slim bezels and a minimalist design make the smartphone look elegant and modern.'
           ,'The MediaTek Dimensity 8400-Ultra processor powers smooth apps, gaming, and multitasking. It works with LPDDR5X memory and fast UFS 4.1 storage for high responsiveness. A 5500 mAh battery provides long runtime, and 67W HyperCharge lets you top up quickly.'
           ,'Xiaomi')
GO

INSERT INTO [dbo].[TB_MobilePhones_History]
           ([Id]
           ,[MobilePhoneId]
           ,[Name]
           ,[Description]
           ,[MainPhoto]
           ,[OtherPhotos]
           ,[CPU]
           ,[GPU]
           ,[Ram]
           ,[Storage]
           ,[DisplayType]
           ,[RefreshRateHz]
           ,[ScreenSizeInches]
           ,[Width]
           ,[Height]
           ,[BatteryType]
           ,[BatteryCapacity]
           ,[GPS]
           ,[AGPS]
           ,[Galileo]
           ,[GLONASS]
           ,[QZSS]
           ,[Accelerometer]
           ,[Gyroscope]
           ,[Proximity]
           ,[Compass]
           ,[Barometer]
           ,[Halla]
           ,[AmbientLight]
           ,[Has5G]
           ,[WiFi]
           ,[NFC]
           ,[Bluetooth]
           ,[Camera]
           ,[FingerPrint]
           ,[FaceId]
           ,[CategoryId]
           ,[PriceAmount]
           ,[PriceCurrency]
           ,[IsActive]
           ,[ChangedAt]
           ,[Operation]
           ,[Description2]
           ,[Description3]
           ,[Brand])
     VALUES
           ('4c10aa34-e8a8-4c09-9b43-067442d64ddb'
           ,'07d39209-33b8-4274-b82d-2f0c4f341a9a'
           ,'Xiaomi 15T 5G 12/512 Black 120Hz'
           ,'The Leica camera system in Xiaomi 15T is designed for users who expect professional results. Wide, ultrawide, and telephoto lenses capture a broad range of scenes, while Light Fusion 800 technology improves image quality in low light.'
           ,'xiaomi-15t-5g-black-main.jpg'
           ,'[""xiaomi-15t-5g-black-1.jpg"",""xiaomi-15t-5g-black-2.jpg""]'
           ,'MediaTek Dimensity 8400 Ultra'
           ,'Mali-G720'
           ,'12 GB'
           ,'512 GB'
           ,'AMOLED'
           ,120
           ,6.83
           ,78
           ,164
           ,'Li-Ion'
           ,5500
           ,1
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,1
           ,1
           ,1
           ,'50 MP (f/1.7) wide + 12 MP ultrawide + 50 MP telephoto, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,2999.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:30:00'
           ,0
           ,'The 6.83-inch AMOLED screen in Xiaomi 15T delivers excellent visuals thanks to 1.5K resolution and a 120 Hz refresh rate. Brightness up to 3200 nits ensures readability in full sun, while eye-protection certifications help during long use. Slim bezels and a minimalist design make the smartphone look elegant and modern.'
           ,'The MediaTek Dimensity 8400-Ultra processor powers smooth apps, gaming, and multitasking. It works with LPDDR5X memory and fast UFS 4.1 storage for high responsiveness. A 5500 mAh battery provides long runtime, and 67W HyperCharge lets you top up quickly.'
           ,'Xiaomi')
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
