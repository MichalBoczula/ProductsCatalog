using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddThirdPhone : Migration
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
           ('6c6c7ad5-9af5-461c-9320-837bc9997ceb'
           ,'50 MP rear + 8 MP ultrawide rear, 20 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'A display that impresses and fast performance
The POCO F7 display has a resolution of 2772 x 1280 px and a 120 Hz refresh rate. The image looks smooth and detailed, making it easier to watch movies and use apps. The smartphone also supports 5G, Wi-Fi 7, and Bluetooth 5.4, so you can download files quickly and browse the internet without delays. Convenience is covered too - the fingerprint reader is in the screen. The USB-C port enables fast charging and fast data transfer. The phone has an IP68 rating, so it is resistant to dust and water.

Cameras and battery
Xiaomi POCO F7 has a 50 MP main camera with the Sony LYT-600 sensor and optical image stabilization. Next to it is an 8 MP ultrawide camera that helps with landscape shots. The front camera is 20 MP and works well for selfies and video calls. The powerful 6500 mAh battery will last a full day of work and entertainment. The 90 W charging allows you to quickly top up the phone.

Modern design and everyday functionality
POCO F7 has an elegant, flat body with a thickness of 8.2 mm and weighs 216 grams. It sits comfortably in the hand and looks great. Available colors are black, white, and silver, so you can choose the version that suits you best. The phone offers everything you need every day - fast performance, a good camera, long battery life, and attractive design. A great choice for people who want a reliable smartphone at a good price.'
           ,'xiaomi-poco-f7-black-main.jpg'
           ,'Xiaomi POCO F7 12/512GB Black'
           ,'[""xiaomi-poco-f7-black-1.jpg"",""xiaomi-poco-f7-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,6500
           ,'Lithium-ion'
           ,'Qualcomm Snapdragon 8s Gen 4'
           ,'Touchscreen, AMOLED'
           ,'Adreno'
           ,163
           ,'12 GB'
           ,120
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
           ('77045b5d-429f-4e80-9e61-c87fc8df6a4f'
           ,'6c6c7ad5-9af5-461c-9320-837bc9997ceb'
           ,'Xiaomi POCO F7 12/512GB Black'
           ,'A display that impresses and fast performance
The POCO F7 display has a resolution of 2772 x 1280 px and a 120 Hz refresh rate. The image looks smooth and detailed, making it easier to watch movies and use apps. The smartphone also supports 5G, Wi-Fi 7, and Bluetooth 5.4, so you can download files quickly and browse the internet without delays. Convenience is covered too - the fingerprint reader is in the screen. The USB-C port enables fast charging and fast data transfer. The phone has an IP68 rating, so it is resistant to dust and water.

Cameras and battery
Xiaomi POCO F7 has a 50 MP main camera with the Sony LYT-600 sensor and optical image stabilization. Next to it is an 8 MP ultrawide camera that helps with landscape shots. The front camera is 20 MP and works well for selfies and video calls. The powerful 6500 mAh battery will last a full day of work and entertainment. The 90 W charging allows you to quickly top up the phone.

Modern design and everyday functionality
POCO F7 has an elegant, flat body with a thickness of 8.2 mm and weighs 216 grams. It sits comfortably in the hand and looks great. Available colors are black, white, and silver, so you can choose the version that suits you best. The phone offers everything you need every day - fast performance, a good camera, long battery life, and attractive design. A great choice for people who want a reliable smartphone at a good price.'
           ,'xiaomi-poco-f7-black-main.jpg'
           ,'[""xiaomi-poco-f7-black-1.jpg"",""xiaomi-poco-f7-black-2.jpg""]'
           ,'Qualcomm Snapdragon 8s Gen 4'
           ,'Adreno'
           ,'12 GB'
           ,'512 GB'
           ,'Touchscreen, AMOLED'
           ,120
           ,6.83
           ,78
           ,163
           ,'Lithium-ion'
           ,6500
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
           ,'50 MP rear + 8 MP ultrawide rear, 20 MP front'
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
