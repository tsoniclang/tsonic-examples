using System;

namespace Tsumo.Engine
{
    public static class Utils_html
    {
        public static Func<string, string> escapeHtml
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> decodeHtml
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            escapeHtml = (string input) =>
            {
                string s = input;
                s = Utils_strings.replaceText(s, "&", "&amp;");
                s = Utils_strings.replaceText(s, "<", "&lt;");
                s = Utils_strings.replaceText(s, ">", "&gt;");
                s = Utils_strings.replaceText(s, "\"", "&quot;");
                s = Utils_strings.replaceText(s, "'", "&#39;");
                return s;
            };
            decodeHtml = (string input) => System.Net.WebUtility.HtmlDecode(input) ?? "";
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class HtmlString
    {
        public string value;
        public HtmlString(string value)
        {
            this.value = value;
        }
    }
}
