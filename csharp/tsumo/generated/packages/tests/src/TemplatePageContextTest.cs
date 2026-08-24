using System;

namespace Tsumo.Tests
{
    public static class TemplatePageContextTest
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            TemplateTestHarness.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class TemplatePageContextTests
    {
        [Xunit.FactAttribute]
        public void date_page_data_and_render_methods_use_typed_context()
        {
            Xunit.Assert.Equal("2024-01-02", TemplateTestHarness.renderWithRoot("{{ .Format \"2006-01-02\" }}", new DateValue("2024-01-02T03:04:05Z")));
            SiteContext site = TemplateTestHarness.createSite();
            PageContext older = TemplateTestHarness.createPage(site, "Older", "2022-04-01T00:00:00Z", "page");
            PageContext newer = TemplateTestHarness.createPage(site, "Newer", "2024-06-01T00:00:00Z", "page");
            older.Params.set("weight", ParamValue.number(20));
            newer.Params.set("weight", ParamValue.number(10));
            PageContext root = TemplateTestHarness.createPage(site, "Home", "", "home");
            root.pages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { older, newer });
            PageContext section = TemplateTestHarness.createPage(site, "Section", "", "section");
            root.pages.push(section);
            site.pages = root.pages;
            site.allPages = root.pages;
            Xunit.Assert.Equal("value", TemplateTestHarness.renderWithRoot("{{ .Scratch.Set \"key\" \"value\" }}{{ .Scratch.Get \"key\" }}", new PageValue(root)));
            Xunit.Assert.Equal("2024:Newer;2022:Older;", TemplateTestHarness.renderWithRoot("{{ range .Data.Pages.GroupByDate \"2006\" }}{{ .Key }}:{{ range .Pages }}{{ .Title }}{{ end }};{{ end }}", new PageValue(root)));
            Xunit.Assert.Equal("0:Section;10:Newer;20:Older;|20:Older;10:Newer;0:Section;|SectionNewerOlder", TemplateTestHarness.renderWithRoot("{{ range .Data.Pages.GroupBy \"Weight\" }}{{ .Key }}:{{ range .ByTitle }}{{ .Title }}{{ end }};{{ end }}|" + "{{ range .Data.Pages.GroupBy \"Weight\" \"desc\" }}{{ .Key }}:{{ range .Pages }}{{ .Title }}{{ end }};{{ end }}|" + "{{ range .Data.Pages.ByWeight }}{{ .Title }}{{ end }}", new PageValue(root)));
            Xunit.Assert.Equal("3", TemplateTestHarness.renderWithRoot("{{ len (union .RegularPages .Sections) }}", new PageValue(root)));
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            Xunit.Assert.Equal("2024", environment.renderTemplate(Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ .Site.Lastmod.Format \"2006\" }}", null), new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            environment.templates.set("_partials/templates/_funcs/child", Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("child={{ . }}", "_partials/templates/_funcs/child.html"));
            Template parent = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ partial \"_funcs/child\" \"exact\" }}", "_partials/templates/parent.html");
            RenderScope parentScope = new RenderScope(new PageValue(root), new PageValue(root), site, environment, null, null, parent.sourcePath);
            TextBuilder output = new TextBuilder();
            parent.renderInto(output, parentScope, environment, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
            Xunit.Assert.Equal("child=exact", output.toString());
            Template pageTemplate = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ .Render \"summary\" }}", null);
            TextBuilder pageOutput = new TextBuilder();
            RenderScope pageScope = new RenderScope(new PageValue(newer), new PageValue(newer), site, environment, null);
            pageTemplate.renderInto(pageOutput, pageScope, environment, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
            Xunit.Assert.Equal("<summary>Newer</summary>", pageOutput.toString());
        }
        [Xunit.FactAttribute]
        public void page_taxonomy_terms_follow_explicit_graph_relations()
        {
            SiteContext site = TemplateTestHarness.createSite();
            PageContext page = TemplateTestHarness.createPage(site, "Article", "2024-01-01T00:00:00Z", "page");
            PageContext term = TemplateTestHarness.createPage(site, "TypeScript", "", "term");
            Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> memberships = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>();
            memberships.set("typescript", new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { page }));
            site.Taxonomies.set("tags", memberships);
            Tsonic.CSharp.Js.Map<string, PageContext> termPages = new Tsonic.CSharp.Js.Map<string, PageContext>();
            termPages.set("typescript", term);
            site.taxonomyTermPages.set("tags", termPages);
            Xunit.Assert.Equal("TypeScript;", TemplateTestHarness.renderWithRoot("{{ range .GetTerms \"tags\" }}{{ .Title }};{{ end }}", new PageValue(page)));
        }
        [Xunit.FactAttribute]
        public void page_menu_methods_use_the_exact_menu_hierarchy()
        {
            SiteContext site = TemplateTestHarness.createSite();
            PageContext section = TemplateTestHarness.createPage(site, "Section", "", "section");
            PageContext article = TemplateTestHarness.createPage(site, "Article", "", "page");
            MenuEntry parent = new MenuEntry("Section", "", "", "", 0, "", "section", "", "", "main");
            MenuEntry child = new MenuEntry("Article", "", "", "", 0, "section", "article", "", "", "main");
            parent.page = section;
            child.page = article;
            parent.children = new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { child });
            site.Menus.set("main", new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { parent }));
            Xunit.Assert.Equal("true|false|false|true|false", TemplateTestHarness.renderWithRoot("{{ range .Site.Menus.main }}{{ $.HasMenuCurrent \"main\" . }}|{{ $.IsMenuCurrent \"main\" . }}|" + "{{ range .Children }}{{ $.HasMenuCurrent \"main\" . }}|{{ $.IsMenuCurrent \"main\" . }}|" + "{{ $.IsMenuCurrent \"other\" . }}{{ end }}{{ end }}", new PageValue(article)));
        }
        [Xunit.FactAttribute]
        public void template_definitions_propagate_across_partial_boundaries()
        {
            SiteContext site = TemplateTestHarness.createSite();
            PageContext root = TemplateTestHarness.createPage(site, "Home", "", "home");
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            environment.templates.set("partials/child", Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ template \"integrity\" . }}", "partials/child"));
            Template parent = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ define \"integrity\" }}integrity={{ . }}{{ end }}{{ partial \"child\" \"external\" }}", "partials/parent");
            Xunit.Assert.Equal("integrity=external", environment.renderTemplate(parent, new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            Template inline = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ define \"_partials/inline\" }}inline={{ . }}{{ end }}{{ partials.IncludeCached \"inline\" \"local\" }}", "partials/inline-owner");
            Xunit.Assert.Equal("inline=local", environment.renderTemplate(inline, new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            environment.templates.set("partials/page-global", Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ page.Title }}|{{ page.Store.Add \"visits\" 1 }}{{ page.Store.Get \"visits\" }}", "partials/page-global"));
            Template contextual = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ partial \"page-global\" (dict \"context\" \"changed\") }}", null);
            Xunit.Assert.Equal("Home|1", environment.renderTemplate(contextual, new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
        }
        [Xunit.FactAttribute]
        public void page_resources_use_the_published_bundle_inventory()
        {
            string root = TestRoot.createTestDirectory("template-page-resources");
            string siteDirectory = Tsonic.CSharp.Node.path.join(root, "site");
            string bundleDirectory = Tsonic.CSharp.Node.path.join(siteDirectory, "content", "article");
            string outputDirectory = Tsonic.CSharp.Node.path.join(root, "output");
            try
            {
                TestRoot.createDirectory(bundleDirectory);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(bundleDirectory, "cover.svg"), "<svg></svg>");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(bundleDirectory, "notes.txt"), "notes");
                ResourceManager manager = new ResourceManager(siteDirectory, null, outputDirectory);
                TestTemplateEnvironment environment = new TestTemplateEnvironment(manager);
                SiteContext site = TemplateTestHarness.createSite();
                PageContext page = TemplateTestHarness.createPage(site, "Article", "", "page");
                page.relPermalink = "/article/";
                page.resourceSourceDir = bundleDirectory;
                Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ $images := .Resources.ByType \"image\" }}" + "{{ with $images.GetMatch \"*.svg\" }}{{ .RelPermalink }}{{ end }}|" + "{{ with ($images.GetMatch \"{*cover*,*thumbnail*}\") }}{{ .RelPermalink }}{{ end }}|" + "{{ with .Resources.Get \"notes.txt\" }}{{ .RelPermalink }}{{ end }}", null);
                Xunit.Assert.Equal("/article/cover.svg|/article/cover.svg|/article/notes.txt", environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
