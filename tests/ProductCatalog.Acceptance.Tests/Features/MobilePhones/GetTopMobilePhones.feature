Feature: Get top mobile phones

Scenario: Get top mobile phones returns records when mobile phones exist
	Given an existing set of mobile phones for top list
		| Field            | Value                                 |
		| Name             | Top Phone                             |
		| Brand            | Brand                                 |
		| Description      | Phone created by top endpoint acceptance test |
		| MainPhoto        | main-photo.jpg                        |
		| OtherPhotos      | photo-1.jpg, photo-2.jpg              |
		| CPU              | Octa-core                             |
		| GPU              | Adreno                                |
		| Ram              | 8GB                                   |
		| Storage          | 256GB                                 |
		| DisplayType      | OLED                                  |
		| RefreshRateHz    | 120                                   |
		| ScreenSizeInches | 6.4                                   |
		| Width            | 72                                    |
		| Height           | 152                                   |
		| BatteryType      | Li-Ion                                |
		| BatteryCapacity  | 4500                                  |
		| Has5G            | true                                  |
		| WiFi             | true                                  |
		| NFC              | true                                  |
		| Bluetooth        | true                                  |
		| GPS              | true                                  |
		| AGPS             | true                                  |
		| Galileo          | true                                  |
		| GLONASS          | true                                  |
		| QZSS             | true                                  |
		| Accelerometer    | true                                  |
		| Gyroscope        | true                                  |
		| Proximity        | true                                  |
		| Compass          | true                                  |
		| Barometer        | true                                  |
		| Halla            | false                                 |
		| AmbientLight     | true                                  |
		| Camera           | camera                                |
		| FingerPrint      | true                                  |
		| FaceId           | true                                  |
		| PriceAmount      | 799.99                                |
		| PriceCurrency    | USD                                   |
		| Description2     | desc2                                 |
		| Description3     | desc3                                 |
	When I request the top mobile phones list
	Then the top mobile phones response is successful and contains records

Scenario: Get top mobile phones returns not found when no mobile phones exist
	Given mobile phones table is empty for top list
	When I request the top mobile phones list
	Then the top mobile phones response is not found
