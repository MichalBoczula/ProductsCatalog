# Generated source documentation (BDD-08)

Products is the first service receiving the BDD packages. Other producers retain
their own contracts; this package does not change Users, Orders or Payments.
There is no second handwritten OpenAPI, maintained request/response snapshot or
RAG index in this producer.

| Information | Owning source | Generated evidence |
| --- | --- | --- |
| Routes, DTOs, status/media, field semantics/examples and headers | API registrations, DTO/serializer metadata and Swagger filters | `sources/openapi.json` |
| Flow intent, ordered steps, decisions, effects and failure conditions | `Documentation`, `FlowStep` and `FlowOutcome` attributes beside executed Application methods | Flow endpoint text and `sources/operation-links.json` |
| Published rule conditions and possible errors | Domain metadata and the same rule collections used by `Validate` and `Describe` | Policy endpoint and source link catalogue |
| Expected primary branch and rejection rules | Existing acceptance matrix plus explicit framework tags | Source branch/rule/scenario links |
| Actual request, response, validation rule and example identity | Scenario HTTP recorder and assertions | Allure HTTP pairs, contract observations and documentation links |

The documentation endpoint shapes remain `actionName`/`steps` and
`policyName`/`rules`. Flow step strings now explain behavior rather than merely
naming methods. Structured identities and semantics are an additive part of the
existing generated operation-links artifact; the running LiveDocs image is not
changed by generating it.

## Identity and join rules

- Operation identity is `(products, operationId)`, preserving the existing
  case-sensitive `GetFiltered MobilePhones` name, including its space. Join the
  method and route with exported OpenAPI. The legacy projection root retains
  `service: ProductsCatalog`; `documentation.serviceId: products` is the canonical
  join value used by bundle and Allure evidence. The generator reads the actual
  mediator call and verifies its handler/descriptor/policy relationships.
- Class-owned `Documentation` IDs identify flows, policies and rules. A step ID
  combines its explicit flow ID and explicit step key; it is independent of its
  method name, wording and order. `FlowOutcome` IDs identify declared branches.
  Keep IDs when editing prose. These attributes are metadata, not execution or
  validation mechanisms.
- The matrix's `branchId` and comma-separated `ruleIds` reference those source
  IDs. They annotate existing scenarios rather than duplicating the API
  specification. The empty-ID check is an inline guard, not a published policy.
  Framework routing/media/binding cases have explicit branch labels in Gherkin.
- Match a source scenario by `scenarioId`, and an expanded example by its stable
  `instanceId`. Join actual evidence using `exchangeId`. Each exchange has a
  request, response, contract observation and `HTTP documentation links` item.
- A primary link requires the target operation and status during the `When`
  phase unless explicitly marked supporting; a technical framework label selects its own `When` request. Exactly one
  primary exchange is required for each bound case. Setup, competing requests,
  follow-up reads and technical probes remain separate exchanges. A first DELETE
  during setup must not claim the repeated-delete branch. When first and repeated
  DELETE share one When step, the first request is explicitly marked supporting;
  its actual phase and bytes are retained.
- `declaredStepIds`/`declaredPolicyIds` describe the nominal source flow, not a
  runtime trace. `expectedBranchId` comes from the scenario's declared intent;
  assertions establish the outcome. `observedErrorRuleIds` come from actual
  validation error names and must include the expected rejection rule. A status
  alone cannot establish a rule or a branch.

## Enforcement and known gaps

The source projection classifies four published policies as invoked by endpoint
flows. `ElectronicDetailsValidationPolicy` and its two rules are registered and
published **standalone**: no catalogue endpoint invokes them. The phone policy's
money-rule collection is empty. Do not present hypothetical hardware or money
constraints as enforced HTTP rules. Unregistered Domain policy classes are outside
this published inventory.

Generated coverage lists missing links rather than synthesizing success. Current
unlinked application branches are incoming-update validation failure, failure of
the later history query after successful existence lookup, and the defensive null
list-result guard. The real list repository returns a non-null list; the latter is
not a declared normal HTTP response. Active rules without a rejection BDD case
are phone common-description validation, negative filter bounds and strict filter
price range. Domain/unit tests and an HTTP rejection scenario are different proof.
A rule with one linked rejection can still have untested error conditions; for
example, a Description2 rejection does not prove the camera/Description3 errors.
BDD-09 must compare possible error messages with actual responses as well as IDs.
These gaps must be assessed in BDD-09; green source checks do not erase them.

The controlled save-failure cases throw before EF SaveChanges. They demonstrate
that queued current/history changes were not saved, not rollback of a real SQL
transaction failure. Concurrency cases separately compare the winning current
state and history. History's existing controlled read fault is at initial GetById,
not at the later history query.

## Verification and retrieval use

Existing source checks reject missing/duplicate IDs, incorrect flow/handler links,
orphaned branches/rules and attaching standalone rules to an invoked branch.
Contract verification compares exported runtime flow text and policy rule
inventories with the generated source catalogue. Regression tests check setup
versus primary evidence and actual rule matching. Full CI and the real result
bundle are required before completing a package.

For future retrieval, keep bundle repository/commit/run identity with each item;
resolve references inside that same bundle. Retrieve OpenAPI for shapes, source
flow/outcome text for decisions/effects, policy descriptors for possible errors,
and scenario exchanges for observed behavior. Do not mix an old portal snapshot
with a newer producer's evidence or treat publication as proof of execution.
No chunker, vector store, retrieval endpoint or RAG evaluation is implemented here.
Exclude credentials, production payloads and raw exception messages.

The portfolio LiveDocs portal and producer Blob archive already exist; ADR-0011
and ADR-0012 supersede ADR-0005's original hosting follow-up. This work changes
producer evidence only. Deployment and promotion of a new portal snapshot remain
deferred until the BDD packages and final audit are complete. Generated output
also does not establish historical design-first approval; record that evidence or
its remaining gap in BDD-09.
