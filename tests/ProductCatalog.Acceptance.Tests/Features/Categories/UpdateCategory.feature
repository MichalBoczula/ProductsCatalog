Feature: Update category

  Scenario: Update category returns ok response
    Given an existing category which will be updated
      | Field | Value           |
      | Code  | UPDATE-CATEGORY |
      | Name  | Update Category |
    And I have updated category details
      | Field | Value            |
      | Code  | UPDATED-CATEGORY |
      | Name  | Updated Category |
    When I submit the request to category update category
	Then response return succesfully updated category
      | Field      | Value            |
      | StatusCode | 200              |
      | HasId      | true             |
      | IsActive   | true             |
      | Code       | UPDATED-CATEGORY |
      | Name       | Updated Category |

  Scenario: Update category fails for missing category
	Given Category does not exist in the database
    And I have updated category details
      | Field | Value    |
      | Code  | MISSING  |
      | Name  | Missing category |
	When I send a request to update the category
	Then response returns an error indicating category not found
      | Field        | Value                                   |
      | Status       | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Category cannot be null.                |
      | ErrorEntity  | Category                                |
      | ErrorName    | CategoryIsNullValidationRule            |
