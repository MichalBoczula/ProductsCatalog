# ADR-0008: Protect MobilePhone writes with an optimistic token

- Status: Accepted
- Date: 2026-09-24

## Context

Concurrent updates could overwrite a newer MobilePhone and leave history
describing a state that did not become current. No-op updates or deletes should
not create a misleading new history event.

## Decision

Use the existing `ChangedAt` property as the EF Core concurrency token. A
changed MobilePhone and its history entry are saved in one `SaveChanges`
transaction. A concurrency failure checks whether the row still exists:
physical deletion maps to not found, while an existing changed row maps to a
conflict. Updates with unchanged information return the existing phone without
a write or history. Delete logic also avoids a duplicate event for the agreed
inactive no-op. No new SQL column or migration is introduced for the token.

See the [acceptance matrix](../acceptance-matrix.md) for the operation-specific
HTTP responses and causes. A missing requested Update/Delete retains the
existing validation behavior; it is distinct from deletion during a race.

## Consequences

Stale writes cannot silently replace the current row; history records only
completed changes. Callers must reload after a conflict. The token depends on
the existing timestamp update behavior and needs explicit race and no-op tests.
There is no general idempotency-key protocol or retry of a write transaction.

## Alternatives considered

- Last write wins: permits lost updates and misleading history.
- Add a new version column: unnecessary schema change for this accepted token.
