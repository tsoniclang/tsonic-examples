using System;

namespace Tsumo.Cli
{
    public static class LogErrorLine
    {
        public static Action<string> logErrorLine
        {
            get;
            private set;
        } = default(Action<string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            logErrorLine = (string message) =>
            {
                Tsonic.CSharp.Js.console.error(message);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
