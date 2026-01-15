Feature: Update mobile phone

  Scenario: Update mobile phone returns ok response
    Given an existing mobile phone which will be updated
    When I submit the update mobile phone request
    Then the mobile phone is updated successfully

  Scenario: Update mobile phone fails for missing mobile phone
    Given a mobile phone id that does not exist
    When I submit the update mobile phone request for missing mobile phone
    Then the mobile phone update fails with validation errors
