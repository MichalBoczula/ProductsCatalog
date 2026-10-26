Feature: Health probes
  Liveness checks the running process. Readiness checks access to both required SQL tables.

  @allure.label.scenarioId:REF09_Health_Live
  Scenario: Liveness returns healthy text while the API is running
    Health probes return plain text rather than the business API JSON problem format.
    Given the scenario SQL database has the required catalogue tables
      | Field               | Value |
      | DatabaseAvailable   | true  |
      | CurrentTablePresent | true  |
      | HistoryTablePresent | true  |
    When I request the health probe "/health/live"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 200        |
      | ContentType | text/plain |
      | Body        | Healthy    |

  @allure.label.scenarioId:REF09_Health_Recovery
  Scenario: Readiness recovers after the deleted scenario database is recreated
    Deleting this test database makes readiness unhealthy while liveness remains healthy; migrations restore readiness.
    Given the scenario SQL database has the required catalogue tables
      | Field               | Value |
      | DatabaseAvailable   | true  |
      | CurrentTablePresent | true  |
      | HistoryTablePresent | true  |
    When I request the health probe "/health/ready"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 200        |
      | ContentType | text/plain |
      | Body        | Healthy    |
    When the scenario SQL database is deleted
    And I request the health probe "/health/ready"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 503        |
      | ContentType | text/plain |
      | Body        | Unhealthy  |
    When I request the health probe "/health/live"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 200        |
      | ContentType | text/plain |
      | Body        | Healthy    |
    When the scenario SQL database is recreated with migrations
    And I request the health probe "/health/ready"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 200        |
      | ContentType | text/plain |
      | Body        | Healthy    |

  @allure.label.scenarioId:REF09_Health_MissingHistoryTable
  Scenario: Readiness rejects a database without its history table
    SQL connectivity alone is insufficient: the history table is required. An unhealthy 503 probe is a documented text response.
    Given the scenario SQL database has the required catalogue tables
      | Field               | Value |
      | DatabaseAvailable   | true  |
      | CurrentTablePresent | true  |
      | HistoryTablePresent | true  |
    When the scenario SQL history table is removed
    And I request the health probe "/health/ready"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 503        |
      | ContentType | text/plain |
      | Body        | Unhealthy  |
    When I request the health probe "/health/live"
    Then the health probe returns this text response
      | Field       | Value      |
      | StatusCode  | 200        |
      | ContentType | text/plain |
      | Body        | Healthy    |
