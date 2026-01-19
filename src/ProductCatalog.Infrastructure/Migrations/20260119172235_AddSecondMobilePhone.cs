using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSecondMobilePhone : Migration
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
           ,[ChangedAt])
     VALUES
           ('efcd7d86-3920-49fa-9b2b-81df588fbd28'
           ,'50 MP rear + 12 MP ultrawide rear + 50 MP telephoto rear, 32 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Professional mobile photography with Leica lenses
Thanks to the collaboration with Leica, Xiaomi 15T Pro offers tools that enable professional-grade photography. The Leica 5x Pro telephoto lens introduces 5x optical zoom and 10x zoom at the optical level in the T series, and at higher magnifications it is supported by Ultra Zoom 2.0 technology. New portrait modes, including creative bokeh effects, allow you to personalize photos to your needs.

Innovative communication and HyperOS system
Xiaomi 15T Pro introduces Xiaomi Astral Communication, a set of features that keep you connected even where coverage is missing. With Xiaomi Offline Communication mode, voice communication between devices is possible over distances up to 1.9 km without cellular or Wi-Fi networks. The new generation Xiaomi HyperOS 3 provides faster app launches, smoother task switching, and a modern interface look.

Premium design and reliable 5500 mAh battery
Modern minimalism in Xiaomi 15T Pro combines with the durability of premium-class materials. The aluminum 6M13 construction and Corning Gorilla Glass 7i increase resistance to damage and scratches, while the IP68 rating confirms full protection against water and dust. Users can choose from elegant color variants: Black, Gray, and Mocha Gold. The smartphone is equipped with a 5500 mAh battery that provides up to 15 hours of operation without charging.'
           ,'xiaomi-15t-pro-black-main.jpg'
           ,'Xiaomi 15T Pro 5G 12/512GB Black 144Hz'
           ,'[""xiaomi-15t-pro-black-1.jpg"",""xiaomi-15t-pro-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,5500
           ,'Lithium-ion'
           ,'MediaTek Dimensity 9400+'
           ,'Touchscreen, AMOLED'
           ,'Immortalis-G925 MC12'
           ,163
           ,'12 GB'
           ,144
           ,6.83
           ,'512 GB'
           ,78
           ,0.00
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
           ,SYSUTCDATETIME());

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
           ,[Operation])
     VALUES
           ('6d14bde9-88b1-429a-9aba-0af4350d8d5b'
           ,'efcd7d86-3920-49fa-9b2b-81df588fbd28'
           ,'Xiaomi 15T Pro 5G 12/512GB Black 144Hz'
           ,'Professional mobile photography with Leica lenses
Thanks to the collaboration with Leica, Xiaomi 15T Pro offers tools that enable professional-grade photography. The Leica 5x Pro telephoto lens introduces 5x optical zoom and 10x zoom at the optical level in the T series, and at higher magnifications it is supported by Ultra Zoom 2.0 technology. New portrait modes, including creative bokeh effects, allow you to personalize photos to your needs.

Innovative communication and HyperOS system
Xiaomi 15T Pro introduces Xiaomi Astral Communication, a set of features that keep you connected even where coverage is missing. With Xiaomi Offline Communication mode, voice communication between devices is possible over distances up to 1.9 km without cellular or Wi-Fi networks. The new generation Xiaomi HyperOS 3 provides faster app launches, smoother task switching, and a modern interface look.

Premium design and reliable 5500 mAh battery
Modern minimalism in Xiaomi 15T Pro combines with the durability of premium-class materials. The aluminum 6M13 construction and Corning Gorilla Glass 7i increase resistance to damage and scratches, while the IP68 rating confirms full protection against water and dust. Users can choose from elegant color variants: Black, Gray, and Mocha Gold. The smartphone is equipped with a 5500 mAh battery that provides up to 15 hours of operation without charging.'
           ,'xiaomi-15t-pro-black-main.jpg'
           ,'[""xiaomi-15t-pro-black-1.jpg"",""xiaomi-15t-pro-black-2.jpg""]'
           ,'MediaTek Dimensity 9400+'
           ,'Immortalis-G925 MC12'
           ,'12 GB'
           ,'512 GB'
           ,'Touchscreen, AMOLED'
           ,144
           ,6.83
           ,78
           ,163
           ,'Lithium-ion'
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
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'50 MP rear + 12 MP ultrawide rear + 50 MP telephoto rear, 32 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,SYSUTCDATETIME()
           ,0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
