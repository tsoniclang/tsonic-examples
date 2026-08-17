using System;

namespace Tsumo.Cli
{
    public static class ReportUsageError
    {
        public static Action<string> reportUsageError
        {
            get;
            private set;
        } = default(Action<string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            LogErrorLine.__tsonic_module_init();
            reportUsageError = (string message) =>
            {
                LogErrorLine.logErrorLine(message);
                Tsonic.CSharp.Node.process.exitCode = 2;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
