Feature: Products acceptance scenario isolation
  Each example receives its own migrated test database with independent current and history records.

  Scenario Outline: A fresh scenario database isolates the marker phone and history (<run>)
    Each example verifies marker absence before creating one linked phone and inserted history; the check does not depend on example execution order.
    Given the isolated scenario starts with this marker state
      | Field       | Value                  |
      | Name        | REF05 isolation marker |
      | CurrentRows | 0                      |
      | HistoryRows | 0                      |
    And I have valid mobile phone details
      | Field | Value                  |
      | Name  | REF05 isolation marker |
    When I submit the create mobile phone request
    Then the mobile phone is created successfully
      | Field      | Value                  |
      | StatusCode | 201                    |
      | HasId      | true                   |
      | IsActive   | true                   |
      | Name       | REF05 isolation marker |
    And the isolated scenario contains this linked marker state
      | Field            | Value                  |
      | Name             | REF05 isolation marker |
      | CurrentRows      | 1                      |
      | HistoryRows      | 1                      |
      | IsActive         | true                   |
      | HistoryOperation | Inserted               |

    Examples:
      | run    | scenarioId             |
      | first  | REF05_Isolation_First  |
      | second | REF05_Isolation_Second |
