Feature: DeleteCurrency

  Scenario: Delete currency returns ok response
    Given an existing currency to delete
    When I submit the delete currency request
    Then the currency is deleted successfully

  Scenario: Delete currency fails for missing currency
    Given a currency id that does not exist
    When I submit the delete currency request for non existing currency
    Then the currency deletion fails with API error