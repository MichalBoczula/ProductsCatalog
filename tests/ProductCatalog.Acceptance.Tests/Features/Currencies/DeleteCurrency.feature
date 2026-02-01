Feature: Delete currency

  Scenario: Delete currency returns ok response
    Given an existing currency to delete
      | Field       | Value           |
      | Code        | DEL             |
      | Description | Delete Currency |
    When I submit the delete currency request
    Then the currency is deleted successfully
      | Field       | Value           |
      | StatusCode  | 200             |
      | HasId       | true            |
      | IsActive    | false           |
      | Code        | DEL             |
      | Description | Delete Currency |

  Scenario: Delete currency fails for missing currency
    Given a currency id that does not exist
    When I submit the delete currency request for non existing currency
    Then the currency deletion fails with API error
      | Field        | Value                                   |
      | Status       | 400                                     |
      | Title        | Validation failed                       |
      | Detail       | One or more validation errors occurred. |
      | ErrorMessage | Currency cannot be null.                |
      | ErrorEntity  | Currency                                |
      | ErrorName    | CurrencyIsNullValidationRule            |
