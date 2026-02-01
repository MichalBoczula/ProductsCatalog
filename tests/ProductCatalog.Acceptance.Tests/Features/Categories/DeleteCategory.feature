Feature: Delete category

  Scenario: Delete category returns ok response
    Given an existing category to delete
      | Field | Value           |
      | Code  | DELETE-CATEGORY |
      | Name  | Delete Category |
    When I submit the delete category request
    Then the category is deleted successfully
      | Field      | Value           |
      | StatusCode | 200             |
      | HasId      | true            |
      | IsActive   | false           |
      | Code       | DELETE-CATEGORY |
      | Name       | Delete Category |

  Scenario: Delete category fails for missing category
    Given a category id that does not exist
    When I submit the delete category request for non existing category
    Then the category deletion fails with API error
      | Field        | Value                                   |
      | Status       | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Category cannot be null.                |
      | ErrorEntity  | Category                                |
      | ErrorName    | CategoryIsNullValidationRule            |
