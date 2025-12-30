Feature: Health Check

  Scenario: Health endpoint returns OK
    When I request the health endpoint
    Then the response status code should be 200