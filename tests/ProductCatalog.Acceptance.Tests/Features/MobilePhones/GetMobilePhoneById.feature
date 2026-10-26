Feature: Get mobile phone by id

  @allure.label.scenarioId:REF07_GetMobilePhoneById_01
  Scenario: Reading a phone by ID returns its complete catalogue details
    A known ID returns the saved descriptions, specifications, features, photos and price.
    Given an existing mobile phone id
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
    When I request the mobile phone by id
    Then the mobile phone details are returned successfully
      | Field         | Value             |
      | StatusCode    | 200               |
      | IsActive      | true              |
      | Name          | Test Mobile Phone |
      | Brand         | Brand             |
      | PriceAmount   | 799.99            |
      | PriceCurrency | USD               |

  @allure.label.scenarioId:REF07_GetMobilePhoneById_02
  Scenario: Reading an unknown phone returns a resource-not-found problem
    A valid GUID with no catalogue record returns 404 with the requested path and a trace ID.
    Given a mobile phone ID that does not exist for reading
    And the catalogue and history are recorded before the rejected request
    When I request the mobile phone using the unknown ID
    Then the unknown phone returns a not-found problem
      | Field                  | Value                                                                                                                            |
      | StatusCode             | 404                                                                                                                              |
      | Code                   | resource_not_found                                                                                                               |
      | ContentType            | application/problem+json                                                                                                         |
      | ErrorCount             | 0                                                                                                                                |
      | MissingPropertiesCount | 0                                                                                                                                |
      | Title                  | Resource not found.                                                                                                              |
      | Detail                 | Resource MobilePhoneDetailsDto identify by id {MobilePhoneId} cannot be found in databese during action GetMobilePhoneByIdQuery. |
      | Instance               | /mobile-phones/{MobilePhoneId}                                                                                                   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
