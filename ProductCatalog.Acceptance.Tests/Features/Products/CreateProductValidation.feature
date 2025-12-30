Feature: Create product validation

  Scenario: Create product fails when category is invalid
    And an existing currency with code "USD"
    And I have invalid product details
    When I submit the create product request
    Then the product creation fails with validation errors
