Feature: Get top mobile phones

@allure.label.scenarioId:REF07_GetTopMobilePhones_01
Scenario: The top list contains the three most recently changed active phones
  Top means most recently changed, not sales or popularity. Older entries fall outside the limit.
	Given the following phones are available for the top list
		| Name         | MainPhoto  | PriceAmount | PriceCurrency | ChangedAt            |
		| Oldest Phone | oldest.jpg | 399.99      | USD           | 2026-01-01T10:00:00Z |
		| Newest Phone | newest.jpg | 999.99      | EUR           | 2026-01-04T10:00:00Z |
		| Older Phone  | older.jpg  | 599.99      | USD           | 2026-01-02T10:00:00Z |
		| Recent Phone | recent.jpg | 799.99      | USD           | 2026-01-03T10:00:00Z |
	When I request the top mobile phones list
	Then the top list contains exactly these phones in order
		| StatusCode | Name         | MainPhoto  | PriceAmount | PriceCurrency |
		| 200        | Newest Phone | newest.jpg | 999.99      | EUR           |
		| 200        | Recent Phone | recent.jpg | 799.99      | USD           |
		| 200        | Older Phone  | older.jpg  | 599.99      | USD           |

@allure.label.scenarioId:REF07_GetTopMobilePhones_02
Scenario: Top phones returns an empty list for an empty catalogue
  There are no active catalogue entries to include in the top list.
	Given mobile phones table is empty for top list
	When I request the top mobile phones list
	Then the top mobile phones response is successful and empty
		| Field      | Value |
		| StatusCode | 200   |
		| Amount     | 0     |

@allure.label.scenarioId:REF08_GetTopMobilePhones_200_StableTie
Scenario: Top phones breaks equal change times by the three greatest IDs
  Four active entries share ChangedAt; ID descending makes the three-entry boundary stable.
	Given four mobile phones with the same change time for top list
		| Field | Value     |
		| Name  | Tie Phone |
	When I request the top mobile phones list
	Then the top list contains the three greatest IDs in stable order
		| Field      | Value |
		| StatusCode | 200   |
		| Amount     | 3     |

@allure.label.scenarioId:REF08_GetTopMobilePhones_200_Inactive
Scenario: Top phones excludes inactive entries while an ID read still returns them
  Soft deletion removes an entry from top without removing its readable catalogue record.
	Given four mobile phones with the same change time for top list
		| Field | Value          |
		| Name  | Inactive Phone |
	And one created phone is inactive
	When I request the top mobile phones list
	Then the inactive phone is omitted from top but readable by ID
		| Field              | Value |
		| StatusCode         | 200   |
		| Amount             | 3     |
		| ReadByIdStatusCode | 200   |
		| ReadByIdIsActive   | false |
