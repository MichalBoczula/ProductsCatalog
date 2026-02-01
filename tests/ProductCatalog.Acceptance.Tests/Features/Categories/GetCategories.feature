Feature: Getd categories

  Scenario: Get categories returns list
    When I request the list of categories
    Then the category list is returned
