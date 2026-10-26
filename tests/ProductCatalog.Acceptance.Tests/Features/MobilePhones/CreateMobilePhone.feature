Feature: Create mobile phone

  @allure.label.scenarioId:REF07_CreateMobilePhone_01
  Scenario: Creating a phone stores an active catalogue entry and its first history snapshot
    A valid catalogue entry returns its details and a Location for subsequent reads.
    Given I have valid mobile phone details
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
    When I submit the create mobile phone request
    Then the mobile phone is created successfully
      | Field         | Value             |
      | StatusCode    | 201               |
      | HasId         | true              |
      | IsActive      | true              |
      | Name          | Test Mobile Phone |
      | Brand         | Brand             |
      | PriceAmount   | 799.99            |
      | PriceCurrency | USD               |
    And the created phone Location returns its saved details
    And the created phone has the following saved state and history
      | Field             | Value             |
      | CurrentRows       | 1                 |
      | Name              | Test Mobile Phone |
      | MainPhoto         | main-photo.jpg    |
      | IsActive          | true              |
      | HistoryOperations | Inserted          |

  @allure.label.scenarioId:REF07_CreateMobilePhone_02
  Scenario: Creating a phone rejects a blank secondary description
    A blank Description2 fails the phone string policy and cannot create a phone or history.
    Given I have invalid mobile phone details
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
      | Description2     |                                  |
      | Description3     | desc3                            |
    And the catalogue and history are recorded before the rejected request
    When I submit the create invalid mobile phone request
    Then the mobile phone creation fails with validation errors
      | Field                  | Value                                      |
      | StatusCode             | 400                                        |
      | Code                   | validation_failed                          |
      | ContentType            | application/problem+json                   |
      | ErrorCount             | 1                                          |
      | MissingPropertiesCount | 0                                          |
      | Title                  | Validation failed                          |
      | Detail                 | One or more validation errors occurred.    |
      | ErrorMessage           | Description2 cannot be null or whitespace. |
      | ErrorEntity            | MobilePhone                                |
      | ErrorName              | MobilePhonesStringValidationRule           |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
