# ADR-0005: Living documentation and RAG

- Status: Accepted
- Date: 2026-09-22

## Context

Traditional documentation drifts when behavior changes but prose is not updated. ProductsCatalog already contains structured knowledge in endpoint metadata, DTOs, validation policies, flow descriptors, and executable acceptance scenarios.

Developers and AI assistants need documentation that explains both the external HTTP contract and the internal business flow. The same material should support a deployed human-readable report and Retrieval-Augmented Generation without treating arbitrary source-code chunks as the primary knowledge base.

## Decision

Adopt living documentation generated from executable sources.

The documentation set consists of:

- OpenAPI generated from endpoint metadata and DTOs;
- Reqnroll scenarios and Allure acceptance-test results;
- ordered flow descriptions exposed by `/products-documentation/flow`;
- validation-policy and error descriptions exposed by `/products-documentation/validation-policies`;
- ADRs and operational documentation stored in the repository.

Use the Description Pattern for application behavior: each command or query exposes ordered, human-readable flow steps close to the code that performs them. Validation policies expose rule names and possible errors in a similarly structured form.

The target automation will:

1. generate and validate documentation artifacts in CI;
2. associate artifacts with a commit SHA and application version;
3. publish the human-readable Allure and API documentation;
4. split content by API operation, application action, validation policy, and acceptance scenario;
5. attach metadata such as route, method, action, response code, version, and source commit;
6. index approved artifacts for RAG retrieval.

## Consequences

Documentation changes with executable behavior and can be validated before publication. RAG answers can cite stable, structured artifacts rather than infer behavior from unrelated code fragments. The documentation endpoints also make application flows and validation rules available to other tooling.

Generated documentation may expose internal or sensitive information if inputs are not controlled. The ingestion pipeline must exclude secrets, credentials, production payloads, stack traces, and personal data. Only approved CI artifacts should enter the RAG index.

The exact Allure publication flow, retention policy, RAG store, chunking implementation, and refresh trigger remain follow-up work. They will be finalized in the living-documentation task and recorded in the README update identified as task 6b.

## Alternatives considered

### Maintain documentation manually

Manual prose is useful for intent and ADRs but cannot reliably mirror every route, response, validation error, and test result.

### Index the repository source directly

Raw source indexing produces noisy chunks, mixes implementation details with public contracts, and makes version and security controls harder. Generated descriptions are the preferred retrieval source.
