# Tsumo: TypeScript to C#

This is a source-only snapshot of the complete Tsumo application translated by
Tsonic from TypeScript to C#.

## Browse the translation

| Program area | Authored TypeScript | Generated C# |
|---|---|---|
| Site entry point | [`source/packages/engine/src/build-site.ts`](source/packages/engine/src/build-site.ts) | [`generated/packages/engine/src/BuildSite.cs`](generated/packages/engine/src/BuildSite.cs) |
| Page model | [`source/packages/engine/src/models/page-context.ts`](source/packages/engine/src/models/page-context.ts) | [`generated/packages/engine/src/models/Models_pageContext.cs`](generated/packages/engine/src/models/Models_pageContext.cs) |
| CLI entry point | [`source/packages/cli/src/cli-main.ts`](source/packages/cli/src/cli-main.ts) | [`generated/packages/cli/src/CliMain.cs`](generated/packages/cli/src/CliMain.cs) |
| Tests | [`source/packages/tests/src`](source/packages/tests/src) | [`generated/packages/tests/src`](generated/packages/tests/src) |

`source` contains all authored TypeScript for the engine, CLI, and compiled test
program, together with each package's `tsonic.json` and `package.json`.
`generated` contains all 556 `.cs` files emitted for those three packages,
including each package's complete generated dependency closure and generated
entry-point/object-shape files.

## Provenance

Generated on 2026-08-24 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo`](https://github.com/tsoniclang/tsumo) | `ba9b3656de7a745646ff8cd310bc90e158ca051a` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `64a29e55093c851b10dbe25e6a72a2e68e2903fd` |
| [`tsoniclang/tsonic-csharp`](https://github.com/tsoniclang/tsonic-csharp) | `e8ecc5c6754e0058ca1f559d532b74cb554ed3e8` |
| [`tsoniclang/csharp-nodejs`](https://github.com/tsoniclang/csharp-nodejs) | `fa02a450085841d6e5c6e35452bbafd57c17f348` |
| [`tsoniclang/csharp-js`](https://github.com/tsoniclang/csharp-js) | `c0d8eda0b18aa476b211c0bc6c9e4915824b7eb4` |
| [`tsoniclang/csharp-runtime`](https://github.com/tsoniclang/csharp-runtime) | `166bdf1734888dc5d7dc9532a44b77894b2591b7` |

Two consecutive normal Tsumo generation passes succeeded for `engine`, `cli`,
and `tests` and produced byte-identical output. All three resulting .NET
projects then compiled with zero warnings and zero errors.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 208 | 828,996 | `447491993c8c6bdbff115331b3493d590250cf79ff1cb3773692ad3511122625` |
| Generated C# | 556 | 4,509,283 | `563a8113e86196c58406750802d09f2051e3fdc5ec19d4166bfd21da5134987f` |

The snapshot intentionally excludes package installations, .NET build output,
compiled binaries, runtime assemblies, vendored third-party implementation
source, temporary files, and generated site content. Logical `node_modules`
paths emitted as dependency source remain part of the generated snapshot.
Build and runtime dependencies remain owned by the upstream project.
