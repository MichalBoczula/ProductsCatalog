Feature: Update currency

  Scenario: Update currency returns ok response
    Given an existing currency which will be updated
      | Field       | Value           |
      | Code        | UPD             |
      | Description | Currency        |
    And I have updated currency details
      | Field       | Value            |
      | Code        | UPD              |
      | Description | Updated Currency |
    When I submit the request to update currency
    Then response return succesfully updated currency
      | Field       | Value            |
      | StatusCode  | 200              |
      | HasId       | true             |
      | IsActive    | true             |
      | Code        | UPD              |
      | Description | Updated Currency |

  Scenario: Update currency fails for missing currency
    Given currency does not exist in the database
    And I have updated currency details
      | Field       | Value            |
      | Code        | MISSING          |
      | Description | Missing Currency |
    When I send a request to update the currency
    Then response returns an error indicating currency not found
      | Field        | Value                                   |
      | Status       | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Currency cannot be null.                |
      | ErrorEntity  | Currency                                |
      | ErrorName    | CurrencyIsNullValidationRule            |
