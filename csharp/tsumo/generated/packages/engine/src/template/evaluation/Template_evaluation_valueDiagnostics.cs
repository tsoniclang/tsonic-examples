using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_valueDiagnostics
    {
        public static Func<TemplateValue, string> templateValueDiagnosticKind
        {
            get;
            private set;
        } = default(Func<TemplateValue, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_contexts.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_evaluation_pageResourceSemantics.__tsonic_module_init();
            templateValueDiagnosticKind = (TemplateValue value) =>
            {
                if ((object?)value is NilValue)
                {
                    return "nil";
                }
                if ((object?)value is StringValue)
                {
                    return "string";
                }
                if ((object?)value is BoolValue)
                {
                    return "boolean";
                }
                if ((object?)value is NumberValue)
                {
                    return "number";
                }
                if ((object?)value is HtmlValue)
                {
                    return "safe HTML";
                }
                if ((object?)value is DateValue)
                {
                    return "date";
                }
                if ((object?)value is DictValue)
                {
                    return "dictionary";
                }
                if ((object?)value is ScratchValue)
                {
                    return "scratch store";
                }
                if ((object?)value is UrlQueryValue)
                {
                    return "URL query";
                }
                if ((object?)value is UrlValue)
                {
                    return "URL";
                }
                if ((object?)value is ResourceNamespaceValue)
                {
                    return "global resource namespace";
                }
                if ((object?)value is ResourceValue)
                {
                    return "resource";
                }
                if ((object?)value is PageResourcesValue)
                {
                    return "page resource namespace";
                }
                if ((object?)value is PageResourceCollectionValue)
                {
                    return "page resource collection";
                }
                if ((object?)value is OutputFormatsValue)
                {
                    return "output-format collection";
                }
                if ((object?)value is OutputFormatsGetValue)
                {
                    return "output-format selector";
                }
                if ((object?)value is OutputFormatValue)
                {
                    return "output format";
                }
                if ((object?)value is PageValue)
                {
                    return "page";
                }
                if ((object?)value is PageArrayValue)
                {
                    return "page collection";
                }
                if ((object?)value is SiteValue)
                {
                    return "site";
                }
                if ((object?)value is SitesValue || (object?)value is SitesArrayValue)
                {
                    return "site collection";
                }
                if ((object?)value is ShortcodeValue)
                {
                    return "shortcode";
                }
                if ((object?)value is StringArrayValue)
                {
                    return "string collection";
                }
                if ((object?)value is AnyArrayValue)
                {
                    return "value collection";
                }
                if ((object?)value is MenuEntryValue)
                {
                    return "menu entry";
                }
                if ((object?)value is MenuArrayValue || (object?)value is MenusValue)
                {
                    return "menu collection";
                }
                if ((object?)value is TaxonomiesValue || (object?)value is TaxonomyTermsValue)
                {
                    return "taxonomy collection";
                }
                return "unsupported template value";
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
