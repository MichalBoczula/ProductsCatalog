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

## BDD and documentation packages

Improve Products first in small, separately verified PRs before reusing the
pattern in the other producers. Generated sources remain authoritative; these
packages do not introduce parallel handwritten contracts or new Azure resources.
BDD-01, BDD-02 and BDD-03 are implemented and verified by PRs #200, #201 and
#202. BDD-03 passed 84 acceptance tests; its 64 BDD cases included 116 paired
HTTP exchanges, and all 15 revised descriptions and 38 tables matched the source.
BDD-04 makes 23 rejection cases explicit and checks the problem contract and
unchanged saved state; full CI and raw report checks are required for completion.
BDD-05 through BDD-09 remain planned. Green checks for one package do not certify
the later packages. Deployment and LiveDocs snapshot promotion are deferred until
all packages are complete.

| Package | Scope | Completion evidence |
| --- | --- | --- |
| BDD-01 | Response coverage | Complete endpoint registrations and exported OpenAPI reject uncovered statuses; DELETE after physical removal returns 404 without extra history. |
| BDD-02 | Shared HTTP attachments | All scenario HTTP clients capture request method/path/query/body and actual response status/body consistently, with stable scenario identity and correct handling of text and malformed JSON. |
| BDD-03 | Successful catalogue scenarios | Read/create/update/delete cases use readable descriptions, Given/When/Then and meaningful input/expected-result tables or examples, with complete evidence. |
| BDD-04 | Validation and framework errors | Invalid fields/pagination, empty IDs, malformed JSON, binding, media and routing failures explain their cause and include the exact request and response. |
| BDD-05 | Concurrency and persistence failures | Reports expose competing actions, no-ops, physical removal and failed saves, with current/history assertions and HTTP evidence. |
| BDD-06 | Technical scenarios | Health, readiness, startup and documentation cases include appropriate actual HTTP/text or startup evidence and explicit technical exceptions. |
| BDD-07 | Request/response contracts | Schema validation covers applicable types, required fields, nullability, enums and formats; generated contracts describe field semantics, examples and relevant response headers. |
| BDD-08 | Flows, rules and RAG links | Source descriptions explain decisions, side effects and failures; distinguish enforced rules from standalone policies; link operation/rule/branch/scenario/evidence identities and reconcile stale docs. |
| BDD-09 | Final structure and completeness audit | After BDD-02 through BDD-08, verify every current Products scenario and the selected LiveDocs snapshot against the checklist below; record actual CI/run/commit evidence and any remaining gap. |

### BDD-09 final acceptance checklist

- [ ] Account for every source scenario and outline example in the real result
  bundle: nonzero executed cases, no unintended omissions, skips or failures.
  Derive counts from that source revision rather than freezing today's total.
- [ ] Review readable scenario intent, Given/When/Then, relevant preconditions
  and meaningful input/expected-output tables or examples as rendered in Allure.
  Keep stable IDs as metadata; document justified technical exceptions.
- [ ] Check all scenario HTTP exchanges, including setup and alternative fault
  clients, for method/path/query/body, actual status/media/body and their links.
  Preserve malformed request text; retain text/empty-body and startup evidence
  faithfully instead of inventing a JSON payload.
- [ ] Resolve every attachment, verify its bytes and media type, and parse each
  attachment advertised as JSON. Reject missing, stale or placeholder evidence.
- [ ] Compare endpoint registrations, matrix and exported OpenAPI in both
  directions. Verify every declared status and distinct documented cause has a
  real case, and check request/response schemas with explicit technical exceptions.
- [ ] Trace operations to described branches, enforced rules, stable scenario
  IDs and actual result/attachment identities. Identify unused published policies
  explicitly; documentation must not imply that unused rules are enforced.
- [ ] Check field semantics/examples, flow conditions, side effects, safe errors,
  current/history outcomes and relevant headers against executable sources.
- [ ] Reconcile README, source-policy, error-contract and backlog claims with the
  reviewed revision. Record design-first approval evidence or its remaining gap;
  do not infer approval chronology solely from generated OpenAPI.
- [ ] Require the existing full CI gates and validate the newly selected archive,
  generated reports, provenance and report links in LiveDocs. Record the producer
  run, source SHA, bundle digest and LiveDocs build identity used for sign-off.

BDD-09 is a final audit task, not an implemented assertion that the existing
reports already meet every criterion. Its scope is Products; applying the same
pattern to Users, Orders/Invoices and Payments remains separate work.

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
