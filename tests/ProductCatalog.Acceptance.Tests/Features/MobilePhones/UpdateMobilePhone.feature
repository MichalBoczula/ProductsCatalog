Feature: Update mobile phone

  Scenario: Update mobile phone returns ok response
    Given an existing mobile phone which will be updated
      | Field               | Value                            |
      | Name                | Updated Mobile Phone             |
      | Brand               | Brand                            |
      | Description         | Updated by acceptance test       |
      | MainPhoto           | updated-main.jpg                 |
      | OtherPhotos         | updated-photo-1.jpg, updated-photo-2.jpg |
      | CPU                 | Deca-core                        |
      | GPU                 | Mali                             |
      | Ram                 | 12GB                             |
      | Storage             | 512GB                            |
      | DisplayType         | AMOLED                           |
      | RefreshRateHz       | 144                              |
      | ScreenSizeInches    | 6.8                              |
      | Width               | 74                               |
      | Height              | 160                              |
      | BatteryType         | Li-Poly                          |
      | BatteryCapacity     | 5200                             |
      | Has5G               | false                            |
      | WiFi                | true                             |
      | NFC                 | false                            |
      | Bluetooth           | true                             |
      | GPS                 | true                             |
      | AGPS                | false                            |
      | Galileo             | true                             |
      | GLONASS             | true                             |
      | QZSS                | false                            |
      | Accelerometer       | true                             |
      | Gyroscope           | false                            |
      | Proximity           | true                             |
      | Compass             | true                             |
      | Barometer           | false                            |
      | Halla               | true                             |
      | AmbientLight        | true                             |
      | Camera              | camera                           |
      | FingerPrint         | false                            |
      | FaceId              | true                             |
      | PriceAmount         | 899.99                           |
      | PriceCurrency       | EUR                              |
      | Description2        | desc2                            |
      | Description3        | desc3                            |
    When I submit the update mobile phone request
    Then the mobile phone is updated successfully
      | Field         | Value                |
      | StatusCode    | 200                  |
      | Name          | Updated Mobile Phone |
      | Brand         | Brand                |
      | PriceAmount   | 899.99               |
      | PriceCurrency | EUR                  |

  Scenario: Update mobile phone fails for missing mobile phone
    Given mobile phone identify by id not exists
      | Field               | Value                            |
      | Name                | Updated Mobile Phone             |
      | Brand               | Brand                            |
      | Description         | Updated by acceptance test       |
      | MainPhoto           | updated-main.jpg                 |
      | OtherPhotos         | updated-photo-1.jpg, updated-photo-2.jpg |
      | CPU                 | Deca-core                        |
      | GPU                 | Mali                             |
      | Ram                 | 12GB                             |
      | Storage             | 512GB                            |
      | DisplayType         | AMOLED                           |
      | RefreshRateHz       | 144                              |
      | ScreenSizeInches    | 6.8                              |
      | Width               | 74                               |
      | Height              | 160                              |
      | BatteryType         | Li-Poly                          |
      | BatteryCapacity     | 5200                             |
      | Has5G               | false                            |
      | WiFi                | true                             |
      | NFC                 | false                            |
      | Bluetooth           | true                             |
      | GPS                 | true                             |
      | AGPS                | false                            |
      | Galileo             | true                             |
      | GLONASS             | true                             |
      | QZSS                | false                            |
      | Accelerometer       | true                             |
      | Gyroscope           | false                            |
      | Proximity           | true                             |
      | Compass             | true                             |
      | Barometer           | false                            |
      | Halla               | true                             |
      | AmbientLight        | true                             |
      | Camera              | camera                           |
      | FingerPrint         | false                            |
      | FaceId              | true                             |
      | PriceAmount         | 899.99                           |
      | PriceCurrency       | EUR                              |
      | Description2        | desc2                            |
      | Description3        | desc3                            |
    When I submit the update mobile phone request for missing mobile phone
    Then the mobile phone update fails with validation errors
      | Field        | Value                                   |
      | StatusCode   | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Mobile phone cannot be null.            |
      | ErrorEntity  | MobilePhone                             |
      | ErrorName    | MobilePhonesIsNullValidationRule        |
