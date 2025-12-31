Feature: GetCategoryById

  Scenario: Get category by id returns category
    Given an existing category id
    When I request the category by id
    Then the category details are returned successfully

  Scenario: Get category by id fails for missing category
    Given a category without specific id doesnt exists
    When I send request for category by not existed id
	Then response show not found error