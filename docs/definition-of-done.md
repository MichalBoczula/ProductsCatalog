# Definition of done for a change

This checklist is the review standard for a change, not a claim that every existing endpoint already meets it. Apply each item when relevant; explain a non-applicable or deferred item in the PR.

- The PR names the `REF-xx` or other backlog item, explains the intended behavior, scope, and affected layers. New dependencies and contract/schema changes have a reason.
- The implementation follows the existing API → Application → Domain/Infrastructure boundaries. Database migration or seed behavior is described and verified if touched.
- An endpoint change includes request validation, safe error responses, correct status/content type and OpenAPI metadata, source flow and validation descriptions, and acceptance tests for the affected success and failure paths. Check durable effects and history for writes.
- Appropriate Domain, Application, SQL integration, and HTTP acceptance tests run. Coverage gates remain at least 70% line coverage separately for Domain and Application. A percentage does not replace the endpoint/status/cause test matrix.
- `dotnet restore`, Release build, and `dotnet format --verify-no-changes` pass; relevant test commands in `AGENTS.md` pass. For contract changes, verify the generated OpenAPI and relevant documentation. CI's security and Docker jobs must pass where applicable.
- A compiler warning is recorded or explained if material, but is not by itself a blocker. Vulnerability findings follow their separate severity policy. Do not weaken gates, silence tests, or commit secrets to pass review.
- README or ADR is updated when the operational or architectural behavior changes. The PR lists actual verification, anything unverified and why, and follow-up work. Update backlog status only when evidence supports it.

Run `bash scripts/verify.sh` for the full local check where the required tools are available; confirm the relevant CI checks and describe any unavailable checks in the PR.
