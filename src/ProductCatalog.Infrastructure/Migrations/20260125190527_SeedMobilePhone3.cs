using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone3 : Migration
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
           ('5e9d3b34-8005-4b73-9e90-1cc25f8f5c34'
           ,'200 MP (f/1.7) rear + 8 MP ultrawide, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'The heart of the Xiaomi Redmi Note 15 Pro is the advanced MediaTek Helio G200 Ultra processor, which delivers smooth performance even in the most demanding apps. Thanks to it, users can enjoy fast game and app launches, translating into a great experience during use. It is an ideal companion for avid gamers.'
           ,'xiaomi-redmi-note-15-pro-black-main.jpg'
           ,'Xiaomi Redmi Note 15 Pro 8/256GB Black'
           ,'[""xiaomi-redmi-note-15-pro-black-1.jpg"",""xiaomi-redmi-note-15-pro-black-2.jpg""]'
           ,1
           ,0
           ,1
           ,1
           ,6500
           ,'Carbon-silicon'
           ,'MediaTek Helio G200 Ultra'
           ,'AMOLED'
           ,'Mali-G57 MC2'
           ,163
           ,'8 GB'
           ,120
           ,6.77
           ,'256 GB'
           ,76
           ,1999.00
           ,'PLN'
           ,0
           ,1
           ,1
           ,1
           ,1
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,1
           ,'2025-12-26T20:25:00'
           ,'The smartphone features a 6.77-inch AMOLED display that delivers excellent image quality regardless of lighting. The 120 Hz refresh rate makes on-screen motion exceptionally smooth, which is valuable for browsing photos and gaming. Bring every moment to life with vivid colors and contrast.'
           ,'Xiaomi Redmi Note 15 Pro impresses with an advanced camera system, including a 200 MP main camera and an 8 MP ultrawide camera. With optical image stabilization and 4x optical zoom, every moment will look as if it was captured by a professional photographer. Share your memories in top quality.'
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
           ('a3f434ed-84d1-4b7a-88fd-4c5d3c598f24'
           ,'5e9d3b34-8005-4b73-9e90-1cc25f8f5c34'
           ,'Xiaomi Redmi Note 15 Pro 8/256GB Black'
           ,'The heart of the Xiaomi Redmi Note 15 Pro is the advanced MediaTek Helio G200 Ultra processor, which delivers smooth performance even in the most demanding apps. Thanks to it, users can enjoy fast game and app launches, translating into a great experience during use. It is an ideal companion for avid gamers.'
           ,'xiaomi-redmi-note-15-pro-black-main.jpg'
           ,'[""xiaomi-redmi-note-15-pro-black-1.jpg"",""xiaomi-redmi-note-15-pro-black-2.jpg""]'
           ,'MediaTek Helio G200 Ultra'
           ,'Mali-G57 MC2'
           ,'8 GB'
           ,'256 GB'
           ,'AMOLED'
           ,120
           ,6.77
           ,76
           ,163
           ,'Carbon-silicon'
           ,6500
           ,1
           ,0
           ,1
           ,1
           ,1
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
           ,'200 MP (f/1.7) rear + 8 MP ultrawide, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,1999.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:25:00'
           ,0
           ,'The smartphone features a 6.77-inch AMOLED display that delivers excellent image quality regardless of lighting. The 120 Hz refresh rate makes on-screen motion exceptionally smooth, which is valuable for browsing photos and gaming. Bring every moment to life with vivid colors and contrast.'
           ,'Xiaomi Redmi Note 15 Pro impresses with an advanced camera system, including a 200 MP main camera and an 8 MP ultrawide camera. With optical image stabilization and 4x optical zoom, every moment will look as if it was captured by a professional photographer. Share your memories in top quality.'
           ,'Xiaomi')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
