using System;

namespace Tsumo.Engine
{
    public static class Template_values_deferred
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_values_base.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DeferredTemplateValue : TemplateValue
    {
        public string? key;
        public TemplateValue data;
        public DeferredTemplateValue(string? key, TemplateValue data) : base()
        {
            this.key = key;
            this.data = data;
        }
    }
}
