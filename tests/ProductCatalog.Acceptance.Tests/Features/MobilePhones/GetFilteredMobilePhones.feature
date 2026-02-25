Feature: Get filtered mobile phones

Scenario: Filter mobile phones by brand returns matching records
	Given existing mobile phones for filtering by brand
		| Name             | Brand    | PriceAmount |
		| Apple One        | Apple    | 999.99      |
		| Samsung One      | Samsung  | 899.99      |
		| Apple Two        | Apple    | 1099.99     |
	When I filter mobile phones by brand
		| Field | Value |
		| Brand | Apple |
	Then only mobile phones matching brand are returned
		| Field      | Value |
		| StatusCode | 200   |
		| Amount     | 2     |
		| Brand      | Apple |

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
