using System;

namespace Tsumo.Engine
{
    public static class Resources_sassProvider
    {
        public static Func<Resource, Tsonic.CSharp.Js.JSArray<string>, Resource> compileSassResource
        {
            get;
            private set;
        } = default(Func<Resource, Tsonic.CSharp.Js.JSArray<string>, Resource>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Resources_externalProcess.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            compileSassResource = (Resource resource, Tsonic.CSharp.Js.JSArray<string> loadPaths) =>
            {
                string sourceText = Resources_text.readResourceText(resource, "css.Sass");
                string? configuredExecutable = Tsonic.CSharp.Node.process.env["TSUMO_SASS"];
                string executable = configuredExecutable is not null && Tsonic.CSharp.Js.String.trim(configuredExecutable) != "" ? Tsonic.CSharp.Js.String.trim(configuredExecutable) : "sass";
                string? configuredImplementation = Tsonic.CSharp.Node.process.env["TSUMO_SASS_IMPLEMENTATION"];
                string implementation = configuredImplementation is null || Tsonic.CSharp.Js.String.trim(configuredImplementation) == "" ? "dart-sass" : Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(configuredImplementation));
                if (implementation != "dart-sass" && implementation != "libsass")
                {
                    throw Diagnostics.createTsumoError("TSUMO_SASS_IMPLEMENTATION_INVALID", $"Unsupported Sass implementation '{implementation}'; expected 'dart-sass' or 'libsass'");
                }
                string workDirectory = Tsonic.CSharp.Node.fs.mkdtempSync(Tsonic.CSharp.Node.path.join(Tsonic.CSharp.Node.os.tmpdir(), "tsumo-sass-"));
                try
                {
                    string inputPath = Tsonic.CSharp.Node.path.join(workDirectory, "input.scss");
                    string outputPath = Tsonic.CSharp.Node.path.join(workDirectory, "output.css");
                    Tsonic.CSharp.Node.fs.writeFileSync(inputPath, sourceText, "utf8");
                    Tsonic.CSharp.Js.JSArray<string> argumentsList = implementation == "dart-sass" ? new Tsonic.CSharp.Js.JSArray<string>(new string[] { "--no-source-map", "--style", "expanded" }) : new Tsonic.CSharp.Js.JSArray<string>(new string[] { "-t", "expanded" });
                    for (int index = 0; index < loadPaths.length; index++)
                    {
                        string loadPath = loadPaths[index];
                        if (!Fs.dirExists(loadPath))
                        {
                            continue;
                        }
                        argumentsList.push(implementation == "dart-sass" ? "--load-path" : "-I");
                        argumentsList.push(loadPath);
                    }
                    argumentsList.push(inputPath);
                    argumentsList.push(outputPath);
                    ExternalProcessResult process = Resources_externalProcess.runExternalProcess(executable, argumentsList, "Sass compiler", "TSUMO_SASS_START_FAILED");
                    if (process.exitCode != 0)
                    {
                        string stderr = process.standardError;
                        throw Diagnostics.createTsumoError("TSUMO_SASS_FAILED", stderr == "" ? $"Sass compiler failed with exit code {process.exitCode}" : stderr);
                    }
                    if (!Tsonic.CSharp.Node.fs.existsSync(outputPath))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_SASS_OUTPUT_MISSING", "Sass compiler completed without producing CSS");
                    }
                    string text = Tsonic.CSharp.Node.fs.readFileSync(outputPath, "utf8");
                    string outputPathRaw = resource.outputRelPath ?? "style.scss";
                    ResourcePathParts path = Resources_paths.splitResourcePath(outputPathRaw);
                    ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                    return new Resource($"{resource.id}|sass", resource.sourcePath, true, path.directory + file.baseName + ".css", Tsonic.CSharp.Node.Buffer.from(text, "utf8"), text, resource.Data, "text/css");
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
}
