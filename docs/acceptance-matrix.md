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
