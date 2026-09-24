# ADR-0009: Scan before conditional Docker Hub publication

- Status: Accepted
- Date: 2026-09-24

## Context

A green build alone does not verify the image that will be distributed.
Rebuilding after a scan could publish different bits from the scanned image.

## Decision

Run image work only after the CI quality gate succeeds. Build one local Docker
image, scan it with Trivy for high/critical findings under the current policy,
and record its image ID. On a `master` push only, tag and push that same local
image to Docker Hub with commit SHA and `latest` tags. Verify both local tags
match the scanned image ID and both pushed tags report the same digest. Pull
requests build and scan without registry login or publication.

Build, format, coverage, Domain/Application/Infrastructure/acceptance tests,
generated OpenAPI, architecture, dependency review on PR and secret checks
precede this job. Ordinary compiler warnings remain visible but are not a
global warnings-as-errors gate; NuGet high/critical audit is separate.

## Consequences

The publish job does not perform a second Docker build. SHA and `latest` point
to the same pushed digest for that run. A mutable `latest` tag remains useful
for discovery but consumers requiring reproducibility should pin a digest or
immutable SHA tag. This decision covers Docker Hub image publication only;
deployment, provenance attestations and rollout/rollback are separate work.

## Alternatives considered

- Rebuild for push after scanning: risks a different published artifact.
- Publish from PRs: gives unreviewed changes a distribution path.
