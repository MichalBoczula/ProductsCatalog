# Source description contract (REF-10/1)

This is the **version 1 logical source format** for ProductsCatalog and
ECommerceStoreUsers. It joins an HTTP operation, its flow, relevant validation
policies and acceptance cases. It is a source model for agents and future
documentation ingestion, not a replacement HTTP endpoint or OpenAPI document.
The current Products `/products-documentation/flow` and
`/products-documentation/validation-policies` return arrays; Users
`/users-documentation/flows` and `/users-documentation/validations` return
wrappers with maps. Their routes and responses remain unchanged in REF-10/1.

## Shape

One UTF-8 JSON object describes one exposed business operation:

| Field | Type | Meaning |
| --- | --- | --- |
| `schemaVersion` | integer | Required, currently `1`; change deliberately when the source shape changes. |
| `service` | string | Required: `products` or `users`; scopes IDs across repositories. |
| `operationId` | string | Required; the exact, case-sensitive registered `.WithName(...)`/OpenAPI operation ID. |
| `method`, `path` | strings | Required; uppercase HTTP method and registered route template, including route constraints. |
| `request`, `response` | objects | Required; `type` names the public DTO or `none`, and `notes` explain relevant query/body requirements or returned data. |
| `outcomes` | array | Required, nonempty; `{status, cause, scenarioIds}` per distinct *observed* response cause. `cause` is a stable descriptive key within an operation. `scenarioIds` is an array of IDs from that service's `docs/acceptance-matrix.tsv`; an empty array means coverage is explicitly pending. |
| `flow` | object | Required; `{flowId, steps}`. A step is `{stepId, description}` in execution order. Include significant branches, not only the happy path. |
| `policies` | array | Required; `{policyId, rules}`. A rule is `{ruleId, description}`. Use `[]` when the operation uses no domain validation policy. Binding/framework checks belong in `outcomes`, not in an invented domain policy. |
| `effects` | array of strings | Required; durable data/history changes or `[]` for a read-only operation. |
| `consistency` | string | Required; transaction/concurrency/rollback or read semantics relevant to the operation. |

The examples are [Products GetMobilePhoneById](examples/source-description-products.json)
and [Users GetFavoritesByClientId](examples/source-description-users.json).
They illustrate the shape using existing endpoint registrations and acceptance
matrix IDs; they do not claim to describe every outcome or policy yet.

## Identity and links

- Keep `operationId` identical to endpoint registration until a deliberate API
  compatibility change. The existing `GetFiltered MobilePhones` contains a
  space; retain that exact value in source descriptions for now. REF-10/2 or
  REF-11 will handle any coordinated rename and affected matrix/OpenAPI links.
- Use lowercase dot-separated IDs with a service prefix for new `flowId` and
  `policyId`, for example `products.mobile-phone.get-by-id` and
  `users.empty-guid`. A flow ID belongs to one operation; a policy ID can be
  referenced by several operations. Never derive IDs from method names,
  routes, display labels, or ordering at runtime.
- `stepId` is unique within a flow and `ruleId` within a policy; keep them
  stable when prose or step order changes. Renaming an existing ID requires
  updating all references and an explicit migration note.
- `scenarioId` is already owned by the service's acceptance matrix. Reference
  it verbatim; do not regenerate from the feature title. One scenario can
  support several documented causes only when it actually asserts each one.
- The tuple `(service, operationId)` identifies the operation. Each distinct
  `(operationId, status, cause)` has its own outcome; several causes may share
  an HTTP status. Link each outcome to the scenario IDs that prove it.
- Descriptions cover public behavior and business branches, including no-op,
  not found, conflict, failure and history effects when applicable. Do not
  include credentials, personal data, sample production payloads, or raw
  exception messages. `response.type = none` describes a bodyless response.

## Adoption boundary

REF-10/2–5 will map all operations and existing descriptors to this shape and
verify the actual content. REF-10/6 will check cross-repository links and
compatibility of the existing documentation routes. This document establishes
the format and identity rules; it does not mark the existing descriptors or
acceptance matrix as semantically complete. OpenAPI/runtime drift and
architecture gates remain REF-11. No hosting, Allure publishing or RAG index is
introduced here.
