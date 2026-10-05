# ADR-0011: Publish documentation inputs to the LiveDocs Blob archive

- Status: Accepted for LD/5
- Date: 2026-10-05

## Context

Products owns its Reqnroll acceptance scenarios and executable API, flow and
validation descriptions. The shared LiveDocs image needs durable evidence for
multiple application versions without generated reports in Git or cross-repository
write access from Products.

## Decision

Export OpenAPI, flows and validation policies from the same running API during
contract verification. Combine generated operation links, raw Allure results and
feature sources from the tested source commit using commit-pinned LiveDocs reusable
workflows. Require bundle validation in the existing quality gate; clear stale
local Allure files before acceptance runs. Pin local Allure CLI to 2.46.1, matching
the shared renderer; preserve the independent .NET adapter version.

Only after the master service image has been built, scanned and published may
an explicitly enabled archive job upload the bundle and versioned discovery receipt
into the private `livedocs` container. Authenticate using Products' dedicated OIDC
writer, provisioned by application Infrastructure and scoped to that container.
No Azure upload occurs on PRs. Publication defaults off until setup exists; when
on, upload/configuration failures fail CI. The archive refuses overwrite and checks
existing bytes on retry.

LiveDocs discovers receipts and verifies successful source CI provenance before
opening its own manifest PR. Products does not receive GitHub write access there.
LiveDocs generates report HTML in its image builder; Terraform owns deployment.

## Consequences

Existing service quality/security gates remain required. The tested commit/run
identifies the service image and bundle. Historical documentation can be rebuilt
from durable inputs. GitHub artifacts are short-lived CI evidence, not the release
archive. The handoff is asynchronous, so application Infrastructure coordinates
matching service/documentation image selections. Azure identities and enable
variables must be configured before live archival can be verified.

## Alternatives considered

- Commit Allure HTML/results in Git: accumulates generated data and attachments.
- Products pushes to LiveDocs: requires another repository's write credentials.
- Rely on Actions artifact retention: cannot guarantee historical rebuild inputs.
