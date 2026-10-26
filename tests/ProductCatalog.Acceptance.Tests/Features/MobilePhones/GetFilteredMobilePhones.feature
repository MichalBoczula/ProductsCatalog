Feature: Get filtered mobile phones

@allure.label.scenarioId:REF07_GetFiltered_MobilePhones_01
Scenario: Filtering by brand returns exactly the matching catalogue entries
  Phones of other brands are excluded from the result.
	Given existing mobile phones for filtering by brand
		| Name        | Brand   | PriceAmount |
		| Apple One   | Apple   | 999.99      |
		| Samsung One | Samsung | 899.99      |
		| Xiaomi One  | Xiaomi  | 1099.99     |
	When I filter mobile phones by brand
		| Field | Value  |
		| Brand | Xiaomi |
	Then only mobile phones matching brand are returned
		| Field         | Value          |
		| StatusCode    | 200            |
		| Amount        | 1              |
		| Name          | Xiaomi One     |
		| Brand         | Xiaomi         |
		| MainPhoto     | main-photo.jpg |
		| PriceAmount   | 1099.99        |
		| PriceCurrency | USD            |

@allure.label.scenarioId:REF07_GetFiltered_MobilePhones_02
Scenario: Filter mobile phones fails when brand is outside enum values
	Given existing mobile phones for filtering by brand
		| Name             | Brand    | PriceAmount |
		| Apple One        | Apple    | 999.99      |
		| Samsung One      | Samsung  | 899.99      |
	When I filter mobile phones with invalid brand
		| Field | Value |
		| Brand | 999   |
	Then filtering mobile phones fails with API error
		| Field        | Value                                          |
		| StatusCode   | 400                                            |
		| Title        | Validation failed                              |
		| Detail       | One or more validation errors occurred.        |
		| ErrorMessage | Brand must exist in MobilePhonesBrand enum.    |
		| ErrorEntity  | MobilePhoneFilterDto                           |
		| ErrorName    | MobilePhoneFilterBrandValidationRule           |

@allure.label.scenarioId:REF07_GetFiltered_MobilePhones_03
Scenario: Filtering with no matching phones returns an empty list
  A valid brand and price filter with no matches is a successful result.
	Given existing mobile phones for filtering by brand
		| Name        | Brand   | PriceAmount |
		| Apple One   | Apple   | 999.99      |
		| Samsung One | Samsung | 899.99      |
	When I filter mobile phones by brand
		| Field        | Value    |
		| Brand        | Motorola |
		| MinimalPrice | 1        |
		| MaximalPrice | 2        |
	Then the filtered mobile phone list is empty
		| Field      | Value |
		| StatusCode | 200   |
		| Amount     | 0     |
