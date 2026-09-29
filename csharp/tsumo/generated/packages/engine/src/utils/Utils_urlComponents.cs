namespace Tsumo.Engine
{
    public static class Utils_urlComponents
    {
        public static string encodeUrlComponent(string value)
        {
            return Tsonic.CSharp.Js.Globals.encodeURIComponent(value);
        }
        public static string decodeUrlComponent(string value)
        {
            return Tsonic.CSharp.Js.Globals.decodeURIComponent(value);
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
}
