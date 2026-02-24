Feature: Get mobile phones by amount

Scenario: Get mobile phones by amount returns matching list
	Given an existing list of mobile phones
		| Field            | Value                                 |
		| Brand            | Brand                                 |
		| Description      | Phone created by filter test          |
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
	When I request mobile phones with amount 2
	Then the mobile phone list is returned with the requested amount
		| Field            | Value        |
		| StatusCode       | 200          |
		| Amount           | 2            |

Scenario: Get mobile phones by amount returns an empty list when no mobile phones exist
	Given no mobile phones exist in the database
	When I request mobile phones with amount 3
	Then an empty mobile phone list is returned
