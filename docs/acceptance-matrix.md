# Acceptance matrix

[`acceptance-matrix.tsv`](acceptance-matrix.tsv) links each exposed, named HTTP operation
to observed status, cause and a Reqnroll scenario. `scenarioId` is a stable key for
the case; retain it when editing the scenario. Paths and operation IDs come from
the actual endpoint registration, including the existing `GetFiltered MobilePhones`
name. Health endpoints and framework-level 404/405/415 remain technical routes;
the framework contract is exercised in `Common/ApiProblemContract.feature`.

`python3 scripts/check-acceptance-matrix.py` runs locally and in CI. It compares
the matrix with all named endpoint registrations and declared response statuses,
checks unique IDs, and confirms the referenced feature/scenario and outline
example exist. For a new distinct response cause, add a scenario and matrix row.
The matrix is a navigation aid; the HTTP and database assertions are in the
linked acceptance tests. REF-08 adds conditional write and no-op cases.

BDD-01 checks complete endpoint registrations, including response metadata after
`.WithName(...)` and nested generic DTOs. A named operation with no parsed response
status fails the check. Matrix statuses must be declared, and every declared
status must have a matrix case. The exported OpenAPI check independently verifies
coverage in both directions; source parsing alone cannot certify the contract.
Regression cases exercise missing coverage and metadata placement through the
existing `scripts/test-openapi-contract.py` entry point.

BDD-07 validates actual request parameters/bodies and response bodies/headers for
all scenario clients against generated OpenAPI. Its `HTTP contract observation`
links to the canonical request/response pair by `exchangeId` and includes the
operation ID. Schema-invalid input is retained and must receive 400/415; its
response still satisfies the problem schema. Structurally valid input can fail a
domain rule. Framework routing/media problems use the shared problem schema;
health/startup probes use their explicit text contract and mark JSON schema
comparison as not applicable. See README for the emitted schema vocabulary,
serializer-required versus optional members and the existing offsetless SQL
timestamp wire form. These checks do not replace the status/cause matrix or
current/history assertions, and do not certify unexecuted input combinations.

MobilePhone uses its existing SQL `ChangedAt` column as an EF Core concurrency
token. Each changed write advances it monotonically; EF updates the phone and
history in one `SaveChanges` transaction. A stale write returns
`concurrency_conflict` (409) without an extra history row. Physical removal
after loading returns `resource_not_found` (404); an ID missing before loading
retains the existing validation 400 pending the separate 400/404 contract
decision. Repeating an identical PUT or soft DELETE returns 200 without new
history. This is a server-side concurrency check; the HTTP DTO has no client
version/precondition, so a later sequential request based on stale client state
is a separate contract decision. The existing column requires no SQL migration.

Physical removal after loading is covered for both PUT and DELETE. The DELETE
case asserts 404 / `resource_not_found`, the missing current row, and preservation
of the original history without an additional delete history entry.
