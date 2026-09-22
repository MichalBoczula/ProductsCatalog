# ADR-0001: Use SQL Server

- Status: Accepted
- Date: 2026-09-22

## Context

The catalog has relational data, product history, filters, projections, pagination, and use cases that require joins. The service needs transactional writes, referential integrity, indexes, deterministic migrations, and efficient read queries. The data is not an unstructured document store: relationships and query composition are part of the model.

The catalog is also expected to be read-heavy. The write model benefits from aggregate persistence, while the read model benefits from direct SQL and narrow projections.

## Decision

Use SQL Server as the service database.

- Use Entity Framework Core for command persistence, aggregate changes, history writes, and migrations.
- Use Dapper for query repositories and explicit read projections.
- Keep the database owned by ProductsCatalog; other services access catalog data through the API.
- Use SQL Server health checks for readiness.
- Use Testcontainers with the same SQL Server engine for integration and acceptance tests.

## Consequences

The service can use relational constraints, transactions, joins, indexing, and mature SQL tooling. Read and write paths can be optimized independently while sharing one consistent data store.

The team must maintain SQL Server infrastructure and migrations. Query performance depends on appropriate indexes and execution-plan monitoring. Horizontal API scaling does not remove the need to scale and operate the database deliberately.

## Alternatives considered

### Document database

A document database could make single-product retrieval simple, but the current relational model, joins, history, filters, and transactional consistency would move complexity into application code or denormalized documents.

### Entity Framework Core for every read

Using EF Core everywhere would reduce the number of persistence technologies, but explicit Dapper queries provide clearer projections and direct control over read-heavy SQL paths.
