using System;

namespace Tsumo.Cli
{
    public static class SourceDateEpoch
    {
        public static Func<Tsonic.CSharp.Js.Date?> readSourceDateEpoch
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Date?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            readSourceDateEpoch = () =>
            {
                string? raw = Tsonic.CSharp.Node.process.env["SOURCE_DATE_EPOCH"];
                if (raw is null)
                {
                    return null;
                }
                string value = Tsonic.CSharp.Js.String.trim(raw);
                if (!new Tsonic.CSharp.Js.RegExp("^\\d+$", "").test(value))
                {
                    throw Node_modules_Tsumo_engine_src_diagnostics.createTsumoError("TSUMO_SOURCE_DATE_EPOCH_INVALID", "SOURCE_DATE_EPOCH must be a non-negative integer number of seconds");
                }
                double seconds = Tsonic.CSharp.Js.Number.parseFloat(value);
                if (!Tsonic.CSharp.Js.Number.isSafeInteger(seconds))
                {
                    throw Node_modules_Tsumo_engine_src_diagnostics.createTsumoError("TSUMO_SOURCE_DATE_EPOCH_OUT_OF_RANGE", "SOURCE_DATE_EPOCH is outside the supported integer range");
                }
                double milliseconds = seconds * 1000;
                if (!Tsonic.CSharp.Js.Number.isFinite(milliseconds) || milliseconds > 253402300799000)
                {
                    throw Node_modules_Tsumo_engine_src_diagnostics.createTsumoError("TSUMO_SOURCE_DATE_EPOCH_OUT_OF_RANGE", "SOURCE_DATE_EPOCH is outside the supported date range");
                }
                return new Tsonic.CSharp.Js.Date(milliseconds);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
