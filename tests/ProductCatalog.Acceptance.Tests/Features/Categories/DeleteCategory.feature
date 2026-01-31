Feature: DeleteCategory

  Scenario: Delete category returns ok response
    Given an existing category to delete
    When I submit the delete category request
    Then the category is deleted successfully

  Scenario: Delete category fails for missing category
	Given existing list of categories
    When I submit the delete category request for non existing category
    Then the category deletion fails with API error