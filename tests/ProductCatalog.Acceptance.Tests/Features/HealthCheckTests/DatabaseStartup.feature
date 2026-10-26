Feature: Database startup migration settings
  Startup evidence records the effective configuration and actual SQL state; connection strings are excluded.

  @allure.label.scenarioId:REF09_Startup_IdempotentSeed
  Scenario: Startup migration settings preserve the seed across repeated starts
    Migrations disabled leaves the test database absent. This repository configuration enables migrations by default; a repeated enabled start preserves every saved value.
    Given the scenario SQL database is absent before startup
    When the API starts with migrations disabled
    Then disabled startup has this observed state
      | Field                    | Value      |
      | EffectiveApplyMigrations | false      |
      | DatabaseExists           | false      |
      | LiveStatusCode           | 200        |
      | LiveContentType          | text/plain |
      | LiveBody                 | Healthy    |
    When the API starts with default migration settings
    Then the first migrated startup has this observed state
      | Field                    | Value      |
      | EffectiveApplyMigrations | true       |
      | DatabaseExists           | true       |
      | CurrentRows              | 15         |
      | HistoryRows              | 15         |
      | AppliedMigrationsAtLeast | 1          |
      | PendingMigrations        | 0          |
      | SeedLinksValid           | true       |
      | ReadyStatusCode          | 200        |
      | ReadyContentType         | text/plain |
      | ReadyBody                | Healthy    |
    When the API starts again with migrations enabled
    Then the repeated startup preserves this state
      | Field                      | Value      |
      | EffectiveApplyMigrations   | true       |
      | CurrentUnchanged           | true       |
      | HistoryUnchanged           | true       |
      | AppliedMigrationsUnchanged | true       |
      | PendingMigrations          | 0          |
      | ReadyStatusCode            | 200        |
      | ReadyContentType           | text/plain |
      | ReadyBody                  | Healthy    |
