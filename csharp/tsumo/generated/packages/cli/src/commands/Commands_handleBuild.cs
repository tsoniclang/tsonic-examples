using System;

namespace Tsumo.Cli
{
    public static class Commands_handleBuild
    {
        public static Action<Tsonic.CSharp.Js.JSArray<string>, int> handleBuild
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<string>, int>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            LogLine.__tsonic_module_init();
            ReportUsageError.__tsonic_module_init();
            SourceDateEpoch.__tsonic_module_init();
            handleBuild = (Tsonic.CSharp.Js.JSArray<string> args, int buildArgStart) =>
            {
                string buildSourceDir = Tsonic.CSharp.Node.process.cwd();
                string buildDestinationDir = "public";
                string? buildBaseURL = null;
                string? buildThemesDir = null;
                bool includeDrafts = false;
                bool cleanDestinationDir = true;
                for (int i = buildArgStart; i < args.length; i++)
                {
                    string a = args[i];
                    if (a == "--source" || a == "-s")
                    {
                        if (i + 1 >= args.length)
                        {
                            ReportUsageError.reportUsageError($"Missing value for {a}");
                            return;
                        }
                        buildSourceDir = args[i + 1];
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
                            buildDestinationDir = args[i + 1];
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
                                buildBaseURL = args[i + 1];
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
                                    buildThemesDir = args[i + 1];
                                    i++;
                                }
                                else
                                {
                                    if (a == "-D" || a == "--buildDrafts")
                                    {
                                        includeDrafts = true;
                                    }
                                    else
                                    {
                                        if (a == "--no-clean")
                                        {
                                            cleanDestinationDir = false;
                                        }
                                        else
                                        {
                                            if (a == "--clean")
                                            {
                                                cleanDestinationDir = true;
                                            }
                                            else
                                            {
                                                ReportUsageError.reportUsageError($"Unknown build option: {a}");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                BuildRequest buildReq = new BuildRequest(buildSourceDir);
                buildReq.destinationDir = buildDestinationDir;
                buildReq.baseURL = buildBaseURL;
                buildReq.themesDir = buildThemesDir;
                buildReq.buildDrafts = includeDrafts;
                buildReq.cleanDestinationDir = cleanDestinationDir;
                buildReq.buildTime = SourceDateEpoch.readSourceDateEpoch() ?? buildReq.buildTime;
                BuildResult result = Node_modules_Tsumo_engine_src_buildSite.buildSite(buildReq);
                LogLine.logLine($"Built → {result.outputDir} ({result.pagesBuilt} pages)");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
