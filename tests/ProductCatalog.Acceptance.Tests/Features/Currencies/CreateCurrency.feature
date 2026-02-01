Feature: Create currency

  Scenario: Create currency returns created response
    Given I have valid currency details
      | Field       | Value        |
      | Code        | JPY          |
      | Description | Japanese Yen |
    When I submit the create currency request
    Then the currency is created successfully
      | Field       | Value        |
      | StatusCode  | 201          |
      | HasId       | true         |
      | IsActive    | true         |
      | Code        | JPY          |
      | Description | Japanese Yen |

  Scenario: Create currency fails with invalid details
    Given I have invalid currency details
      | Field       | Value |
      | Code        |       |
      | Description |       |
    When I submit the create invalid currency request
    Then the currency creation fails with API error
      | Field         | Value                                      |
      | StatusCode    | 400                                        |
      | Title         | Validation failed                          |
      | Detail        | One or more validation errors occurred.    |
      | ErrorMessage1 | Code cannot be null or whitespace.         |
      | ErrorEntity1  | Currency                                   |
      | ErrorName1    | CurrenciesCodeValidationRule               |
      | ErrorMessage2 | Description cannot be null or whitespace.  |
      | ErrorEntity2  | Currency                                   |
      | ErrorName2    | CurrenciesDescriptionValidationRule        |
