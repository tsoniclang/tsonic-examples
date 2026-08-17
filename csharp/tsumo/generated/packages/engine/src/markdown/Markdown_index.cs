using System;

namespace Tsumo.Engine
{
    public static class Markdown_index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Markdown_result.__tsonic_module_init();
            Markdown_pipeline.__tsonic_module_init();
            Markdown_toc.__tsonic_module_init();
            Markdown_renderHooks.__tsonic_module_init();
            Markdown_shortcodes.__tsonic_module_init();
            Markdown_renderBasic.__tsonic_module_init();
            Markdown_renderWithShortcodes.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
