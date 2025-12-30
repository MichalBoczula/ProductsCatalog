Feature: Create product

  Scenario: Create product returns created response
    Given an existing category with code "MOBILE"
    And an existing currency with code "USD"
    And I have valid product details
    When I submit the create product request
    Then the product is created successfully
