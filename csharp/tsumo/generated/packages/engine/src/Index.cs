using System;

namespace Tsumo.Engine
{
    public static class Index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Scaffold_initSite.__tsonic_module_init();
            Scaffold_newContent.__tsonic_module_init();
            BuildSite.__tsonic_module_init();
            ServeSite.__tsonic_module_init();
            Models.__tsonic_module_init();
            Diagnostics.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
