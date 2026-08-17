using System;

namespace Tsumo.Cli
{
    public static class ParseInt
    {
        public static Func<string, int?> parseIntArg
        {
            get;
            private set;
        } = default(Func<string, int?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            parseIntArg = (string value) =>
            {
                string trimmed = Tsonic.CSharp.Js.String.trim(value);
                if (!new Tsonic.CSharp.Js.RegExp("^-?\\d+$", "").test(trimmed))
                {
                    return null;
                }
                double parsed = Tsonic.CSharp.Js.Number.parseInt(trimmed, 10);
                if (Tsonic.CSharp.Js.Number.isInteger(parsed) && parsed >= -2147483648 && parsed <= 2147483647)
                {
                    return (int)parsed;
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
