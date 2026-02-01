Feature: Create category

  Scenario: Create category returns created response
    Given I have valid category details
    When I submit the create category request
    Then the category is created successfully

  Scenario: Create category fails with invalid details
    Given I have invalid category details
    When I submit the create invalid category request
    Then the category creation fails with API error