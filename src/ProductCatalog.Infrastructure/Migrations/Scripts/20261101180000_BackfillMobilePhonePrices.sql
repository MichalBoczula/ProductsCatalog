-- Fixed demo catalogue prices in PLN. Preserve positive/custom prices and other products.
-- Include all seed IDs so databases with older zero-valued Xiaomi seeds are repaired too.
DECLARE @SeedPriceCorrectionTime datetime2(7) = SYSUTCDATETIME();

UPDATE phone
SET PriceAmount = prices.Amount,
    ChangedAt = CASE
        WHEN phone.ChangedAt >= @SeedPriceCorrectionTime THEN DATEADD(NANOSECOND, 100, phone.ChangedAt)
        ELSE @SeedPriceCorrectionTime
    END
-- The updated snapshot and current row are written by the same atomic statement.
-- Keep the original Inserted history, including its original zero price.
OUTPUT NEWID(), inserted.Id, inserted.Name, inserted.Brand,
       inserted.Description, inserted.MainPhoto, inserted.OtherPhotos,
       inserted.CPU, inserted.GPU, inserted.Ram, inserted.Storage,
       inserted.DisplayType, inserted.RefreshRateHz, inserted.ScreenSizeInches,
       inserted.Width, inserted.Height, inserted.BatteryType, inserted.BatteryCapacity,
       inserted.GPS, inserted.AGPS, inserted.Galileo, inserted.GLONASS, inserted.QZSS,
       inserted.Accelerometer, inserted.Gyroscope, inserted.Proximity, inserted.Compass,
       inserted.Barometer, inserted.Halla, inserted.AmbientLight,
       inserted.[5G], inserted.WiFi, inserted.NFC, inserted.Bluetooth,
       inserted.Camera, inserted.FingerPrint, inserted.FaceId,
       inserted.PriceAmount, inserted.PriceCurrency,
       inserted.Description2, inserted.Description3, inserted.IsActive, inserted.ChangedAt,
       1 -- Operation.Updated
INTO dbo.TB_MobilePhones_History
     (Id, MobilePhoneId, Name, Brand,
      Description, MainPhoto, OtherPhotos,
      CPU, GPU, Ram, Storage,
      DisplayType, RefreshRateHz, ScreenSizeInches,
      Width, Height, BatteryType, BatteryCapacity,
      GPS, AGPS, Galileo, GLONASS, QZSS,
      Accelerometer, Gyroscope, Proximity, Compass,
      Barometer, Halla, AmbientLight,
      Has5G, WiFi, NFC, Bluetooth,
      Camera, FingerPrint, FaceId,
      PriceAmount, PriceCurrency,
      Description2, Description3, IsActive, ChangedAt, Operation)
FROM dbo.TB_MobilePhones AS phone
INNER JOIN (VALUES
    (CAST('0f62c3e1-8e3e-4b1f-9d74-3d6e2ff2c6d2' AS uniqueidentifier), CAST(2499.00 AS decimal(18, 2))), -- Xiaomi POCO F7
    ('a243cf6f-dde1-4c05-8a58-2fa7f04ed984',  999.00), -- Xiaomi Redmi 15
    ('5e9d3b34-8005-4b73-9e90-1cc25f8f5c34', 1999.00), -- Xiaomi Redmi Note 15 Pro
    ('07d39209-33b8-4274-b82d-2f0c4f341a9a', 2999.00), -- Xiaomi 15T
    ('8d5f9e61-8a71-4d9a-9d86-3f4ef0c5ed4a', 2799.00), -- Apple iPhone 15 128GB
    ('5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2', 3299.00), -- Apple iPhone 16 128GB
    ('3cf6e155-2a07-40e3-9c40-9be5a4c97d6c', 3999.00), -- Apple iPhone 17 256GB
    ('2a3c3fa7-3ffb-4df1-9454-5c243fcbfc7b', 5499.00), -- Apple iPhone 17 Pro 256GB
    ('b7a2a7c1-3caa-4de0-8f3b-0b3bdcc55d5f', 2499.00), -- Samsung Galaxy S24
    ('3c1c2b2e-0a63-4a6d-8f0c-5d3a1e8f9a21', 3499.00), -- Samsung Galaxy S25
    ('2c1d2f69-3a9f-4c51-8a6b-8f2c5d6b7a10', 1199.00), -- Samsung Galaxy A36
    ('f2c7d5b1-4a9e-4a1a-8e3c-9b2f1d7c6a55', 1599.00), -- Samsung Galaxy A56
    ('c9b7d8e1-0f4a-4d3b-9a1c-2e6f7b8c9d10',  599.00), -- Samsung Galaxy A16
    ('d7b0f3c2-4a6f-4b2d-8c1e-1f2a3b4c5d6e', 3999.00), -- Motorola Signature
    ('a5f1c3d7-2b9e-4f6a-8c1d-0e9b7a6c5d4f', 1299.00)  -- Motorola moto g86 power
) AS prices (Id, Amount) ON prices.Id = phone.Id
WHERE phone.PriceAmount = 0 AND phone.PriceCurrency = 'PLN';
