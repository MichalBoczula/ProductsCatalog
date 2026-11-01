# Database migration strategy

The checked-in `appsettings.json` and local Docker Compose enable runtime
migrations (`Database:ApplyMigrations=true`). Compose maps `APPLY_MIGRATIONS`
from `.env` to `Database__ApplyMigrations`, defaulting to `true` when unset or
empty. The example `.env` also enables migrations. An existing `.env` value
still overrides that default; set it to `true` to migrate on local startup or
`false` when applying migrations separately.

For hosted environments, explicitly set `Database__ApplyMigrations=false` to
prevent multiple API replicas from migrating the same database during scale-out.

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
current phones, 26 history entries (15 original inserts and 11 price corrections)
and applied migration records unchanged. With
the setting disabled, startup does not create a missing database; readiness reports
unhealthy until a deployment step or an explicitly opted-in local startup migrates it.

Runtime migration application runs once during startup when enabled. Failure to
connect or apply a migration stops startup; there is no retry of the whole migration,
seed or transaction. After resolving the cause, rerun the deployment step or restart
an opt-in local instance. Do not enable runtime migrations simultaneously on multiple
hosted replicas. The fresh-database repeatability test does not verify the CLEAN-01
migration against an existing database with real data: back up and test that path
separately before applying it to a hosted database.

## Development phone photo seeds

The existing `SeedMobilePhone1` through `SeedMobilePhone15` migrations contain
the canonical URLs from the verified Products photo manifest. Both the current
phone and its original history row use `Main.webp` as `MainPhoto`, with
`OtherPhotos` ordered **Front, Back, Side**. The URLs target
`stecproductsdevmb261013/photos`; spaces and Polish characters are percent encoded.

These seed files were edited for the disposable development database. Recreate
that database and rerun the complete migration chain to receive the updated seed
values. Running migrations against an already-seeded database will not rerun
these migration IDs. There is no additional corrective migration in this change.

The container remains private. Browser image delivery and Angular photo bindings
are separate application steps; a canonical Blob URL alone does not grant access.

## Mobile phone price correction (STORE-01)

`20261101180000_BackfillMobilePhonePrices` executes the separately versioned
`Migrations/Scripts/20261101180000_BackfillMobilePhonePrices.sql`. The SQL is an
embedded Infrastructure assembly resource, so runtime migration, published
containers, migration bundles and generated deployment scripts use the same
bytes without depending on a working directory or a loose file. The historical
seed migrations are unchanged; existing databases receive this new migration.

The following are fixed demo catalogue prices, maintained with the seed rather
than fetched from a retailer during deployment:

| Seed phone | Price (PLN) |
| --- | ---: |
| Xiaomi POCO F7 12/512GB | 2,499.00 |
| Xiaomi Redmi 15 5G 4/128GB | 999.00 |
| Xiaomi Redmi Note 15 Pro 8/256GB | 1,999.00 |
| Xiaomi 15T 5G 12/512 | 2,999.00 |
| Apple iPhone 15 128GB | 2,799.00 |
| Apple iPhone 16 128GB | 3,299.00 |
| Apple iPhone 17 256GB | 3,999.00 |
| Apple iPhone 17 Pro 256GB | 5,499.00 |
| Samsung Galaxy S24 8GB/128GB | 2,499.00 |
| Samsung Galaxy S25 12/256GB | 3,499.00 |
| Samsung Galaxy A36 5G 8/256GB | 1,199.00 |
| Samsung Galaxy A56 5G 8/128GB | 1,599.00 |
| Samsung Galaxy A16 4/128GB | 599.00 |
| Motorola Signature 5G 16/512GB | 3,999.00 |
| Motorola moto g86 power 5G 12/256GB | 1,299.00 |

The script matches only these 15 fixed seed IDs and corrects only rows with
`PriceAmount = 0` and `PriceCurrency = 'PLN'`. All positive prices, other
currencies and unrelated IDs remain untouched. Missing seeds are not recreated.
The four Xiaomi prices already present in the current seeds are preserved, but
their IDs remain in the map to repair older zero-valued databases too.

Each changed phone receives a strictly newer `ChangedAt` concurrency token and
one complete `Operation.Updated` history snapshot. SQL `UPDATE ... OUTPUT INTO`
writes the current state and history atomically inside the EF migration
transaction. Existing history is never rewritten, so an original zero-priced
`Inserted` entry remains an accurate record of the old seed. On a fresh database
this adds 11 entries to the original 15. The guarded update is also a no-op when
the SQL is rerun directly; repeated EF migration/deployment script execution
preserves timestamps, history and migration records.

For an existing hosted database, back it up, inspect the zero-priced seed IDs
and test the generated idempotent script on a copy before applying it once in
the deployment step described above. The data-only correction changes no API
contract or schema. A previous API version can still read the corrected data.
Automatic `Down` is deliberately unsupported: resetting prices or deleting
history could overwrite subsequent business changes. Restore reviewed data
from the backup or apply a forward corrective migration if a data reversal is
required. A failed application rolls back its data and migration marker; after
fixing the cause, retry the same migration.
