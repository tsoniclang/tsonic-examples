using System;

namespace Tsumo.Engine
{
    public static class Resources_externalProcess
    {
        public static Func<string, Tsonic.CSharp.Js.JSArray<string>, string, string, ExternalProcessResult> runExternalProcess
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<string>, string, string, ExternalProcessResult>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            runExternalProcess = (string executable, Tsonic.CSharp.Js.JSArray<string> argumentsList, string toolName, string startDiagnosticCode) =>
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = executable;
                for (int index = 0; index < argumentsList.length; index++)
                {
                    startInfo.ArgumentList.Add(argumentsList[index]);
                }
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                System.Diagnostics.Process? process = null;
                try
                {
                    process = System.Diagnostics.Process.Start(startInfo);
                }
                catch (System.Exception error)
                {
                    throw Diagnostics.createTsumoError(startDiagnosticCode, $"Failed to start {toolName} '{executable}': {error}");
                }
                if (process is null)
                {
                    throw Diagnostics.createTsumoError(startDiagnosticCode, $"Failed to start {toolName} '{executable}'");
                }
                string standardError = Tsonic.CSharp.Js.String.trim(process.StandardError.ReadToEnd());
                process.WaitForExit();
                return new ExternalProcessResult(process.ExitCode, standardError);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ExternalProcessResult
    {
        public int exitCode;
        public string standardError;
        public ExternalProcessResult(int exitCode, string standardError)
        {
            this.exitCode = exitCode;
            this.standardError = standardError;
        }
    }
}
