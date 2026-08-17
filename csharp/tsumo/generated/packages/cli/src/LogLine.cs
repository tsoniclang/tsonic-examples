using System;

namespace Tsumo.Cli
{
    public static class LogLine
    {
        public static Action<string> logLine
        {
            get;
            private set;
        } = default(Action<string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            logLine = (string message) =>
            {
                Tsonic.CSharp.Js.console.log(message);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
