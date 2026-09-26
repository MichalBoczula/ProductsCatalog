# ADR-0010: Run core CI checks through repository scripts

- Status: Accepted
- Date: 2026-09-27

## Context

GitHub Actions previously repeated restore, build, tests, coverage and OpenAPI
commands inline. The local verification script had a separate implementation.
This made the quality checks harder to reuse from another CI platform and
allowed local and remote behavior to drift.

## Decision

Use `scripts/ci.sh` as the executable entry point for source checks, audited
restore and Release build, formatting, Domain/Application/Infrastructure and
Acceptance suites, layer coverage and generated OpenAPI validation. The local
`scripts/verify.sh` invokes the same commands and additionally builds an image.
Each CI test job runs one suite, restores its dependencies and writes TRX and
coverage under `artifacts/verification/`.

GitHub Actions retains job scheduling, artifact upload, the quality gate,
Dependency Review, Gitleaks, Trivy and the conditional Docker Hub publication
of the scanned image described in [ADR-0009](0009-image-publication-boundary.md).
The same script commands can be invoked by a future GitLab template without
changing the service's quality thresholds.

## Consequences

One repository script defines each core check for both local and remote runs.
The separate 70% Domain and Application gates and the Infrastructure report
remain intact. Independent test jobs each restore dependencies; this adds
restore work but keeps jobs isolated. Platform-specific security and publishing
steps stay in the workflow.

## Alternatives considered

- Keep commands inline in each CI provider: duplicates the quality policy and
  makes local verification drift more likely.
- Run the entire `verify.sh` in one CI job: loses separate suite artifacts,
  failure attribution and the existing quality gate.
