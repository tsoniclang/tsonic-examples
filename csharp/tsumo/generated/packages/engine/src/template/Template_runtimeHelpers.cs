using System;

namespace Tsumo.Engine
{
    public static class Template_runtimeHelpers
    {
        public static TemplateValue nil
        {
            get;
            private set;
        } = default(TemplateValue)!;
        public static Func<TemplateValue, bool> isTemplateMap
        {
            get;
            private set;
        } = default(Func<TemplateValue, bool>)!;
        public static Func<TemplateValue, bool> isTemplateSlice
        {
            get;
            private set;
        } = default(Func<TemplateValue, bool>)!;
        public static Func<TemplateValue, bool> isTruthy
        {
            get;
            private set;
        } = default(Func<TemplateValue, bool>)!;
        public static Func<TemplateValue, bool> isDefaultSet
        {
            get;
            private set;
        } = default(Func<TemplateValue, bool>)!;
        public static Func<TemplateValue, bool, string> stringify
        {
            get;
            private set;
        } = default(Func<TemplateValue, bool, string>)!;
        public static Func<TemplateValue, string> toPlainString
        {
            get;
            private set;
        } = default(Func<TemplateValue, string>)!;
        public static Func<TemplateValue, int> toNumber
        {
            get;
            private set;
        } = default(Func<TemplateValue, int>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_html.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            nil = new NilValue();
            isTemplateMap = (TemplateValue value) => (object?)value is DictValue || (object?)value is MenusValue || (object?)value is TaxonomiesValue || (object?)value is TaxonomyTermsValue;
            isTemplateSlice = (TemplateValue value) => (object?)value is AnyArrayValue || (object?)value is DocsMountArrayValue || (object?)value is MenuArrayValue || (object?)value is NavArrayValue || (object?)value is PageArrayValue || (object?)value is SitesArrayValue || (object?)value is StringArrayValue;
            isTruthy = (TemplateValue value) =>
            {
                if ((object?)value is NilValue)
                {
                    return false;
                }
                if ((object?)value is BoolValue)
                {
                    return ((BoolValue)value).value;
                }
                if ((object?)value is NumberValue)
                {
                    return ((NumberValue)value).value != 0;
                }
                if ((object?)value is StringValue)
                {
                    return ((StringValue)value).value != "";
                }
                if ((object?)value is HtmlValue)
                {
                    return ((HtmlValue)value).value.value != "";
                }
                if ((object?)value is DateValue)
                {
                    return Tsonic.CSharp.Js.String.trim(((DateValue)value).value) != "";
                }
                if ((object?)value is DictValue)
                {
                    return ((DictValue)value).value.size > 0;
                }
                if ((object?)value is PageArrayValue)
                {
                    return ((PageArrayValue)value).value.length > 0;
                }
                if ((object?)value is StringArrayValue)
                {
                    return ((StringArrayValue)value).value.length > 0;
                }
                if ((object?)value is SitesArrayValue)
                {
                    return ((SitesArrayValue)value).value.length > 0;
                }
                if ((object?)value is DocsMountArrayValue)
                {
                    return ((DocsMountArrayValue)value).value.length > 0;
                }
                if ((object?)value is NavArrayValue)
                {
                    return ((NavArrayValue)value).value.length > 0;
                }
                if ((object?)value is AnyArrayValue)
                {
                    return ((AnyArrayValue)value).value.length > 0;
                }
                return true;
            };
            isDefaultSet = (TemplateValue value) =>
            {
                if ((object?)value is NilValue)
                {
                    return false;
                }
                if ((object?)value is BoolValue)
                {
                    return true;
                }
                if ((object?)value is NumberValue)
                {
                    return ((NumberValue)value).value != 0;
                }
                if ((object?)value is StringValue)
                {
                    return ((StringValue)value).value != "";
                }
                if ((object?)value is HtmlValue)
                {
                    return ((HtmlValue)value).value.value != "";
                }
                if ((object?)value is DateValue)
                {
                    return Tsonic.CSharp.Js.String.trim(((DateValue)value).value) != "";
                }
                if ((object?)value is DictValue)
                {
                    return ((DictValue)value).value.size > 0;
                }
                if ((object?)value is PageArrayValue)
                {
                    return ((PageArrayValue)value).value.length > 0;
                }
                if ((object?)value is StringArrayValue)
                {
                    return ((StringArrayValue)value).value.length > 0;
                }
                if ((object?)value is SitesArrayValue)
                {
                    return ((SitesArrayValue)value).value.length > 0;
                }
                if ((object?)value is DocsMountArrayValue)
                {
                    return ((DocsMountArrayValue)value).value.length > 0;
                }
                if ((object?)value is NavArrayValue)
                {
                    return ((NavArrayValue)value).value.length > 0;
                }
                if ((object?)value is AnyArrayValue)
                {
                    return ((AnyArrayValue)value).value.length > 0;
                }
                return true;
            };
            stringify = (TemplateValue value, bool escape) =>
            {
                if ((object?)value is DeferredTemplateValue)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_CONTEXT_INVALID", "templates.Defer can only be evaluated by a with block");
                }
                if ((object?)value is NilValue)
                {
                    return "";
                }
                if ((object?)value is HtmlValue)
                {
                    return ((HtmlValue)value).value.value;
                }
                if ((object?)value is StringValue)
                {
                    string s = ((StringValue)value).value;
                    return escape ? Utils_html.escapeHtml(s) : s;
                }
                if ((object?)value is BoolValue)
                {
                    return ((BoolValue)value).value ? "true" : "false";
                }
                if ((object?)value is NumberValue)
                {
                    return $"{((NumberValue)value).value}";
                }
                if ((object?)value is DateValue)
                {
                    return escape ? Utils_html.escapeHtml(((DateValue)value).value) : ((DateValue)value).value;
                }
                return "";
            };
            toPlainString = (TemplateValue value) =>
            {
                if ((object?)value is DeferredTemplateValue)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_CONTEXT_INVALID", "templates.Defer cannot be converted to text outside a with block");
                }
                if ((object?)value is StringValue)
                {
                    return ((StringValue)value).value;
                }
                if ((object?)value is HtmlValue)
                {
                    return ((HtmlValue)value).value.value;
                }
                if ((object?)value is DateValue)
                {
                    return ((DateValue)value).value;
                }
                if ((object?)value is BoolValue)
                {
                    return ((BoolValue)value).value ? "true" : "false";
                }
                if ((object?)value is NumberValue)
                {
                    return $"{((NumberValue)value).value}";
                }
                if ((object?)value is PageValue)
                {
                    return ((PageValue)value).value.relPermalink;
                }
                if ((object?)value is VersionStringValue)
                {
                    return ((VersionStringValue)value).value;
                }
                return "";
            };
            toNumber = (TemplateValue value) =>
            {
                if ((object?)value is NumberValue)
                {
                    return ((NumberValue)value).value;
                }
                if ((object?)value is StringValue)
                {
                    return Utils_int32.parseInt32(((StringValue)value).value) ?? 0;
                }
                if ((object?)value is BoolValue)
                {
                    return ((BoolValue)value).value ? 1 : 0;
                }
                return 0;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
