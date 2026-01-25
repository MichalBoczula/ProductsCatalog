using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone9 : Migration
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
           ('b7a2a7c1-3caa-4de0-8f3b-0b3bdcc55d5f'
           ,'50 MP (f/1.8) wide + 12 MP ultrawide + 10 MP telephoto, 12 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Circle to Search with Google brings a new way to search. While scrolling your favorite social app, use the S Pen or your finger to circle anything and get Google search results instantly.'
           ,'samsung-galaxy-s24-black-main.jpg'
           ,'Samsung Galaxy S24 8GB/128GB Black'
           ,'[""samsung-galaxy-s24-black-1.jpg"",""samsung-galaxy-s24-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,4000
           ,'Li-Ion'
           ,'Samsung Exynos 2400'
           ,'Dynamic AMOLED 2X'
           ,'Samsung GPU'
           ,147
           ,'8 GB'
           ,120
           ,6.20
           ,'128 GB'
           ,70
           ,0.00
           ,'PLN'
           ,0
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
           ,'2025-12-26T20:55:00'
           ,'Live Translate delivers quick translation during phone calls. AI helps you communicate in another language while speaking on the phone, and it works for text messages too.'
           ,'Photo Assist uses AI to fill backgrounds, move elements, resize images, or remove objects entirely. It is just the start of intelligent photo editing.'
           ,'Samsung')
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
           ('1aebd9d9-9c55-49bd-9e9b-8f34d05b2b66'
           ,'b7a2a7c1-3caa-4de0-8f3b-0b3bdcc55d5f'
           ,'Samsung Galaxy S24 8GB/128GB Black'
           ,'Circle to Search with Google brings a new way to search. While scrolling your favorite social app, use the S Pen or your finger to circle anything and get Google search results instantly.'
           ,'samsung-galaxy-s24-black-main.jpg'
           ,'[""samsung-galaxy-s24-black-1.jpg"",""samsung-galaxy-s24-black-2.jpg""]'
           ,'Samsung Exynos 2400'
           ,'Samsung GPU'
           ,'8 GB'
           ,'128 GB'
           ,'Dynamic AMOLED 2X'
           ,120
           ,6.20
           ,70
           ,147
           ,'Li-Ion'
           ,4000
           ,1
           ,0
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
           ,'50 MP (f/1.8) wide + 12 MP ultrawide + 10 MP telephoto, 12 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:55:00'
           ,0
           ,'Live Translate delivers quick translation during phone calls. AI helps you communicate in another language while speaking on the phone, and it works for text messages too.'
           ,'Photo Assist uses AI to fill backgrounds, move elements, resize images, or remove objects entirely. It is just the start of intelligent photo editing.'
           ,'Samsung')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
