using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_paramSemantics
    {
        public static Func<ParamValue, TemplateValue> paramToTemplateValue
        {
            get;
            private set;
        } = default(Func<ParamValue, TemplateValue>)!;
        public static Func<Tsonic.CSharp.Js.Map<string, ParamValue>, string, ParamValue?> findParam
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Map<string, ParamValue>, string, ParamValue?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            paramToTemplateValue = (ParamValue value) =>
            {
                if (value.kind == ParamKind.Bool)
                {
                    return new BoolValue(value.boolValue);
                }
                if (value.kind == ParamKind.Number)
                {
                    return new NumberValue(value.numberValue);
                }
                return new StringValue(value.stringValue);
            };
            findParam = (Tsonic.CSharp.Js.Map<string, ParamValue> values, string name) =>
            {
                ParamValue? exact = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(values, name);
                if (exact is not null)
                {
                    return exact;
                }
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(name);
                foreach (string key in values.keys())
                {
                    if (Tsonic.CSharp.Js.String.toLowerCase(key) == normalized)
                    {
                        return Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(values, key);
                    }
                }
                return null;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
