Feature: Update currency

Scenario: Update currency returns ok response
	Given an existing currency which will be updated
	And I have updated currency details
    When I submit the request to update currency
	Then response return succesfully updated currency

Scenario: Update currency fails for missing category
	Given currency does not exist in the database
    And I have updated currency details
	When I send a request to update the currency
	Then response returns an error indicating currency not found