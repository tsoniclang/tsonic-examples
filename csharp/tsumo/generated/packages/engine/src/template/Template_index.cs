using System;

namespace Tsumo.Engine
{
    public static class Template_index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_values.__tsonic_module_init();
            Template_contexts.__tsonic_module_init();
            Template_scope.__tsonic_module_init();
            Template_environment.__tsonic_module_init();
            Template_nodes.__tsonic_module_init();
            Template_template.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_syntax_expressions.__tsonic_module_init();
            Template_parser_parseTemplate.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
