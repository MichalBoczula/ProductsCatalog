# ProductsCatalog backlog

This repository is one of two reference services alongside ECommerceStoreUsers.
The status below reflects source and CI on `master` after REF-11 and ADR review
REF-12/3. See [the ADR index](adr/README.md) for accepted decisions,
[generated OpenAPI](../README.md#api-contract-and-executable-documentation)
for operations, and the [acceptance matrix](acceptance-matrix.md) for the
status/cause scenarios. This file does not duplicate those contracts.

## Implemented and checked in CI

- **REF-01–03, CI and dependencies:** build, format, four test suites, separate
  Domain/Application 70% line-coverage gates, Infrastructure coverage report,
  NuGet high/critical audit, Dependency Review on PRs, Gitleaks and Trivy. The
  image job follows the quality gate. On a `master` push only, Docker Hub SHA
  and `latest` tags are pushed from the scanned local image; matching image IDs
  and registry digests are checked. Ordinary compiler warnings remain visible
  without a global warnings-as-errors gate.
- **CLEAN-01 code:** Categories/Currencies operations and their dependencies
  were removed; `Price.Currency` remains. The schema change exists in EF
  migrations. Existing populated-database migration verification is separate
  below.
- **REF-04–08, HTTP and data:** safe problem responses and acceptance
  status/cause matrix cover query and write paths, including controlled write
  failures. MobilePhone `ChangedAt` protects concurrent writes; changed state
  and history save together, and update/delete no-ops avoid new history.
  Read ordering and cancellation are explicit.
- **REF-09, local readiness:** configuration fails fast; `/health/live` is
  independent of SQL and `/health/ready` checks it. Dapper retries selected
  connection opens within bounds, not failed queries or writes. Opt-in
  migrations and deterministic seed were repeated on an isolated fresh SQL
  database.
- **REF-10–11, contracts and architecture:** flow steps and validation
  policies come from executed code; operation-to-flow-to-policy-to-scenario
  links are generated and checked. CI exports/lints OpenAPI, compares actual
  acceptance HTTP responses with declared statuses/media/schema, and gates
  forbidden layer and persistence dependencies with negative cases. The
  runtime comparison proves the executed scenarios, not every possible input.
- **REF-12/3, decisions:** existing ADRs were reconciled with implementation;
  the index includes errors, acceptance isolation, concurrency and the scanned
  image publication boundary.

## Remaining before a hosted release

- **CLEAN-01 data migration:** back up and test
  `20260922220000_RemoveCatalogs` on a copy of an existing database with real
  Categories/Currencies and `CategoryId` data. Verify upgrade, failure recovery
  and the agreed rollout; a fresh-database test cannot prove this path.
- **Deployment-specific security and operations:** define authorization,
  credentials/identity, backup and restore, migration runner, release and
  rollback for the actual environment. Current local and CI checks do not
  certify a public production deployment.
- **Repository settings:** confirm required checks/rulesets and GitHub native
  secret-scanning settings with repository administration access. Gitleaks is
  already a CI gate; absence of settings has not been established.
- **Dependency evidence:** examine why Infrastructure pins
  `System.Security.Cryptography.Xml` before removing or changing it.

## Deferred portfolio work

- Host Allure/API reports and decide artifact retention, version identity and
  report URL. CI currently generates/validates contract sources and stores
  test results, without a hosted documentation portal.
- Design and evaluate RAG ingestion, chunking, access controls and refresh
  from approved artifacts. Exclude secrets, production payloads and personal
  data. No vector store or retrieval endpoint exists in this service.
- Consumer-owned Kiota/client compatibility, Entra integration and BFF token
  flow belong to their consuming services and the later security phase.
- Assess pagination/request limits, versioning, browser CORS, performance
  budgets and observability against real clients and deployment needs; do not
  introduce these by default solely to match the other reference service.
- Central reusable CI workflows and cloud deployment follow validation of
  both reference repositories. `latest` is mutable; deployed consumers should
  pin a SHA or digest according to their release policy.

## Decisions retired

- No generated API clients or handwritten per-operation OpenAPI in this
  producer repository.
- No Categories/Currencies endpoints or `CategoryId` contract.
- Empty collection queries return successful empty collections rather than
  invented not-found responses.
- No general warnings-as-errors gate or forced CQRS/MongoDB symmetry with Users.
