Feature: Get mobile phone history

  @allure.label.scenarioId:REF07_GetMobilePhoneHistory_01
  Scenario: Reading a newly created phone history returns its inserted snapshot
    The first history page contains exactly the state saved when the phone was created.
    Given an existing mobile phone with history
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
    When I request the saved phone history page
      | Field      | Value |
      | PageNumber | 1     |
      | PageSize   | 10    |
    Then the mobile phone history is returned successfully
      | Field         | Value             |
      | StatusCode    | 200               |
      | Operation     | Inserted          |
      | IsActive      | true              |
      | ChangedAt     | set               |
      | Amount        | 1                 |
      | Name          | Test Mobile Phone |
      | Brand         | Brand             |
      | MainPhoto     | main-photo.jpg    |
      | PriceAmount   | 799.99            |
      | PriceCurrency | USD               |

  @allure.label.scenarioId:REF07_GetMobilePhoneHistory_02
  Scenario: Reading history for an unknown phone returns a resource-not-found problem
    Valid pagination reaches resource lookup; an unknown phone ID returns 404.
    Given a missing mobile phone id
    And the catalogue and history are recorded before the rejected request
    When I request the mobile phone history for the missing id
    Then the unknown phone history returns a not-found problem
      | Field                  | Value                                                                                                                               |
      | StatusCode             | 404                                                                                                                                 |
      | Code                   | resource_not_found                                                                                                                  |
      | ContentType            | application/problem+json                                                                                                            |
      | ErrorCount             | 0                                                                                                                                   |
      | MissingPropertiesCount | 0                                                                                                                                   |
      | Title                  | Resource not found.                                                                                                                 |
      | Detail                 | Resource MobilePhoneHistoryDto identify by id {MobilePhoneId} cannot be found in databese during action GetMobilePhoneHistoryQuery. |
      | Instance               | /mobile-phones/{MobilePhoneId}/history                                                                                              |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetMobilePhoneHistory_03
  Scenario: Reading an existing phone with no history returns an empty page
    An existing catalogue entry without history is a successful empty result.
    Given an existing mobile phone without history
      | Field     | Value                 |
      | Name      | Phone without history |
      | Brand     | Brand                 |
      | MainPhoto | no-history.jpg        |
    When I request the saved phone history page
      | Field      | Value |
      | PageNumber | 1     |
      | PageSize   | 10    |
    Then the mobile phone history list is empty
      | Field      | Value |
      | StatusCode | 200   |
      | Amount     | 0     |

  Scenario Outline: Reading history rejects page number <pageNumber> before resource lookup
    Page numbers must be positive. Invalid pagination takes precedence even for an unknown phone ID.
    Given a missing mobile phone id
    And the catalogue and history are recorded before the rejected request
    When I request the mobile phone history with page number <pageNumber> and page size 10
    Then the mobile phone history request fails with validation error
      | Field                  | Value                                  |
      | StatusCode             | 400                                    |
      | Code                   | validation_failed                      |
      | ContentType            | application/problem+json               |
      | ErrorCount             | 1                                      |
      | MissingPropertiesCount | 0                                      |
      | ErrorMessage           | Page number must be greater than zero. |
      | ErrorEntity            | PageNumber                             |
      | ErrorName              | PaginationParametersValidationRule     |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

    Examples:
      | pageNumber | scenarioId                     |
      | 0          | REF07_GetMobilePhoneHistory_04 |
      | -1         | REF07_GetMobilePhoneHistory_04 |

  Scenario Outline: Reading history rejects page size <pageSize> before resource lookup
    Page sizes must be positive. Invalid pagination takes precedence even for an unknown phone ID.
    Given a missing mobile phone id
    And the catalogue and history are recorded before the rejected request
    When I request the mobile phone history with page number 1 and page size <pageSize>
    Then the mobile phone history request fails with validation error
      | Field                  | Value                                |
      | StatusCode             | 400                                  |
      | Code                   | validation_failed                    |
      | ContentType            | application/problem+json             |
      | ErrorCount             | 1                                    |
      | MissingPropertiesCount | 0                                    |
      | ErrorMessage           | Page size must be greater than zero. |
      | ErrorEntity            | PageSize                             |
      | ErrorName              | PaginationParametersValidationRule   |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

    Examples:
      | pageSize | scenarioId                     |
      | 0        | REF07_GetMobilePhoneHistory_05 |
      | -1       | REF07_GetMobilePhoneHistory_05 |
