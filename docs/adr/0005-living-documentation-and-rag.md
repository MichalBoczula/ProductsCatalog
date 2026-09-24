# ADR-0005: Living documentation and RAG

- Status: Accepted
- Date: 2026-09-22

## Context

Traditional documentation drifts when behavior changes but prose is not updated. ProductsCatalog already contains structured knowledge in endpoint metadata, DTOs, validation policies, flow descriptors, and executable acceptance scenarios.

Developers and AI assistants need documentation that explains both the external HTTP contract and the internal business flow. The same material should support a deployed human-readable report and Retrieval-Augmented Generation without treating arbitrary source-code chunks as the primary knowledge base.

## Decision

Adopt living documentation generated from executable sources.

The implemented source set consists of:

- OpenAPI generated from endpoint metadata and DTOs;
- Reqnroll scenarios linked by stable scenario IDs to named operations;
- ordered flow descriptions exposed by `/products-documentation/flow`;
- validation-policy and error descriptions exposed by `/products-documentation/validation-policies`;
- ADRs and operational documentation stored in the repository.

Use the Description Pattern for application behavior: each command or query exposes ordered, human-readable flow steps close to the code that performs them. Validation policies expose rule names and possible errors in a similarly structured form.

The current CI checks executed flow steps and validation-policy exposure,
generates operation-to-flow-to-policy-to-scenario links, exports and lints
OpenAPI, and compares declared responses with actual HTTP outcomes exercised
by acceptance tests. Generated projections are not committed as parallel
specifications.

The following work is planned, not implemented by this decision:

1. publish human-readable Allure and API documentation with commit/version identity;
2. prepare controlled chunks and metadata for retrieval;
3. index approved artifacts for RAG retrieval and evaluate the answers.

## Consequences

Source descriptions change with executable behavior and are validated in CI.
The documentation endpoints expose flows and validation rules to other tooling.
No hosted portal, RAG ingestion, index or retrieval service is provided today.

Generated documentation may expose internal or sensitive information if inputs are not controlled. The ingestion pipeline must exclude secrets, credentials, production payloads, stack traces, and personal data. Only approved CI artifacts should enter the RAG index.

Allure publication, retention, RAG store, chunking and refresh remain follow-up
decisions. Accepted here means adopting generated sources, not accepting an
unimplemented hosting or RAG architecture.

## Alternatives considered

### Maintain documentation manually

Manual prose is useful for intent and ADRs but cannot reliably mirror every route, response, validation error, and test result.

### Index the repository source directly

Raw source indexing produces noisy chunks, mixes implementation details with public contracts, and makes version and security controls harder. Generated descriptions are the preferred retrieval source.
