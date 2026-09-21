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

Generated on 2026-09-21 with the certified compiler sources now merged to `main`:

| Component | Commit |
|---|---|
| [`tsoniclang/tsumo-rust`](https://github.com/tsoniclang/tsumo-rust) | `59d988e8857f523d77c1d20a90fe498038512257` |
| [`tsoniclang/tsonic`](https://github.com/tsoniclang/tsonic) | `59e96dc9a57440d7c26971719f98c1e00f69c6aa` |
| [`tsoniclang/tsonic-rust`](https://github.com/tsoniclang/tsonic-rust) | `7095b343878a4f7413d0f07621bbecb313bd4ffe` |
| [`tsoniclang/rust-nodejs`](https://github.com/tsoniclang/rust-nodejs) | `e9ff840d4731ba6381bf715ed22757603b48deaf` |
| [`tsoniclang/rust-js`](https://github.com/tsoniclang/rust-js) | `3160d8d3361ed84e28b7f66951814a11d108fc8f` |
| [`tsoniclang/rust-runtime`](https://github.com/tsoniclang/rust-runtime) | `b31a26553f662ac7fc99ccf4cf267af6a73d3c63` |

All three projects passed the September 21 certification: deterministic double
generation, Cargo/Clippy, 83 compiled tests, 28 application tests, 11 native tests
and release/debug generated-site equivalence.

| Tree | Files | Bytes | Sorted relative-path/content manifest SHA-256 |
|---|---:|---:|---|
| Authored source | 210 | 832,161 | `868307d801861977acab6d76759b080fc207ca8c92ad6639b1e80c70ee126b02` |
| Generated Rust | 224 | 3,943,320 | `7f6fdfe89d30909d385f9d7ffc4cbb2bb22b56fcf3d835e6d480b70028ba7f0f` |

The manifest hashes sorted lines of `<file SHA-256>  <relative path>\n`.

The snapshot intentionally excludes `node_modules`, Cargo build output,
compiled binaries, runtime packages, temporary files, and generated site
content. Build and runtime dependencies remain owned by the upstream project.
