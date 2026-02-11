Feature: Get top mobile phones

Scenario: Get top mobile phones returns records when mobile phones exist
	Given an existing set of mobile phones for top list
	When I request the top mobile phones list
	Then the top mobile phones response is successful and contains records

Scenario: Get top mobile phones returns not found when no mobile phones exist
	Given mobile phones table is empty for top list
	When I request the top mobile phones list
	Then the top mobile phones response is not found
