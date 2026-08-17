# Tsumo: TypeScript to Rust

This is a source-only snapshot of the complete Tsumo application translated by
Tsonic from TypeScript to Rust.

## Browse the translation

| Program area | Authored TypeScript | Generated Rust |
|---|---|---|
| Site entry point | [`source/packages/engine/src/build-site.ts`](source/packages/engine/src/build-site.ts) | [`generated/packages/engine/src/build_site.rs`](generated/packages/engine/src/build_site.rs) |
| Page model | [`source/packages/engine/src/models/page-context.ts`](source/packages/engine/src/models/page-context.ts) | [`generated/packages/engine/src/models/page_context.rs`](generated/packages/engine/src/models/page_context.rs) |
| CLI entry point | [`source/packages/cli/src/cli-main.ts`](source/packages/cli/src/cli-main.ts) | [`generated/packages/cli/src/cli_main.rs`](generated/packages/cli/src/cli_main.rs) |
| Tests | [`source/packages/tests/src`](source/packages/tests/src) | [`generated/packages/tests/src`](generated/packages/tests/src) |

`source` contains all authored TypeScript for the engine, CLI, and compiled test
program, together with each package's `tsonic.json` and `package.json`.
`generated` contains all 593 `.rs` files emitted for those three packages,
including each package's complete generated dependency closure.

## Provenance

Generated on 2026-08-17 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo-rust`](https://github.com/tsoniclang/tsumo-rust) | `6f73395e6901c6a0b34d9931fde61f5a6fcbb042` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `ff734c515bd015a091028da4e723ed47d29c2329` |
| [`tsoniclang/tsonic-rust`](https://github.com/tsoniclang/tsonic-rust) | `e889ef684138c60d1ef35cbfbe81eaf7828678aa` |

The normal Tsumo generation command was run successfully for `engine`, `cli`,
and `tests` immediately before copying the snapshot. The resulting locked Rust
workspace then compiled successfully with Cargo.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 208 | 811,001 | `99c6cfb4073db535ca3aa618ac7cea0f32f0e0783de54036cb71ddc8fbaaf2ee` |
| Generated Rust | 593 | 14,151,109 | `40a2f1c78436df8d58f17b735c35fbf38fdf15eedb044a2d5f3c52569cd790e5` |

The snapshot intentionally excludes `node_modules`, Cargo build output,
compiled binaries, runtime packages, temporary files, and generated site
content. Build and runtime dependencies remain owned by the upstream project.
