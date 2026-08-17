using System;

namespace Tsumo.Engine
{
    public static class Resources_paths
    {
        public static Func<string, string> normalizeResourceSlashes
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> normalizeResourceRelativePath
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> resourcePathToOsPath
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string, string> resolveContainedResourcePath
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        public static Func<string, ResourcePathParts> splitResourcePath
        {
            get;
            private set;
        } = default(Func<string, ResourcePathParts>)!;
        public static Func<string, ResourceFileNameParts> splitResourceFileName
        {
            get;
            private set;
        } = default(Func<string, ResourceFileNameParts>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_paths.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            normalizeResourceSlashes = (string path) => Tsonic.CSharp.Js.String.replaceAll(path, "\\", "/");
            normalizeResourceRelativePath = (string path) =>
            {
                string normalized = normalizeResourceSlashes(Tsonic.CSharp.Js.String.trim(path));
                while (Tsonic.CSharp.Js.String.startsWith(normalized, "/"))
                {
                    normalized = Utils_strings.substringFrom(normalized, 1);
                }
                bool driveQualified = normalized.Length >= 2 && Utils_strings.substringCount(normalized, 1, 1) == ":";
                if (Tsonic.CSharp.Node.path.isAbsolute(normalized) || driveQualified)
                {
                    throw Diagnostics.createTsumoError("TSUMO_RESOURCE_PATH_ABSOLUTE", $"Resource path must be source-root relative: {path}");
                }
                Tsonic.CSharp.Js.JSArray<string> segments = Tsonic.CSharp.Js.String.split(normalized, "/");
                Tsonic.CSharp.Js.JSArray<string> accepted = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int index = 0; index < segments.length; index++)
                {
                    string segment = segments[index];
                    if (segment == "" || segment == ".")
                    {
                        continue;
                    }
                    if (segment == "..")
                    {
                        throw Diagnostics.createTsumoError("TSUMO_RESOURCE_PATH_ESCAPES_ROOT", $"Resource path escapes its root: {path}");
                    }
                    if (Tsonic.CSharp.Js.String.includes(segment, "\0"))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_RESOURCE_PATH_INVALID", "Resource path contains a null character");
                    }
                    accepted.push(segment);
                }
                return Tsonic.CSharp.Js.Array.join(accepted, "/");
            };
            resourcePathToOsPath = (string relativePath) => Utils_strings.replaceText(relativePath, "/", $"{Tsonic.CSharp.Node.path.sep}");
            resolveContainedResourcePath = (string root, string relativePath) =>
            {
                string normalized = normalizeResourceRelativePath(relativePath);
                string rootPath = Tsonic.CSharp.Node.path.resolve(root);
                string candidate = Tsonic.CSharp.Node.path.resolve(rootPath, resourcePathToOsPath(normalized));
                if (!Utils_paths.pathContainsOrEquals(rootPath, candidate))
                {
                    throw Diagnostics.createTsumoError("TSUMO_RESOURCE_PATH_ESCAPES_ROOT", $"Resource path escapes its root: {relativePath}");
                }
                return candidate;
            };
            splitResourcePath = (string relativePath) =>
            {
                string normalized = normalizeResourceRelativePath(relativePath);
                int index = Tsonic.CSharp.Js.String.lastIndexOf(normalized, "/");
                if (index < 0)
                {
                    return new ResourcePathParts("", normalized);
                }
                return new ResourcePathParts(Utils_strings.substringCount(normalized, 0, index + 1), Utils_strings.substringFrom(normalized, index + 1));
            };
            splitResourceFileName = (string fileName) =>
            {
                int index = Tsonic.CSharp.Js.String.lastIndexOf(fileName, ".");
                if (index < 0)
                {
                    return new ResourceFileNameParts(fileName, "");
                }
                return new ResourceFileNameParts(Utils_strings.substringCount(fileName, 0, index), Utils_strings.substringFrom(fileName, index));
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ResourcePathParts
    {
        public string directory;
        public string fileName;
        public ResourcePathParts(string directory, string fileName)
        {
            this.directory = directory;
            this.fileName = fileName;
        }
    }
    public class ResourceFileNameParts
    {
        public string baseName;
        public string extension;
        public ResourceFileNameParts(string baseName, string extension)
        {
            this.baseName = baseName;
            this.extension = extension;
        }
    }
}
