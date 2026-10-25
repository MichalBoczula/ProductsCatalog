# ADR-0012: Upload LiveDocs inputs to the consolidated store archive

- Status: Accepted; supersedes the writer authentication in ADR-0011
- Date: 2026-10-25

## Context

The rebuilt portfolio has one ECommerceStore.Terraform root and an existing private
`livedocs` container in its shared storage account. Products already collects raw
Allure results and packages validated documentation, but its archive job expects
the dedicated OIDC writer from the older infrastructure. This task publishes the
Products inputs; LiveDocs reader/import and image rollout come later.

## Decision

Retain the required acceptance outputs, generated contracts and commit-pinned shared
packager. Pin its tooling to the same LiveDocs revision as the deployed empty portal.
The package contains raw Allure results/attachments, features, OpenAPI, flows,
validation policies and operation links. Render Allure HTML in LiveDocs, preserving
the existing artifact contract rather than introducing a second report format.

After successful master quality checks, image scanning and Docker Hub publication,
an explicitly enabled Products archive job uses Azure CLI with a container-scoped
SAS from `LIVEDOCS_STORAGE_SAS_TOKEN`. Keep the default target account
`stecproductsdevmb261013`, allow the existing account/version variables to override
it, and require read/create/write permissions to `livedocs`. No Azure OIDC login,
management credentials or new cloud resources are required for this producer.

Keep all existing security/coverage gates and the required documentation-bundle job.
PRs never upload; publication defaults off until `LIVEDOCS_PUBLISH_ENABLED=true`.
Once enabled, missing/expired credentials and storage errors fail the archive job.
Before upload, verify bundle metadata matches the Products repository, source commit
and workflow run. Compute the ZIP's SHA-256 and size and publish the bundle before
its stable discovery receipt, using the existing reports/candidates paths.

Refuse overwrite. On retry, download and compare each existing object's bytes.
Store no SAS in the receipt, logs, generated reports or public image. The receipt
and job summary expose only stable URLs and provenance for subsequent import.

## Consequences

The consolidated archive works without provisioning a separate CI identity. The SAS
expires and must be renewed in the same Actions secret; the README gives a 90-day
setup. Storage writes are restricted to this container, while non-overwrite remains
a publisher rule. Upload does not mutate the running LiveDocs replica. LiveDocs'
reader authentication, manifest path and documentation image deployment remain the
next step. An Azure upload remains unverified until the configured archive job succeeds.

There are no runtime API, package, database-schema, migration or application behavior
changes. The asynchronously published bundle still belongs to the tested commit/run.

## Alternatives considered

- Reuse the old dedicated OIDC writer: requires identity/federation/RBAC setup absent
  from the consolidated store and outside this producer-only task.
- Use the storage account key: gives the producer access beyond `livedocs`.
- Upload generated HTML only: changes the existing validated source-bundle contract
  and loses inputs required by LiveDocs' versioned renderer.
