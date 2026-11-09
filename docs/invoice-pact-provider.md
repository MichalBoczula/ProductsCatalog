# PACT/10 Products provider verification

Products acceptance CI replays the exact Invoice and Web Pact v4 artifacts with
PactNet 5.0.1. The `invoice-source.json` and `web-source.json` pins in
`contracts/pact/` record each consumer's `master` commit, GitHub run and artifact
IDs, and archive and contract SHA-256 hashes. Keep the original JSON bytes when
updating a pin.

The verifier uses a loopback Kestrel listener and a migrated Testcontainers SQL
Server database for each Pact. Each test-only provider state inserts one active
priced phone and checks that the missing ID is absent. The real API and Dapper
read path serve `POST /mobile-phones/by-ids` for both consumers.

| Interaction | Required response |
| --- | --- |
| All requested IDs exist | HTTP 200 with the exact fixture ID, name, brand, amount and currency. Web also checks the photo URL, camera, display type and screen size. |
| One priced phone and one absent ID | HTTP 404 with a problem response for the whole lookup |

Catalog list, detail, top and filter operations remain open for Pact coverage.
Products OpenAPI and Reqnroll checks cover those API behaviors.

Run the focused check from the repository root with Docker available:

```bash
dotnet test tests/ProductCatalog.Acceptance.Tests --configuration Release --filter FullyQualifiedName~ProductsPactProviderTests
```

## Hosted evidence

The required acceptance suite uploads `invoice-products-verification` and
`web-products-verification`. The `publish-pact-verification` matrix runs after a
successful push or manual CI run on `master` when `PACT_REGISTRY_URL` is set. To
start a manual run, open **Actions → CI → Run workflow**, select `master`, and
click **Run workflow**. Each job checks the registry publication's source and
artifact bytes, registers the exact Products commit, and attaches the verified
result to its pinned consumer publication. The receipts are
`invoice-products-verification-publication` and `web-products-verification-publication`.

Configure these values before enabling the job:

| Location | Setting | Value |
| --- | --- | --- |
| Terraform `pact_publishers` | `products-ci` | `["products"]`, alongside the existing entries |
| Terraform `TF_VAR_pact_publisher_token_sha256` | `products-ci` | SHA-256 of a new, distinct raw Products token, alongside the existing hashes |
| ProductsCatalog Actions secret | `PACT_PUBLISHER_TOKEN` | Raw Products token |
| ProductsCatalog Actions variable | `PACT_REGISTRY_URL` | Existing Pact API HTTPS origin |

Apply the scoped Terraform configuration and verify Pact readiness before setting
the Actions variable. Then run CI on `master` and review both receipts for
`Passed`, two verified interactions each, and the Products commit SHA.
