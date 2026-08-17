using System;

namespace Tsumo.Cli
{
    public static class Commands_handleServe
    {
        public static Action<Tsonic.CSharp.Js.JSArray<string>> handleServe
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<string>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            ParseInt.__tsonic_module_init();
            ReportUsageError.__tsonic_module_init();
            SourceDateEpoch.__tsonic_module_init();
            handleServe = (Tsonic.CSharp.Js.JSArray<string> args) =>
            {
                string serveSourceDir = Tsonic.CSharp.Node.process.cwd();
                string serveDestinationDir = "public";
                string? serveBaseURL = null;
                string? serveThemesDir = null;
                string serveHost = "localhost";
                int servePort = 1313;
                bool serveWatch = true;
                bool serveBuildDrafts = false;
                bool serveClean = true;
                for (int i = 1; i < args.length; i++)
                {
                    string a = args[i];
                    if (a == "--source" || a == "-s")
                    {
                        if (i + 1 >= args.length)
                        {
                            ReportUsageError.reportUsageError($"Missing value for {a}");
                            return;
                        }
                        serveSourceDir = args[i + 1];
                        i++;
                    }
                    else
                    {
                        if (a == "--destination" || a == "-d")
                        {
                            if (i + 1 >= args.length)
                            {
                                ReportUsageError.reportUsageError($"Missing value for {a}");
                                return;
                            }
                            serveDestinationDir = args[i + 1];
                            i++;
                        }
                        else
                        {
                            if (a == "--baseURL" || a == "--baseurl")
                            {
                                if (i + 1 >= args.length)
                                {
                                    ReportUsageError.reportUsageError($"Missing value for {a}");
                                    return;
                                }
                                serveBaseURL = args[i + 1];
                                i++;
                            }
                            else
                            {
                                if (a == "--themesDir" || a == "--themesdir")
                                {
                                    if (i + 1 >= args.length)
                                    {
                                        ReportUsageError.reportUsageError($"Missing value for {a}");
                                        return;
                                    }
                                    serveThemesDir = args[i + 1];
                                    i++;
                                }
                                else
                                {
                                    if (a == "--host" || a == "--bind")
                                    {
                                        if (i + 1 >= args.length)
                                        {
                                            ReportUsageError.reportUsageError($"Missing value for {a}");
                                            return;
                                        }
                                        serveHost = args[i + 1];
                                        i++;
                                    }
                                    else
                                    {
                                        if (a == "--port" || a == "-p")
                                        {
                                            if (i + 1 >= args.length)
                                            {
                                                ReportUsageError.reportUsageError($"Missing value for {a}");
                                                return;
                                            }
                                            string portText = args[i + 1];
                                            int? p = ParseInt.parseIntArg(portText);
                                            if (p is null || p.Value < 1 || p.Value > 65535)
                                            {
                                                ReportUsageError.reportUsageError($"Invalid port: {portText}");
                                                return;
                                            }
                                            servePort = p.Value;
                                            i++;
                                        }
                                        else
                                        {
                                            if (a == "--watch")
                                            {
                                                serveWatch = true;
                                            }
                                            else
                                            {
                                                if (a == "--no-watch")
                                                {
                                                    serveWatch = false;
                                                }
                                                else
                                                {
                                                    if (a == "-D" || a == "--buildDrafts")
                                                    {
                                                        serveBuildDrafts = true;
                                                    }
                                                    else
                                                    {
                                                        if (a == "--no-clean")
                                                        {
                                                            serveClean = false;
                                                        }
                                                        else
                                                        {
                                                            if (a == "--clean")
                                                            {
                                                                serveClean = true;
                                                            }
                                                            else
                                                            {
                                                                ReportUsageError.reportUsageError($"Unknown server option: {a}");
                                                                return;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ServeRequest serveReq = new ServeRequest(serveSourceDir);
                serveReq.destinationDir = serveDestinationDir;
                serveReq.baseURL = serveBaseURL;
                serveReq.themesDir = serveThemesDir;
                serveReq.host = serveHost;
                serveReq.port = servePort;
                serveReq.watch = serveWatch;
                serveReq.buildDrafts = serveBuildDrafts;
                serveReq.cleanDestinationDir = serveClean;
                serveReq.buildTime = SourceDateEpoch.readSourceDateEpoch() ?? serveReq.buildTime;
                Node_modules_Tsumo_engine_src_serveSite.serveSite(serveReq);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
