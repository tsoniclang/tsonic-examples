using System;

namespace Tsumo.Cli
{
    public static class CliMain
    {
        public static string VERSION
        {
            get;
            private set;
        } = default(string)!;
        public static void main()
        {
            Tsonic.CSharp.Js.JSArray<string> args = Tsonic.CSharp.Js.Array.slice(Tsonic.CSharp.Node.process.argv, 2);
            string first = "";
            foreach (string arg in args)
            {
                first = arg;
                break;
            }
            if (first == "-h" || first == "--help" || first == "help")
            {
                PrintUsage.printUsage();
                return;
            }
            if (first == "-v" || first == "--version" || first == "version")
            {
                LogLine.logLine(VERSION);
                return;
            }
            string cmd = first == "" || Tsonic.CSharp.Js.String.startsWith(first, "-") ? "build" : first;
            if (cmd == "new")
            {
                Commands_handleNew.handleNew(args);
                return;
            }
            if (cmd == "server" || cmd == "serve")
            {
                Commands_handleServe.handleServe(args);
                return;
            }
            if (cmd == "build" || cmd == "gen" || cmd == "generate")
            {
            }
            else
            {
                LogErrorLine.logErrorLine($"Unknown command: {cmd}");
                PrintUsage.printUsage();
                Tsonic.CSharp.Node.process.exitCode = 2;
                return;
            }
            int buildArgStart = first == "build" || first == "gen" || first == "generate" ? 1 : 0;
            Commands_handleBuild.handleBuild(args, buildArgStart);
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            LogErrorLine.__tsonic_module_init();
            LogLine.__tsonic_module_init();
            PrintUsage.__tsonic_module_init();
            Commands_handleBuild.__tsonic_module_init();
            Commands_handleNew.__tsonic_module_init();
            Commands_handleServe.__tsonic_module_init();
            VERSION = "0.0.0";
            try
            {
                main();
            }
            catch (System.Exception __tsonic_catch0)
            {
                Tsonic.CSharp.Runtime.TsValue error = Tsonic.CSharp.Runtime.TsThrownValueException.toValue(__tsonic_catch0);
                LogErrorLine.logErrorLine(Tsonic.CSharp.Runtime.TsValue.IsDynamicInstanceOf<TsumoError>(error) ? Tsonic.CSharp.Runtime.TsValue.CastDynamic<TsumoError>(error).diagnostic.format() : $"{error}");
                Tsonic.CSharp.Node.process.exitCode = 1;
            }
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
