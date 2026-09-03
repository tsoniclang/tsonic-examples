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

Generated on 2026-09-03 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo`](https://github.com/tsoniclang/tsumo) | `8473b9f44575ba60e61fa4fd6fff71df5e9d5203` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `7bcfca0770c91c545f6a4abc4dff03f204a4ec36` |
| [`tsoniclang/tsonic-csharp`](https://github.com/tsoniclang/tsonic-csharp) | `e1d652aefbffef848d1063587170229ac0712791` |
| [`tsoniclang/csharp-nodejs`](https://github.com/tsoniclang/csharp-nodejs) | `9ace32b396cbb03bf31c8073edfc7010ccf90614` |
| [`tsoniclang/csharp-js`](https://github.com/tsoniclang/csharp-js) | `d61f31da03904e4c3be9bca53ede85d26ec8ada7` |
| [`tsoniclang/csharp-runtime`](https://github.com/tsoniclang/csharp-runtime) | `8ae04c5e727f8b8a626d4c2fc8a976b8c60c943a` |

Two consecutive normal Tsumo generation passes succeeded for `engine`, `cli`,
and `tests` and produced byte-identical output. All three resulting .NET
projects then compiled with zero warnings and zero errors.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 209 | 829,215 | `13e53cad359cddd48b558424319b8c5feb38bf72bacdd5b51d128ce042d7fe49` |
| Generated C# | 556 | 4,509,865 | `d38e2203229f38f31e2c9fcccaddf4b9198cc239bfadff04a23abbe83d1da00a` |

The snapshot intentionally excludes package installations, .NET build output,
compiled binaries, runtime assemblies, vendored third-party implementation
source, temporary files, and generated site content. Logical `node_modules`
paths emitted as dependency source remain part of the generated snapshot.
Build and runtime dependencies remain owned by the upstream project.
