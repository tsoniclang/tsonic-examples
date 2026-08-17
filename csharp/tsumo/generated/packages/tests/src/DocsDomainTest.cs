using System;

namespace Tsumo.Tests
{
    public static class DocsDomainTest
    {
        public static Func<Action, string> captureDocsDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        public static Func<string, string, DocsMountConfig> createMount
        {
            get;
            private set;
        } = default(Func<string, string, DocsMountConfig>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureDocsDiagnostic = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception error)
                {
                    if (error is TsumoError)
                    {
                        return ((TsumoError)error).diagnostic.code;
                    }
                    throw;
                }
                throw new System.Exception("Expected a docs diagnostic");
            };
            createMount = (string sourceDir, string prefix) => new DocsMountConfig("Docs", sourceDir, prefix, null, "main", null, null);
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DocsDomainTests
    {
        [Xunit.FactAttribute]
        public void route_discovery_is_sorted_and_rejects_output_collisions()
        {
            string root = TestRoot.createTestDirectory("docs-routes");
            try
            {
                string source = System.IO.Path.Combine(root, "source");
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(source, "nested"));
                System.IO.File.WriteAllText(System.IO.Path.Combine(source, "z.md"), "# Z");
                System.IO.File.WriteAllText(System.IO.Path.Combine(source, "a.md"), "# A");
                System.IO.File.WriteAllText(System.IO.Path.Combine(source, "nested", "asset.txt"), "asset");
                DocsMountRoutes routes = Node_modules_Tsumo_engine_src_docs_routes.discoverDocsMountRoutes(DocsDomainTest.createMount(source, "/docs/"));
                Xunit.Assert.Equal<double>(2, routes.markdown.length);
                Xunit.Assert.True(routes.markdown[0].relPath == "a.md");
                Xunit.Assert.True(routes.markdown[1].relPath == "z.md");
                Xunit.Assert.Equal<double>(1, routes.assets.length);
                Xunit.Assert.True(routes.assets[0].outputRelPath == "docs/nested/asset.txt");
                string conflicting = System.IO.Path.Combine(root, "conflicting");
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(conflicting, "guide"));
                System.IO.File.WriteAllText(System.IO.Path.Combine(conflicting, "guide.md"), "# Guide");
                System.IO.File.WriteAllText(System.IO.Path.Combine(conflicting, "guide", "index.md"), "# Other guide");
                Xunit.Assert.Equal("TSUMO_DOCS_ROUTE_CONFLICT", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_docs_routes.discoverDocsMountRoutes(DocsDomainTest.createMount(conflicting, "/docs/"));
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void content_inventory_excludes_draft_leaf_routes()
        {
            string root = TestRoot.createTestDirectory("docs-content");
            try
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(root, "published.md"), "---\ntitle: Published\n---\nBody");
                System.IO.File.WriteAllText(System.IO.Path.Combine(root, "draft.md"), "---\ntitle: Draft\ndraft: true\n---\nHidden");
                Tsonic.CSharp.Js.JSArray<DocsMarkdownRoute> routes = Node_modules_Tsumo_engine_src_docs_routes.discoverDocsMountRoutes(DocsDomainTest.createMount(root, "/docs/")).markdown;
                DocsContentInventory production = Node_modules_Tsumo_engine_src_docs_content.loadDocsContent(routes, false);
                Xunit.Assert.Equal<double>(1, production.leaves.length);
                Xunit.Assert.True(production.permalinkByRelativePath.has("published.md"));
                Xunit.Assert.True(!production.permalinkByRelativePath.has("draft.md"));
                DocsContentInventory withDrafts = Node_modules_Tsumo_engine_src_docs_content.loadDocsContent(routes, true);
                Xunit.Assert.Equal<double>(2, withDrafts.leaves.length);
                Xunit.Assert.True(withDrafts.permalinkByRelativePath.has("draft.md"));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void docs_config_has_one_closed_schema()
        {
            string root = TestRoot.createTestDirectory("docs-config");
            try
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(root, "content"));
                string configPath = System.IO.Path.Combine(root, "tsumo.docs.json");
                System.IO.File.WriteAllText(configPath, "{\"siteName\":\"Contract\",\"mounts\":[{\"name\":\"Main\",\"source\":\"./content\",\"prefix\":\"/docs/\"}]}");
                LoadedDocsConfig? loaded = Node_modules_Tsumo_engine_src_docs_config.loadDocsConfig(root);
                Xunit.Assert.True(loaded is not null && loaded.config.mounts.length == 1);
                Xunit.Assert.True(loaded is not null && loaded.config.mounts[0].urlPrefix == "/docs/");
                System.IO.File.WriteAllText(configPath, "{\"mounts\":[{\"source\":\"./content\",\"prefix\":\"/docs/\",\"repo\":\"https://example.invalid\"}]}");
                Xunit.Assert.Equal("TSUMO_DOCS_CONFIG_UNKNOWN_PROPERTY", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_docs_config.loadDocsConfig(root);
                }));
                System.IO.File.WriteAllText(configPath, "{\"search\":\"yes\",\"mounts\":[{\"source\":\"./content\",\"prefix\":\"/docs/\"}]}");
                Xunit.Assert.Equal("TSUMO_DOCS_CONFIG_TYPE", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_docs_config.loadDocsConfig(root);
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void output_and_search_plans_are_exact_and_deterministic()
        {
            string root = TestRoot.createTestDirectory("docs-output");
            try
            {
                Xunit.Assert.Equal("guide/index.html", Node_modules_Tsumo_engine_src_docs_output.docsOutputPathForPermalink("/guide/"));
                Xunit.Assert.Equal("TSUMO_DOCS_OUTPUT_PATH_ESCAPES_ROOT", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_docs_output.resolveDocsOutputPath(root, "../outside.html");
                }));
                Xunit.Assert.Equal("TSUMO_DOCS_OUTPUT_PATH_ABSOLUTE", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_docs_output.resolveDocsOutputPath(root, "/outside.html");
                }));
                DocsOutputClaims claims = new DocsOutputClaims();
                claims.add("docs/index.html", "first.md");
                Xunit.Assert.Equal("TSUMO_DOCS_ROUTE_CONFLICT", DocsDomainTest.captureDocsDiagnostic(() =>
                {
                    claims.add("DOCS/index.html", "second.md");
                }));
                Tsonic.CSharp.Js.JSArray<SearchDocument> documents = new Tsonic.CSharp.Js.JSArray<SearchDocument>(new SearchDocument[] { new SearchDocument("Zulu", "/z/", "Docs", "last"), new SearchDocument("Alpha", "/a/", "Docs", "quoted \"value\"") });
                string expected = "[{\"title\":\"Alpha\",\"url\":\"/a/\",\"mount\":\"Docs\",\"text\":\"quoted \\\"value\\\"\"},{\"title\":\"Zulu\",\"url\":\"/z/\",\"mount\":\"Docs\",\"text\":\"last\"}]";
                Xunit.Assert.Equal(expected, Node_modules_Tsumo_engine_src_docs_searchIndex.renderSearchIndexJson(documents));
                Xunit.Assert.Equal(expected, Node_modules_Tsumo_engine_src_docs_searchIndex.renderSearchIndexJson(documents));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void strict_markdown_links_fail_closed()
        {
            DocsMountConfig mount = DocsDomainTest.createMount("/docs", "/docs/");
            Tsonic.CSharp.Js.Map<string, string> routes = new Tsonic.CSharp.Js.Map<string, string>();
            routes.set("known.md", "/docs/known/");
            DocsLinkRewriteContext context = new DocsLinkRewriteContext(mount, "/docs/current.md", "", routes, true);
            MarkdownResult rendered = Node_modules_Tsumo_engine_src_docs_markdown.renderDocsMarkdown("[Known](known.md)", context);
            Xunit.Assert.True(Tsonic.CSharp.Js.String.includes(rendered.html, "/docs/known/"));
            Xunit.Assert.Equal("TSUMO_DOCS_LINK_UNRESOLVED", DocsDomainTest.captureDocsDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_docs_markdown.renderDocsMarkdown("[Missing](missing.md)", context);
            }));
            Xunit.Assert.Equal("TSUMO_DOCS_LINK_UNSAFE", DocsDomainTest.captureDocsDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_docs_markdown.renderDocsMarkdown("[Unsafe](javascript:alert(1))", context);
            }));
        }
    }
}
