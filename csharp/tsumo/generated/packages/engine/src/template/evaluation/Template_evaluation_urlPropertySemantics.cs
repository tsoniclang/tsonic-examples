using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_urlPropertySemantics
    {
        public static Func<System.Uri, UrlParts> splitUrlParts
        {
            get;
            private set;
        } = default(Func<System.Uri, UrlParts>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            splitUrlParts = (System.Uri uri) =>
            {
                string rawQuery = "";
                string fragment = "";
                if (uri.IsAbsoluteUri)
                {
                    rawQuery = Tsonic.CSharp.Js.String.startsWith(uri.Query, "?") ? Utils_strings.substringFrom(uri.Query, 1) : uri.Query;
                    fragment = Tsonic.CSharp.Js.String.startsWith(uri.Fragment, "#") ? Utils_strings.substringFrom(uri.Fragment, 1) : uri.Fragment;
                    return new UrlParts(uri.AbsolutePath, rawQuery, fragment);
                }
                string raw = uri.OriginalString;
                int hashIndex = Tsonic.CSharp.Js.String.indexOf(raw, "#");
                string beforeHash = hashIndex >= 0 ? Utils_strings.substringCount(raw, 0, hashIndex) : raw;
                fragment = hashIndex >= 0 ? Utils_strings.substringFrom(raw, hashIndex + 1) : "";
                int queryIndex = Tsonic.CSharp.Js.String.indexOf(beforeHash, "?");
                string path = queryIndex >= 0 ? Utils_strings.substringCount(beforeHash, 0, queryIndex) : beforeHash;
                rawQuery = queryIndex >= 0 ? Utils_strings.substringFrom(beforeHash, queryIndex + 1) : "";
                return new UrlParts(path, rawQuery, fragment);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
