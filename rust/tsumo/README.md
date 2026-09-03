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
`generated` contains all 224 `.rs` files emitted for those three packages.
Generated package boundaries are retained directly, so consumers no longer
duplicate dependency source inside their own output trees.

## Provenance

Generated on 2026-09-03 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo-rust`](https://github.com/tsoniclang/tsumo-rust) | `3bd06f7be6e5839f741bbc1fc8f3b54a8234a990` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `7bcfca0770c91c545f6a4abc4dff03f204a4ec36` |
| [`tsoniclang/tsonic-rust`](https://github.com/tsoniclang/tsonic-rust) | `d8ac1c71bee79cd0f7a71f3640fcfedbb6e6d63a` |
| [`tsoniclang/rust-nodejs`](https://github.com/tsoniclang/rust-nodejs) | `25a3e1097f6c41b8e41cd3fd9b6a52f3bc41b66e` |
| [`tsoniclang/rust-js`](https://github.com/tsoniclang/rust-js) | `62b4f3e1a436634d30045dc900bf83ca4f494697` |
| [`tsoniclang/rust-runtime`](https://github.com/tsoniclang/rust-runtime) | `9e6b15493a513ac385e374e04dc652616a845c6b` |

Two consecutive normal Tsumo generation passes succeeded for `engine`, `cli`,
and `tests` and produced byte-identical output. The resulting locked Rust
workspace then compiled successfully with Cargo.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 209 | 822,621 | `5848c6e3275df9c0195edb903b815cbbdc64f583d579378f96fd5703b24832c3` |
| Generated Rust | 224 | 3,818,671 | `7027b36f441cb5492b6ab809c141f2d12be11b4b964dc068d5cec37f250f8d3e` |

The snapshot intentionally excludes `node_modules`, Cargo build output,
compiled binaries, runtime packages, temporary files, and generated site
content. Build and runtime dependencies remain owned by the upstream project.
