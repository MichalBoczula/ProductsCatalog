# Agent instructions

Read this file and [the definition of done](docs/definition-of-done.md) before editing. This repository and ECommerceStoreUsers are equal reference implementations. Borrow a useful practice from either repository when it fits this service; do not change its architecture just to make the two look alike.

## Scope and architecture

- Preserve .NET 10 Minimal API, CQRS/MediatR, EF Core for writes, Dapper for reads, and SQL Server.
- `src/ProductCatalog.Api` owns routes, HTTP response metadata, exception mapping, OpenAPI, and composition. `src/ProductCatalog.Application` owns use cases and flow descriptors. `src/ProductCatalog.Domain` owns aggregates and validation policies. `src/ProductCatalog.Infrastructure` owns persistence, migrations, and SQL readiness.
- Follow existing naming, dependency direction, and test conventions before adding a new abstraction. Explain any package, schema, public contract, or architecture change in the PR.
- Work on the assigned backlog item. Do not change secrets, repository rulesets, security gates, or unrelated services as incidental cleanup.

## Contracts and tests

- For an endpoint change, review request and response DTOs, validation, error status and content type, `.Produces` metadata, generated Swashbuckle OpenAPI, flow descriptions, validation-policy descriptions, and relevant acceptance scenarios together. Do not maintain a second handwritten OpenAPI specification or generated clients in this producer repository.
- Put pure rules in Domain tests, use-case behavior in Application tests, real SQL behavior in Infrastructure tests, and externally observable behavior in Reqnroll acceptance tests. Use Testcontainers when the behavior requires SQL Server. Check effects on both current data and history for writes.
- Do not edit generated `.feature.cs` by hand. Change `.feature` and step definitions; use the project's generation process. Do not skip failing tests, hide failures with `continue-on-error`, or lower the existing 70% Domain/Application coverage thresholds to make a PR pass.
- Ordinary compiler warnings are allowed and must remain visible. Build, formatting, tests, coverage, agreed vulnerability checks (including NuGet high/critical), secret scanning, OpenAPI, and image scanning have their own gates.

## Local verification

From the repository root, with SDK 10.0.100 or a newer .NET 10 feature band (selected by `global.json`), Docker, Node.js 22 and Bash available, run the full local check:

```bash
bash scripts/verify.sh
```

For focused work, use the individual commands below (they do not include all coverage and OpenAPI checks):

```bash
dotnet restore ProductsCatalog.sln
dotnet build ProductsCatalog.sln --configuration Release --no-restore
dotnet format ProductsCatalog.sln --verify-no-changes --no-restore
dotnet test tests/ProductCatalog.Domain.UnitTests/ProductCatalog.Domain.UnitTests.csproj --configuration Release --no-restore
dotnet test tests/ProductsCatalog.Application.UnitTests/ProductsCatalog.Application.UnitTests.csproj --configuration Release --no-restore
dotnet test tests/ProductsCatalog.Infrastructure.UnitTests/ProductsCatalog.Infrastructure.UnitTests.csproj --configuration Release --no-restore
dotnet test tests/ProductCatalog.Acceptance.Tests/ProductCatalog.Acceptance.Tests.csproj --configuration Release --no-restore
```

CI additionally scans secrets, dependencies and the image before any conditional publish. Review `.github/workflows/dotnet.yml` for the actual commands and results. Never report a check as passing if it was not run.

## Handoff

Use `.github/pull_request_template.md`. Report changed behavior, linked backlog ID, tests and commands actually run, checks not run with reasons, and any remaining contract or migration risk. Mark the backlog item complete only after its acceptance criteria are satisfied in both repositories where applicable.
