Feature: UpdateCategory

  Scenario: Update category returns ok response
    Given an existing category to update
    When I submit the update category request
    Then the category is updated successfully

  Scenario: Update category fails for missing category
    Given a category id that does not exist
    When I submit the update category request for non existing category
    Then the category update fails with API error
