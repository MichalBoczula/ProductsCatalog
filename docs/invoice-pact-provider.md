# PACT/10 Invoice → Products provider verification

Products acceptance CI replays the exact Invoice Pact v4 artifact pinned in
`contracts/pact/invoice-source.json` with PactNet 5.0.1. The pin records the Invoice
`master` commit, GitHub run and artifact IDs, and archive and contract SHA-256
hashes. Keep the original JSON bytes when updating the pin.

The verifier uses a loopback Kestrel listener and a migrated Testcontainers SQL
Server database. Test-only provider states insert one active priced phone and
check that the missing ID is absent. The real API and Dapper read path serve
`POST /mobile-phones/by-ids` for both interactions.

| Interaction | Required response |
| --- | --- |
| One priced phone and one absent ID | HTTP 200 and one phone with the expected ID, name, brand, amount and currency |
| Only an absent ID | HTTP 404 with a problem response |

Other catalog operations and fields such as photos and specifications are
outside this Pact. Products OpenAPI and Reqnroll checks cover those API behaviors.

Run the focused check from the repository root with Docker available:

```bash
dotnet test tests/ProductCatalog.Acceptance.Tests --configuration Release --filter FullyQualifiedName~InvoicePactProviderTests
```

## Hosted evidence

The required acceptance suite uploads `invoice-products-verification` on success.
The `publish-invoice-pact-verification` job runs on a successful `master` push when
`PACT_REGISTRY_URL` is set. It verifies the publication's source and the registry's
artifact bytes, registers the exact Products commit, and attaches the verified
result to the pinned Invoice publication. The receipt is uploaded as
`invoice-products-verification-publication`.

Configure these values before enabling the job:

| Location | Setting | Value |
| --- | --- | --- |
| Terraform `pact_publishers` | `products-ci` | `["products"]`, alongside the existing entries |
| Terraform `TF_VAR_pact_publisher_token_sha256` | `products-ci` | SHA-256 of a new, distinct raw Products token, alongside the existing hashes |
| ProductsCatalog Actions secret | `PACT_PUBLISHER_TOKEN` | Raw Products token |
| ProductsCatalog Actions variable | `PACT_REGISTRY_URL` | Existing Pact API HTTPS origin |

Apply the scoped Terraform configuration and verify Pact readiness before setting
the Actions variable. Then run CI on `master` and review the receipt for
`Passed`, two verified interactions, and the Products commit SHA.
