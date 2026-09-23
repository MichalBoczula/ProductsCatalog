Feature: Opt-in database startup

  Scenario: Repeated local startup preserves seeded phones and history
    When the scenario SQL database is removed before startup
    And the API starts with default migration settings
    Then startup leaves the database absent and liveness healthy
    When the API starts with migrations enabled
    Then the database contains the initial seed and no pending migrations
    When the API starts again with migrations enabled
    Then the seeded phones, history and applied migrations are unchanged
