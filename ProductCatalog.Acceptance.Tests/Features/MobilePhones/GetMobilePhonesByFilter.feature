Feature: Get mobile phones by filter

Scenario: Get mobile phones by filter returns matching list
	Given an existing list of mobile phones
	When I request mobile phones with amount 2
	Then the mobile phone list is returned with the requested amount

Scenario: Get mobile phones by filter returns an empty list when no mobile phones exist
	Given no mobile phones exist in the database
	When I request mobile phones with amount 3
	Then an empty mobile phone list is returned
