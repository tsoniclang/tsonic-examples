# Tsumo: TypeScript to Rust

This is a source-only snapshot of the complete Tsumo application translated by
Tsonic from TypeScript to Rust.

## Browse the translation

- [Site entry point: TypeScript](source/packages/engine/src/build-site.ts)
- [Site entry point: Rust](generated/packages/engine/src/build_site.rs)
- [Page model: TypeScript](source/packages/engine/src/models/page-context.ts)
- [Page model: Rust](generated/packages/engine/src/models/page_context.rs)
- [CLI entry point: TypeScript](source/packages/cli/src/cli-main.ts)
- [CLI entry point: Rust](generated/packages/cli/src/cli_main.rs)
- [Tests: TypeScript](source/packages/tests/src)
- [Tests: Rust](generated/packages/tests/src)

`source` contains all authored TypeScript for the engine, CLI, and compiled test
program, together with project configuration and package metadata.
`generated` contains all 225 `.rs` files emitted for those three packages.
Generated package boundaries are retained directly, so consumers no longer
duplicate dependency source inside their own output trees.

## Provenance

Generated and verified on 2026-09-29 from these exact source revisions:

- [Tsumo Rust: `27a281f843f362bd3f216d449bf88b5fdf12af7f`](https://github.com/tsoniclang/tsumo-rust/commit/27a281f843f362bd3f216d449bf88b5fdf12af7f)
- [Tsonic: `fec51270a9b96f38cb83791b3043853962e83728`](https://github.com/tsoniclang/tsonic/commit/fec51270a9b96f38cb83791b3043853962e83728)
- [Rust target: `898548d9d7339389d86c28efb3002e5d158373c6`](https://github.com/tsoniclang/tsonic-rust/commit/898548d9d7339389d86c28efb3002e5d158373c6)
- [Rust Node: `0d29cb009ed734ca4adb9cd6da4acf201bdf99c0`](https://github.com/tsoniclang/rust-nodejs/commit/0d29cb009ed734ca4adb9cd6da4acf201bdf99c0)
- [Rust JS: `dd60d9795f07758c71d1f467045e7b08ae50dfb6`](https://github.com/tsoniclang/rust-js/commit/dd60d9795f07758c71d1f467045e7b08ae50dfb6)
- [Rust runtime: `fcb679ffbc343151a5ef23b52d61ec3cc3fe1021`](https://github.com/tsoniclang/rust-runtime/commit/fcb679ffbc343151a5ef23b52d61ec3cc3fe1021)

All three projects pass deterministic double generation, native formatting,
Cargo builds and Clippy with warnings denied. Verification passes 87 compiled
tests, 29 application/architecture checks, 13 native tests, and exact
release/debug generated-site equivalence across 21 files. The native workspace
lockfile remains unchanged.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 211 | 842,838 | `067097281e023209685a8330b44b7bee5dadfa5fcdbb5a4b5de3f27ed7246fa0` |
| Generated Rust | 225 | 4,083,300 | `cd58a5ff5a09e83507736e485650048e62b6676f989a07036f13d6fe0f45d3b1` |

The manifest hashes sorted lines of `<file SHA-256>  <relative path>\n`.

The snapshot intentionally excludes `node_modules`, Cargo build output,
compiled binaries, runtime packages, temporary files, and generated site
content. Build and runtime dependencies remain owned by the upstream project.
