# ADR-0003: OpenAPI and API client strategy

- Status: Accepted
- Date: 2026-09-22

## Context

The API needs one accurate contract for developers, CI, future documentation automation, and RAG ingestion. Maintaining parallel Swashbuckle, NSwag, and handwritten specifications creates drift. Generated API clients are not consumed or versioned in this repository.

The team follows a design-first process: routes, models, responses, validation, and acceptance behavior are agreed before implementation. The executable implementation must then remain the single technical source used to generate artifacts.

## Decision

- Use ASP.NET Core endpoint metadata and DTOs as the executable OpenAPI source.
- Use Swashbuckle as the only OpenAPI generator.
- Generate the document from the running API in CI.
- Validate the complete document with a pinned Redocly CLI and its OpenAPI `spec` ruleset.
- Link named operations to executed flows, policies and acceptance scenarios
  from code, and check declared responses against acceptance HTTP behavior.
- Treat structural errors and unresolved references as build failures.
- Do not store or automatically generate Angular, .NET, Kiota, or other API clients in this repository.
- Do not add client-freshness checks while generated clients are absent.

## Consequences

The OpenAPI document follows the deployed code and is validated on every change. Reviewers can compare endpoint behavior, acceptance scenarios, and generated contract without synchronizing multiple specifications.

Endpoint metadata is production code and must be reviewed with the same care as handlers. Design decisions still occur before implementation, but the repository does not use a handwritten spec-first workflow.

Consumers that need generated clients own their generation from the API's
OpenAPI contract. Publishing a versioned contract for consumers is a separate
deployment decision; this repository currently validates the generated file
in CI and does not publish a client package.

## Alternatives considered

### Handwritten OpenAPI as the repository source

This is a valid spec-first model but would create a second source alongside endpoint metadata and require additional synchronization tooling.

### Multiple generators

Combining NSwag, Swashbuckle, and `Microsoft.AspNetCore.OpenApi` adds duplicate configuration and can produce inconsistent documents.
