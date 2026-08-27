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

Generated on 2026-08-27 from clean checkouts:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo-rust`](https://github.com/tsoniclang/tsumo-rust) | `7fd1567c9ccedb4f916a042a2522a2ae3e061a9c` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `64a29e55093c851b10dbe25e6a72a2e68e2903fd` |
| [`tsoniclang/tsonic-rust`](https://github.com/tsoniclang/tsonic-rust) | `8b0fbd2a1d8fb8c6d41939fe0eb4670e7db95813` |
| [`tsoniclang/rust-nodejs`](https://github.com/tsoniclang/rust-nodejs) | `7c7c5fd2cae446e331776a1db66a4611350df995` |
| [`tsoniclang/rust-js`](https://github.com/tsoniclang/rust-js) | `ea11f81c57d747fbb626a90352c0e0e0dc75427a` |
| [`tsoniclang/rust-runtime`](https://github.com/tsoniclang/rust-runtime) | `ce62bff50f3f4bef52976a020ba9b1771574b2a5` |

Two consecutive normal Tsumo generation passes succeeded for `engine`, `cli`,
and `tests` and produced byte-identical output. The resulting locked Rust
workspace then compiled successfully with Cargo.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 209 | 822,621 | `5848c6e3275df9c0195edb903b815cbbdc64f583d579378f96fd5703b24832c3` |
| Generated Rust | 224 | 4,702,144 | `92ce86e854eefdb095e78c217f1c6e217e4b280ebd6c4e6d2c949fe45bf611b8` |

The snapshot intentionally excludes `node_modules`, Cargo build output,
compiled binaries, runtime packages, temporary files, and generated site
content. Build and runtime dependencies remain owned by the upstream project.
