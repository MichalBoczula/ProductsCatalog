Feature: Get category by id

  Scenario: Get category by id returns category
    Given an existing category id
      | Field | Value          |
      | Code  | BOOKS          |
      | Name  | Books category |
    When I request the category by id
    Then the category details are returned successfully
      | Field      | Value          |
      | StatusCode | 200            |
      | IsActive   | true           |
      | Code       | BOOKS          |
      | Name       | Books category |

  Scenario: Get category by id fails for missing category
    Given a category without specific id doesnt exists
    When I send request for category by not existed id
    Then response show not found error
      | Field      | Value                                                                                                            |
      | StatusCode | 404                                                                                                              |
      | Title      | Resource not found.                                                                                               |
      | Detail     | Resource CategoryDto identify by id {CategoryId} cannot be found in databese during action GetCategoryByIdQuery. |
      | Instance   | /categories/{CategoryId}                                                                                          |
