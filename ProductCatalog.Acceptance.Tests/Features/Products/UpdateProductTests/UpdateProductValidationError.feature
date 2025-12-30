Feature: UpdateProductValidationError

Scenario: Update product with invalid name returns validation errors
	Given an existing product
	When I update the product with:
		| name | description         | amount | currency |
		|      | Updated description | 149.99 | USD      |
	Then the response status code should be 400
	And the response should match the validation problem details contract
