# ADR-0006: Use a stable public problem response

- Status: Accepted
- Date: 2026-09-24

## Context

Domain validation, missing resources, JSON binding, routing and unexpected
failures arise at different points in the HTTP pipeline. Clients need a safe,
consistent response without depending on internal exception messages.

## Decision

The API owns error translation and emits `application/problem+json` with a
stable `code`, HTTP `status`, safe detail, request path and trace ID. Domain
validation can include structured errors. JSON inspection may include missing
property names only when they can be inferred safely from the request contract.
Unexpected failures use a generic public 500 and retain details in server logs.
Unmatched routes, methods and unsupported media types also receive controlled
errors. Health checks keep their ASP.NET Core plain-text responses.

Endpoint response metadata and generated OpenAPI describe declared errors;
acceptance requests compare real status, media type and response schema with
that contract. See [the public error contract](../api-problem-contract.md) for
the current codes and exceptional cases; do not duplicate that mapping here.

## Consequences

Clients can branch on `code` instead of text. Adding a new failure path requires
coordinated metadata and acceptance coverage. The generic 500 protects internal
details but depends on server logs for diagnosis. A framework error before
endpoint execution may not be a declared operation response; the acceptance
suite checks its safe shape separately.

## Alternatives considered

- Return raw framework or exception messages: leaks internals and gives clients
  unstable strings.
- Maintain an independent handwritten error schema: risks drift from endpoint
  metadata and runtime responses.
