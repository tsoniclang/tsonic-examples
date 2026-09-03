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
| [`tsoniclang/tsumo-rust`](https://github.com/tsoniclang/tsumo-rust) | `5e4bc235df7f9b985c638e9ac23fd69abf432e57` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `762ecb7e9c1de1152bac34c07ecf5b424182c684` |
| [`tsoniclang/tsonic-rust`](https://github.com/tsoniclang/tsonic-rust) | `422e32f927df18121b8e464c66956d06f64a1ffa` |
| [`tsoniclang/rust-nodejs`](https://github.com/tsoniclang/rust-nodejs) | `4afba946d30f4359b43edaa384072bb21cf87894` |
| [`tsoniclang/rust-js`](https://github.com/tsoniclang/rust-js) | `eb72739c8845b8e9e0ec5d6e061dd4d063857092` |
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
