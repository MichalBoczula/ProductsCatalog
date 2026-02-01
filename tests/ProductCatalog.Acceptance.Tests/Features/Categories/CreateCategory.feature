Feature: Create category

  Scenario: Create category returns created response
    Given I have valid category details
      | Field | Value     |
      | Code  | HOME      |
      | Name  | Home goods |
    When I submit the create category request
    Then the category is created successfully
      | Field      | Value     |
      | StatusCode | 201       |
      | HasId      | true      |
      | IsActive   | true      |
      | Code       | HOME      |
      | Name       | Home goods |

  Scenario: Create category fails with invalid details
    Given I have invalid category details
      | Field | Value |
      | Code  |       |
      | Name  |       |
    When I submit the create invalid category request
    Then the category creation fails with API error
      | Field         | Value                                      |
      | StatusCode    | 400                                        |
      | Title         | Validation failed                          |
      | Detail        | One or more validation errors occurred.    |
      | ErrorMessage1 | Code cannot be null or whitespace.         |
      | ErrorEntity1  | Category                                   |
      | ErrorName1    | CategoriesCodeValidationRule               |
      | ErrorMessage2 | Name cannot be null or whitespace.         |
      | ErrorEntity2  | Category                                   |
      | ErrorName2    | CategoriesNameValidationRule               |
