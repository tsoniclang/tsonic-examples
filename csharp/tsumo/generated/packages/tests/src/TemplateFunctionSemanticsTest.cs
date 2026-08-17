using System;

namespace Tsumo.Tests
{
    public static class TemplateFunctionSemanticsTest
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            TemplateTestHarness.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class TemplateFunctionSemanticsTests
    {
        [Xunit.FactAttribute]
        public void template_namespaces_expose_exact_string_and_hugo_functions()
        {
            Xunit.Assert.Equal("=====", TemplateTestHarness.render("{{ strings.Repeat 5 \"=\" }}"));
            Xunit.Assert.Equal("Hello World", TemplateTestHarness.render("{{ strings.Title \"hello world\" }}"));
            Xunit.Assert.Equal("3|9|4|4|5", TemplateTestHarness.render("{{ math.Min 9 3 7 }}|{{ math.Max 9 3 7 }}|{{ math.Round 4 }}|{{ math.Ceil 4 }}|{{ math.Add 2 3 }}"));
            Xunit.Assert.Equal("c,b,a|a,b", TemplateTestHarness.render("{{ delimit (collections.Reverse (slice \"a\" \"b\" \"c\")) `,` }}|{{ delimit (strings.Split \"a,b\" `,`) `,` }}"));
            Xunit.Assert.Equal("string|bool|int|map[string]interface {}|&quot;quoted&quot;|true|3", TemplateTestHarness.render("{{ printf \"%T|%T|%T|%T|%q|%t|%v\" \"value\" true 3 (dict \"key\" \"value\") \"quoted\" true 3 }}"));
            Xunit.Assert.Equal("<meta name=\"generator\" content=\"Hugo 0.146.2\">", TemplateTestHarness.render("{{ hugo.Generator }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_STRING_REPEAT_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ strings.Repeat -1 \"=\" }}");
            }));
            Xunit.Assert.Equal("a,b", TemplateTestHarness.render("{{ delimit (collections.First 2 (collections.Slice \"a\" \"b\" \"c\")) \",\" }}"));
            Xunit.Assert.Equal("fallback", TemplateTestHarness.render("{{ compare.Default \"fallback\" \"\" }}"));
            Xunit.Assert.Equal("false|only|42", TemplateTestHarness.render("{{ default \"fallback\" false }}|{{ default \"only\" }}|{{ default 42 0 }}"));
            Xunit.Assert.Equal("nil", TemplateTestHarness.render("{{ if nil }}value{{ else }}nil{{ end }}"));
            Xunit.Assert.Equal("line", TemplateTestHarness.render("{{ chomp \"line\\n\" }}"));
            Xunit.Assert.Equal("2024", TemplateTestHarness.render("{{ now.Year }}"));
            Xunit.Assert.Equal("configured", TemplateTestHarness.render("{{ getenv \"TSUMO_TEST_VALUE\" }}"));
            Xunit.Assert.Equal("", TemplateTestHarness.render("{{ getenv \"TSUMO_MISSING_VALUE\" }}"));
            Xunit.Assert.Equal("true|false", TemplateTestHarness.render("{{ fileExists \"static/existing.css\" }}|{{ fileExists \"static/missing.css\" }}"));
            Xunit.Assert.Equal("true", TemplateTestHarness.render("{{ collections.IsSet (dict \"key\" \"value\") \"key\" }}"));
            Xunit.Assert.Equal("translated", TemplateTestHarness.render("{{ T \"translated\" }}"));
            Xunit.Assert.Equal("2026|42", TemplateTestHarness.render("{{ int \"2026\" }}|{{ string 42 }}"));
            Xunit.Assert.Equal("true|false", TemplateTestHarness.render("{{ collections.In (collections.Slice \"first\" \"second\") \"second\" }}|{{ collections.In (collections.Slice \"first\") \"second\" }}"));
            Xunit.Assert.Equal("one two|first|one two|url.Values", TemplateTestHarness.render("{{ $url := urls.Parse \"/page?classes=one+two&name=first&name=second\" }}" + "{{ $url.Query.Get \"classes\" }}|{{ $url.Query.Get \"name\" }}|{{ $url.Query.classes }}|{{ printf \"%T\" $url.Query }}"));
            Xunit.Assert.Equal("", TemplateTestHarness.render("{{ $url := urls.Parse \"/page?name=value\" }}{{ $url.Query.Get \"missing\" }}"));
            Xunit.Assert.Equal("🙂", TemplateTestHarness.render("{{ $url := urls.Parse \"/page?name=%F0%9F%99%82\" }}{{ $url.Query.Get \"name\" }}"));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_URL_QUERY_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ $url := urls.Parse \"/page?name=%ZZ\" }}{{ $url.Query.Get \"name\" }}");
            }));
            Xunit.Assert.Equal("TSUMO_TEMPLATE_URL_QUERY_INVALID", TemplateTestHarness.captureDiagnosticCode(() =>
            {
                TemplateTestHarness.render("{{ $url := urls.Parse \"/page?name=%F0%28%8C%28\" }}{{ $url.Query.Get \"name\" }}");
            }));
            Xunit.Assert.Equal("value|nested", TemplateTestHarness.render("{{ hugo.Store.Set \"name\" \"value\" }}{{ hugo.Store.SetInMap \"items\" \"key\" \"nested\" }}" + "{{ hugo.Store.Get \"name\" }}|{{ index (hugo.Store.Get \"items\") \"key\" }}"));
            Xunit.Assert.Equal("first,second", TemplateTestHarness.render("{{ delimit (transform.Unmarshal \"- first\\n- second\") \",\" }}"));
            Xunit.Assert.Equal("value", TemplateTestHarness.render("{{ (transform.Unmarshal \"{\\\"key\\\":\\\"value\\\"}\").key }}"));
            Xunit.Assert.Equal("_partials/site-style.html", TemplateTestHarness.render("{{ fmt.Print \"_partials/\" \"site-style.html\" }}"));
            Xunit.Assert.Equal("true", TemplateTestHarness.render("{{ hasPrefix \"<svg viewBox=0>\" \"<svg\" }}"));
            Xunit.Assert.Equal("true|true|false", TemplateTestHarness.render("{{ reflect.IsMap (dict \"key\" \"value\") }}|{{ reflect.IsSlice (slice \"value\") }}|{{ reflect.IsMap (slice) }}"));
            Xunit.Assert.Equal("value|true|trimmed", TemplateTestHarness.render("{{ strings.ToLower \"VALUE\" }}|{{ strings.HasSuffix \"index.html\" \".html\" }}|{{ strings.Trim \"/trimmed/\" \"/\" }}"));
            Xunit.Assert.Equal("a%20b=c%2Fd|.css|content/page.md|900150983cd24fb0d6963f7d28e17f72|Hello World|3", TemplateTestHarness.render("{{ collections.Querify \"a b\" \"c/d\" }}|{{ path.Ext \"assets/main.css\" }}|" + "{{ path.Join \"content\" \"posts\" \"..\" \"page.md\" }}|{{ crypto.MD5 \"abc\" }}|" + "{{ inflect.Humanize \"hello-world\" }}|{{ math.Ceil 3 }}"));
            Xunit.Assert.Equal("/asset.css|https://example.test/asset.css|https://example.test/asset.css|&lt;x&gt;", TemplateTestHarness.render("{{ urls.RelURL \"asset.css\" }}|{{ urls.AbsURL \"/asset.css\" }}|" + "{{ urls.AbsLangURL \"/asset.css\" }}|{{ safeHTML (transform.HTMLEscape \"<x>\") }}"));
        }
    }
}
