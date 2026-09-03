using System;

namespace Tsumo.Engine
{
    public static class Resources_transforms
    {
        public static Func<string, Tsonic.CSharp.Js.JSArray<Resource>, Resource> concatenateResources
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<Resource>, Resource>)!;
        public static Func<string, string, Resource> createStringResource
        {
            get;
            private set;
        } = default(Func<string, string, Resource>)!;
        public static Func<Resource, Resource> minifyResource
        {
            get;
            private set;
        } = default(Func<Resource, Resource>)!;
        public static Func<Resource, Resource> fingerprintResource
        {
            get;
            private set;
        } = default(Func<Resource, Resource>)!;
        public static Func<string, Resource, Resource> copyResource
        {
            get;
            private set;
        } = default(Func<string, Resource, Resource>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            Resources_mediaTypes.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            concatenateResources = (string targetPath, Tsonic.CSharp.Js.JSArray<Resource> resources) =>
            {
                string target = Resources_paths.normalizeResourceRelativePath(targetPath);
                TextBuilder identity = new TextBuilder();
                identity.append("concat:");
                identity.append(target);
                TextBuilder text = new TextBuilder();
                for (int index = 0; index < resources.length; index++)
                {
                    Resource resource = resources[index];
                    identity.append("|" + resource.id);
                    if (text.length > 0)
                    {
                        text.append("\n");
                    }
                    text.append(Resources_text.readResourceText(resource, "resources.Concat"));
                }
                string content = text.toString();
                ResourcePathParts path = Resources_paths.splitResourcePath(target);
                ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                return new Resource(identity.toString(), null, true, target, Tsonic.CSharp.Node.Buffer.from(content, "utf8"), content, new ResourceData(""), Resources_mediaTypes.resourceMediaTypeForExtension(file.extension));
            };
            createStringResource = (string name, string content) =>
            {
                string normalizedName = Resources_paths.normalizeResourceRelativePath(name);
                ResourcePathParts path = Resources_paths.splitResourcePath(normalizedName);
                ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                string contentHash = Tsonic.CSharp.Node.crypto.createHash("sha256").update(Tsonic.CSharp.Node.Buffer.from(content, "utf8")).digest("hex");
                return new Resource($"fromString:{normalizedName}:{contentHash}", null, true, normalizedName, Tsonic.CSharp.Node.Buffer.from(content, "utf8"), content, new ResourceData(""), Resources_mediaTypes.resourceMediaTypeForExtension(file.extension));
            };
            minifyResource = (Resource resource) =>
            {
                string identity = $"{resource.id}|minify";
                string resourceText = Resources_text.readResourceText(resource, "resources.Minify");
                Tsonic.CSharp.Js.JSArray<string> lines = Tsonic.CSharp.Js.String.split(Utils_strings.replaceLineEndings(resourceText, "\n"), "\n");
                TextBuilder output = new TextBuilder();
                for (int index = 0; index < lines.length; index++)
                {
                    string line = Tsonic.CSharp.Js.String.trim(lines[index]);
                    if (line == "")
                    {
                        continue;
                    }
                    if (output.length > 0)
                    {
                        output.append("\n");
                    }
                    output.append(line);
                }
                string text = output.toString();
                return new Resource(identity, resource.sourcePath, resource.publishable, resource.outputRelPath, Tsonic.CSharp.Node.Buffer.from(text, "utf8"), text, resource.Data, resource.mediaType, resource.width, resource.height);
            };
            fingerprintResource = (Resource resource) =>
            {
                Tsonic.CSharp.Node.Hash hash = Tsonic.CSharp.Node.crypto.createHash("sha256").update(resource.bytes);
                string integrity = $"sha256-{hash.digest("base64")}";
                string fullHex = Tsonic.CSharp.Node.crypto.createHash("sha256").update(resource.bytes).digest("hex");
                string shortHex = Utils_strings.substringCount(fullHex, 0, 16);
                string? outputPath = resource.outputRelPath;
                string? hashedPath = null;
                if (outputPath is not null)
                {
                    ResourcePathParts path = Resources_paths.splitResourcePath(outputPath);
                    ResourceFileNameParts file = Resources_paths.splitResourceFileName(path.fileName);
                    string hashedFile = file.extension == "" ? $"{file.baseName}.{shortHex}" : $"{file.baseName}.{shortHex}{file.extension}";
                    hashedPath = path.directory + hashedFile;
                }
                return new Resource($"{resource.id}|fingerprint", resource.sourcePath, resource.publishable, hashedPath, resource.bytes, resource.text, new ResourceData(integrity), resource.mediaType, resource.width, resource.height);
            };
            copyResource = (string targetPath, Resource resource) => new Resource($"{resource.id}|copy:{Resources_paths.normalizeResourceRelativePath(targetPath)}", resource.sourcePath, resource.publishable, Resources_paths.normalizeResourceRelativePath(targetPath), resource.bytes, resource.text, resource.Data, resource.mediaType, resource.width, resource.height);
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
