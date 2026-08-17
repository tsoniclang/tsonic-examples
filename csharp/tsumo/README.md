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
`generated` contains all 553 `.cs` files emitted for those three packages,
including each package's complete generated dependency closure and generated
entry-point/object-shape files.

## Provenance

Generated on 2026-08-17 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo`](https://github.com/tsoniclang/tsumo) | `60a62064191ac8a69d72b4fe7e2132b46d3a0ab8` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `ff734c515bd015a091028da4e723ed47d29c2329` |
| [`tsoniclang/tsonic-csharp`](https://github.com/tsoniclang/tsonic-csharp) | `c5debf8ad1c7bfceb1d405e0d0b3f64ee80be57a` |

The normal Tsumo generation command was run successfully for `engine`, `cli`,
and `tests` immediately before copying the snapshot. All three resulting .NET
projects then compiled with zero warnings and zero errors.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 207 | 828,515 | `bd4ca504d20b03c041d68676b40d6d0447bcbb9084c4ef1048a96526b8dd59c5` |
| Generated C# | 553 | 4,490,751 | `fd7797a34f0e502d8fc9d583e354d2f4b903e7e89347d549ea5702fbd3c5ecbe` |

The snapshot intentionally excludes `node_modules`, .NET build output,
compiled binaries, runtime assemblies, vendored third-party implementation
source, temporary files, and generated site content. Build and runtime
dependencies remain owned by the upstream project.
