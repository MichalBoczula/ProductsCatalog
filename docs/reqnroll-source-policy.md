# Acceptance source and generated code

The 12 Reqnroll `.feature` files and their step definitions are the source of
the acceptance suite. They contain 31 declared scenarios or scenario outlines.
Reqnroll generates `.feature.cs` during build; do not edit or commit these
generated files. The previously tracked 11 generated files have been removed.
The scenario for server errors has no previously checked-in generated file;
the clean checkout build must generate it alongside the others.

Run the acceptance project using the command in `AGENTS.md`. CI restores,
builds and runs the suite from a clean checkout, including the SQL Server
container. Keep this check enabled when changing the feature generation policy
so that a missing scenario cannot silently disappear from discovery.

Infrastructure still pins `System.Security.Cryptography.Xml` 10.0.12. There
is no direct source import, but that alone cannot establish whether it pins a
transitive dependency needed for security. REF-04 retains the pin and does not
alter the NuGet audit gate; a future package graph/security review can justify
an upgrade or removal.
