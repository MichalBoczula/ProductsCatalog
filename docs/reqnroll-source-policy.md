# Acceptance source and generated code

The 12 Reqnroll `.feature` files and their step definitions are the source of
the acceptance suite. They contain 31 declared scenarios or scenario outlines.
The clean checkout CI run of CLEAN-01 discovered 35 executable cases after
expanding outlines; REF-04 must keep these cases discoverable.
Reqnroll generates `.feature.cs` during build; do not edit or commit these
generated files. The previously tracked 11 generated files have been removed.
The scenario for server errors has no previously checked-in generated file;
the clean checkout build must generate it alongside the others.

Run the acceptance project using the command in `AGENTS.md`. CI restores,
builds and runs the suite from a clean checkout, including the SQL Server
container. Keep this check enabled when changing the feature generation policy
so that a missing scenario cannot silently disappear from discovery.

Infrastructure retains its `System.Security.Cryptography.Xml` pin, updated to
10.0.13. The October 2026 security advisories affect 10.0.12 and its transitive
`System.Security.Cryptography.Pkcs` dependency. The API separately resolves PKCS
through SqlClient, so it directly pins the patched 9.0.21 version instead of
the vulnerable 9.0.4 version. The existing high/critical NuGet audit gate remains
enabled; these package updates let restore satisfy it.

See the Microsoft advisories for [PKCS](https://github.com/advisories/GHSA-846f-f764-76mx)
and XML cryptography: [GHSA-7hqp-7c5m-4vq6](https://github.com/advisories/GHSA-7hqp-7c5m-4vq6),
[GHSA-c399-9853-6jmj](https://github.com/advisories/GHSA-c399-9853-6jmj), and
[GHSA-r955-8q5j-3h83](https://github.com/advisories/GHSA-r955-8q5j-3h83).
