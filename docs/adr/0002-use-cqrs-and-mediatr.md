# ADR-0002: Use CQRS and MediatR

- Status: Accepted
- Date: 2026-09-22

## Context

Catalog commands change aggregates and write history. Catalog queries return purpose-built DTOs, lists, filtered results, top products, and paged history. The two paths have different persistence and performance needs.

The API should remain thin, and application behavior should be testable without binding handlers to HTTP or SQL Server details.

## Decision

Separate application operations into commands and queries and dispatch them through MediatR.

- Commands use domain aggregates, validation policies, command repositories, and EF Core persistence.
- Queries use read repositories, Dapper projections, and query-specific DTOs.
- Minimal API endpoints translate HTTP input into a command or query and translate the result into an HTTP response.
- MediatR pipeline behaviors handle cross-cutting application concerns such as logging.
- Each operation uses a flow descriptor to make its ordered processing steps available as executable documentation.

## Consequences

Handlers remain focused, independently testable, and decoupled from HTTP. Read queries can evolve independently from aggregate persistence. Flow descriptors create a shared implementation and documentation vocabulary.

CQRS introduces more types and registrations than a direct service call. MediatR is an in-process dispatcher, not a message broker, and does not by itself provide distributed messaging, retries, or eventual consistency.

## Alternatives considered

### Endpoint-to-repository calls

Direct calls reduce the number of classes initially but couple transport, orchestration, validation, persistence, and documentation concerns.

### Separate read and write services

Independent deployments are not justified by the current scale. The logical CQRS split preserves the option without adding distributed-system complexity today.
