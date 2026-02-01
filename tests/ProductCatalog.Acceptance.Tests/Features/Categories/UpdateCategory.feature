Feature: Update category

  Scenario: Update category returns ok response
    Given an existing category which will be updated
    When I submit the request to category update category
	Then response return succesfully updated category

  Scenario: Update category fails for missing category
	Given Category does not exist in the database
	When I send a request to update the category
	Then response returns an error indicating category not found