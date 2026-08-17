using System;

namespace Tsumo.Engine
{
    public static class Template_functions_functionContext
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
    public class TemplateFunctionContext
    {
        public RenderScope scope;
        public TemplateEnvironment environment;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defines;
        public TemplateFunctionContext(RenderScope scope, TemplateEnvironment environment, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defines)
        {
            this.scope = scope;
            this.environment = environment;
            this.overrides = overrides;
            this.defines = defines;
        }
    }
}
