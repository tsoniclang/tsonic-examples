using System;

namespace Tsumo.Tests
{
    public static class TemplateTestHarness
    {
        public static Func<SiteContext> createSite
        {
            get;
            private set;
        } = default(Func<SiteContext>)!;
        public static Func<string, TemplateValue, string> renderWithRoot
        {
            get;
            private set;
        } = default(Func<string, TemplateValue, string>)!;
        public static Func<string, string> render
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<SiteContext, string, string, string, PageContext> createPage
        {
            get;
            private set;
        } = default(Func<SiteContext, string, string, string, PageContext>)!;
        public static Func<Action, string> captureDiagnosticCode
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        public static Func<Action, TsumoError> captureDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, TsumoError>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            createSite = () =>
            {
                SiteConfig config = new SiteConfig("Test Site", "https://example.test/", "en", null, null);
                return new SiteContext(config, new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { }), null, null);
            };
            renderWithRoot = (string source, TemplateValue root) =>
            {
                Template template = Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate(source, null);
                TestTemplateEnvironment environment = new TestTemplateEnvironment();
                SiteContext site = createSite();
                RenderScope scope = new RenderScope(root, root, site, environment, null);
                System.Text.StringBuilder output = new System.Text.StringBuilder();
                template.renderInto(output, scope, environment, new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>());
                return output.ToString();
            };
            render = (string source) => renderWithRoot(source, new DictValue(new Tsonic.CSharp.Js.Map<string, TemplateValue>()));
            createPage = (SiteContext site, string title, string date, string kind) =>
            {
                Tsonic.CSharp.Js.JSArray<PageContext> emptyPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                Tsonic.CSharp.Js.JSArray<string> emptyStrings = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                HtmlString emptyHtml = new HtmlString("");
                return new PageContext(title, date, date, false, kind, kind == "page" ? "posts" : "", kind, Tsonic.CSharp.Js.String.toLowerCase(title), $"/{Tsonic.CSharp.Js.String.toLowerCase(title)}/", "", emptyHtml, new HtmlString($"<p>{title}</p>"), new HtmlString($"<p>{title}</p>"), "", emptyStrings, emptyStrings, new Tsonic.CSharp.Js.Map<string, ParamValue>(), null, site.Language, emptyPages, null, site, emptyPages, null, emptyPages, null);
            };
            captureDiagnosticCode = (Action operation) =>
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
                throw new System.Exception("Expected a TsumoError diagnostic");
            };
            captureDiagnostic = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception error)
                {
                    if (error is TsumoError)
                    {
                        return (TsumoError)error;
                    }
                    throw;
                }
                throw new System.Exception("Expected a TsumoError diagnostic");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class TestTemplateEnvironment : TemplateEnvironment
    {
        public Tsonic.CSharp.Js.Map<string, Template> templates;
        public ResourceManager? resourceManager;
        public I18nStore? i18nStore;
        public TestTemplateEnvironment(ResourceManager? resourceManager = null) : base(new Tsonic.CSharp.Js.Date(1704067200000))
        {
            this.templates = new Tsonic.CSharp.Js.Map<string, Template>();
            this.resourceManager = resourceManager;
            this.i18nStore = null;
        }
        public override string? getEnvironmentVariable(string name)
        {
            return name == "TSUMO_TEST_VALUE" ? "configured" : null;
        }
        public override bool sourceFileExists(string path)
        {
            return path == "static/existing.css";
        }
        public override Template? getTemplate(string path)
        {
            return Tsonic.CSharp.Js.Map.getReference<string, Template>(this.templates, path);
        }
        public override string? getTemplateSourceRelativePath(string sourcePath)
        {
            return sourcePath;
        }
        public override ResourceManager? getResourceManager()
        {
            return this.resourceManager;
        }
        public override string getI18n(string lang, string key, int? count = null)
        {
            return this.i18nStore?.translate(lang, key, count) ?? key;
        }
        public override string renderTextTemplateSource(string source, TemplateValue context, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState? state = null)
        {
            return this.renderTextTemplate(Node_modules_Tsumo_engine_src_template_parser_parseTemplate.parseTemplate(source, null), context, site, overrides, state);
        }
        public override string? renderPageView(PageContext page, string view, RenderState? _state)
        {
            return view == "summary" ? $"<summary>{page.title}</summary>" : null;
        }
        public override string renderTemplate(Template template, TemplateValue context, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState? state = null)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            RenderScope scope = new RenderScope(context, context, site, this, null, state, template.sourcePath);
            template.renderInto(output, scope, this, overrides);
            return output.ToString();
        }
        public override string renderTextTemplate(Template template, TemplateValue context, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState? state = null)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            RenderScope scope = new RenderScope(context, context, site, this, null, state, template.sourcePath);
            template.renderTextInto(output, scope, this, overrides);
            return output.ToString();
        }
        public override string renderTemplateDefinition(Tsonic.CSharp.Js.JSArray<TemplateNode> nodes, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> definitions, string? sourcePath, TemplateValue context, SiteContext site, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, RenderState? state = null)
        {
            return this.renderTemplate(new Template(nodes, definitions, sourcePath), context, site, overrides, state);
        }
    }
}
