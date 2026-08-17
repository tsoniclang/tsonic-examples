using System;

namespace Tsumo.Tests
{
    public static class TemplateRuntimeTest
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
    public class TemplateRuntimeTests
    {
        [Xunit.FactAttribute]
        public void parser_and_evaluator_render_control_flow_and_pipeline()
        {
            string source = "{{ if true }}yes{{ else }}no{{ end }}|{{ \"ab\" | upper }}";
            Xunit.Assert.Equal("yes|AB", TemplateTestHarness.render(source));
            SiteContext site = TemplateTestHarness.createSite();
            PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
            Xunit.Assert.Equal("false|exact", TemplateTestHarness.renderWithRoot("{{ in (slice \"posts\" \"tags\") .Section }}|{{ (dict \"value\" \"exact\").value }}", new PageValue(page)));
            Xunit.Assert.Equal("inner|outer|empty|chosen:chosen|changed|changed", TemplateTestHarness.render("{{ $value := \"outer\" }}" + "{{ if $value := \"inner\" }}{{ $value }}{{ end }}|{{ $value }}|" + "{{ with $selected := \"\" }}invalid{{ else }}{{ if eq $selected \"\" }}empty{{ end }}{{ end }}|" + "{{ with $selected := \"chosen\" }}{{ $selected }}:{{ . }}{{ end }}|" + "{{ if $value = \"changed\" }}{{ $value }}{{ end }}|{{ $value }}"));
        }
        [Xunit.FactAttribute]
        public void collection_functions_preserve_exact_split_segments()
        {
            Xunit.Assert.Equal("a|b|", TemplateTestHarness.render("{{ delimit (split \"a--b--\" \"--\") \"|\" }}"));
            Xunit.Assert.Equal("a|b", TemplateTestHarness.render("{{ delimit (split \"ab\" \"\") \"|\" }}"));
        }
        [Xunit.FactAttribute]
        public void collection_union_accepts_slices_and_nil_without_collapsing_distinct_values()
        {
            Xunit.Assert.Equal("a,b,c|a,b|a,b|", TemplateTestHarness.render("{{ delimit (union (slice \"a\" \"b\") (slice \"b\" \"c\")) \",\" }}|" + "{{ delimit (union (slice \"a\" \"b\") nil) \",\" }}|" + "{{ delimit (union nil (slice \"a\" \"b\")) \",\" }}|" + "{{ delimit (union nil nil) \",\" }}"));
            Xunit.Assert.Equal("one,three", TemplateTestHarness.render("{{ delimit (collections.Complement (slice \"two\") (slice \"one\" \"two\" \"three\")) \",\" }}"));
        }
        [Xunit.FactAttribute]
        public void page_has_shortcode_uses_the_exact_parsed_page_inventory()
        {
            SiteContext site = TemplateTestHarness.createSite();
            PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
            page.shortcodeNames = Node_modules_Tsumo_engine_src_shortcode.collectShortcodeNames("{{< outer >}}{{< inner / >}}{{< /outer >}}\n```text\n{{< ignored >}}\n```", "content/home.md");
            Xunit.Assert.Equal("true|true|false|false", TemplateTestHarness.renderWithRoot("{{ .HasShortcode \"outer\" }}|{{ .HasShortcode \"inner\" }}|" + "{{ .HasShortcode \"ignored\" }}|{{ .HasShortcode \"Outer\" }}", new PageValue(page)));
        }
        [Xunit.FactAttribute]
        public void hugo_sites_exposes_the_checked_site_graph()
        {
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            SiteContext site = TemplateTestHarness.createSite();
            PageContext root = TemplateTestHarness.createPage(site, "Home", "", "home");
            site.home = root;
            site.Sites = new Tsonic.CSharp.Js.JSArray<SiteContext>(new SiteContext[] { site });
            Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ range hugo.Sites }}{{ .Title }};{{ end }}|{{ hugo.Sites.Default.Home.RelPermalink }}", null);
            Xunit.Assert.Equal("Test Site;|/home/", environment.renderTemplate(template, new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
        }
        [Xunit.FactAttribute]
        public void related_pages_use_exact_default_keyword_and_tag_evidence()
        {
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            SiteContext site = TemplateTestHarness.createSite();
            PageContext current = TemplateTestHarness.createPage(site, "Current", "2026-08-15T00:00:00Z", "page");
            PageContext older = TemplateTestHarness.createPage(site, "Older", "2025-08-15T00:00:00Z", "page");
            PageContext newer = TemplateTestHarness.createPage(site, "Newer", "2027-08-15T00:00:00Z", "page");
            PageContext unrelated = TemplateTestHarness.createPage(site, "Unrelated", "2024-08-15T00:00:00Z", "page");
            current.tags = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "shared" });
            older.tags = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "shared" });
            newer.tags = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "shared" });
            unrelated.tags = new Tsonic.CSharp.Js.JSArray<string>(new string[] { "other" });
            site.allPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { current, older, newer, unrelated });
            Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ range site.RegularPages.Related page }}{{ .Title }}{{ end }}", null);
            Xunit.Assert.Equal("Older", environment.renderTemplate(template, new PageValue(current), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
        }
        [Xunit.FactAttribute]
        public void css_build_applies_its_closed_resource_options()
        {
            string root = TestRoot.createTestDirectory("template-css-build");
            string siteDirectory = System.IO.Path.Combine(root, "site");
            string outputDirectory = System.IO.Path.Combine(root, "output");
            try
            {
                System.IO.Directory.CreateDirectory(siteDirectory);
                ResourceManager manager = new ResourceManager(siteDirectory, null, outputDirectory);
                TestTemplateEnvironment environment = new TestTemplateEnvironment(manager);
                SiteContext site = TemplateTestHarness.createSite();
                PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
                Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ $style := resources.FromString \"theme.css\" \"body { color: red; }\\n\" }}" + "{{ $style = $style | css.Build (dict \"targetPath\" \"css/main.css\" \"minify\" true \"sourceMap\" \"none\") }}" + "{{ $style.RelPermalink }}|{{ $style.Content }}", null);
                Xunit.Assert.Equal("/css/main.css|body { color: red; }", environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
                Template namespaceTemplate = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ $namespace := resources }}" + "{{ $copy := $namespace.FromString \"css/copy.css\" \"p { color: blue; }\" }}" + "{{ $copy.RelPermalink }}|{{ $copy.Content }}", null);
                Xunit.Assert.Equal("/css/copy.css|p { color: blue; }", environment.renderTemplate(namespaceTemplate, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void i18n_layers_parse_structured_formats_and_render_plural_context()
        {
            string root = TestRoot.createTestDirectory("template-i18n");
            string themeDirectory = System.IO.Path.Combine(root, "theme");
            string siteDirectory = System.IO.Path.Combine(root, "site");
            try
            {
                System.IO.Directory.CreateDirectory(themeDirectory);
                System.IO.Directory.CreateDirectory(siteDirectory);
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDirectory, "en.toml"), "toggleMenu = \"Theme Menu\"\n" + "[footer]\n" + "builtWith = \"Built with {{ .Generator }}\"\n" + "[list.page]\n" + "one = \"{{ .Count }} page\"\n" + "other = \"{{ .Count }} pages\"\n");
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDirectory, "fr.json"), "{\"local\":\"Locale française\"}");
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDirectory, "en.yaml"), "- id: toggleMenu # site override\n" + "  translation: Site Menu\n" + "- id: legacy\n" + "  translation: Legacy {{ .Name }}\n" + "- id: continued\n" + "  translation:\n" + "    \"Continued scalar\"\n" + "- id: folded\n" + "  translation: >-\n" + "    Folded\n" + "    scalar\n" + "- id: literal\n" + "  translation: |\n" + "    Literal\n" + "    scalar\n" + "- id: escapedQuoted\n" + "  translation:\n" + "    \"Generated with " + "\\" + "\n" + "    exact continuity.\"\n" + "- id: foldedQuoted\n" + "  translation: \"Folded\n" + "  quoted scalar\"\n" + "- id: singleQuoted\n" + "  translation:\n" + "    'Single\n" + "    quoted ''value'''\n" + "- id: plainWithQuotes\n" + "  translation: Tagged '{{ . }}'\n");
                I18nStore store = new I18nStore();
                store.loadFromDir(themeDirectory);
                store.loadFromDir(siteDirectory);
                Xunit.Assert.Equal("Site Menu", store.translate("en-US", "toggleMenu"));
                Xunit.Assert.Equal("{{ .Count }} page", store.translate("en", "list.page", 1));
                Xunit.Assert.Equal("{{ .Count }} pages", store.translate("en", "list.page", 2));
                Xunit.Assert.Equal("Locale française", store.translate("fr-FR", "local"));
                Xunit.Assert.Equal("Folded scalar", store.translate("en", "folded"));
                Xunit.Assert.Equal("Literal\nscalar\n", store.translate("en", "literal"));
                Xunit.Assert.Equal("Generated with exact continuity.", store.translate("en", "escapedQuoted"));
                Xunit.Assert.Equal("Folded quoted scalar", store.translate("en", "foldedQuoted"));
                Xunit.Assert.Equal("Single quoted 'value'", store.translate("en", "singleQuoted"));
                Xunit.Assert.Equal("Tagged '{{ . }}'", store.translate("en", "plainWithQuotes"));
                TestTemplateEnvironment environment = new TestTemplateEnvironment();
                environment.i18nStore = store;
                SiteContext site = TemplateTestHarness.createSite();
                PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
                Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ T \"toggleMenu\" }}|" + "{{ T \"footer.builtWith\" (dict \"Generator\" \"<strong>Tsumo</strong>\") | safeHTML }}|" + "{{ T \"list.page\" 1 }}|{{ T \"list.page\" 2 }}|" + "{{ T \"legacy\" (dict \"Name\" \"Ada\") }}|{{ T \"continued\" }}", null);
                Xunit.Assert.Equal("Site Menu|Built with <strong>Tsumo</strong>|1 page|2 pages|Legacy Ada|Continued scalar", environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void deferred_templates_finalize_after_normal_render_and_share_keyed_results()
        {
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            SiteContext site = TemplateTestHarness.createSite();
            PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
            Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ with (templates.Defer (dict \"key\" \"shared\")) }}" + "{{ site.Store.Add \"runs\" 1 }}{{ site.Store.Get \"late\" }}{{ end }}" + "{{ site.Store.Set \"late\" \"ready\" }}", "layouts/baseof.html");
            string first = environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
            string second = environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
            Tsonic.CSharp.Js.Map<string, string> results = environment.finalizeDeferredTemplates();
            foreach (string token in results.keys())
            {
                string? result = Tsonic.CSharp.Js.Map.getReference<string, string>(results, token);
                if (result is null)
                {
                    throw new System.Exception("Expected a finalized deferred-template result");
                }
                first = Tsonic.CSharp.Js.String.replaceAll(first, token, result);
                second = Tsonic.CSharp.Js.String.replaceAll(second, token, result);
            }
            Xunit.Assert.Equal("ready", first);
            Xunit.Assert.Equal("ready", second);
            Xunit.Assert.Equal("1", environment.renderTemplate(Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ site.Store.Get \"runs\" }}", null), new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
        }
        [Xunit.FactAttribute]
        public void deferred_templates_distinguish_authored_occurrences_with_the_same_key()
        {
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            SiteContext site = TemplateTestHarness.createSite();
            PageContext page = TemplateTestHarness.createPage(site, "Home", "", "home");
            Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ with (templates.Defer (dict \"key\" \"shared\")) }}first{{ end }}|" + "{{ with (templates.Defer (dict \"key\" \"shared\")) }}second{{ end }}", "layouts/distinct-deferred.html");
            string output = environment.renderTemplate(template, new PageValue(page), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
            Tsonic.CSharp.Js.Map<string, string> results = environment.finalizeDeferredTemplates();
            Xunit.Assert.Equal<double>(2, results.size);
            foreach (string token in results.keys())
            {
                string? result = Tsonic.CSharp.Js.Map.getReference<string, string>(results, token);
                if (result is null)
                {
                    throw new System.Exception("Expected a finalized deferred-template result");
                }
                output = Tsonic.CSharp.Js.String.replaceAll(output, token, result);
            }
            Xunit.Assert.Equal("first|second", output);
        }
        [Xunit.FactAttribute]
        public void return_evaluates_its_complete_value_expression()
        {
            TestTemplateEnvironment environment = new TestTemplateEnvironment();
            environment.templates.set("partials/selection", Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ return cond true \"selected\" \"rejected\" }}", "partials/selection"));
            SiteContext site = TemplateTestHarness.createSite();
            PageContext root = TemplateTestHarness.createPage(site, "Home", "", "home");
            Template parent = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ partial \"selection\" . }}", "partials/parent");
            Xunit.Assert.Equal("selected", environment.renderTemplate(parent, new PageValue(root), site, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>()));
        }
        [Xunit.FactAttribute]
        public void template_string_literals_decode_exact_interpreted_and_raw_forms()
        {
            Xunit.Assert.Equal("line\nnext", TemplateTestHarness.render("{{ print \"line\\nnext\" }}"));
            Xunit.Assert.Equal("line\\nnext", TemplateTestHarness.render("{{ print `line\\nnext` }}"));
            Xunit.Assert.Equal("\u001b", TemplateTestHarness.render("{{ print \"\\033\" }}"));
            Xunit.Assert.Equal("🔗", TemplateTestHarness.render("{{ print \"\\U0001F517\" }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_STRING_ESCAPE_INVALID", TemplateTestHarness.captureDiagnostic(() =>
            {
                TemplateTestHarness.render("{{ print \"\\q\" }}");
            }).diagnostic.code);
        }
        [Xunit.FactAttribute]
        public void template_text_compatibility_functions_are_deterministic()
        {
            Xunit.Assert.Equal("a-b---c", TemplateTestHarness.render("{{ anchorize \"a b   c\" }}"));
            Xunit.Assert.Equal("-a-b--c-", TemplateTestHarness.render("{{ anchorize \"< a, b, & c >\" }}"));
            Xunit.Assert.Equal("maingo|hugö", TemplateTestHarness.render("{{ anchorize \"main.go\" }}|{{ anchorize \"Hugö\" }}"));
            Xunit.Assert.Equal("I ❤️ Tsumo :unknown:", TemplateTestHarness.render("{{ emojify \"I :heart: Tsumo :unknown:\" }}"));
        }
        [Xunit.FactAttribute]
        public void template_regular_expression_functions_preserve_matches_groups_and_limits()
        {
            Xunit.Assert.Equal("ab,ac", TemplateTestHarness.render("{{ delimit (findRE `a.` `ab ac ad` 2) `,` }}"));
            Xunit.Assert.Equal("item42|item|42|item|42", TemplateTestHarness.render("{{ range findRESubmatch `([a-z]+)([0-9]+)` `item42` }}" + "{{ delimit . `|` }}|{{ index . 1 }}|{{ index . 2 }}{{ end }}"));
            Xunit.Assert.Equal("x2 item3", TemplateTestHarness.render("{{ replaceRE `item` `x` `item2 item3` 1 }}"));
        }
        [Xunit.FactAttribute]
        public void template_scanning_preserves_unicode_scalars_and_utf16_locations()
        {
            Xunit.Assert.Equal("before 🔗 after", TemplateTestHarness.render("before 🔗 after"));
            Xunit.Assert.Equal("🔗", TemplateTestHarness.render("{{ print \"🔗\" }}"));
            Xunit.Assert.Equal("🔗", TemplateTestHarness.render("{{ \"<span>🔗</span>\" | plainify }}"));
            TsumoDiagnostic located = TemplateTestHarness.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("🔗{{ if true", "layouts/unicode.html");
            }).diagnostic;
            Xunit.Assert.Equal("TSUMO_TEMPLATE_ACTION_UNCLOSED", located.code);
            Xunit.Assert.Equal<double?>(1, located.line);
            Xunit.Assert.Equal<double?>(3, located.column);
            Tsonic.CSharp.Js.JSArray<string> largeTemplateLines = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
            for (double index = 0; index < 2000; index++)
            {
                largeTemplateLines.push($"line {index}: {{{{ print \"{index}\" }}}}");
            }
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate(Tsonic.CSharp.Js.Array.join(largeTemplateLines, "\n"), "layouts/large.html") != null);
        }
        [Xunit.FactAttribute]
        public void dictionary_range_order_is_deterministic()
        {
            string source = "{{ range $key, $value := dict \"z\" \"last\" \"a\" \"first\" }}{{$key}}={{$value}};{{end}}";
            Xunit.Assert.Equal("a=first;z=last;", TemplateTestHarness.render(source));
            Xunit.Assert.Equal("a=first;z=last;", TemplateTestHarness.render(source));
        }
        [Xunit.FactAttribute]
        public void parser_reports_exact_malformed_input_diagnostics()
        {
            Xunit.Assert.Equal("TSUMO_TEMPLATE_ACTION_UNCLOSED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("before {{ if true", null);
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_STRING_UNCLOSED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ print \"unterminated }}", null);
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_BLOCK_UNCLOSED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ if true }}body", null);
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_DEFINE_DUPLICATE", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("{{ define \"x\" }}a{{ end }}{{ define \"x\" }}b{{ end }}", null);
            }));
            TsumoDiagnostic located = TemplateTestHarness.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate("first\n{{ if true", "layouts/single.html");
            }).diagnostic;
            Xunit.Assert.Equal("TSUMO_TEMPLATE_ACTION_UNCLOSED", located.code);
            Xunit.Assert.Equal("layouts/single.html", located.file);
            Xunit.Assert.Equal<double?>(2, located.line);
            Xunit.Assert.Equal<double?>(1, located.column);
        }
        [Xunit.FactAttribute]
        public void shortcode_parser_rejects_ambiguous_input_with_exact_locations()
        {
            TsumoDiagnostic unclosed = TemplateTestHarness.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_shortcode.parseShortcodes("first\n{{< figure", "content/post.md");
            }).diagnostic;
            Xunit.Assert.Equal("TSUMO_SHORTCODE_ACTION_UNCLOSED", unclosed.code);
            Xunit.Assert.Equal("content/post.md", unclosed.file);
            Xunit.Assert.Equal<double?>(2, unclosed.line);
            Xunit.Assert.Equal<double?>(1, unclosed.column);
            Xunit.Assert.Equal("TSUMO_SHORTCODE_PARAMETER_DUPLICATE", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_shortcode.parseShortcodes("{{< figure src='one' src='two' >}}", "content/post.md");
            }));
            Xunit.Assert.Equal("TSUMO_SHORTCODE_PARAMETER_STYLE_MIXED", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                Node_modules_Tsumo_engine_src_shortcode.parseShortcodes("{{< figure 'one' src='two' >}}", "content/post.md");
            }));
            Tsonic.CSharp.Js.JSArray<ShortcodeCall> quoted = Node_modules_Tsumo_engine_src_shortcode.parseShortcodes("{{< figure caption=\"\" published=\"true\" count=2 >}}", "content/post.md");
            Xunit.Assert.Equal<double>(1, quoted.length);
            Xunit.Assert.Equal("", Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(quoted[0].@params, "caption")?.stringValue);
            Xunit.Assert.Equal("true", Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(quoted[0].@params, "published")?.stringValue);
            Xunit.Assert.Equal<double?>(2, Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(quoted[0].@params, "count")?.numberValue);
        }
        [Xunit.FactAttribute]
        public void evaluator_reports_exact_unknown_and_invalid_operations()
        {
            Xunit.Assert.Equal("TSUMO_TEMPLATE_UNKNOWN_FUNCTION", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ imaginary \"x\" }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_FUNCTION_ARGUMENTS_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ div 1 }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_DIVIDE_BY_ZERO", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ div 4 0 }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_MODULO_BY_ZERO", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ mod 4 0 }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_PARTIAL_MISSING", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ partial \"absent\" . }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_METHOD_UNKNOWN", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ (\"value\").Missing \"argument\" }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_METHOD_UNKNOWN", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ $value := slice \"item\" }}{{ $value.Missing \"argument\" }}");
            }));
        }
        [Xunit.FactAttribute]
        public void dictionary_values_are_resolved_without_name_fallbacks()
        {
            Tsonic.CSharp.Js.Map<string, TemplateValue> values = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
            values.set("message", new StringValue("exact"));
            Xunit.Assert.Equal("exact", TemplateTestHarness.renderWithRoot("{{ .message }}", new DictValue(values)));
        }
    }
}
