using System;

namespace Tsumo.Engine
{
    public static class Resources_javascriptProvider
    {
        public static Func<string, string> cacheKeyPart
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<Resource, string> sourceExtension
        {
            get;
            private set;
        } = default(Func<Resource, string>)!;
        public static Func<Resource, JavaScriptBuildOptions, string> outputRelativePath
        {
            get;
            private set;
        } = default(Func<Resource, JavaScriptBuildOptions, string>)!;
        public static Func<Resource, JavaScriptBuildOptions, Resource> buildJavaScriptResource
        {
            get;
            private set;
        } = default(Func<Resource, JavaScriptBuildOptions, Resource>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Resources_externalProcess.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            cacheKeyPart = (string value) => $"{value.Length}:{value}";
            sourceExtension = (Resource resource) =>
            {
                string raw = resource.outputRelPath ?? resource.sourcePath ?? "input.js";
                string extension = Tsonic.CSharp.Js.String.toLowerCase(Resources_paths.splitResourceFileName(Resources_paths.splitResourcePath(raw).fileName).extension);
                if (extension == ".ts" || extension == ".tsx" || extension == ".jsx")
                {
                    return extension;
                }
                return ".js";
            };
            outputRelativePath = (Resource resource, JavaScriptBuildOptions options) =>
            {
                string raw = options.targetPath ?? resource.outputRelPath ?? "script.js";
                ResourcePathParts path = Resources_paths.splitResourcePath(raw);
                ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                return path.directory + file.baseName + ".js";
            };
            buildJavaScriptResource = (Resource resource, JavaScriptBuildOptions options) =>
            {
                string sourceText = Resources_text.readResourceText(resource, "js.Build");
                if (options.sourceMap != "none")
                {
                    throw Diagnostics.createTsumoError("TSUMO_JAVASCRIPT_SOURCE_MAP_UNSUPPORTED", "js.Build currently supports only sourceMap 'none'");
                }
                string? configuredExecutable = Tsonic.CSharp.Node.process.env["TSUMO_ESBUILD"];
                string executable = configuredExecutable is not null && Tsonic.CSharp.Js.String.trim(configuredExecutable) != "" ? Tsonic.CSharp.Js.String.trim(configuredExecutable) : "esbuild";
                string workDirectory = Tsonic.CSharp.Node.fs.mkdtempSync(Tsonic.CSharp.Node.path.join(Tsonic.CSharp.Node.os.tmpdir(), "tsumo-esbuild-"));
                try
                {
                    string inputPath = Tsonic.CSharp.Node.path.join(workDirectory, "input" + sourceExtension(resource));
                    string? sourcePath = resource.sourcePath;
                    if (sourcePath is not null && Fs.fileExists(sourcePath) && Fs.readTextFile(sourcePath) == sourceText)
                    {
                        inputPath = sourcePath;
                    }
                    else
                    {
                        Tsonic.CSharp.Node.fs.writeFileSync(inputPath, sourceText, "utf8");
                    }
                    string outputPath = Tsonic.CSharp.Node.path.join(workDirectory, "output.js");
                    Tsonic.CSharp.Js.JSArray<string> argumentsList = new Tsonic.CSharp.Js.JSArray<string>(new string[] { inputPath, "--bundle", $"--outfile={outputPath}", $"--format={options.format}", $"--target={options.target}", $"--platform={options.platform}", "--charset=utf8", "--log-level=warning" });
                    if (options.minify)
                    {
                        argumentsList.push("--minify");
                    }
                    string? jsxFactory = options.jsxFactory;
                    if (jsxFactory is not null)
                    {
                        argumentsList.push($"--jsx-factory={jsxFactory}");
                    }
                    string? paramsJson = options.paramsJson;
                    if (paramsJson is not null)
                    {
                        string paramsPath = Tsonic.CSharp.Node.path.join(workDirectory, "params.json");
                        Tsonic.CSharp.Node.fs.writeFileSync(paramsPath, paramsJson, "utf8");
                        argumentsList.push($"--alias:@params={paramsPath}");
                    }
                    ExternalProcessResult process = Resources_externalProcess.runExternalProcess(executable, argumentsList, "esbuild", "TSUMO_ESBUILD_START_FAILED");
                    if (process.exitCode != 0)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_ESBUILD_FAILED", process.standardError == "" ? $"esbuild failed with exit code {process.exitCode}" : process.standardError);
                    }
                    if (!Tsonic.CSharp.Node.fs.existsSync(outputPath))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_ESBUILD_OUTPUT_MISSING", "esbuild completed without producing JavaScript");
                    }
                    string text = Tsonic.CSharp.Node.fs.readFileSync(outputPath, "utf8");
                    return new Resource($"{resource.id}|js-build:{options.cacheKey()}", resource.sourcePath, true, outputRelativePath(resource, options), Tsonic.CSharp.Node.Buffer.from(text, "utf8"), text, resource.Data, "application/javascript");
                }
                finally
                {
                    Tsonic.CSharp.Node.fs.rmSync(workDirectory, new Tsonic.CSharp.Node.RmOptions
                    {
                        recursive = true,
                        force = true,
                    });
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class JavaScriptBuildOptions
    {
        public string? targetPath;
        public bool minify;
        public string format;
        public string target;
        public string platform;
        public string sourceMap;
        public string? paramsJson;
        public string? jsxFactory;
        public JavaScriptBuildOptions()
        {
            this.targetPath = null;
            this.minify = false;
            this.format = "iife";
            this.target = "esnext";
            this.platform = "browser";
            this.sourceMap = "none";
            this.paramsJson = null;
            this.jsxFactory = null;
        }
        public string cacheKey()
        {
            Tsonic.CSharp.Js.JSArray<string> values = new Tsonic.CSharp.Js.JSArray<string>(new string[] { this.targetPath ?? "", this.minify ? "1" : "0", this.format, this.target, this.platform, this.sourceMap, this.paramsJson ?? "", this.jsxFactory ?? "" });
            string result = "";
            for (int index = 0; index < values.length; index++)
            {
                result += Resources_javascriptProvider.cacheKeyPart(values[index]);
            }
            return result;
        }
    }
}
