using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone5 : Migration
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
           ('8d5f9e61-8a71-4d9a-9d86-3f4ef0c5ed4a'
           ,'48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Everyday performance The Apple A16 Bionic delivers smooth app and game performance with high energy efficiency. As a result, iPhone 15 stays fast and responsive regardless of the number of running tasks. It is an ideal choice for those who want reliable performance.'
           ,'apple-iphone-15-blue-main.jpg'
           ,'Apple iPhone 15 128GB Blue'
           ,'[""apple-iphone-15-blue-1.jpg"",""apple-iphone-15-blue-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,0
           ,'Li-Ion'
           ,'Apple A16 Bionic'
           ,'OLED'
           ,'Apple GPU'
           ,148
           ,'6 GB'
           ,60
           ,6.10
           ,'128 GB'
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
           ,'2025-12-26T20:45:00'
           ,'Photos full of detail The 48 MP dual camera captures shots with exceptional quality. The ultrawide lens lets you fit more into the frame, and photography features let you experiment with styles. High-resolution videos will satisfy anyone who values a professional look.'
           ,'Safety and durability The model includes Face ID for fast, secure unlocking. The IP68 rating protects against water and dust, while Ceramic Shield strengthens the display. This blend of technology prepares iPhone 15 for everyday challenges.'
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
           ('e2e2f8f1-1d2a-4689-a8f5-4df9d1b3e5c9'
           ,'8d5f9e61-8a71-4d9a-9d86-3f4ef0c5ed4a'
           ,'Apple iPhone 15 128GB Blue'
           ,'Everyday performance The Apple A16 Bionic delivers smooth app and game performance with high energy efficiency. As a result, iPhone 15 stays fast and responsive regardless of the number of running tasks. It is an ideal choice for those who want reliable performance.'
           ,'apple-iphone-15-blue-main.jpg'
           ,'[""apple-iphone-15-blue-1.jpg"",""apple-iphone-15-blue-2.jpg""]'
           ,'Apple A16 Bionic'
           ,'Apple GPU'
           ,'6 GB'
           ,'128 GB'
           ,'OLED'
           ,60
           ,6.10
           ,72
           ,148
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
           ,'48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:45:00'
           ,0
           ,'Photos full of detail The 48 MP dual camera captures shots with exceptional quality. The ultrawide lens lets you fit more into the frame, and photography features let you experiment with styles. High-resolution videos will satisfy anyone who values a professional look.'
           ,'Safety and durability The model includes Face ID for fast, secure unlocking. The IP68 rating protects against water and dust, while Ceramic Shield strengthens the display. This blend of technology prepares iPhone 15 for everyday challenges.'
           ,'Apple')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
