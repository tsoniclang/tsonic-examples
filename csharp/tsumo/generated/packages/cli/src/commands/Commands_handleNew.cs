using System;

namespace Tsumo.Cli
{
    public static class Commands_handleNew
    {
        public static Action<Tsonic.CSharp.Js.JSArray<string>> handleNew
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<string>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            LogLine.__tsonic_module_init();
            ReportUsageError.__tsonic_module_init();
            SourceDateEpoch.__tsonic_module_init();
            handleNew = (Tsonic.CSharp.Js.JSArray<string> args) =>
            {
                if (args.length >= 2 && args[1] == "site")
                {
                    if (args.length < 3)
                    {
                        ReportUsageError.reportUsageError("Missing <dir> for `tsumo new site`");
                        return;
                    }
                    if (args.length > 3)
                    {
                        ReportUsageError.reportUsageError($"Unknown new site option: {args[3]}");
                        return;
                    }
                    string dir = args[2];
                    Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(dir, SourceDateEpoch.readSourceDateEpoch());
                    LogLine.logLine($"Created site: {dir}");
                    return;
                }
                if (args.length < 2)
                {
                    ReportUsageError.reportUsageError("Missing <path.md> for `tsumo new`");
                    return;
                }
                string contentSourceDir = Tsonic.CSharp.Node.process.cwd();
                for (int i = 2; i < args.length; i++)
                {
                    string a = args[i];
                    if (a == "--source" || a == "-s")
                    {
                        if (i + 1 >= args.length)
                        {
                            ReportUsageError.reportUsageError($"Missing value for {a}");
                            return;
                        }
                        contentSourceDir = args[i + 1];
                        i++;
                    }
                    else
                    {
                        ReportUsageError.reportUsageError($"Unknown new option: {a}");
                        return;
                    }
                }
                string created = Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(contentSourceDir, args[1], SourceDateEpoch.readSourceDateEpoch());
                LogLine.logLine($"Created content: {created}");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
