Feature: Update Product
  Scenario: Update product returns updated contract
    Given an existing product
    When I update the product with:
      | name          | description        | amount | currency |
      | Updated Phone | Even nicer phone   | 149.99 | USD      |
    Then the response status code should be 200
    And the updated product contract matches the request