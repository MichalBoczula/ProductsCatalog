# Architecture Decision Records

Architecture Decision Records capture decisions that materially affect the service. A record explains the context, the decision, its consequences, and any follow-up work. Accepted records remain in the repository even if a later ADR supersedes them.

| ADR | Status | Decision |
|---|---|---|
| [0001](0001-use-sql-server.md) | Accepted | Use SQL Server with EF Core for writes and Dapper for reads. |
| [0002](0002-use-cqrs-and-mediatr.md) | Accepted | Separate commands and queries and dispatch them through MediatR. |
| [0003](0003-openapi-and-api-client-strategy.md) | Accepted | Generate one OpenAPI document from endpoint code and do not store generated clients. |
| [0004](0004-database-migration-strategy.md) | Accepted | Apply migrations as an explicit deployment activity. |
| [0005](0005-living-documentation-and-rag.md) | Accepted | Generate documentation from executable sources and prepare it for RAG ingestion. |
