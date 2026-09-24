# Architecture Decision Records

Architecture Decision Records capture decisions that materially affect the service. A record explains the context, the decision, its consequences, and any follow-up work. Accepted records remain in the repository even if a later ADR supersedes them.

| ADR | Status | Decision |
|---|---|---|
| [0001](0001-use-sql-server.md) | Accepted | Use SQL Server with EF Core for writes and Dapper for reads. |
| [0002](0002-use-cqrs-and-mediatr.md) | Accepted | Separate commands and queries and dispatch them through MediatR. |
| [0003](0003-openapi-and-api-client-strategy.md) | Accepted | Generate one OpenAPI document from endpoint code and do not store generated clients. |
| [0004](0004-database-migration-strategy.md) | Accepted | Apply migrations as an explicit deployment activity. |
| [0005](0005-living-documentation-and-rag.md) | Accepted | Generate source descriptions and links in CI; hosting and RAG remain planned. |
| [0006](0006-public-error-contract.md) | Accepted | Translate failures into safe problem responses. |
| [0007](0007-acceptance-isolation.md) | Accepted | Use one test SQL Server with a separate database per scenario. |
| [0008](0008-optimistic-concurrency-and-history.md) | Accepted | Protect MobilePhone writes and history with an optimistic token. |
| [0009](0009-image-publication-boundary.md) | Accepted | Scan one image before conditional Docker Hub publication. |
