using System;

namespace Tsumo.Engine
{
    public static class Utils_urlPath
    {
        public static Func<Tsonic.CSharp.Js.JSArray<string>, string> combineUrlPath
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<string>, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            combineUrlPath = (Tsonic.CSharp.Js.JSArray<string> parts) =>
            {
                string slash = "/";
                Tsonic.CSharp.Js.JSArray<string> cleaned = parts.map((string part, int _, Tsonic.CSharp.Js.JSArray<string> _) => Utils_strings.trimEndChar(Utils_strings.trimStartChar(Tsonic.CSharp.Js.String.trim(part), slash), slash)).filter((string part, int _, Tsonic.CSharp.Js.JSArray<string> _) => part != "");
                return cleaned.length == 0 ? "/" : "/" + Tsonic.CSharp.Js.Array.join(cleaned, "/") + "/";
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
