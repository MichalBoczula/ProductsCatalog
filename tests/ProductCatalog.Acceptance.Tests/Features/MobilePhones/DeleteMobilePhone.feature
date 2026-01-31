Feature: Delete mobile phone

  Scenario: Delete mobile phone returns ok response
    Given an existing mobile phone to delete
      | Field            | Value                             |
      | Name             | Test Mobile Phone                |
      | Brand            | Brand                             |
      | Description      | Phone created by acceptance test |
      | MainPhoto        | main-photo.jpg                    |
      | OtherPhotos      | photo-1.jpg, photo-2.jpg          |
      | CPU              | Octa-core                         |
      | GPU              | Adreno                            |
      | Ram              | 8GB                               |
      | Storage          | 256GB                             |
      | DisplayType      | OLED                              |
      | RefreshRateHz    | 120                               |
      | ScreenSizeInches | 6.4                               |
      | Width            | 72                                |
      | Height           | 152                               |
      | BatteryType      | Li-Ion                            |
      | BatteryCapacity  | 4500                              |
      | Has5G            | true                              |
      | WiFi             | true                              |
      | NFC              | true                              |
      | Bluetooth        | true                              |
      | GPS              | true                              |
      | AGPS             | true                              |
      | Galileo          | true                              |
      | GLONASS          | true                              |
      | QZSS             | true                              |
      | Accelerometer    | true                              |
      | Gyroscope        | true                              |
      | Proximity        | true                              |
      | Compass          | true                              |
      | Barometer        | true                              |
      | Halla            | false                             |
      | AmbientLight     | true                              |
      | Camera           | camera                            |
      | FingerPrint      | true                              |
      | FaceId           | true                              |
      | PriceAmount      | 799.99                            |
      | PriceCurrency    | USD                               |
      | Description2     | desc2                             |
      | Description3     | desc3                             |
    When I submit the delete mobile phone request
    Then the mobile phone is deleted successfully
      | Field         | Value             |
      | StatusCode    | 200               |
      | HasId         | true              |
      | IsActive      | false             |
      | Name          | Test Mobile Phone |
      | Brand         | Brand             |
      | PriceAmount   | 799.99            |
      | PriceCurrency | USD               |

  Scenario: Delete mobile phone fails for missing mobile phone
    Given a mobile phone id that does not exist
    When I submit the delete mobile phone request for missing mobile phone
    Then the mobile phone deletion fails with validation errors
      | Field        | Value                                   |
      | StatusCode   | 400                                     |
      | Status       | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Mobile phone cannot be null.            |
      | ErrorEntity  | MobilePhone                             |
      | ErrorName    | MobilePhonesIsNullValidationRule        |
