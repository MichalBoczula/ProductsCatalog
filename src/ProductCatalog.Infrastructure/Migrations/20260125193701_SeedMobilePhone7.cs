using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone7 : Migration
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
           ('3cf6e155-2a07-40e3-9c40-9be5a4c97d6c'
           ,'48 MP (f/1.6) rear + 48 MP ultrawide, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Phenomenal display The 6.3-inch Super Retina XDR OLED screen with a 120 Hz refresh rate makes browsing, watching movies, and gaming a standout experience. ProMotion adjusts the refresh rate to the task, improving smoothness and saving power. The 2622 x 1206 resolution delivers crisp detail. True Tone and Haptic Touch make interaction natural and intuitive.'
           ,'apple-iphone-17-black-main.jpg'
           ,'Apple iPhone 17 256GB Black'
           ,'[""apple-iphone-17-black-1.jpg"",""apple-iphone-17-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,0
           ,'Li-Ion'
           ,'Apple A19'
           ,'OLED'
           ,'Apple GPU'
           ,150
           ,'8 GB'
           ,120
           ,6.30
           ,'256 GB'
           ,72
           ,0.00
           ,'PLN'
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,0
           ,1
           ,1
           ,'2025-12-26T20:50:00'
           ,'Professional photos and video Apple iPhone 17 has a 48 MP main camera and a 48 MP ultrawide camera, so you can capture every detail in different conditions. The 18 MP front camera is great for selfies and video calls. High-quality video recording lets you capture footage that looks professional and detailed. Slow-motion adds creativity and lets you create impressive shots.'
           ,'Performance and smooth operation The Apple A19 processor keeps the device fast and stable, even with many apps open. Whether you use media, office apps, or games, the smartphone stays smooth. With 256 GB of storage, you have space for your most important data, photos, and videos. iOS 26 offers intuitive operation and a wide range of features that make everyday life easier.'
           ,'Apple')
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
           ('6f3d9d16-2a11-4950-a20c-6b1b55bfe6d6'
           ,'3cf6e155-2a07-40e3-9c40-9be5a4c97d6c'
           ,'Apple iPhone 17 256GB Black'
           ,'Phenomenal display The 6.3-inch Super Retina XDR OLED screen with a 120 Hz refresh rate makes browsing, watching movies, and gaming a standout experience. ProMotion adjusts the refresh rate to the task, improving smoothness and saving power. The 2622 x 1206 resolution delivers crisp detail. True Tone and Haptic Touch make interaction natural and intuitive.'
           ,'apple-iphone-17-black-main.jpg'
           ,'[""apple-iphone-17-black-1.jpg"",""apple-iphone-17-black-2.jpg""]'
           ,'Apple A19'
           ,'Apple GPU'
           ,'8 GB'
           ,'256 GB'
           ,'OLED'
           ,120
           ,6.30
           ,72
           ,150
           ,'Li-Ion'
           ,0
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,0
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'48 MP (f/1.6) rear + 48 MP ultrawide, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:50:00'
           ,0
           ,'Professional photos and video Apple iPhone 17 has a 48 MP main camera and a 48 MP ultrawide camera, so you can capture every detail in different conditions. The 18 MP front camera is great for selfies and video calls. High-quality video recording lets you capture footage that looks professional and detailed. Slow-motion adds creativity and lets you create impressive shots.'
           ,'Performance and smooth operation The Apple A19 processor keeps the device fast and stable, even with many apps open. Whether you use media, office apps, or games, the smartphone stays smooth. With 256 GB of storage, you have space for your most important data, photos, and videos. iOS 26 offers intuitive operation and a wide range of features that make everyday life easier.'
           ,'Apple')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
