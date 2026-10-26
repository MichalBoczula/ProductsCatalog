# BDD-09 completion record

Status: **Complete for the Products source and published LiveDocs image audit**,
2026-10-26. Azure rollout is a separate manual Terraform apply and is not claimed
executed by this record. The selected snapshot is frozen to the source below;
a later documentation-only commit does not silently change that selection.

## Verified identities

| Artifact | Identity |
| --- | --- |
| Products source | `ab8fe05e2fee4870c6b1255a8f4d6e5d49d90f5e` ([PR #208](https://github.com/MichalBoczula/ProductsCatalog/pull/208)) |
| Products full master CI | [40394312976](https://github.com/MichalBoczula/ProductsCatalog/actions/runs/40394312976) |
| Bundle generated at | `2026-10-26T20:24:44Z` |
| Bundle SHA-256 / size | `9625b79583bec33425fd2ca7c1cc43b0a4f323483698614534fbdcf0b25d5da6` / 941982 bytes |
| LiveDocs source | `0ca91fc8be1231f6b6f8ad17c41226e3bc2bf6f2` ([PR #12](https://github.com/MichalBoczula/ECommerceStore.LiveDocs/pull/12)) |
| LiveDocs full main CI | [40396430674](https://github.com/MichalBoczula/ECommerceStore.LiveDocs/actions/runs/40396430674) |
| Published image | `mb0101/ecommerce-store-livedocs@sha256:f3749f07d4c439329ad781471e9bf9f9629d30d28c70bad0aa64d67b5af2107f` |
| Image built at | `2026-10-26T20:32:54Z` |

Producer master and LiveDocs main CI both completed successfully. The image was
built once, checked over actual Nginx HTTP, scanned and published with matching
full-SHA/latest registry digests. Its `livedocs-image/image.json` artifact and
`/build-info.json` carry the LiveDocs build identity above. The Blob upload receipt
was compared with the independently downloaded and checked master bundle. The PR
merge-ref bundle was reviewed separately and was not selected for publication.

## Final checklist evidence

| Check | Result |
| --- | --- |
| Source/result discovery | 19 features, 60 declarations, 76 expanded BDD cases, 73 stable scenario IDs and 76 instance IDs; every source/example identity accounted for. All prior 64 instance IDs preserved. |
| Execution | 144 acceptance-project tests passed, zero failed/skipped; 76 are BDD cases emitted to Allure. All four suites and existing coverage/security/contract gates passed. |
| Scenario structure | Readable intent and Given/When/Then preserved; input/expected tables and examples resolve in the generated report. Health, startup, malformed JSON and other technical exceptions retain truthful formats. |
| HTTP evidence | 140 request/response pairs, 140 contract observations and 140 source-documentation links share actual exchange identities. Supporting/setup calls do not claim primary outcomes. |
| Attachments and inventory | All 1,026 attachments resolve and JSON-labelled attachments parse; every one of 1,201 manifest entries matches its SHA-256 and byte count. |
| Contract coverage | Bidirectional source registrations/matrix/OpenAPI checks cover 11 operations and 61 matrix cases. Schema checks retain field presence/nullability/types/formats, actual media/status, error details and Location semantics. OpenAPI contains 32 schemas and 184 described properties with applicable typed examples. |
| Flow/rule evidence | Nine flows, 57 steps and five policies remain linked. All 15 published active policy error signatures plus the inline IDs guard occur in actual responses. All active rule IDs have rejection evidence. Seventy primary outcomes link 49/50 declared branches; the remaining defensive guard is classified below. |
| Durable state and claims | Current/history assertions cover accepted, rejected, no-op, race and controlled-failure outcomes. README/source policy/error contract/backlog reflect the reviewed revision and explicit proof limits. |
| Selected LiveDocs snapshot | Private archive identity/integrity, pinned Allure generation, all rendered cases/attachments over actual Nginx HTTP, assets/404/readiness/build identity and image security/publication checks passed. The other three producer references are unchanged. |

The envelopes preserve 217 JSON bodies, 50 absent bodies, 11 text bodies, one empty
body and one malformed JSON body. A JSON attachment describing malformed request
bytes is different from claiming those request bytes are valid JSON.

## Recorded limits

- `products.branch.list.null-result` remains unlinked: the real Dapper repository
  returns a non-null read-only collection, including an empty collection as 200.
  This guard concerns a substituted repository violating that contract. No normal
  list 404 or 100% runtime branch coverage is claimed.
- Hardware validation remains published standalone; the phone money-rule list is
  empty. Source branch/step links are declared semantics, not runtime telemetry.
- Every published active error signature has an example; all combinations of
  invalid input are not exhaustively tested. Controlled faults before SaveChanges
  do not demonstrate rollback of an actual SQL transaction failure.
- Historical design-first approval before original implementation is not evidenced
  by generated contracts. Current source/contract consistency is verified; that
  chronology remains an explicit evidence gap.
- RAG ingestion/indexing, populated-database upgrade/recovery, production readiness
  and equivalent BDD packages for the other services remain separate work.

## Manual Azure rollout

In the shared ECommerceStore.Terraform working directory, replace only the existing
`livedocs` entry inside `app_images` in the ignored Terraform variables file:

```hcl
  livedocs = "docker.io/mb0101/ecommerce-store-livedocs@sha256:f3749f07d4c439329ad781471e9bf9f9629d30d28c70bad0aa64d67b5af2107f"
```

Keep the other image entries and existing database/registry variable values.
Create and inspect a fresh plan; apply the reviewed plan:

```powershell
terraform plan "-out=livedocs-refresh.tfplan"
terraform apply "livedocs-refresh.tfplan"

$liveDocsUrl = (terraform output -json app_urls | ConvertFrom-Json).livedocs
$build = Invoke-RestMethod "$liveDocsUrl/build-info.json" -TimeoutSec 180
$build.commitSha -eq '0ca91fc8be1231f6b6f8ad17c41226e3bc2bf6f2'
$bundle = Invoke-RestMethod "$liveDocsUrl/livedoc/v1/products/bundle.json" -TimeoutSec 180
$bundle.commitSha -eq 'ab8fe05e2fee4870c6b1255a8f4d6e5d49d90f5e'
Start-Process "$liveDocsUrl/livedoc/v1/products/allure/"
```

Both identity comparisons should return `True` after rollout. Publishing the image
and advancing the manifest do not update a running Azure revision by themselves.

