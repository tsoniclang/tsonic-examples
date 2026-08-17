using System;

namespace Tsumo.Tests
{
    public static class ThemeCompatibilityTest
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
    public class ThemeCompatibilityTests
    {
        [Xunit.FactAttribute]
        public void chained_alternatives_preserve_the_selected_context()
        {
            Xunit.Assert.Equal("second|selected|fallback", TemplateTestHarness.render("{{ if false }}first{{ else if true }}second{{ else }}third{{ end }}|" + "{{ with nil }}first{{ else with \"selected\" }}{{ . }}{{ else }}third{{ end }}|" + "{{ with nil }}first{{ else with nil }}second{{ else }}fallback{{ end }}"));
            Xunit.Assert.Equal("2026-08-15T00:00:00Z|2026-08-15T00:00:00Z", TemplateTestHarness.renderWithRoot("{{ time . }}|{{ time.AsTime . }}", new DateValue("2026-08-15T00:00:00Z")));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_TIME_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ time \"not-a-date\" }}");
            }));
        }
        [Xunit.FactAttribute]
        public void date_methods_and_unicode_substrings_follow_hugo_semantics()
        {
            Xunit.Assert.Equal("2024-03-02|true", TemplateTestHarness.renderWithRoot("{{ (.AddDate 0 1 0).Format \"2006-01-02\" }}|" + "{{ (.AddDate 0 0 2).After (.AddDate 0 0 1) }}", new DateValue("2024-01-31T00:00:00Z")));
            Xunit.Assert.Equal("😀B|ef|bcd|", TemplateTestHarness.render("{{ substr \"A😀BC\" 1 2 }}|{{ strings.Substr \"abcdef\" -2 }}|" + "{{ substr \"abcdef\" 1 -2 }}|{{ substr \"abcdef\" 20 }}"));
            Xunit.Assert.Equal("1704067200|1704067200000000000", TemplateTestHarness.render("{{ now.Unix }}|{{ now.UnixNano }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_DATE_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.renderWithRoot("{{ .AddDate 2147483647 0 0 }}", new DateValue("2024-01-31T00:00:00Z"));
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_DATE_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.renderWithRoot("{{ .AddDate 0 0 2147483647 }}", new DateValue("2024-01-31T00:00:00Z"));
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_SUBSTRING_ARGUMENT_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ substr \"abc\" \"invalid\" }}");
            }));
        }
        [Xunit.FactAttribute]
        public void integer_sequences_follow_hugo_semantics_and_limits()
        {
            Xunit.Assert.Equal("1,2,3,|-2,-1,0,1,2,|6,4,2,|-1,-2,-3,", TemplateTestHarness.render("{{ range seq 3 }}{{ . }},{{ end }}|" + "{{ range collections.Seq -2 2 }}{{ . }},{{ end }}|" + "{{ range seq 6 -2 2 }}{{ . }},{{ end }}|" + "{{ range seq -3 }}{{ . }},{{ end }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_SEQUENCE_INCREMENT_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ seq 1 0 2 }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_SEQUENCE_SIZE_UNSUPPORTED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ seq -1000001 }}");
            }));
        }
        [Xunit.FactAttribute]
        public void string_cutset_functions_follow_unicode_semantics()
        {
            Xunit.Assert.Equal("path😀|😀/path|value|middle", TemplateTestHarness.render("{{ strings.TrimLeft \"😀/\" \"😀/path😀\" }}|" + "{{ strings.TrimRight \"😀/\" \"😀/path😀/\" }}|" + "{{ strings.TrimSpace \" value　\" }}|" + "{{ strings.Trim \"😀/middle/😀\" \"😀/\" }}"));
        }
        [Xunit.FactAttribute]
        public void where_filters_structured_slices_and_rejects_unproven_inputs()
        {
            Xunit.Assert.Equal("one,three,|two,", TemplateTestHarness.render("{{ $items := slice (dict \"kind\" \"x\" \"name\" \"one\") " + "(dict \"kind\" \"y\" \"name\" \"two\") (dict \"kind\" \"x\" \"name\" \"three\") }}" + "{{ range where $items \"kind\" \"x\" }}{{ .name }},{{ end }}|" + "{{ range where $items \"kind\" \"ne\" \"x\" }}{{ .name }},{{ end }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_WHERE_COLLECTION_UNSUPPORTED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ where \"scalar\" \"\" \"scalar\" }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_WHERE_OPERATOR_UNSUPPORTED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ where (slice \"value\") \"\" \"approximately\" \"value\" }}");
            }));
        }
        [Xunit.FactAttribute]
        public void site_data_layers_are_structured_deterministic_and_conflict_checked()
        {
            string root = TestRoot.createTestDirectory("theme-data-layers");
            string siteDirectory = System.IO.Path.Combine(root, "site");
            string themeDirectory = System.IO.Path.Combine(root, "theme");
            string mountDirectory = System.IO.Path.Combine(root, "module-data");
            try
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(siteDirectory, "data"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(themeDirectory, "data", "nested"));
                System.IO.Directory.CreateDirectory(mountDirectory);
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDirectory, "data", "theme.toml"), "value = \"theme\"\n");
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDirectory, "data", "shared.toml"), "value = \"theme\"\n");
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDirectory, "data", "nested", "entry.json"), "{\"value\":\"nested\"}");
                System.IO.File.WriteAllText(System.IO.Path.Combine(mountDirectory, "module.json"), "{\"value\":\"module\"}");
                System.IO.File.WriteAllText(System.IO.Path.Combine(mountDirectory, "shared.json"), "{\"value\":\"module\"}");
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDirectory, "data", "site.yaml"), "value: site\n");
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDirectory, "data", "shared.yaml"), "value: site\n");
                DictValue data = Node_modules_Tsumo_engine_src_template_dataLoader.loadSiteData(siteDirectory, themeDirectory, new Tsonic.CSharp.Js.JSArray<ModuleMount>(new ModuleMount[] { new ModuleMount(mountDirectory, "data") }));
                TestTemplateEnvironment environment = new TestTemplateEnvironment();
                environment.setSiteData(data);
                SiteContext site = TemplateTestHarness.createSite();
                PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
                Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ hugo.Data.theme.value }}|{{ hugo.Data.module.value }}|" + "{{ .Site.Data.shared.value }}|{{ hugo.Data.nested.entry.value }}", null);
                Xunit.Assert.Equal("theme|module|site|nested", environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDirectory, "data", "shared.toml"), "value = \"duplicate\"\n");
                Xunit.Assert.Equal("TSUMO_DATA_IDENTITY_CONFLICT", TemplateTestHarness.captureDiagnosticCode(() =>
                {
                    Node_modules_Tsumo_engine_src_template_dataLoader.loadSiteData(siteDirectory, themeDirectory, new Tsonic.CSharp.Js.JSArray<ModuleMount>(new ModuleMount[] { new ModuleMount(mountDirectory, "data") }));
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void embedded_page_image_partial_selects_published_page_resources()
        {
            string root = TestRoot.createTestDirectory("embedded-page-images");
            string siteDirectory = System.IO.Path.Combine(root, "site");
            string bundleDirectory = System.IO.Path.Combine(siteDirectory, "content", "home");
            string outputDirectory = System.IO.Path.Combine(root, "output");
            try
            {
                System.IO.Directory.CreateDirectory(bundleDirectory);
                System.IO.File.WriteAllText(System.IO.Path.Combine(bundleDirectory, "cover.svg"), "<svg></svg>");
                string? source = Node_modules_Tsumo_engine_src_template_embeddedTemplates.getEmbeddedTemplateSource("_partials/_funcs/get-page-images.html");
                if (source is null)
                {
                    Xunit.Assert.True(false);
                    return;
                }
                TestTemplateEnvironment environment = new TestTemplateEnvironment(new ResourceManager(siteDirectory, null, outputDirectory));
                environment.templates.set("_partials/_funcs/get-page-images", Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate(source, "_partials/_funcs/get-page-images.html"));
                SiteContext site = TemplateTestHarness.createSite();
                PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
                page.resourceSourceDir = bundleDirectory;
                Xunit.Assert.Equal("/home/cover.svg", environment.renderTemplate(Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ with index (partial \"_funcs/get-page-images\" .) 0 }}{{ .RelPermalink }}{{ end }}", null), new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
