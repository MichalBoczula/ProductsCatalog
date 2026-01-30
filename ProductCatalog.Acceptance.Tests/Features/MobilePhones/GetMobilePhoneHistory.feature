Feature: GetMobilePhoneHistory

  Scenario: Get mobile phone history returns history
    Given an existing mobile phone with history
      | Field           | Value                                |
      | Name            | Test Mobile Phone                    |
      | Brand           | Brand                                |
      | Description     | Phone created by acceptance test     |
      | MainPhoto       | main-photo.jpg                       |
      | OtherPhotos     | photo-1.jpg, photo-2.jpg             |
      | CPU             | Octa-core                            |
      | GPU             | Adreno                               |
      | Ram             | 8GB                                  |
      | Storage         | 256GB                                |
      | DisplayType     | OLED                                 |
      | RefreshRateHz   | 120                                  |
      | ScreenSizeInches| 6.4                                  |
      | Width           | 72                                   |
      | Height          | 152                                  |
      | BatteryType     | Li-Ion                               |
      | BatteryCapacity | 4500                                 |
      | Has5G           | true                                 |
      | WiFi            | true                                 |
      | NFC             | true                                 |
      | Bluetooth       | true                                 |
      | GPS             | true                                 |
      | AGPS            | true                                 |
      | Galileo         | true                                 |
      | GLONASS         | true                                 |
      | QZSS            | true                                 |
      | Accelerometer   | true                                 |
      | Gyroscope       | true                                 |
      | Proximity       | true                                 |
      | Compass         | true                                 |
      | Barometer       | true                                 |
      | Halla           | false                                |
      | AmbientLight    | true                                 |
      | Camera          | camera                               |
      | FingerPrint     | true                                 |
      | FaceId          | true                                 |
      | PriceAmount     | 799.99                               |
      | PriceCurrency   | USD                                  |
      | Description2    | desc2                                |
      | Description3    | desc3                                |
    When I request the mobile phone history
    Then the mobile phone history is returned successfully
      | Field       | Expected |
      | Operation   | Inserted |
      | IsActive    | true     |
      | ChangedAt   | set      |

  Scenario: Get mobile phone history fails for missing mobile phone
    Given a missing mobile phone id
    When I request the mobile phone history for the missing id
    Then response show not found error for mobile phone history
