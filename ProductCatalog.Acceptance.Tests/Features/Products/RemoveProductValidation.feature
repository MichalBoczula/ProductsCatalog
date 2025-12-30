Feature: Remove product validation

  Scenario: Remove product fails when product is missing
    Given a product id that does not exist
    When I submit the remove product request
    Then the product removal fails with validation errors
