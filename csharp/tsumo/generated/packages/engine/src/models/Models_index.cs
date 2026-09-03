namespace Tsumo.Engine
{
    public static class Models_index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models_menuEntry.__tsonic_module_init();
            Models_siteConfig.__tsonic_module_init();
            Models_siteContext.__tsonic_module_init();
            Models_pageContext.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
