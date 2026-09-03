namespace Tsumo.Engine
{
    public static class Frontmatter_index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Frontmatter_data.__tsonic_module_init();
            Frontmatter_parsedContent.__tsonic_module_init();
            Frontmatter_parse.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
