# ProductsCatalog backlog

Status legend:

- **Done** — implemented and verified;
- **Obsolete** — no longer matches the product direction;
- **Intentionally skipped** — evaluated and excluded by decision;
- **Remaining** — still requires work.

## Done

### Dependencies and CI

- Removed the obsolete direct `Microsoft.AspNetCore.Http.Abstractions 2.3.0` dependency.
- Kept `Microsoft.EntityFrameworkCore.Design` in Infrastructure because that project owns migrations; EF Tools is not a project dependency.
- Enabled `NuGetAudit` for restore.
- Enabled production-project warnings as errors; test and benchmark projects retain warnings for deliberate invalid/null test inputs.
- CI runs restore, Release build, format verification, Domain tests, Application tests, Infrastructure integration tests, and acceptance tests.
- Added independent 70% line-coverage gates for Domain and Application with separate reports.
- Enabled Dependency Review for pull requests and Automatic Dependency Submission for the dependency graph.
- Added Gitleaks secret scanning and Trivy image vulnerability scanning without `continue-on-error`.
- Made the image build depend on all tests, both coverage gates, and secret scanning. Docker Hub publication depends on the gated image job.

### OpenAPI

- Retained Swashbuckle as the single OpenAPI generator.
- Removed unused generated Angular and .NET clients and generator configuration.
- Added full OpenAPI specification validation in CI using a pinned Redocly CLI.
- Documented the decision not to store or generate API clients in this repository.

### MobilePhones query contracts

- `GET /mobile-phones/{id}` returns `200`, `404`, or `500`.
- `GET /mobile-phones?amount={amount}` returns `200`, `400`, or `500`; empty results return `200 []`.
- `POST /mobile-phones/by-ids` returns `200`, `400`, `404`, or `500`; an empty ID list returns `400`.
- `GET /mobile-phones/{id}/history` validates pagination and returns `200`, `400`, `404`, or `500`; an existing phone without history returns `200 []`.
- `GET /mobile-phones/top` returns `200` or `500`; empty results return `200 []`.
- `POST /mobile-phones/filter` uses `MobilePhoneFilterValidationPolicy` and returns `200`, `400`, or `500`; no matches return `200 []`.
- Added a separate acceptance scenario for every supported MobilePhones query response path, including controlled safe `500` responses.
- Added a focused test proving `ValidationException` maps to HTTP `400` with validation details and a trace identifier.

### Documentation

- Added the project README, architecture description, local and Docker instructions, migrations, test commands, endpoint overview, health checks, CI/CD description, and scaling rationale.
- Added ADRs for SQL Server, CQRS/MediatR, OpenAPI, migrations, and living documentation/RAG.
- Added this status-based backlog.

## Obsolete

- Further acceptance-test investment in Currency and Categories. Both modules are planned for removal.
- Returning `404` for empty query collections such as top, filter, amount, or history of an existing phone.
- Keeping generated API clients synchronized inside ProductsCatalog.

## Intentionally skipped

- Kiota and automatic API-client generation in this repository.
- Client-freshness checks while clients are neither stored nor generated here.
- Dependabot; Dependency Graph, Automatic Dependency Submission, NuGet audit, and Dependency Review remain in use.
- Treating expected test-project nullability warnings as build errors. Production projects remain strict.

## Remaining

### P1

- Verify whether the explicit `System.Security.Cryptography.Xml` package is required or only pins a transitive security version; remove it only with dependency evidence.
- Compare `actions/setup-dotnet` with the remaining portfolio repositories and align the major version if needed.
- Verify repository-level GitHub native secret-scanning settings; Gitleaks already gates CI.

### Product cleanup

- Remove Currency and Categories endpoints, application flows, validation, repositories, tests, and obsolete documentation when their replacement plan is approved.
- Decide whether MobilePhones command endpoints require dedicated safe-`500` acceptance scenarios in addition to the completed query matrix.

### Living documentation and RAG

- Finalize automatic Allure generation and publication behavior, artifact retention, and stable report URL.
- Define the RAG store, chunk schema, metadata, ingestion trigger, version replacement policy, and access controls.
- Generate versioned OpenAPI, flow, validation, and acceptance artifacts in one documentation pipeline.
- Ingest only approved artifacts and exclude secrets, production payloads, stack traces, and personal data.
- Complete task 6b by adding the deployed Allure URL and the final publication/RAG runbook to the README.
