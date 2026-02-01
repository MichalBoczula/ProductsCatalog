Feature: Get categories

  Scenario: Get categories returns list
    When I request the list of categories
    Then the category list is returned
      | Field      | Value  |
      | StatusCode | 200    |
      | Code       | MOBILE |
