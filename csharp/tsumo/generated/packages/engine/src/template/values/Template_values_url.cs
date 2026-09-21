namespace Tsumo.Engine
{
    public static class Template_values_url
    {
        internal static string trimLeadingCharacter(string value, string character)
        {
            return Tsonic.CSharp.Js.String.startsWith(value, character) ? Tsonic.CSharp.Js.String.slice(value, character.Length) : value;
        }
        internal static string trimTrailingCharacter(string value, string character)
        {
            return Tsonic.CSharp.Js.String.endsWith(value, character) ? Tsonic.CSharp.Js.String.slice(value, 0, value.Length - character.Length) : value;
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ParsedUrl
    {
        public string originalString;
        public bool absolute;
        public string scheme;
        public string host;
        public string path;
        public string rawQuery;
        public string fragment;
        public ParsedUrl(string originalString, Tsonic.CSharp.Node.LegacyUrlObject value)
        {
            string protocol = value.protocol ?? "";
            string host = value.host ?? "";
            string pathname = value.pathname ?? "";
            string search = value.search ?? "";
            string hash = value.hash ?? "";
            this.originalString = originalString;
            this.absolute = protocol != "";
            this.scheme = Template_values_url.trimTrailingCharacter(protocol, ":");
            this.host = host;
            this.path = pathname;
            this.rawQuery = Template_values_url.trimLeadingCharacter(search, "?");
            this.fragment = Template_values_url.trimLeadingCharacter(hash, "#");
        }
    }
    public class UrlParts
    {
        public string path;
        public string rawQuery;
        public string fragment;
        public UrlParts(string path, string rawQuery, string fragment)
        {
            this.path = path;
            this.rawQuery = rawQuery;
            this.fragment = fragment;
        }
    }
    public class UrlValue : TemplateValue
    {
        public ParsedUrl value;
        public UrlValue(ParsedUrl value) : base()
        {
            this.value = value;
        }
    }
    public class UrlQueryValue : TemplateValue
    {
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>> value;
        public UrlQueryValue(Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>> value) : base()
        {
            this.value = value;
        }
    }
}
