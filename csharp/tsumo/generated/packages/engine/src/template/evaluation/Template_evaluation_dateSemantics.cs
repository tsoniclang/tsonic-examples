namespace Tsumo.Engine
{
    public static class Template_evaluation_dateSemantics
    {
        public static TemplateValue? callDateMethod(TemplateValue receiver, string method, Tsonic.CSharp.Js.JSArray<TemplateValue> args)
        {
            if (!(receiver is DateValue))
            {
                return null;
            }
            if (method == "format")
            {
                if (args.length != 1)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_ARGUMENTS_INVALID", "Date.Format requires one layout argument");
                }
                return new StringValue(Template_evaluation_scalarSemantics.formatDateTime(((DateValue)receiver).value, Template_runtimeHelpers.toPlainString(args[0])) ?? "");
            }
            if (method == "adddate")
            {
                if (args.length != 3)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_ARGUMENTS_INVALID", "Date.AddDate requires year, month, and day offsets");
                }
                int? years = Utils_int32.parseInt32(Template_runtimeHelpers.toPlainString(args[0]));
                int? months = Utils_int32.parseInt32(Template_runtimeHelpers.toPlainString(args[1]));
                int? days = Utils_int32.parseInt32(Template_runtimeHelpers.toPlainString(args[2]));
                if (years is null || months is null || days is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_ARGUMENTS_INVALID", "Date.AddDate offsets must be 32-bit integers");
                }
                string? result = Template_evaluation_scalarSemantics.addCalendarDate(((DateValue)receiver).value, years.Value, months.Value, days.Value);
                if (result is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_INVALID", $"Date.AddDate cannot operate on '{((DateValue)receiver).value}'");
                }
                return new DateValue(result);
            }
            if (method == "after")
            {
                TemplateValue? other = args.length == 1 ? args[0] : null;
                if (!(other is DateValue))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_ARGUMENTS_INVALID", "Date.After requires one date argument");
                }
                bool? result_1 = Template_evaluation_scalarSemantics.isDateAfter(((DateValue)receiver).value, ((DateValue)other).value);
                if (result_1 is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DATE_INVALID", "Date.After requires two valid dates");
                }
                return new BoolValue(result_1.Value);
            }
            return null;
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_int32.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_evaluation_scalarSemantics.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
