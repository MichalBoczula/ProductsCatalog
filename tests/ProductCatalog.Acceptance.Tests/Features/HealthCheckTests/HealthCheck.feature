Feature: Health Check

  Scenario: Health endpoint returns OK
    When I request the health endpoint
    Then the response status code should be 200

  Scenario: Readiness recovers after the scenario SQL database becomes available again
    When I request the ready health endpoint
    Then the response status code should be 200
    When the scenario SQL database becomes unavailable
    And I request the ready health endpoint
    Then the response status code should be 503
    When I request the health endpoint
    Then the response status code should be 200
    When the scenario SQL database becomes available again
    And I request the ready health endpoint
    Then the response status code should be 200

  Scenario: Readiness fails when the history table is missing
    When the scenario SQL history table is removed
    And I request the ready health endpoint
    Then the response status code should be 503
    When I request the health endpoint
    Then the response status code should be 200
