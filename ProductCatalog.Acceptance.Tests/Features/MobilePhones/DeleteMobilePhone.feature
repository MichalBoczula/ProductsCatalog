Feature: Delete mobile phone

  Scenario: Delete mobile phone returns ok response
    Given an existing mobile phone to delete
    When I submit the delete mobile phone request
    Then the mobile phone is deleted successfully

  Scenario: Delete mobile phone fails for missing mobile phone
    Given a mobile phone id that does not exist
    When I submit the delete mobile phone request for missing mobile phone
    Then the mobile phone deletion fails with validation errors
