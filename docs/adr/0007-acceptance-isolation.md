# ADR-0007: Isolate acceptance scenarios on one SQL Server container

- Status: Accepted
- Date: 2026-09-24

## Context

Acceptance tests must exercise the real SQL Server implementation and history
without one scenario's writes influencing another. Starting a new engine for
every scenario would make the suite slower and more fragile.

## Decision

Start one SQL Server Testcontainer for the acceptance test run. For each
Reqnroll scenario, create a distinct database name, migrate it, and start a
separate application factory and HTTP client. Dispose the client and host and
drop the database after the scenario, including failure paths. A response
handler compares exercised HTTP outcomes to the generated OpenAPI contract.
The `.feature` files and step definitions are the test source; generated
Reqnroll C# files are not committed.

## Consequences

The expensive server is reused while current data and history remain isolated.
Failures in database cleanup must fail visibly. This suite establishes
scenario isolation, not behavior against a production database. SQL migrations
on a fresh test database do not prove CLEAN-01 safe on an existing database
with real data.

## Alternatives considered

- Share one test database: order-dependent state can hide regressions.
- Start a SQL Server container for each scenario: preserves isolation but
  adds substantial setup cost without improving the database boundary.
