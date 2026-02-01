Feature: Get currencies

  Scenario: Get currencies returns list
    When I request the list of currencies
    Then the currency list is returned
      | Field      | Value |
      | StatusCode | 200   |
      | Code       | USD   |
