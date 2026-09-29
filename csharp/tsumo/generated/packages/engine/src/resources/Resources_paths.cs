namespace Tsumo.Engine
{
    public static class Resources_paths
    {
        public static string normalizeResourceSlashes(string path)
        {
            return Tsonic.CSharp.Js.String.replaceAll(path, "\\", "/");
        }
        public static string normalizeResourceRelativePath(string path)
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
            Tsonic.CSharp.Js.JSArray<string> accepted = Tsonic.CSharp.Js.JSArray<string>.of([]);
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
        }
        public static string resourcePathToOsPath(string relativePath)
        {
            return Utils_strings.replaceText(relativePath, "/", $"{Tsonic.CSharp.Node.path.sep}");
        }
        public static string resolveContainedResourcePath(string root, string relativePath)
        {
            string normalized = normalizeResourceRelativePath(relativePath);
            string rootPath = Tsonic.CSharp.Node.path.resolve(root);
            string candidate = Tsonic.CSharp.Node.path.resolve(rootPath, resourcePathToOsPath(normalized));
            if (!Utils_paths.pathContainsOrEquals(rootPath, candidate))
            {
                throw Diagnostics.createTsumoError("TSUMO_RESOURCE_PATH_ESCAPES_ROOT", $"Resource path escapes its root: {relativePath}");
            }
            return candidate;
        }
        public static ResourcePathParts splitResourcePath(string relativePath)
        {
            string normalized = normalizeResourceRelativePath(relativePath);
            int index = Tsonic.CSharp.Js.String.lastIndexOf(normalized, "/");
            if (index < 0)
            {
                return new ResourcePathParts("", normalized);
            }
            return new ResourcePathParts(Utils_strings.substringCount(normalized, 0, index + 1), Utils_strings.substringFrom(normalized, index + 1));
        }
        public static ResourceFileNameParts splitResourceFileName(string fileName)
        {
            int index = Tsonic.CSharp.Js.String.lastIndexOf(fileName, ".");
            if (index < 0)
            {
                return new ResourceFileNameParts(fileName, "");
            }
            return new ResourceFileNameParts(Utils_strings.substringCount(fileName, 0, index), Utils_strings.substringFrom(fileName, index));
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_paths.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
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
