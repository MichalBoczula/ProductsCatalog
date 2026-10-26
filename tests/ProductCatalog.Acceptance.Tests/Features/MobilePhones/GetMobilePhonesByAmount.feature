Feature: Get mobile phones by amount

@allure.label.scenarioId:REF07_GetMobilePhones_01
Scenario: Browsing the catalogue returns the requested number of phones in name order
  The amount limits the active catalogue list, ordered by name and then ID.
	Given no mobile phones exist in the database
	And the following phones are available in the catalogue
		| Name        | Brand | MainPhoto | PriceAmount | PriceCurrency |
		| Cedar Phone | Brand | cedar.jpg | 799.99      | USD           |
		| Aspen Phone | Brand | aspen.jpg | 599.99      | USD           |
		| Birch Phone | Brand | birch.jpg | 699.99      | USD           |
	When I request mobile phones with amount 2
	Then the catalogue page contains exactly these phones in order
		| StatusCode | Name        | MainPhoto | PriceAmount | PriceCurrency |
		| 200        | Aspen Phone | aspen.jpg | 599.99      | USD           |
		| 200        | Birch Phone | birch.jpg | 699.99      | USD           |

@allure.label.scenarioId:REF07_GetMobilePhones_02
Scenario: Browsing an empty catalogue returns an empty list
  An empty catalogue is a successful result even when more phones are requested.
	Given no mobile phones exist in the database
	When I request mobile phones with amount 3
	Then an empty mobile phone list is returned
		| Field      | Value |
		| StatusCode | 200   |
		| Amount     | 0     |

Scenario Outline: Browsing the catalogue rejects a nonpositive amount <amount>
  The requested amount must be greater than zero; rejection cannot change catalogue data or history.
	Given the catalogue and history are recorded before the rejected request
	When I request mobile phones with amount <amount>
	Then the mobile phone request fails with validation error
		| Field                  | Value                               |
		| StatusCode             | 400                                 |
		| Code                   | validation_failed                   |
		| ContentType            | application/problem+json            |
		| ErrorCount             | 1                                   |
		| MissingPropertiesCount | 0                                   |
		| ErrorMessage           | Amount must be greater than zero.   |
		| ErrorEntity            | Amount                              |
		| ErrorName              | AmountGreaterThanZeroValidationRule |
	And the rejected request leaves the catalogue and history unchanged
		| Field                | Value |
		| CurrentRowsChange    | 0     |
		| HistoryRowsChange    | 0     |
		| CurrentDataUnchanged | true  |
		| HistoryDataUnchanged | true  |

	Examples:
		| amount | scenarioId               |
		| 0      | REF07_GetMobilePhones_03 |
		| -1     | REF07_GetMobilePhones_03 |
