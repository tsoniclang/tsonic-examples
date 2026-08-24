using System;

namespace Tsumo.Engine
{
    public static class Template_dataLoader
    {
        public static Func<string, string?> dataFormat
        {
            get;
            private set;
        } = default(Func<string, string?>)!;
        public static Func<string, string> normalizeDataPath
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Action<string, Tsonic.CSharp.Js.Map<string, SelectedDataFile>> collectDataLayer
        {
            get;
            private set;
        } = default(Action<string, Tsonic.CSharp.Js.Map<string, SelectedDataFile>>)!;
        public static Action<DictValue, string, TemplateValue, string> setDataPath
        {
            get;
            private set;
        } = default(Action<DictValue, string, TemplateValue, string>)!;
        public static Func<string, string?, Tsonic.CSharp.Js.JSArray<ModuleMount>?, DictValue> loadSiteData
        {
            get;
            private set;
        } = default(Func<string, string?, Tsonic.CSharp.Js.JSArray<ModuleMount>?, DictValue>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_evaluation_structuredData.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            dataFormat = (string path) =>
            {
                string extension = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Node.path.extname(path));
                if (extension == ".json")
                {
                    return "json";
                }
                if (extension == ".yaml" || extension == ".yml")
                {
                    return "yaml";
                }
                if (extension == ".toml")
                {
                    return "toml";
                }
                if (extension == ".xml")
                {
                    return "xml";
                }
                return null;
            };
            normalizeDataPath = (string path) => Utils_strings.replaceText(path, "\\", "/");
            collectDataLayer = (string root, Tsonic.CSharp.Js.Map<string, SelectedDataFile> selected) =>
            {
                if (!Fs.dirExists(root))
                {
                    return;
                }
                Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesRecursive(root, "*");
                Tsonic.CSharp.Js.Map<string, SelectedDataFile> layer = new Tsonic.CSharp.Js.Map<string, SelectedDataFile>();
                for (int index = 0; index < files.length; index++)
                {
                    string sourcePath = files[index];
                    string? format = dataFormat(sourcePath);
                    if (format is null)
                    {
                        continue;
                    }
                    string relativePath = normalizeDataPath(Tsonic.CSharp.Node.path.relative(root, sourcePath));
                    string extension = Tsonic.CSharp.Node.path.extname(relativePath);
                    string semanticPath = Tsonic.CSharp.Js.String.slice(relativePath, 0, relativePath.Length - extension.Length);
                    SelectedDataFile? existing = Tsonic.CSharp.Js.Map.getReference<string, SelectedDataFile>(layer, semanticPath);
                    if (existing is not null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_DATA_IDENTITY_CONFLICT", $"Data files '{existing.sourcePath}' and '{sourcePath}' define the same data identity '{semanticPath}'", sourcePath);
                    }
                    layer.set(semanticPath, new SelectedDataFile(semanticPath, sourcePath, format));
                }
                foreach (SelectedDataFile file in layer.values())
                {
                    selected.set(file.semanticPath, file);
                }
            };
            setDataPath = (DictValue root, string semanticPath, TemplateValue value, string sourcePath) =>
            {
                Tsonic.CSharp.Js.JSArray<string> segments = Tsonic.CSharp.Js.String.split(semanticPath, "/");
                DictValue current = root;
                for (int index = 0; index < segments.length - 1; index++)
                {
                    string segment = segments[index];
                    TemplateValue? existing = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(current.value, segment);
                    if (existing is null)
                    {
                        DictValue created = new DictValue(new Tsonic.CSharp.Js.Map<string, TemplateValue>());
                        current.value.set(segment, created);
                        current = created;
                        continue;
                    }
                    if (!(existing is DictValue))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_DATA_TREE_CONFLICT", $"Data identity '{semanticPath}' conflicts with a data file at '{Tsonic.CSharp.Js.Array.join(Tsonic.CSharp.Js.Array.slice(segments, 0, index + 1), "/")}'", sourcePath);
                    }
                    current = (DictValue)existing;
                }
                string name = segments[segments.length - 1];
                if (current.value.has(name))
                {
                    throw Diagnostics.createTsumoError("TSUMO_DATA_TREE_CONFLICT", $"Data identity '{semanticPath}' is declared more than once", sourcePath);
                }
                current.value.set(name, value);
            };
            loadSiteData = (string siteDir, string? themeDir, Tsonic.CSharp.Js.JSArray<ModuleMount>? mounts) =>
            {
                Tsonic.CSharp.Js.Map<string, SelectedDataFile> selected = new Tsonic.CSharp.Js.Map<string, SelectedDataFile>();
                if (themeDir is not null)
                {
                    collectDataLayer(Tsonic.CSharp.Node.path.join(themeDir, "data"), selected);
                }
                if (mounts is not null)
                {
                    for (int index = mounts.length - 1; index >= 0; index--)
                    {
                        ModuleMount mount = mounts[index];
                        string target = Utils_strings.trimEndChar(Utils_strings.trimStartChar(normalizeDataPath(mount.target), "/"), "/");
                        if (target != "data")
                        {
                            continue;
                        }
                        string root = Tsonic.CSharp.Node.path.isAbsolute(mount.source) ? mount.source : Tsonic.CSharp.Node.path.join(siteDir, mount.source);
                        collectDataLayer(root, selected);
                    }
                }
                collectDataLayer(Tsonic.CSharp.Node.path.join(siteDir, "data"), selected);
                Tsonic.CSharp.Js.JSArray<string> identities = Tsonic.CSharp.Js.JSArrayStatics.from<string>(selected.keys());
                identities.sort();
                DictValue root_1 = new DictValue(new Tsonic.CSharp.Js.Map<string, TemplateValue>());
                for (int index_1 = 0; index_1 < identities.length; index_1++)
                {
                    string identity = identities[index_1];
                    SelectedDataFile? file = Tsonic.CSharp.Js.Map.getReference<string, SelectedDataFile>(selected, identity);
                    if (file is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_DATA_SELECTION_INCONSISTENT", $"Selected data identity '{identity}' disappeared");
                    }
                    TemplateValue value = Template_evaluation_structuredData.parseTemplateDataText(Fs.readTextFile(file.sourcePath), file.format, file.sourcePath);
                    setDataPath(root_1, file.semanticPath, value, file.sourcePath);
                }
                return root_1;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class SelectedDataFile
    {
        public string semanticPath;
        public string sourcePath;
        public string format;
        public SelectedDataFile(string semanticPath, string sourcePath, string format)
        {
            this.semanticPath = semanticPath;
            this.sourcePath = sourcePath;
            this.format = format;
        }
    }
}
