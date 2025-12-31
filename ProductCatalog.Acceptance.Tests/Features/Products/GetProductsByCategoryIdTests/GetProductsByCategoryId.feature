Feature: Get products by category
  In order to view all products for a category
  As a client of the catalog API
  I want to retrieve the products filtered by category id

Scenario: Get products by category returns matching products
	Given an existing list of products with code "MobileCategoryId"
	When I request products for the category "MobileCategoryId"
	Then  only products from the category are returned

Scenario: Get products by category returns an empty list when no products exist
	Given an list of products with code "MobileCategoryId"
	When I request tablets for the category "TabletCategoryId"
	Then an empty product list is returned
