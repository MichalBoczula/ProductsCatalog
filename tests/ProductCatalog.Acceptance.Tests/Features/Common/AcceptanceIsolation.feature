Feature: Products acceptance scenario isolation
  Each scenario gets its own migrated database, including current and history records.

  Scenario Outline: A previous scenario cannot leave a phone or its history in this scenario (<run>)
    Given no Products phone or history has the name "REF05 isolation marker"
    And I have valid mobile phone details
      | Field | Value                  |
      | Name  | REF05 isolation marker |
    When I submit the create mobile phone request
    Then the mobile phone is created successfully
      | Field      | Value |
      | StatusCode | 201   |
    And this Products scenario has one phone and history named "REF05 isolation marker"

    Examples:
      | run    |
      | first  |
      | second |
