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

The model seed uses EF Core `HasData` and fixed identifiers. Changes are represented by migrations, so applying the same migration set repeatedly is deterministic and idempotent.
