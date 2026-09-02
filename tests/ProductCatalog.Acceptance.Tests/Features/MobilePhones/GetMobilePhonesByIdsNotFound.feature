Feature: Get mobile phones by ids not found

  Scenario: Get mobile phones by ids fails when one requested mobile phone does not exist
    Given an existing mobile phone and a missing mobile phone id
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
    When I request the mobile phones by ids
      | Field                 | Value                  |
      | ExistingMobilePhoneId | {MobilePhoneId}        |
      | MissingMobilePhoneId  | {MissingMobilePhoneId} |
    Then the get mobile phones by ids response is not found
      | Field      | Value                                                                                                                           |
      | StatusCode | 404                                                                                                                             |
      | Title      | Resource not found.                                                                                                             |
      | Detail     | Resource MobilePhoneDto identify by id {MissingMobilePhoneId} cannot be found in databese during action GetMobilePhoneByIdsQuery. |
      | Instance   | /mobile-phones/by-ids                                                                                                           |
