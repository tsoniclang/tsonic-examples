using System;

namespace Tsumo.Cli
{
    public static class PrintUsage
    {
        public static Action printUsage
        {
            get;
            private set;
        } = default(Action)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            LogLine.__tsonic_module_init();
            printUsage = () =>
            {
                LogLine.logLine("tsumo - Hugo-inspired blog engine (Tsonic)");
                LogLine.logLine("");
                LogLine.logLine("USAGE:");
                LogLine.logLine("  tsumo [build] [options]");
                LogLine.logLine("  tsumo server [options]");
                LogLine.logLine("  tsumo new site <dir>");
                LogLine.logLine("  tsumo new <path.md> [--source <dir>]");
                LogLine.logLine("  tsumo version");
                LogLine.logLine("");
                LogLine.logLine("BUILD OPTIONS:");
                LogLine.logLine("  -s, --source <dir>         Site directory (default: cwd)");
                LogLine.logLine("  -d, --destination <dir>    Output directory (default: public)");
                LogLine.logLine("  -D, --buildDrafts          Include drafts");
                LogLine.logLine("  --baseURL <url>            Override baseURL");
                LogLine.logLine("  --themesDir <dir>          Themes directory (like Hugo --themesDir)");
                LogLine.logLine("  --no-clean                 Do not wipe destination dir");
                LogLine.logLine("");
                LogLine.logLine("SERVER OPTIONS:");
                LogLine.logLine("  -s, --source <dir>         Site directory (default: cwd)");
                LogLine.logLine("  -p, --port <port>          Port (default: 1313)");
                LogLine.logLine("  --host <host>              Host (default: localhost)");
                LogLine.logLine("  --watch / --no-watch       Watch and rebuild (default: on)");
                LogLine.logLine("  -D, --buildDrafts          Include drafts");
                LogLine.logLine("  --themesDir <dir>          Themes directory (like Hugo --themesDir)");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
