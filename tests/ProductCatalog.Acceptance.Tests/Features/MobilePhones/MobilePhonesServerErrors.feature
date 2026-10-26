Feature: Mobile phone query server errors
  Unexpected read-repository failures return a safe problem without exposing internal details or changing stored data.

  @allure.label.scenarioId:REF07_GetMobilePhoneById_03
  Scenario: Reading a phone returns a safe error when its repository fails
    A controlled exception from the requested read dependency returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field  | Value                                               |
      | Method | GET                                                 |
      | Path   | /mobile-phones/00000000-0000-0000-0000-000000000001 |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetById                       |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetMobilePhones_04
  Scenario: Browsing phones returns a safe error when its repository fails
    A controlled exception from the requested read dependency returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field  | Value                   |
      | Method | GET                     |
      | Path   | /mobile-phones?amount=1 |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetPhones                     |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetMobilePhoneHistory_06
  Scenario: Reading phone history returns a safe error when its repository fails
    The history flow fails during its initial phone lookup, before loading history; it returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field  | Value                                                                                |
      | Method | GET                                                                                  |
      | Path   | /mobile-phones/00000000-0000-0000-0000-000000000001/history?pageNumber=1&pageSize=10 |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetById                       |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetTopMobilePhones_03
  Scenario: Reading top phones returns a safe error when its repository fails
    A controlled exception from the requested read dependency returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field  | Value              |
      | Method | GET                |
      | Path   | /mobile-phones/top |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetTop                        |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetFiltered_MobilePhones_04
  Scenario: Filtering phones returns a safe error when its repository fails
    A controlled exception from the requested read dependency returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field    | Value                 |
      | Method   | POST                  |
      | Path     | /mobile-phones/filter |
      | JsonBody | {"brand":"Motorola"}  |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetFilteredPhones             |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |

  @allure.label.scenarioId:REF07_GetMobilePhonesByIds_04
  Scenario: Reading selected phones returns a safe error when its repository fails
    A controlled exception from the requested read dependency returns internal_error and a trace ID.
    Given mobile phone reads fail unexpectedly
    And the catalogue and history are recorded before the rejected request
    When I send this request during the read failure
      | Field    | Value                                    |
      | Method   | POST                                     |
      | Path     | /mobile-phones/by-ids                    |
      | JsonBody | ["00000000-0000-0000-0000-000000000001"] |
    Then the read failure returns this safe problem
      | Field                  | Value                         |
      | StatusCode             | 500                           |
      | ContentType            | application/problem+json      |
      | Code                   | internal_error                |
      | Title                  | Server error                  |
      | Detail                 | An unexpected error occurred. |
      | ErrorCount             | 0                             |
      | MissingPropertiesCount | 0                             |
      | RepositoryCalls        | 1                             |
      | RepositoryMethod       | GetByIds                      |
    And the rejected request leaves the catalogue and history unchanged
      | Field                | Value |
      | CurrentRowsChange    | 0     |
      | HistoryRowsChange    | 0     |
      | CurrentDataUnchanged | true  |
      | HistoryDataUnchanged | true  |
