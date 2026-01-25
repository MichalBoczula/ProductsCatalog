using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone2 : Migration
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
           ('a243cf6f-dde1-4c05-8a58-2fa7f04ed984'
           ,'50 MP (f/1.8) rear, 8 MP (f/2.05) front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'The smartphone is equipped with a Qualcomm Snapdragon 6s Gen 3 processor, which delivers smooth performance even in demanding apps and games. The device handles everyday tasks as well as heavier processes with ease. Optimization for Android 15 lets you use the latest system features without performance drops.'
           ,'xiaomi-redmi-15-5g-black-main.jpg'
           ,'Xiaomi Redmi 15 5G 4/128GB Midnight Black'
           ,'[""xiaomi-redmi-15-5g-black-1.jpg"",""xiaomi-redmi-15-5g-black-2.jpg""]'
           ,1
           ,1
           ,0
           ,1
           ,7000
           ,'Li-Ion'
           ,'Qualcomm Snapdragon 6s Gen 3'
           ,'IPS'
           ,'Adreno'
           ,172
           ,'4 GB'
           ,144
           ,6.90
           ,'128 GB'
           ,83
           ,999.00
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
           ,'2025-12-26T20:20:00'
           ,'The 6.9-inch Full HD+ display with a 144 Hz refresh rate delivers exceptionally smooth animations and sharp visuals. It is a great choice for gamers as well as for watching high-quality videos. The wide workspace makes using apps, browsing the web, and editing photos more comfortable.'
           ,'The 50 MP main camera with an f/1.8 aperture captures crisp, detailed photos even in lower light. The built-in flash helps with better-lit night shots. In addition, the 8 MP front camera with an f/2.05 aperture delivers sharp selfies and convenient video calls.'
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
           ('3bff45f5-006b-449b-bd13-4f281e6fbd1b'
           ,'a243cf6f-dde1-4c05-8a58-2fa7f04ed984'
           ,'Xiaomi Redmi 15 5G 4/128GB Midnight Black'
           ,'The smartphone is equipped with a Qualcomm Snapdragon 6s Gen 3 processor, which delivers smooth performance even in demanding apps and games. The device handles everyday tasks as well as heavier processes with ease. Optimization for Android 15 lets you use the latest system features without performance drops.'
           ,'xiaomi-redmi-15-5g-black-main.jpg'
           ,'[""xiaomi-redmi-15-5g-black-1.jpg"",""xiaomi-redmi-15-5g-black-2.jpg""]'
           ,'Qualcomm Snapdragon 6s Gen 3'
           ,'Adreno'
           ,'4 GB'
           ,'128 GB'
           ,'IPS'
           ,144
           ,6.90
           ,83
           ,172
           ,'Li-Ion'
           ,7000
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
           ,1
           ,1
           ,0
           ,1
           ,'50 MP (f/1.8) rear, 8 MP (f/2.05) front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,999.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:20:00'
           ,0
           ,'The 6.9-inch Full HD+ display with a 144 Hz refresh rate delivers exceptionally smooth animations and sharp visuals. It is a great choice for gamers as well as for watching high-quality videos. The wide workspace makes using apps, browsing the web, and editing photos more comfortable.'
           ,'The 50 MP main camera with an f/1.8 aperture captures crisp, detailed photos even in lower light. The built-in flash helps with better-lit night shots. In addition, the 8 MP front camera with an f/2.05 aperture delivers sharp selfies and convenient video calls.'
           ,'Xiaomi')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
