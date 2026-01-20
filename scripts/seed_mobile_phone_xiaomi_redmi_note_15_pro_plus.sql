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
           ('62e053f3-b8b0-4080-b93b-d430b62eb56e'
           ,'200 MP rear + 8 MP ultrawide rear, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Powerful performance
Xiaomi Redmi Note 15 Pro+ is powered by the Qualcomm Snapdragon 7s Gen 4 processor, which delivers impressive performance even in the most demanding applications. With 8 GB of RAM, the smartphone runs smoothly regardless of the number of tasks running. Users can enjoy fast app switching and instant game launches.

Impressive display
The smartphone is equipped with a 6.83-inch AMOLED display, delivering vivid colors and deep blacks. Thanks to the 2772 x 1280 resolution and 120 Hz refresh rate, every movie, game, or photo looks stunning. It is an ideal solution for multimedia and gaming enthusiasts who expect the highest image quality.

Advanced camera system
Xiaomi Redmi Note 15 Pro+ stands out with a triple camera setup, including an impressive 200 MP main lens that captures the finest details. Additionally, the 8 MP ultrawide lens provides endless photographic possibilities. This smartphone is perfect for photography lovers, offering UHD 4K video recording.'
           ,'xiaomi-redmi-note-15-pro-plus-black-main.jpg'
           ,'Xiaomi Redmi Note 15 Pro+ 5G 8/256GB Black'
           ,'["xiaomi-redmi-note-15-pro-plus-black-1.jpg","xiaomi-redmi-note-15-pro-plus-black-2.jpg"]'
           ,1
           ,1
           ,1
           ,1
           ,6500
           ,'Carbon-silicon'
           ,'Qualcomm Snapdragon 7s Gen 4 (1x 2.7 GHz, A720 + 3x 2.4 GHz, A720 + 4x 1.8 GHz, A520)'
           ,'Touchscreen, AMOLED'
           ,'Adreno'
           ,163
           ,'8 GB'
           ,120
           ,6.83
           ,'256 GB'
           ,78
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
           ('2b7c2e69-8f90-4f5c-86f3-183ddfb2f1be'
           ,'62e053f3-b8b0-4080-b93b-d430b62eb56e'
           ,'Xiaomi Redmi Note 15 Pro+ 5G 8/256GB Black'
           ,'Powerful performance
Xiaomi Redmi Note 15 Pro+ is powered by the Qualcomm Snapdragon 7s Gen 4 processor, which delivers impressive performance even in the most demanding applications. With 8 GB of RAM, the smartphone runs smoothly regardless of the number of tasks running. Users can enjoy fast app switching and instant game launches.

Impressive display
The smartphone is equipped with a 6.83-inch AMOLED display, delivering vivid colors and deep blacks. Thanks to the 2772 x 1280 resolution and 120 Hz refresh rate, every movie, game, or photo looks stunning. It is an ideal solution for multimedia and gaming enthusiasts who expect the highest image quality.

Advanced camera system
Xiaomi Redmi Note 15 Pro+ stands out with a triple camera setup, including an impressive 200 MP main lens that captures the finest details. Additionally, the 8 MP ultrawide lens provides endless photographic possibilities. This smartphone is perfect for photography lovers, offering UHD 4K video recording.'
           ,'xiaomi-redmi-note-15-pro-plus-black-main.jpg'
           ,'["xiaomi-redmi-note-15-pro-plus-black-1.jpg","xiaomi-redmi-note-15-pro-plus-black-2.jpg"]'
           ,'Qualcomm Snapdragon 7s Gen 4 (1x 2.7 GHz, A720 + 3x 2.4 GHz, A720 + 4x 1.8 GHz, A520)'
           ,'Adreno'
           ,'8 GB'
           ,'256 GB'
           ,'Touchscreen, AMOLED'
           ,120
           ,6.83
           ,78
           ,163
           ,'Carbon-silicon'
           ,6500
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
           ,0
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'200 MP rear + 8 MP ultrawide rear, 32 MP front'
           ,1
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,SYSUTCDATETIME()
           ,0);
