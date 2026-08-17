using System;

namespace Tsumo.Engine
{
    public static class Resources_manager
    {
        public static Action<Tsonic.CSharp.Js.JSArray<string>> sortResourcePaths
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Action<Tsonic.CSharp.Js.JSArray<Resource>> sortResourcesByIdentity
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<Resource>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Resources_imageDimensions.__tsonic_module_init();
            Resources_imageProvider.__tsonic_module_init();
            Resources_mediaTypes.__tsonic_module_init();
            Resources_models.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            Resources_glob.__tsonic_module_init();
            Resources_sassProvider.__tsonic_module_init();
            Resources_javascriptProvider.__tsonic_module_init();
            Resources_transforms.__tsonic_module_init();
            sortResourcePaths = (Tsonic.CSharp.Js.JSArray<string> paths) =>
            {
                for (int leftIndex = 0; leftIndex < paths.length; leftIndex++)
                {
                    for (int rightIndex = leftIndex + 1; rightIndex < paths.length; rightIndex++)
                    {
                        string left = paths[leftIndex];
                        string right = paths[rightIndex];
                        if (Utils_strings.compareText(Resources_paths.normalizeResourceSlashes(left), Resources_paths.normalizeResourceSlashes(right)) <= 0)
                        {
                            continue;
                        }
                        paths[leftIndex] = right;
                        paths[rightIndex] = left;
                    }
                }
            };
            sortResourcesByIdentity = (Tsonic.CSharp.Js.JSArray<Resource> resources) =>
            {
                for (int leftIndex = 0; leftIndex < resources.length; leftIndex++)
                {
                    for (int rightIndex = leftIndex + 1; rightIndex < resources.length; rightIndex++)
                    {
                        Resource left = resources[leftIndex];
                        Resource right = resources[rightIndex];
                        if (Utils_strings.compareText(left.id, right.id) <= 0)
                        {
                            continue;
                        }
                        resources[leftIndex] = right;
                        resources[rightIndex] = left;
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
    public class ResourceManager
    {
        public string siteDir;
        public string? themeDir;
        public string outputDir;
        public string siteAssetsDir;
        public string? themeAssetsDir;
        public Tsonic.CSharp.Js.Map<string, Resource> cache;
        public Tsonic.CSharp.Js.JSArray<string> siteAssetFiles;
        public Tsonic.CSharp.Js.JSArray<string> themeAssetFiles;
        public ResourceManager(string siteDir, string? themeDir, string outputDir)
        {
            this.siteDir = siteDir;
            this.themeDir = themeDir;
            this.outputDir = outputDir;
            this.siteAssetsDir = System.IO.Path.Combine(siteDir, "assets");
            this.themeAssetsDir = themeDir is null ? null : System.IO.Path.Combine(themeDir, "assets");
            this.cache = new Tsonic.CSharp.Js.Map<string, Resource>();
            this.siteAssetFiles = Fs.listFilesRecursive(this.siteAssetsDir, "*");
            Resources_manager.sortResourcePaths(this.siteAssetFiles);
            string? themeAssetsDir = this.themeAssetsDir;
            this.themeAssetFiles = themeAssetsDir is null ? new Tsonic.CSharp.Js.JSArray<string>(new string[] { }) : Fs.listFilesRecursive(themeAssetsDir, "*");
            Resources_manager.sortResourcePaths(this.themeAssetFiles);
        }
        public string? resolveAssetFullPath(string relativePath)
        {
            string normalized = Resources_paths.normalizeResourceRelativePath(relativePath);
            if (normalized == "")
            {
                return null;
            }
            string sitePath = Resources_paths.resolveContainedResourcePath(this.siteAssetsDir, normalized);
            if (System.IO.File.Exists(sitePath))
            {
                return sitePath;
            }
            string? themeAssetsDir = this.themeAssetsDir;
            if (themeAssetsDir is null)
            {
                return null;
            }
            string themePath = Resources_paths.resolveContainedResourcePath(themeAssetsDir, normalized);
            return System.IO.File.Exists(themePath) ? themePath : null;
        }
        public Resource? get(string relativePath)
        {
            string normalized = Resources_paths.normalizeResourceRelativePath(relativePath);
            if (normalized == "")
            {
                return null;
            }
            string identity = $"get:{normalized}";
            string? fullPath = this.resolveAssetFullPath(normalized);
            if (fullPath is null)
            {
                return null;
            }
            return this.loadFile(identity, fullPath, normalized);
        }
        public Resource loadFile(string identity, string fullPath, string outputRelPath)
        {
            Resource? cached = Tsonic.CSharp.Js.Map.getReference<string, Resource>(this.cache, identity);
            if (cached is not null)
            {
                return cached;
            }
            if (!System.IO.File.Exists(fullPath))
            {
                throw Diagnostics.createTsumoError("TSUMO_RESOURCE_SOURCE_MISSING", $"Resource source file does not exist: {fullPath}");
            }
            Tsonic.CSharp.Node.Buffer bytes = Fs.readBinaryFile(fullPath);
            string extension = Tsonic.CSharp.Js.String.toLowerCase((System.IO.Path.GetExtension(fullPath) ?? ""));
            string mediaType = Resources_mediaTypes.resourceMediaTypeForExtension(extension);
            int width = 0;
            int height = 0;
            if (Resources_mediaTypes.isImageResourceExtension(extension))
            {
                ImageDimensions? dimensions = Resources_imageDimensions.parseImageDimensions(bytes);
                if (dimensions is not null)
                {
                    width = dimensions.width;
                    height = dimensions.height;
                }
            }
            Resource resource = new Resource(identity, fullPath, true, outputRelPath, bytes, null, new ResourceData(""), mediaType, width, height);
            this.cache.set(identity, resource);
            return resource;
        }
        public Resource? getMatch(string pattern)
        {
            string normalized = Resources_paths.normalizeResourceRelativePath(pattern);
            if (normalized == "")
            {
                return null;
            }
            if (!Tsonic.CSharp.Js.String.includes(normalized, "*"))
            {
                return this.get(normalized);
            }
            for (int index = 0; index < this.siteAssetFiles.length; index++)
            {
                string fullPath = this.siteAssetFiles[index];
                string relativePath = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(this.siteAssetsDir, fullPath));
                if (Resources_glob.resourceGlobMatches(normalized, relativePath))
                {
                    return this.get(relativePath);
                }
            }
            string? themeAssetsDir = this.themeAssetsDir;
            if (themeAssetsDir is not null)
            {
                for (int index_1 = 0; index_1 < this.themeAssetFiles.length; index_1++)
                {
                    string fullPath_1 = this.themeAssetFiles[index_1];
                    string relativePath_1 = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(themeAssetsDir, fullPath_1));
                    if (Resources_glob.resourceGlobMatches(normalized, relativePath_1))
                    {
                        return this.get(relativePath_1);
                    }
                }
            }
            return null;
        }
        public Tsonic.CSharp.Js.JSArray<Resource> match(string pattern)
        {
            string normalized = Resources_paths.normalizeResourceRelativePath(pattern);
            Tsonic.CSharp.Js.JSArray<Resource> result = new Tsonic.CSharp.Js.JSArray<Resource>(new Resource[] { });
            if (normalized == "")
            {
                return result;
            }
            Tsonic.CSharp.Js.Map<string, bool> selected = new Tsonic.CSharp.Js.Map<string, bool>();
            for (int index = 0; index < this.siteAssetFiles.length; index++)
            {
                string fullPath = this.siteAssetFiles[index];
                string relativePath = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(this.siteAssetsDir, fullPath));
                if (!Resources_glob.resourceGlobMatches(normalized, relativePath))
                {
                    continue;
                }
                Resource? resource = this.get(relativePath);
                if (resource is null)
                {
                    continue;
                }
                result.push(resource);
                selected.set(relativePath, true);
            }
            string? themeAssetsDir = this.themeAssetsDir;
            if (themeAssetsDir is not null)
            {
                for (int index_1 = 0; index_1 < this.themeAssetFiles.length; index_1++)
                {
                    string fullPath_1 = this.themeAssetFiles[index_1];
                    string relativePath_1 = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(themeAssetsDir, fullPath_1));
                    if (selected.has(relativePath_1) || !Resources_glob.resourceGlobMatches(normalized, relativePath_1))
                    {
                        continue;
                    }
                    Resource? resource_1 = this.get(relativePath_1);
                    if (resource_1 is not null)
                    {
                        result.push(resource_1);
                    }
                }
            }
            Resources_manager.sortResourcesByIdentity(result);
            return result;
        }
        public Tsonic.CSharp.Js.JSArray<Resource> byType(string mediaType)
        {
            Tsonic.CSharp.Js.JSArray<Resource> result = new Tsonic.CSharp.Js.JSArray<Resource>(new Resource[] { });
            Tsonic.CSharp.Js.Map<string, bool> selected = new Tsonic.CSharp.Js.Map<string, bool>();
            for (int index = 0; index < this.siteAssetFiles.length; index++)
            {
                string fullPath = this.siteAssetFiles[index];
                string relativePath = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(this.siteAssetsDir, fullPath));
                Resource? resource = this.get(relativePath);
                if (resource is null || !Resources_mediaTypes.resourceMatchesMediaType(resource.mediaType, mediaType))
                {
                    continue;
                }
                result.push(resource);
                selected.set(relativePath, true);
            }
            string? themeAssetsDir = this.themeAssetsDir;
            if (themeAssetsDir is not null)
            {
                for (int index_1 = 0; index_1 < this.themeAssetFiles.length; index_1++)
                {
                    string fullPath_1 = this.themeAssetFiles[index_1];
                    string relativePath_1 = Resources_paths.normalizeResourceSlashes(System.IO.Path.GetRelativePath(themeAssetsDir, fullPath_1));
                    if (selected.has(relativePath_1))
                    {
                        continue;
                    }
                    Resource? resource_1 = this.get(relativePath_1);
                    if (resource_1 is not null && Resources_mediaTypes.resourceMatchesMediaType(resource_1.mediaType, mediaType))
                    {
                        result.push(resource_1);
                    }
                }
            }
            Resources_manager.sortResourcesByIdentity(result);
            return result;
        }
        public Resource concat(string targetPath, Tsonic.CSharp.Js.JSArray<Resource> resources)
        {
            return this.cacheResource(Resources_transforms.concatenateResources(targetPath, resources));
        }
        public Resource fromString(string name, string content)
        {
            return this.cacheResource(Resources_transforms.createStringResource(name, content));
        }
        public Resource minify(Resource resource)
        {
            return this.cacheResource(Resources_transforms.minifyResource(resource));
        }
        public Resource fingerprint(Resource resource)
        {
            return this.cacheResource(Resources_transforms.fingerprintResource(resource));
        }
        public Resource copy(string targetPath, Resource resource)
        {
            return this.cacheResource(Resources_transforms.copyResource(targetPath, resource));
        }
        public Resource sassCompile(Resource resource)
        {
            string identity = $"{resource.id}|sass";
            Resource? cached = Tsonic.CSharp.Js.Map.getReference<string, Resource>(this.cache, identity);
            if (cached is not null)
            {
                return cached;
            }
            Tsonic.CSharp.Js.JSArray<string> loadPaths = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
            string? sourcePath = resource.sourcePath;
            if (sourcePath is not null)
            {
                string? sourceDirectory = System.IO.Path.GetDirectoryName(sourcePath);
                if (sourceDirectory is not null && sourceDirectory != "")
                {
                    loadPaths.push(sourceDirectory);
                }
            }
            loadPaths.push(this.siteAssetsDir);
            string? themeAssetsDir = this.themeAssetsDir;
            if (themeAssetsDir is not null)
            {
                loadPaths.push(themeAssetsDir);
            }
            return this.cacheResource(Resources_sassProvider.compileSassResource(resource, loadPaths));
        }
        public Resource javascriptBuild(Resource resource, JavaScriptBuildOptions options)
        {
            string identity = $"{resource.id}|js-build:{options.cacheKey()}";
            Resource? cached = Tsonic.CSharp.Js.Map.getReference<string, Resource>(this.cache, identity);
            if (cached is not null)
            {
                return cached;
            }
            return this.cacheResource(Resources_javascriptProvider.buildJavaScriptResource(resource, options));
        }
        public Resource resize(Resource resource, string specification)
        {
            string identity = $"{resource.id}|resize:{specification}";
            Resource? cached = Tsonic.CSharp.Js.Map.getReference<string, Resource>(this.cache, identity);
            if (cached is not null)
            {
                return cached;
            }
            return this.cacheResource(Resources_imageProvider.resizeImageResource(resource, specification));
        }
        public void ensurePublished(Resource resource)
        {
            if (!resource.publishable)
            {
                return;
            }
            string? outputRelPath = resource.outputRelPath;
            if (outputRelPath is null)
            {
                throw Diagnostics.createTsumoError("TSUMO_RESOURCE_OUTPUT_PATH_MISSING", "Publishable resource has no output path");
            }
            string normalized = Resources_paths.normalizeResourceRelativePath(outputRelPath);
            if (normalized == "")
            {
                throw Diagnostics.createTsumoError("TSUMO_RESOURCE_OUTPUT_PATH_MISSING", "Publishable resource has an empty output path");
            }
            string destination = Resources_paths.resolveContainedResourcePath(this.outputDir, normalized);
            string? directory = System.IO.Path.GetDirectoryName(destination);
            if (directory is not null && directory != "")
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            Tsonic.CSharp.Node.fs.writeFileSync(destination, resource.bytes);
        }
        public Resource cacheResource(Resource resource)
        {
            Resource? cached = Tsonic.CSharp.Js.Map.getReference<string, Resource>(this.cache, resource.id);
            if (cached is not null)
            {
                return cached;
            }
            this.cache.set(resource.id, resource);
            return resource;
        }
    }
}
