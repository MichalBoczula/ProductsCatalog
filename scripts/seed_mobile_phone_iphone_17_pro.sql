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
           ('9a981d50-7d2d-4c80-9d13-08ef6b182979'
           ,'48 MP rear + 48 MP ultrawide rear + 48 MP telephoto rear, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,'Durable construction combines Ceramic Shield 2 glass on the display with rear protection, and the IP68 rating helps protect against water and dust. iPhone 17 Pro supports eSIM (Dual eSIM), so you can add a plan in moments without a physical card, while MagSafe makes wireless charging and accessory use easier. It is a practical choice when you want a premium smartphone ready for an intense day.

iPhone 17 Pro delivers natural colors and high contrast, which helps when editing photos and watching video. ProMotion 120 Hz makes scrolling and animations smooth and touch response fast. With 2622 × 1206 pixels and 460 ppi, text stays readable and details remain sharp. This display is designed for eye comfort and reliable performance in any conditions.

Apple A19 Pro and iOS 26 work together to provide speed and stability - from many browser tabs, to office apps, to creative projects. iPhone 17 Pro runs smoothly even under heavier load, so you spend less time waiting and get more done. This performance translates into real benefits for work and entertainment.'
           ,'iphone-17-pro-silver-main.jpg'
           ,'Apple iPhone 17 Pro 256GB Silver'
           ,'["iphone-17-pro-silver-1.jpg","iphone-17-pro-silver-2.jpg"]'
           ,1
           ,1
           ,1
           ,1
           ,0
           ,'Lithium-ion'
           ,'Apple A19 Pro'
           ,'Touchscreen, OLED, Super Retina XDR, True Tone, Haptic Touch, ProMotion'
           ,'Apple GPU'
           ,150
           ,'Not specified'
           ,120
           ,6.3
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
           ('ebc76062-eaa4-46f1-a7e8-0c3af878b6b2'
           ,'9a981d50-7d2d-4c80-9d13-08ef6b182979'
           ,'Apple iPhone 17 Pro 256GB Silver'
           ,'Durable construction combines Ceramic Shield 2 glass on the display with rear protection, and the IP68 rating helps protect against water and dust. iPhone 17 Pro supports eSIM (Dual eSIM), so you can add a plan in moments without a physical card, while MagSafe makes wireless charging and accessory use easier. It is a practical choice when you want a premium smartphone ready for an intense day.

iPhone 17 Pro delivers natural colors and high contrast, which helps when editing photos and watching video. ProMotion 120 Hz makes scrolling and animations smooth and touch response fast. With 2622 × 1206 pixels and 460 ppi, text stays readable and details remain sharp. This display is designed for eye comfort and reliable performance in any conditions.

Apple A19 Pro and iOS 26 work together to provide speed and stability - from many browser tabs, to office apps, to creative projects. iPhone 17 Pro runs smoothly even under heavier load, so you spend less time waiting and get more done. This performance translates into real benefits for work and entertainment.'
           ,'iphone-17-pro-silver-main.jpg'
           ,'["iphone-17-pro-silver-1.jpg","iphone-17-pro-silver-2.jpg"]'
           ,'Apple A19 Pro'
           ,'Apple GPU'
           ,'Not specified'
           ,'256 GB'
           ,'Touchscreen, OLED, Super Retina XDR, True Tone, Haptic Touch, ProMotion'
           ,120
           ,6.3
           ,72
           ,150
           ,'Lithium-ion'
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
           ,'48 MP rear + 48 MP ultrawide rear + 48 MP telephoto rear, 18 MP front'
           ,0
           ,1
           ,'587480bb-c126-4f9b-b531-b0244daa4ba4'
           ,0.00
           ,'PLN'
           ,1
           ,SYSUTCDATETIME()
           ,0);
