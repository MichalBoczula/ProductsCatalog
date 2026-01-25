using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone6 : Migration
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
           ('5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2'
           ,'48 MP (f/1.6) rear + 12 MP ultrawide, 12 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Top-class display The 6.1-inch Super Retina XDR OLED screen makes watching movies and photos incredibly enjoyable. The 2556 x 1179 resolution delivers impressive sharpness and realism. True Tone and Haptic Touch improve everyday comfort. It is a display that raises the standard of mobile entertainment.'
           ,'apple-iphone-16-white-main.jpg'
           ,'Apple iPhone 16 128GB White'
           ,'[""apple-iphone-16-white-1.jpg"",""apple-iphone-16-white-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,3000
           ,'Li-Ion'
           ,'Apple A18'
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
           ,'2025-12-26T20:40:00'
           ,'Professional photography The 48 MP camera captures detailed photos in any lighting. The 12 MP ultrawide lens lets you capture a wider perspective. Modern imaging algorithms make videos and photos look professional. It is a solution for those who want more than standard photos.'
           ,'Top performance The Apple A18 processor delivers outstanding speed and stability. The smartphone runs iOS 18 with new features and improvements. The built-in battery supports fast and wireless charging for daily convenience. It is a phone that performs well in every situation.'
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
           ('4ee1f28f-4cfe-4a38-9d13-122f5c2c1f12'
           ,'5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2'
           ,'Apple iPhone 16 128GB White'
           ,'Top-class display The 6.1-inch Super Retina XDR OLED screen makes watching movies and photos incredibly enjoyable. The 2556 x 1179 resolution delivers impressive sharpness and realism. True Tone and Haptic Touch improve everyday comfort. It is a display that raises the standard of mobile entertainment.'
           ,'apple-iphone-16-white-main.jpg'
           ,'[""apple-iphone-16-white-1.jpg"",""apple-iphone-16-white-2.jpg""]'
           ,'Apple A18'
           ,'Apple GPU'
           ,'6 GB'
           ,'128 GB'
           ,'OLED'
           ,60
           ,6.10
           ,72
           ,148
           ,'Li-Ion'
           ,3000
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
           ,1
           ,0
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
           ,'2025-12-26T20:40:00'
           ,0
           ,'Professional photography The 48 MP camera captures detailed photos in any lighting. The 12 MP ultrawide lens lets you capture a wider perspective. Modern imaging algorithms make videos and photos look professional. It is a solution for those who want more than standard photos.'
           ,'Top performance The Apple A18 processor delivers outstanding speed and stability. The smartphone runs iOS 18 with new features and improvements. The built-in battery supports fast and wireless charging for daily convenience. It is a phone that performs well in every situation.'
           ,'Apple')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
