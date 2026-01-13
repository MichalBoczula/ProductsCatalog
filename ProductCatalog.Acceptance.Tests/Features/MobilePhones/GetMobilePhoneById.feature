Feature: GetMobilePhoneById

  Scenario: Get mobile phone by id returns mobile phone
    Given an existing mobile phone id
    When I request the mobile phone by id
    Then the mobile phone details are returned successfully

  Scenario: Get mobile phone by id fails for missing mobile phone
    Given a mobile phone without specific id doesnt exists
    When I send request for mobile phone by not existed id
    Then response show not found error for mobile phone
