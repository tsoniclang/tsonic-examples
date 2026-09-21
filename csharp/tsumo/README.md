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

Generated on 2026-09-21 with the certified compiler sources now merged to `main`:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo-csharp`](https://github.com/tsoniclang/tsumo-csharp) | `6478a1f2f76ee78e9ce750e71e01308875696716` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `59e96dc9a57440d7c26971719f98c1e00f69c6aa` |
| [`tsoniclang/tsonic-csharp`](https://github.com/tsoniclang/tsonic-csharp) | `687b0f7116243a31d657720e02ef031c93cfc3df` |
| [`tsoniclang/csharp-nodejs`](https://github.com/tsoniclang/csharp-nodejs) | `4c61b86b067472b4824a5b6ac6691736a7ea0977` |
| [`tsoniclang/csharp-js`](https://github.com/tsoniclang/csharp-js) | `e09d29ca6e8e0620426db543d0bee8067cea362d` |
| [`tsoniclang/csharp-runtime`](https://github.com/tsoniclang/csharp-runtime) | `e8443be75fc0d35ec945c3f80e8f8402f5d73885` |

All three projects passed the September 21 certification: 71 compiled tests,
25 application tests, NativeAOT publication and generated-site equivalence.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 208 | 829,662 | `3a09758d8ef37fa00e868bba2346f353a9e0be0cf92b4b244747c9d04a548263` |
| Generated C# | 556 | 4,234,826 | `c0ac83c3ea09abcab4d3d6ceba6f501938d078be0c90a598fd06766bab275d00` |

The manifest hashes sorted lines of `<file SHA-256>  <relative path>\n`.

The snapshot intentionally excludes package installations, .NET build output,
compiled binaries, runtime assemblies, vendored third-party implementation
source, temporary files, and generated site content. Logical `node_modules`
paths emitted as dependency source remain part of the generated snapshot.
Build and runtime dependencies remain owned by the upstream project.
