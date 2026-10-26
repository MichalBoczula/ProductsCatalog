Feature: Get mobile phones by ids

  @allure.label.scenarioId:REF07_GetMobilePhonesByIds_02
  Scenario: Reading selected phone IDs returns exactly those catalogue entries
    Each requested active phone includes its summary, photo, specifications and price.
    Given an existing mobile phone for the by ids request
      | Field            | Value                                |
      | Name             | First By Ids Phone                   |
      | Brand            | First Brand                          |
      | Description      | First phone requested by id          |
      | MainPhoto        | first-main-photo.jpg                 |
      | OtherPhotos      | first-photo-1.jpg, first-photo-2.jpg |
      | CPU              | First Octa-core                      |
      | GPU              | First Adreno                         |
      | Ram              | 8GB                                  |
      | Storage          | 256GB                                |
      | DisplayType      | First OLED                           |
      | RefreshRateHz    | 120                                  |
      | ScreenSizeInches | 6.4                                  |
      | Width            | 72                                   |
      | Height           | 152                                  |
      | BatteryType      | Li-Ion                               |
      | BatteryCapacity  | 4500                                 |
      | Has5G            | true                                 |
      | WiFi             | true                                 |
      | NFC              | true                                 |
      | Bluetooth        | true                                 |
      | GPS              | true                                 |
      | AGPS             | true                                 |
      | Galileo          | true                                 |
      | GLONASS          | true                                 |
      | QZSS             | true                                 |
      | Accelerometer    | true                                 |
      | Gyroscope        | true                                 |
      | Proximity        | true                                 |
      | Compass          | true                                 |
      | Barometer        | true                                 |
      | Halla            | false                                |
      | AmbientLight     | true                                 |
      | Camera           | first camera                         |
      | FingerPrint      | true                                 |
      | FaceId           | true                                 |
      | PriceAmount      | 799.99                               |
      | PriceCurrency    | USD                                  |
      | Description2     | first desc2                          |
      | Description3     | first desc3                          |
    And an existing mobile phone for the by ids request
      | Field            | Value                                  |
      | Name             | Second By Ids Phone                    |
      | Brand            | Second Brand                           |
      | Description      | Second phone requested by id           |
      | MainPhoto        | second-main-photo.jpg                  |
      | OtherPhotos      | second-photo-1.jpg, second-photo-2.jpg |
      | CPU              | Second Octa-core                       |
      | GPU              | Second Adreno                          |
      | Ram              | 12GB                                   |
      | Storage          | 512GB                                  |
      | DisplayType      | Second AMOLED                          |
      | RefreshRateHz    | 144                                    |
      | ScreenSizeInches | 6.8                                    |
      | Width            | 76                                     |
      | Height           | 160                                    |
      | BatteryType      | Li-Poly                                |
      | BatteryCapacity  | 5000                                   |
      | Has5G            | true                                   |
      | WiFi             | true                                   |
      | NFC              | true                                   |
      | Bluetooth        | true                                   |
      | GPS              | true                                   |
      | AGPS             | true                                   |
      | Galileo          | true                                   |
      | GLONASS          | true                                   |
      | QZSS             | true                                   |
      | Accelerometer    | true                                   |
      | Gyroscope        | true                                   |
      | Proximity        | true                                   |
      | Compass          | true                                   |
      | Barometer        | true                                   |
      | Halla            | true                                   |
      | AmbientLight     | true                                   |
      | Camera           | second camera                          |
      | FingerPrint      | true                                   |
      | FaceId           | true                                   |
      | PriceAmount      | 999.99                                 |
      | PriceCurrency    | EUR                                    |
      | Description2     | second desc2                           |
      | Description3     | second desc3                           |
    When I request the mobile phones by their ids
      | Id                    |
      | {FirstMobilePhoneId}  |
      | {SecondMobilePhoneId} |
    Then the requested mobile phones are returned successfully
      | StatusCode | Id                    | Name                | Brand        | DisplayType   | ScreenSizeInches | Camera        | PriceAmount | PriceCurrency | MainPhoto             |
      | 200        | {FirstMobilePhoneId}  | First By Ids Phone  | First Brand  | First OLED    | 6.4              | first camera  | 799.99      | USD           | first-main-photo.jpg  |
      | 200        | {SecondMobilePhoneId} | Second By Ids Phone | Second Brand | Second AMOLED | 6.8              | second camera | 999.99      | EUR           | second-main-photo.jpg |


  @allure.label.scenarioId:REF07_GetMobilePhonesByIds_03
  Scenario: Reading selected phones rejects an empty ID list
    The by-IDs endpoint requires at least one ID and rejects an empty JSON array.
    Given the catalogue and history are recorded before the rejected request
    When I request the mobile phones with an empty id list
      | Field | Value |
      | Ids   | []    |
    Then the get mobile phones by ids request fails with validation error
      | Field                  | Value                                          |
      | StatusCode             | 400                                            |
      | Code                   | validation_failed                              |
      | ContentType            | application/problem+json                       |
      | ErrorCount             | 1                                              |
      | MissingPropertiesCount | 0                                              |
      | ErrorMessage           | At least one mobile phone Id must be provided. |
      | ErrorEntity            | mobilePhoneIds                                 |
      | ErrorName              | GetMobilePhoneByIdsQuery                       |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
