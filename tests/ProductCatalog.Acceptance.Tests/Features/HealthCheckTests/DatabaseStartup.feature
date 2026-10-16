Feature: Database startup migration settings

  Scenario: Repeated local startup preserves seeded phones and history
    When the scenario SQL database is removed before startup
    And the API starts with migrations disabled
    Then startup leaves the database absent and liveness healthy
    When the API starts with default migration settings
    Then the database contains the initial seed and no pending migrations
    When the API starts again with migrations enabled
    Then the seeded phones, history and applied migrations are unchanged
