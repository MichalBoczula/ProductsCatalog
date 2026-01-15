Feature: GetMobilePhoneHistory

  Scenario: Get mobile phone history returns history
    Given an existing mobile phone with history
    When I request the mobile phone history
    Then the mobile phone history is returned successfully

  Scenario: Get mobile phone history fails for missing mobile phone
    Given a missing mobile phone id
    When I request the mobile phone history for the missing id
    Then response show not found error for mobile phone history
