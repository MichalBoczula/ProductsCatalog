using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone10 : Migration
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
           ('3c1c2b2e-0a63-4a6d-8f0c-5d3a1e8f9a21'
           ,'50 MP (f/1.8) rear + 12 MP ultrawide (f/2.2) + 10 MP telephoto (f/2.4), 12 MP front (f/2.2)'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Galaxy AI. Welcome to a new era of mobile artificial intelligence with an AI partner that stays ahead of your needs. Simply let a natural conversation guide you to effortless completion of everyday tasks.'
           ,'photo'
           ,'Samsung Galaxy S25 12/256GB Green'
           ,'[]'
           ,1
           ,1
           ,1
           ,1
           ,4000
           ,'Li-Ion'
           ,'Qualcomm Snapdragon 8 Elite (2x 4.32 GHz + 6x 3.53 GHz)'
           ,'Dynamic AMOLED 2X'
           ,''
           ,147
           ,'12 GB'
           ,120
           ,6.20
           ,'256 GB'
           ,71
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
           ,0
           ,1
           ,1
           ,1
           ,1
           ,'2026-01-25T00:00:00'
           ,'Easy reel creation. Create reels effortlessly by gathering your video and simply using Auto Trim. AI detects key activities and trims them into highlight-ready clips that you can easily adjust to the desired length.'
           ,'A powerful processor designed specifically for Galaxy. Introducing a custom-built processor designed and optimized for Galaxy. With improved real-time ray tracing and Vulkan optimization, dive into the action and enjoy ultra-smooth, immersive gameplay. Galaxy S25 also features a battery optimized for longevity with a 4000 mAh capacity.'
           ,'Samsung');

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
           ('9b6a0e6b-6d39-4c61-9f3f-2c6c1ed9b9c5'
           ,'3c1c2b2e-0a63-4a6d-8f0c-5d3a1e8f9a21'
           ,'Samsung Galaxy S25 12/256GB Green'
           ,'Galaxy AI. Welcome to a new era of mobile artificial intelligence with an AI partner that stays ahead of your needs. Simply let a natural conversation guide you to effortless completion of everyday tasks.'
           ,'photo'
           ,'[]'
           ,'Qualcomm Snapdragon 8 Elite (2x 4.32 GHz + 6x 3.53 GHz)'
           ,''
           ,'12 GB'
           ,'256 GB'
           ,'Dynamic AMOLED 2X'
           ,120
           ,6.20
           ,71
           ,147
           ,'Li-Ion'
           ,4000
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
           ,1
           ,1
           ,'50 MP (f/1.8) rear + 12 MP ultrawide (f/2.2) + 10 MP telephoto (f/2.4), 12 MP front (f/2.2)'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,'2026-01-25T00:00:00'
           ,0
           ,'Easy reel creation. Create reels effortlessly by gathering your video and simply using Auto Trim. AI detects key activities and trims them into highlight-ready clips that you can easily adjust to the desired length.'
           ,'A powerful processor designed specifically for Galaxy. Introducing a custom-built processor designed and optimized for Galaxy. With improved real-time ray tracing and Vulkan optimization, dive into the action and enjoy ultra-smooth, immersive gameplay. Galaxy S25 also features a battery optimized for longevity with a 4000 mAh capacity.'
           ,'Samsung');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
