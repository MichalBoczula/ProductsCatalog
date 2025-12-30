Feature: Remove product

  Scenario: Remove product returns ok response
    Given an existing category with code "MOBILE"
    And an existing currency with code "USD"
    And an existing active product
    When I submit the remove product request
    Then the product is removed successfully
