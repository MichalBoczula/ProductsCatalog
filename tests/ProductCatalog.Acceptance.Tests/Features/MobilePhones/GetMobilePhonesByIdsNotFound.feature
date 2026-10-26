Feature: Get mobile phones by ids not found

  @allure.label.scenarioId:REF07_GetMobilePhonesByIds_01
  Scenario: Reading selected phones returns not found when any requested ID is unknown
    One ID exists and one does not. The entire request fails instead of returning a partial list.
    Given an existing mobile phone and a missing mobile phone id
      | Field            | Value                            |
      | Name             | Test Mobile Phone                |
      | Brand            | Brand                            |
      | Description      | Phone created by acceptance test |
      | MainPhoto        | main-photo.jpg                   |
      | OtherPhotos      | photo-1.jpg, photo-2.jpg         |
      | CPU              | Octa-core                        |
      | GPU              | Adreno                           |
      | Ram              | 8GB                              |
      | Storage          | 256GB                            |
      | DisplayType      | OLED                             |
      | RefreshRateHz    | 120                              |
      | ScreenSizeInches | 6.4                              |
      | Width            | 72                               |
      | Height           | 152                              |
      | BatteryType      | Li-Ion                           |
      | BatteryCapacity  | 4500                             |
      | Has5G            | true                             |
      | WiFi             | true                             |
      | NFC              | true                             |
      | Bluetooth        | true                             |
      | GPS              | true                             |
      | AGPS             | true                             |
      | Galileo          | true                             |
      | GLONASS          | true                             |
      | QZSS             | true                             |
      | Accelerometer    | true                             |
      | Gyroscope        | true                             |
      | Proximity        | true                             |
      | Compass          | true                             |
      | Barometer        | true                             |
      | Halla            | false                            |
      | AmbientLight     | true                             |
      | Camera           | camera                           |
      | FingerPrint      | true                             |
      | FaceId           | true                             |
      | PriceAmount      | 799.99                           |
      | PriceCurrency    | USD                              |
      | Description2     | desc2                            |
      | Description3     | desc3                            |
    And the catalogue and history are recorded before the rejected request
    When I request the mobile phones by ids
      | Field                 | Value                  |
      | ExistingMobilePhoneId | {MobilePhoneId}        |
      | MissingMobilePhoneId  | {MissingMobilePhoneId} |
    Then the get mobile phones by ids response is not found
      | Field                  | Value                                                                                                                             |
      | StatusCode             | 404                                                                                                                               |
      | Code                   | resource_not_found                                                                                                                |
      | ContentType            | application/problem+json                                                                                                          |
      | ErrorCount             | 0                                                                                                                                 |
      | MissingPropertiesCount | 0                                                                                                                                 |
      | Title                  | Resource not found.                                                                                                               |
      | Detail                 | Resource MobilePhoneDto identify by id {MissingMobilePhoneId} cannot be found in databese during action GetMobilePhoneByIdsQuery. |
      | Instance               | /mobile-phones/by-ids                                                                                                             |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
