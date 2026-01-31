Feature: CreateCurrency

  Scenario: Create currency returns created response
    Given I have valid currency details
    When I submit the create currency request
    Then the currency is created successfully

  Scenario: Create currency fails with invalid details
    Given I have invalid currency details
    When I submit the create invalid currency request
    Then the currency creation fails with API error