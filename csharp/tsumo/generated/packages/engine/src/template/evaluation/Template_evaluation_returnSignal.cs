using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_returnSignal
    {
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
    public class TemplateReturnSignal : Tsonic.CSharp.Runtime.Error
    {
        public TemplateValue value;
        public TemplateReturnSignal(TemplateValue value) : base("template return")
        {
            this.name = "TemplateReturnSignal";
            this.value = value;
        }
    }
}
