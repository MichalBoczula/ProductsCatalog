Feature: GetProductById

  Scenario: Get product by id returns the product details
    Given an existing category with code "MOBILE"
    And an existing currency with code "USD"
    And an active product
    When I request the product by id
    Then the product details are returned successfully

  Scenario: Get product by id returns not found for missing product
    Given a non existing product id
    When I request the product by id
    Then a resource not found problem details is returned
