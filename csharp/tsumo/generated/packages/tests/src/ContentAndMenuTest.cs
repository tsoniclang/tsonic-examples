using System;

namespace Tsumo.Tests
{
    public static class ContentAndMenuTest
    {
        public static Func<Action, string> captureContentDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        public static Func<string, string, int, string, MenuEntry> createMenuEntry
        {
            get;
            private set;
        } = default(Func<string, string, int, string, MenuEntry>)!;
        public static Func<SiteContext, string, string, PageContext> createPage
        {
            get;
            private set;
        } = default(Func<SiteContext, string, string, PageContext>)!;
        public static Func<string, PageContext, ContentPageSource> createSource
        {
            get;
            private set;
        } = default(Func<string, PageContext, ContentPageSource>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureContentDiagnostic = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception __tsonic_catch0)
                {
                    Tsonic.CSharp.Runtime.TsValue error = Tsonic.CSharp.Runtime.TsThrownValueException.toValue(__tsonic_catch0);
                    if (Tsonic.CSharp.Runtime.TsValue.IsDynamicInstanceOf<TsumoError>(error))
                    {
                        return Tsonic.CSharp.Runtime.TsValue.CastDynamic<TsumoError>(error).diagnostic.code;
                    }
                    throw;
                }
                throw new Tsonic.CSharp.Runtime.Error("Expected a content or menu diagnostic");
            };
            createMenuEntry = (string identity, string parent, int weight, string pageRef) => new MenuEntry(identity, "", pageRef, "", weight, parent, identity, "", "", "main");
            createPage = (SiteContext site, string route, string slug) =>
            {
                HtmlString emptyHtml = new HtmlString("");
                Tsonic.CSharp.Js.JSArray<PageContext> emptyPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<string> emptyStrings = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                return new PageContext(slug, "2026-01-01T00:00:00.000Z", "2026-01-01T00:00:00.000Z", false, "page", "articles", "articles", slug, route, "", emptyHtml, emptyHtml, emptyHtml, "", emptyStrings, emptyStrings, site.Params, null, site.Language, emptyPages, null, site, emptyPages, null, emptyPages, null);
            };
            createSource = (string sourcePath, PageContext page) =>
            {
                Tsonic.CSharp.Js.JSArray<FrontMatterMenu> emptyMenus = new Tsonic.CSharp.Js.JSArray<FrontMatterMenu>(new FrontMatterMenu[] { });
                return new ContentPageSource(sourcePath, page.section, page.type, page.slug, page.title, new Tsonic.CSharp.Js.Date("2026-01-01T00:00:00.000Z"), page.date, page.lastmod, false, false, "", page.tags, page.categories, page.Params, "", page.relPermalink, "articles/post/index.html", null, new PageFile(sourcePath, "articles/", page.slug), emptyMenus);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ContentAndMenuTests
    {
        [Xunit.FactAttribute]
        public void content_discovery_is_deterministic_and_excludes_drafts_before_claiming_routes()
        {
            string root = TestRoot.createTestDirectory("content-discovery");
            try
            {
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "z.md"), """
                ---
                title: Z
                date: 2026-01-01T00:00:00Z
                ---
                Z
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "a.md"), """
                ---
                title: A
                date: 2026-01-01T00:00:00Z
                ---
                A
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "published.md"), """
                ---
                title: Published
                date: 2025-01-01T00:00:00Z
                slug: shared
                ---
                Published
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "draft.md"), """
                ---
                title: Draft
                date: 2025-01-01T00:00:00Z
                slug: shared
                draft: true
                ---
                Draft
                """);
                ContentInventory production = Node_modules_Tsumo_engine_src_build_discoverContent.discoverContent(root, false);
                Xunit.Assert.Equal<double>(3, production.pages.length);
                Xunit.Assert.True(production.pages[0].relPermalink == "/a/");
                Xunit.Assert.True(production.pages[1].relPermalink == "/z/");
                Xunit.Assert.True(production.pages[2].relPermalink == "/shared/");
                Xunit.Assert.Equal("TSUMO_CONTENT_ROUTE_CONFLICT", ContentAndMenuTest.captureContentDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_build_discoverContent.discoverContent(root, true);
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void content_routes_reject_escape_segments_and_duplicate_outputs()
        {
            string escapeRoot = TestRoot.createTestDirectory("content-route-escape");
            string conflictRoot = TestRoot.createTestDirectory("content-route-conflict");
            try
            {
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(escapeRoot, "bad.md"), """
                ---
                title: Bad
                slug: ../outside
                ---
                Bad
                """);
                Xunit.Assert.Equal("TSUMO_CONTENT_ROUTE_SEGMENT_INVALID", ContentAndMenuTest.captureContentDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_build_discoverContent.discoverContent(escapeRoot, false);
                }));
                TestRoot.createDirectory(Tsonic.CSharp.Node.path.join(conflictRoot, "guide"));
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(conflictRoot, "guide.md"), """
                ---
                title: Guide
                ---
                Page
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(conflictRoot, "guide", "_index.md"), """
                ---
                title: Guide index
                ---
                List
                """);
                Xunit.Assert.Equal("TSUMO_CONTENT_ROUTE_CONFLICT", ContentAndMenuTest.captureContentDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_build_discoverContent.discoverContent(conflictRoot, false);
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(conflictRoot);
                TestRoot.deleteTestDirectory(escapeRoot);
            }
        }
        [Xunit.FactAttribute]
        public void menu_hierarchy_is_deterministic_and_fails_closed()
        {
            Tsonic.CSharp.Js.JSArray<MenuEntry> hierarchy = Node_modules_Tsumo_engine_src_menus.buildMenuHierarchy(new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { ContentAndMenuTest.createMenuEntry("beta", "", 0, ""), ContentAndMenuTest.createMenuEntry("child", "alpha", 0, ""), ContentAndMenuTest.createMenuEntry("alpha", "", 0, "") }));
            Xunit.Assert.Equal<double>(2, hierarchy.length);
            Xunit.Assert.True(hierarchy[0].identifier == "alpha");
            Xunit.Assert.True(hierarchy[0].children[0].identifier == "child");
            Xunit.Assert.True(hierarchy[1].identifier == "beta");
            Xunit.Assert.Equal("TSUMO_MENU_IDENTITY_DUPLICATE", ContentAndMenuTest.captureContentDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_menus.buildMenuHierarchy(new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { ContentAndMenuTest.createMenuEntry("same", "", 0, ""), ContentAndMenuTest.createMenuEntry("same", "", 1, "") }));
            }));
            Xunit.Assert.Equal("TSUMO_MENU_PARENT_NOT_FOUND", ContentAndMenuTest.captureContentDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_menus.buildMenuHierarchy(new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { ContentAndMenuTest.createMenuEntry("child", "missing", 0, "") }));
            }));
            Xunit.Assert.Equal("TSUMO_MENU_PARENT_CYCLE", ContentAndMenuTest.captureContentDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_menus.buildMenuHierarchy(new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { ContentAndMenuTest.createMenuEntry("one", "two", 0, ""), ContentAndMenuTest.createMenuEntry("two", "one", 0, "") }));
            }));
        }
        [Xunit.FactAttribute]
        public void menu_page_references_use_exact_routes_without_slug_fallback()
        {
            SiteConfig config = new SiteConfig("Test", "https://example.invalid/", "en", null, null);
            SiteContext site = new SiteContext(config, new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { }), null, null);
            PageContext page = ContentAndMenuTest.createPage(site, "/articles/post/", "post");
            ContentPageSource source = ContentAndMenuTest.createSource("/content/articles/post.md", page);
            Tsonic.CSharp.Js.JSArray<ContentPageSource> sources = new Tsonic.CSharp.Js.JSArray<ContentPageSource>(new ContentPageSource[] { source });
            Tsonic.CSharp.Js.JSArray<PageContext> pages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { page });
            site.pages = pages;
            MenuEntry exact = ContentAndMenuTest.createMenuEntry("exact", "", 0, "/articles/post/");
            site.Menus.set("main", new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { exact }));
            Node_modules_Tsumo_engine_src_build_menuResolution.configureSiteMenus(sources, pages, site);
            PageContext? resolvedPage = exact.page;
            Xunit.Assert.True(resolvedPage is not null);
            if (resolvedPage is null)
            {
                throw new Tsonic.CSharp.Runtime.Error("Expected exact menu page resolution");
            }
            Xunit.Assert.Equal("/articles/post/", resolvedPage.relPermalink);
            site.Menus.set("main", new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { ContentAndMenuTest.createMenuEntry("shorthand", "", 0, "post") }));
            Xunit.Assert.Equal("TSUMO_MENU_PAGE_REF_NOT_FOUND", ContentAndMenuTest.captureContentDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_build_menuResolution.configureSiteMenus(sources, pages, site);
            }));
        }
        [Xunit.FactAttribute]
        public void page_graph_finalizes_home_ancestry_and_taxonomies_before_rendering()
        {
            string root = TestRoot.createTestDirectory("standard-page-graph");
            try
            {
                TestRoot.createDirectory(Tsonic.CSharp.Node.path.join(root, "posts", "series"));
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "posts", "_index.md"), """
                ---
                title: Posts
                ---
                Posts
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "posts", "series", "_index.md"), """
                ---
                title: Series
                ---
                Series
                """);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(root, "posts", "series", "part.md"), """
                ---
                title: Part
                date: 2026-01-01T00:00:00Z
                tags: [alpha]
                categories: [guides]
                ---
                Part
                """);
                SiteConfig config = new SiteConfig("Test", "https://example.invalid/", "en", null, null);
                StandardPageGraph graph = Node_modules_Tsumo_engine_src_build_standardPageGraph.createStandardPageGraph(config, Node_modules_Tsumo_engine_src_build_discoverContent.discoverContent(root, false));
                StandardTaxonomyGraph taxonomies = Node_modules_Tsumo_engine_src_build_standardTaxonomies.createStandardTaxonomies(graph);
                PageContext page = graph.contentPages[0];
                PageContext? parent = page.parent;
                Xunit.Assert.True(parent is not null);
                if (parent is null)
                {
                    throw new Tsonic.CSharp.Runtime.Error("Expected page parent");
                }
                Xunit.Assert.Equal("/posts/series/", parent.relPermalink);
                Xunit.Assert.Equal<double>(3, page.ancestors.length);
                Xunit.Assert.Equal("/", page.ancestors[0].relPermalink);
                Xunit.Assert.Equal("/posts/", page.ancestors[1].relPermalink);
                Xunit.Assert.Equal("/posts/series/", page.ancestors[2].relPermalink);
                PageContext? home = graph.site.home;
                Xunit.Assert.True(home is not null);
                if (home is null)
                {
                    throw new Tsonic.CSharp.Runtime.Error("Expected site home");
                }
                Xunit.Assert.Equal("/", home.relPermalink);
                Xunit.Assert.Equal<double>(1, home.pages.length);
                Xunit.Assert.Equal<double>(2, taxonomies.taxonomies.length);
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>? tags = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>>(graph.site.Taxonomies, "tags");
                Xunit.Assert.True(tags is not null);
                if (tags is null)
                {
                    throw new Tsonic.CSharp.Runtime.Error("Expected tags taxonomy");
                }
                Tsonic.CSharp.Js.JSArray<PageContext>? tagPages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(tags, "alpha");
                Xunit.Assert.True(tagPages is not null);
                Xunit.Assert.Equal<double>(8, graph.site.allPages.length);
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
