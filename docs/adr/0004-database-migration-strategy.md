# ADR-0004: Database migration strategy

- Status: Accepted
- Date: 2026-09-22

## Context

ProductsCatalog can run with multiple API replicas. Applying EF Core migrations automatically from every starting replica can cause concurrent schema changes, startup failures, and permissions that are broader than the runtime service needs.

Local development still needs a convenient way to create and update the database.

## Decision

- Disable runtime migrations by default with `Database:ApplyMigrations=false`.
- The application reads `Database:ApplyMigrations`; `Database__ApplyMigrations=true`
  opts a process in. Docker Compose maps its local `APPLY_MIGRATIONS=true`
  variable to this setting. The application does not read `APPLY_MIGRATIONS`
  directly.
- For hosted environments, generate an idempotent EF Core migration script and execute it once in a dedicated deployment step before the API rollout.
- Give the deployment identity schema-change permissions.
- Give the runtime identity only the data permissions required by the service.
- Keep deterministic seed data in EF Core migrations where appropriate.

## Consequences

Schema changes are observable deployment events and do not race during horizontal scale-out. Runtime credentials can follow least privilege.

Deployments require migration orchestration and must define rollback or forward-fix handling for incompatible schema changes. Local developers must deliberately enable or execute migrations.

Startup with the setting enabled applies migrations once and fails if they
cannot complete; it does not retry the whole migration. A repeat startup on a
fresh isolated database has been tested. The CLEAN-01 migration against a
copy of an existing database with data still requires separate verification.

Implementation commands are documented in [the migration guide](../database-migrations.md).

## Alternatives considered

### Apply migrations on every API startup

This is convenient for a single local instance but unsafe for concurrent production replicas.

### Manual non-versioned SQL changes

Manual scripts without EF migration history make environments difficult to reproduce and audit.
