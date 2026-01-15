Feature: Create mobile phone

  Scenario: Create mobile phone returns created response
    Given I have valid mobile phone details
    When I submit the create mobile phone request
    Then the mobile phone is created successfully

  Scenario: Create mobile phone fails with invalid details
    Given I have invalid mobile phone details
    When I submit the create invalid mobile phone request
    Then the mobile phone creation fails with validation errors
