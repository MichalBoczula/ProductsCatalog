using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FourthMobilePhone : Migration
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
           ('28e3f472-c81f-48a9-a1e3-de00ad034c1b'
           ,'50 MP rear, 8 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Qualcomm Snapdragon processor
The smartphone is equipped with the Qualcomm Snapdragon 6s Gen 3 processor, which ensures smooth performance even with demanding apps and games. The device handles everyday tasks as well as more intensive processes with ease. Android 15 optimization lets you use new system features without performance drops.

Large display
The 6.9-inch Full HD+ display with a 144 Hz refresh rate delivers exceptional animation smoothness and image sharpness. It is a great solution for gamers and for people watching high-quality movies. The wide workspace makes using apps, browsing the internet, or editing photos more comfortable.

Camera for detailed photos
The 50 MP main camera with an f/1.8 aperture captures clear, detailed photos even in lower light. The built-in flash enables better-lit night shots. Additionally, the 8 MP front camera with an f/2.05 aperture provides sharp selfies and convenient video calls.'
           ,'xiaomi-redmi-15-midnight-black-main.jpg'
           ,'Xiaomi Redmi 15 5G 4/128GB Midnight Black'
           ,'[""xiaomi-redmi-15-midnight-black-1.jpg"",""xiaomi-redmi-15-midnight-black-2.jpg""]'
           ,1
           ,1
           ,0
           ,1
           ,7000
           ,'Lithium-ion'
           ,'Qualcomm Snapdragon 6s Gen 3 (2x 2.3 GHz, A78 + 6x 2.0 GHz, A55)'
           ,'Touchscreen, IPS'
           ,'Adreno'
           ,172
           ,'4 GB'
           ,144
           ,6.9
           ,'128 GB'
           ,83
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
           ('dd0783ef-1fa2-439b-bb49-a6b7d5027577'
           ,'28e3f472-c81f-48a9-a1e3-de00ad034c1b'
           ,'Xiaomi Redmi 15 5G 4/128GB Midnight Black'
           ,'Qualcomm Snapdragon processor
The smartphone is equipped with the Qualcomm Snapdragon 6s Gen 3 processor, which ensures smooth performance even with demanding apps and games. The device handles everyday tasks as well as more intensive processes with ease. Android 15 optimization lets you use new system features without performance drops.

Large display
The 6.9-inch Full HD+ display with a 144 Hz refresh rate delivers exceptional animation smoothness and image sharpness. It is a great solution for gamers and for people watching high-quality movies. The wide workspace makes using apps, browsing the internet, or editing photos more comfortable.

Camera for detailed photos
The 50 MP main camera with an f/1.8 aperture captures clear, detailed photos even in lower light. The built-in flash enables better-lit night shots. Additionally, the 8 MP front camera with an f/2.05 aperture provides sharp selfies and convenient video calls.'
           ,'xiaomi-redmi-15-midnight-black-main.jpg'
           ,'[""xiaomi-redmi-15-midnight-black-1.jpg"",""xiaomi-redmi-15-midnight-black-2.jpg""]'
           ,'Qualcomm Snapdragon 6s Gen 3 (2x 2.3 GHz, A78 + 6x 2.0 GHz, A55)'
           ,'Adreno'
           ,'4 GB'
           ,'128 GB'
           ,'Touchscreen, IPS'
           ,144
           ,6.9
           ,83
           ,172
           ,'Lithium-ion'
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
           ,1
           ,1
           ,1
           ,0
           ,1
           ,'50 MP rear, 8 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,SYSUTCDATETIME()
           ,0);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
