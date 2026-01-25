using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMobilePhone1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[TB_MobilePhones]
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
           ('0f62c3e1-8e3e-4b1f-9d74-3d6e2ff2c6d2'
           ,'50 MP (Sony LYT-600, OIS) + 8 MP ultrawide, 20 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'The POCO F7 display has a 2772 x 1280 px resolution and a 120 Hz refresh rate. The image looks smooth and detailed, making it easier to watch movies and use apps. The smartphone also supports 5G, Wi-Fi 7, and Bluetooth 5.4, so you can download files quickly and browse the internet without delays. The in-display fingerprint reader improves convenience, and the USB-C port enables fast charging and data transfer.'
           ,'xiaomi-poco-f7-black-main.jpg'
           ,'Xiaomi POCO F7 12/512GB Black'
           ,'[""xiaomi-poco-f7-black-1.jpg"",""xiaomi-poco-f7-black-2.jpg""]'
           ,1
           ,1
           ,1
           ,1
           ,6500
           ,'Li-Ion'
           ,'Qualcomm Snapdragon 8s Gen 4'
           ,'AMOLED'
           ,'Adreno'
           ,163
           ,'12 GB'
           ,120
           ,6.83
           ,'512 GB'
           ,78
           ,2499.00
           ,'PLN'
           ,0
           ,0
           ,1
           ,0
           ,0
           ,1
           ,1
           ,0
           ,1
           ,1
           ,0
           ,1
           ,1
           ,'2025-12-26T20:15:00'
           ,'Xiaomi POCO F7 has a 50 MP main camera with a Sony LYT-600 sensor and optical image stabilization. Next to it is an 8 MP ultra-wide camera for landscape shots. The front camera is 20 MP and works well for selfies and video calls. A powerful 6500 mAh battery will last you through a full day of work and entertainment. 90 W charging lets you quickly top up the phone.'
           ,'POCO F7 has an elegant, flat body that is 8.2 mm thick and weighs 216 grams. It feels comfortable in your hand and looks great. Available colors include black, white, and silver, so you can choose the version that suits you best. The phone offers everything you need every day: fast performance, a good camera, long battery life, and attractive design. A great choice for anyone who wants a reliable smartphone at a good price.'
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
           ('5d1e8a59-2d8a-4f16-b251-9388e69b3a3f'
           ,'0f62c3e1-8e3e-4b1f-9d74-3d6e2ff2c6d2'
           ,'Xiaomi POCO F7 12/512GB Black'
           ,'The POCO F7 display has a 2772 x 1280 px resolution and a 120 Hz refresh rate. The image looks smooth and detailed, making it easier to watch movies and use apps. The smartphone also supports 5G, Wi-Fi 7, and Bluetooth 5.4, so you can download files quickly and browse the internet without delays. The in-display fingerprint reader improves convenience, and the USB-C port enables fast charging and data transfer.'
           ,'xiaomi-poco-f7-black-main.jpg'
           ,'[""xiaomi-poco-f7-black-1.jpg"",""xiaomi-poco-f7-black-2.jpg""]'
           ,'Qualcomm Snapdragon 8s Gen 4'
           ,'Adreno'
           ,'12 GB'
           ,'512 GB'
           ,'AMOLED'
           ,120
           ,6.83
           ,78
           ,163
           ,'Li-Ion'
           ,6500
           ,1
           ,0
           ,0
           ,0
           ,0
           ,1
           ,1
           ,1
           ,1
           ,0
           ,0
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'50 MP (Sony LYT-600, OIS) + 8 MP ultrawide, 20 MP front'
           ,1
           ,0
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,2499.00
           ,'PLN'
           ,1
           ,'2025-12-26T20:15:00'
           ,0
           ,'Xiaomi POCO F7 has a 50 MP main camera with a Sony LYT-600 sensor and optical image stabilization. Next to it is an 8 MP ultra-wide camera for landscape shots. The front camera is 20 MP and works well for selfies and video calls. A powerful 6500 mAh battery will last you through a full day of work and entertainment. 90 W charging lets you quickly top up the phone.'
           ,'POCO F7 has an elegant, flat body that is 8.2 mm thick and weighs 216 grams. It feels comfortable in your hand and looks great. Available colors include black, white, and silver, so you can choose the version that suits you best. The phone offers everything you need every day: fast performance, a good camera, long battery life, and attractive design. A great choice for anyone who wants a reliable smartphone at a good price.'
           ,'Xiaomi')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
