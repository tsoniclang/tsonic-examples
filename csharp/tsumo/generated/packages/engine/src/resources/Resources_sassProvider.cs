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
            Diagnostics.__tsonic_module_init();
            Resources_models.__tsonic_module_init();
            Resources_externalProcess.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            Resources_text.__tsonic_module_init();
            compileSassResource = (Resource resource, Tsonic.CSharp.Js.JSArray<string> loadPaths) =>
            {
                string sourceText = Resources_text.readResourceText(resource, "css.Sass");
                string? configuredExecutable = System.Environment.GetEnvironmentVariable("TSUMO_SASS");
                string executable = configuredExecutable is not null && Tsonic.CSharp.Js.String.trim(configuredExecutable) != "" ? Tsonic.CSharp.Js.String.trim(configuredExecutable) : "sass";
                string? configuredImplementation = System.Environment.GetEnvironmentVariable("TSUMO_SASS_IMPLEMENTATION");
                string implementation = configuredImplementation is null || Tsonic.CSharp.Js.String.trim(configuredImplementation) == "" ? "dart-sass" : Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(configuredImplementation));
                if (implementation != "dart-sass" && implementation != "libsass")
                {
                    throw Diagnostics.createTsumoError("TSUMO_SASS_IMPLEMENTATION_INVALID", $"Unsupported Sass implementation '{implementation}'; expected 'dart-sass' or 'libsass'");
                }
                string workDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"tsumo-sass-{System.Guid.NewGuid().ToString("n")}");
                System.IO.Directory.CreateDirectory(workDirectory);
                try
                {
                    string inputPath = System.IO.Path.Combine(workDirectory, "input.scss");
                    string outputPath = System.IO.Path.Combine(workDirectory, "output.css");
                    System.IO.File.WriteAllText(inputPath, sourceText);
                    Tsonic.CSharp.Js.JSArray<string> argumentsList = implementation == "dart-sass" ? new Tsonic.CSharp.Js.JSArray<string>(new string[] { "--no-source-map", "--style", "expanded" }) : new Tsonic.CSharp.Js.JSArray<string>(new string[] { "-t", "expanded" });
                    for (int index = 0; index < loadPaths.length; index++)
                    {
                        string loadPath = loadPaths[index];
                        if (!System.IO.Directory.Exists(loadPath))
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
                    if (!System.IO.File.Exists(outputPath))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_SASS_OUTPUT_MISSING", "Sass compiler completed without producing CSS");
                    }
                    string text = System.IO.File.ReadAllText(outputPath);
                    string outputPathRaw = resource.outputRelPath ?? "style.scss";
                    ResourcePathParts path = Resources_paths.splitResourcePath(outputPathRaw);
                    ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                    return new Resource($"{resource.id}|sass", resource.sourcePath, true, path.directory + file.baseName + ".css", Tsonic.CSharp.Node.Buffer.from(text, "utf8"), text, resource.Data, "text/css");
                }
                finally
                {
                    if (System.IO.Directory.Exists(workDirectory))
                    {
                        System.IO.Directory.Delete(workDirectory, true);
                    }
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
