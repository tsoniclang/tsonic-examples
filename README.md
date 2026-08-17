# Tsonic Examples

Complete source-to-source examples for Tsonic targets.

## Tsumo

Tsumo is a real static-site generator authored in TypeScript. These snapshots
place the authored program beside every source file emitted by Tsonic:

| Target | Authored TypeScript | Generated target source |
|---|---|---|
| Rust | [`rust/tsumo/source`](rust/tsumo/source) | [`rust/tsumo/generated`](rust/tsumo/generated) |
| C# | [`csharp/tsumo/source`](csharp/tsumo/source) | [`csharp/tsumo/generated`](csharp/tsumo/generated) |

Each target directory records the exact source and compiler revisions used to
produce it. The repository contains no compiled binaries, package installs,
runtime assemblies, Cargo target directories, or .NET build directories.
