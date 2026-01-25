using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone8 : Migration
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
           ('2a3c3fa7-3ffb-4df1-9454-5c243fcbfc7b'
           ,'48 MP wide + 48 MP ultrawide + 48 MP telephoto, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'The durable build combines Ceramic Shield 2 glass on the display with reinforced back protection, and an IP68 rating helps protect against water and dust. iPhone 17 Pro supports eSIM (Dual eSIM), so you can add a plan in moments without a physical card, while MagSafe makes wireless charging and accessories effortless. It is a practical choice when you want a premium smartphone ready for a fast-paced day.'
           ,'apple-iphone-17-pro-silver-main.jpg'
           ,'Apple iPhone 17 Pro 256GB Silver'
           ,'[""apple-iphone-17-pro-silver-1.jpg"",""apple-iphone-17-pro-silver-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,0
           ,'Li-Ion'
           ,'Apple A19 Pro'
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
           ,'2025-12-26T20:35:00'
           ,'iPhone 17 Pro shows natural colors and strong contrast, which helps when editing photos and watching video. ProMotion 120 Hz makes scrolling and animations smooth, with fast touch response. With 2622 x 1206 pixels and 460 ppi, text stays readable and details stay sharp. It is a screen designed for eye comfort and reliable performance in any conditions.'
           ,'Apple A19 Pro and iOS 26 work together for speed and stability, from many browser tabs to office apps and creative projects. iPhone 17 Pro stays smooth under heavier loads, so you wait less and do more. This performance translates into real benefits for work and entertainment.'
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
           ('a624844f-2d5b-4018-8dad-6e12b18ca315'
           ,'2a3c3fa7-3ffb-4df1-9454-5c243fcbfc7b'
           ,'Apple iPhone 17 Pro 256GB Silver'
           ,'The durable build combines Ceramic Shield 2 glass on the display with reinforced back protection, and an IP68 rating helps protect against water and dust. iPhone 17 Pro supports eSIM (Dual eSIM), so you can add a plan in moments without a physical card, while MagSafe makes wireless charging and accessories effortless. It is a practical choice when you want a premium smartphone ready for a fast-paced day.'
           ,'apple-iphone-17-pro-silver-main.jpg'
           ,'[""apple-iphone-17-pro-silver-1.jpg"",""apple-iphone-17-pro-silver-2.jpg""]'
           ,'Apple A19 Pro'
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
           ,'48 MP wide + 48 MP ultrawide + 48 MP telephoto, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:35:00'
           ,0
           ,'iPhone 17 Pro shows natural colors and strong contrast, which helps when editing photos and watching video. ProMotion 120 Hz makes scrolling and animations smooth, with fast touch response. With 2622 x 1206 pixels and 460 ppi, text stays readable and details stay sharp. It is a screen designed for eye comfort and reliable performance in any conditions.'
           ,'Apple A19 Pro and iOS 26 work together for speed and stability, from many browser tabs to office apps and creative projects. iPhone 17 Pro stays smooth under heavier loads, so you wait less and do more. This performance translates into real benefits for work and entertainment.'
           ,'Apple')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
