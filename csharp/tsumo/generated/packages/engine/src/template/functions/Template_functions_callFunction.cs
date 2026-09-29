namespace Tsumo.Engine
{
    public static class Template_functions_callFunction
    {
        public static TemplateValue callTemplateFunction(string nameRaw, Tsonic.CSharp.Js.JSArray<TemplateValue> args, RenderScope scope, TemplateEnvironment environment, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> overrides, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> defines)
        {
            string name = Template_functions_functionRegistry.canonicalTemplateFunctionName(Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(nameRaw)));
            TemplateFunctionContext context = new TemplateFunctionContext(scope, environment, overrides, defines);
            TemplateValue? result = Template_functions_contextFunctions.callContextFunction(nameRaw, name, args, context);
            if (result is not null)
            {
                return result;
            }
            result = Template_functions_resourceFunctions.callResourceFunction(name, args, context);
            if (result is not null)
            {
                return result;
            }
            result = Template_functions_templateFunctions.callTemplateFunctionFamily(name, args, context);
            if (result is not null)
            {
                return result;
            }
            result = Template_functions_collectionFunctions.callCollectionFunction(name, args, context);
            if (result is not null)
            {
                return result;
            }
            result = Template_functions_scalarFunctions.callScalarFunction(name, args, context);
            if (result is not null)
            {
                return result;
            }
            if (Template_functions_functionRegistry.isKnownTemplateFunction(name))
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_FUNCTION_ARGUMENTS_INVALID", $"Template function '{nameRaw}' does not accept the supplied arguments", context.scope.templateSourcePath);
            }
            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNKNOWN_FUNCTION", $"Unknown template function: {nameRaw}", context.scope.templateSourcePath);
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_functions_collectionFunctions.__tsonic_module_init();
            Template_functions_contextFunctions.__tsonic_module_init();
            Template_functions_functionRegistry.__tsonic_module_init();
            Template_functions_resourceFunctions.__tsonic_module_init();
            Template_functions_scalarFunctions.__tsonic_module_init();
            Template_functions_templateFunctions.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
