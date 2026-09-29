# Tsumo: TypeScript to C#

This is a source-only snapshot of the complete Tsumo application translated by
Tsonic from TypeScript to C#.

## Browse the translation

- [Site entry point: TypeScript](source/packages/engine/src/build-site.ts)
- [Site entry point: C#](generated/packages/engine/src/BuildSite.cs)
- [Page model: TypeScript](source/packages/engine/src/models/page-context.ts)
- [Page model: C#](generated/packages/engine/src/models/Models_pageContext.cs)
- [CLI entry point: TypeScript](source/packages/cli/src/cli-main.ts)
- [CLI entry point: C#](generated/packages/cli/src/CliMain.cs)
- [Tests: TypeScript](source/packages/tests/src)
- [Tests: C#](generated/packages/tests/src)

`source` contains all authored TypeScript for the engine, CLI, and compiled test
program, together with project configuration and package metadata.
`generated` contains all 559 `.cs` files emitted for those three packages,
including each package's complete generated dependency closure and generated
entry-point/object-shape files.

## Provenance

Generated and verified on 2026-09-29 from these exact source revisions:

- [Tsumo C#: `71ecd99519dbfa48f29dff0466e515b3a2974e91`](https://github.com/tsoniclang/tsumo-csharp/commit/71ecd99519dbfa48f29dff0466e515b3a2974e91)
- [Tsonic: `fec51270a9b96f38cb83791b3043853962e83728`](https://github.com/tsoniclang/tsonic/commit/fec51270a9b96f38cb83791b3043853962e83728)
- [C# target: `83f59ad36a279f6753ac18209670c540f607b57b`](https://github.com/tsoniclang/tsonic-csharp/commit/83f59ad36a279f6753ac18209670c540f607b57b)
- [C# Node: `47f6fd92ee9c6b251d9f0fb93c5bc21f6ed5c35a`](https://github.com/tsoniclang/csharp-nodejs/commit/47f6fd92ee9c6b251d9f0fb93c5bc21f6ed5c35a)
- [C# JS: `7ef1963680b3b39cc7c0599f1a91a6e74f24c313`](https://github.com/tsoniclang/csharp-js/commit/7ef1963680b3b39cc7c0599f1a91a6e74f24c313)
- [C# runtime: `8cca2c508d65dcc126d40b658817e9f99c77a88d`](https://github.com/tsoniclang/csharp-runtime/commit/8cca2c508d65dcc126d40b658817e9f99c77a88d)

All three projects generated and built successfully. Verification passes
74 compiled tests, 26 application/architecture checks, NativeAOT publication,
and exact generated-site equivalence across 21 files. CLI verification uses
the installed .NET runtime through `DOTNET_ROOT`; the NativeAOT executable
runs independently. The vendored Markdig build reports its existing CA2265
warning; no test assertion or warning policy was weakened.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 210 | 836,566 | `82e7a81f9780a08db550db7594fbb4520e474a24b60ec4ed230adbc9670bd7d2` |
| Generated C# | 559 | 3,948,899 | `6b59421624793e2b0f00e0fc5fd8faa933c96cfc500df3f54edd5134386cfe8a` |

The manifest hashes sorted lines of `<file SHA-256>  <relative path>\n`.

The snapshot intentionally excludes package installations, .NET build output,
compiled binaries, runtime assemblies, vendored third-party implementation
source, temporary files, and generated site content. Logical `node_modules`
paths emitted as dependency source remain part of the generated snapshot.
Build and runtime dependencies remain owned by the upstream project.
