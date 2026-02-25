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
