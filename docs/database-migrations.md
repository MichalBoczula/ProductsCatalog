# Database migration strategy

Runtime migrations are disabled by default (`Database:ApplyMigrations=false`). This prevents multiple API replicas from trying to migrate the same database during a scale-out deployment.

For local development, set `APPLY_MIGRATIONS=true` in the local `.env` file when starting Docker Compose.

For hosted environments, generate an idempotent EF Core migration script and execute it once in a dedicated deployment step before rolling out the API:

```bash
dotnet ef migrations script --idempotent \
  --project src/ProductCatalog.Infrastructure \
  --startup-project src/ProductCatalog.Api \
  --output artifacts/migrations.sql
```

The deployment identity should have schema modification permissions. The runtime API identity should only receive the data permissions required by the service.

The catalogue seed lives in versioned EF migrations with fixed identifiers and dates.
On a fresh, isolated SQL database, a second opt-in API startup must leave the 15
current phones, 15 history entries and applied migration records unchanged. With
the default setting, startup does not create a missing database; readiness reports
unhealthy until a deployment step or an explicitly opted-in local startup migrates it.

Runtime migration application runs once during startup when enabled. Failure to
connect or apply a migration stops startup; there is no retry of the whole migration,
seed or transaction. After resolving the cause, rerun the deployment step or restart
an opt-in local instance. Do not enable runtime migrations simultaneously on multiple
hosted replicas. The fresh-database repeatability test does not verify the CLEAN-01
migration against an existing database with real data: back up and test that path
separately before applying it to a hosted database.
