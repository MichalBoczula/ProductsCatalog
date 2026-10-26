Feature: Mobile phone query server errors
  Unexpected read failures return a safe error response with a trace identifier.
  Technical exception details must not be exposed to API consumers.

  @allure.label.scenarioId:REF07_GetMobilePhoneById_03
  Scenario: Get by ID returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Get by ID" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |

  @allure.label.scenarioId:REF07_GetMobilePhones_04
  Scenario: Get by amount returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Get by amount" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |

  @allure.label.scenarioId:REF07_GetMobilePhoneHistory_06
  Scenario: Get history returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Get history" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |

  @allure.label.scenarioId:REF07_GetTopMobilePhones_03
  Scenario: Get top returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Get top" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |

  @allure.label.scenarioId:REF07_GetFiltered_MobilePhones_04
  Scenario: Filter returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Filter" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |

  @allure.label.scenarioId:REF07_GetMobilePhonesByIds_04
  Scenario: Get by IDs returns a safe 500 response when reading fails
    Given mobile phone reads fail unexpectedly
    When I request "Get by IDs" during the read failure
    Then a safe mobile phone server error is returned
      | Field      | Value                         |
      | StatusCode | 500                           |
      | Title      | Server error                  |
      | Detail     | An unexpected error occurred. |
