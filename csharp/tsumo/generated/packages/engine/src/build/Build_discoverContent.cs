using System;

namespace Tsumo.Engine
{
    public static class Build_discoverContent
    {
        public static Func<string, bool> isBranchIndexFile
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> isLeafBundleIndexFile
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, string, string, PageFile> createPageFile
        {
            get;
            private set;
        } = default(Func<string, string, string, PageFile>)!;
        public static Func<ContentPageSource, ContentPageSource, double> compareContentPages
        {
            get;
            private set;
        } = default(Func<ContentPageSource, ContentPageSource, double>)!;
        public static Action<Tsonic.CSharp.Js.Map<string, string>, string, string> assertUniqueOutput
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.Map<string, string>, string, string>)!;
        public static Func<string, bool, ContentInventory> discoverContent
        {
            get;
            private set;
        } = default(Func<string, bool, ContentInventory>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Frontmatter.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Models.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Build_contentModel.__tsonic_module_init();
            Build_siteRoutes.__tsonic_module_init();
            isBranchIndexFile = (string name) => Tsonic.CSharp.Js.String.toLowerCase(name) == "_index.md";
            isLeafBundleIndexFile = (string name) => Tsonic.CSharp.Js.String.toLowerCase(name) == "index.md";
            createPageFile = (string directory, string fileName, string filePath) => new PageFile(Tsonic.CSharp.Node.path.resolve(filePath), directory == "" ? "" : directory + "/", Build_siteRoutes.withoutMarkdownExtension(fileName));
            compareContentPages = (ContentPageSource left, ContentPageSource right) =>
            {
                double leftTime = left.dateUtc.getTime();
                double rightTime = right.dateUtc.getTime();
                if (rightTime > leftTime)
                {
                    return 1;
                }
                if (rightTime < leftTime)
                {
                    return -1;
                }
                int route = Utils_strings.compareText(left.relPermalink, right.relPermalink);
                return route != 0 ? route : Build_siteRoutes.compareSitePaths(left.sourcePath, right.sourcePath);
            };
            assertUniqueOutput = (Tsonic.CSharp.Js.Map<string, string> outputs, string outputPath, string sourcePath) =>
            {
                string key = Tsonic.CSharp.Js.String.toLowerCase(outputPath);
                string? previous = Tsonic.CSharp.Js.Map.getReference<string, string>(outputs, key);
                if (previous is not null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_CONTENT_ROUTE_CONFLICT", $"Content sources '{previous}' and '{sourcePath}' both map to '{outputPath}'", sourcePath);
                }
                outputs.set(key, sourcePath);
            };
            discoverContent = (string contentDir, bool buildDrafts) =>
            {
                Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesRecursive(contentDir, "*.md");
                files.sort((string left, string right) => Build_siteRoutes.compareSitePaths(left, right));
                Tsonic.CSharp.Js.JSArray<ContentPageSource> pages = new Tsonic.CSharp.Js.JSArray<ContentPageSource>(new ContentPageSource[] { });
                Tsonic.CSharp.Js.Map<string, ListPageSource> listPagesByRoute = new Tsonic.CSharp.Js.Map<string, ListPageSource>();
                Tsonic.CSharp.Js.Map<string, string> outputs = new Tsonic.CSharp.Js.Map<string, string>();
                for (int fileIndex = 0; fileIndex < files.length; fileIndex++)
                {
                    string filePath = files[fileIndex];
                    string relativePath = Build_siteRoutes.normalizeSitePath(Tsonic.CSharp.Node.path.relative(contentDir, filePath));
                    if (relativePath == "" || relativePath == ".." || Tsonic.CSharp.Js.String.startsWith(relativePath, "../"))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_CONTENT_SOURCE_PATH_INVALID", $"Content source is outside its content root: {filePath}", filePath);
                    }
                    Tsonic.CSharp.Js.JSArray<string> pathSegments = Build_siteRoutes.splitSitePath(relativePath);
                    for (int index = 0; index < pathSegments.length; index++)
                    {
                        Build_siteRoutes.assertSiteRouteSegment(pathSegments[index], filePath);
                    }
                    string fileName = pathSegments[pathSegments.length - 1];
                    Tsonic.CSharp.Js.JSArray<string> directorySegments = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                    for (int index_1 = 0; index_1 < pathSegments.length - 1; index_1++)
                    {
                        directorySegments.push(pathSegments[index_1]);
                    }
                    string directory = Build_siteRoutes.joinSitePath(directorySegments);
                    ParsedContent parsed = Frontmatter_parse.parseContent(Fs.readTextFile(filePath), filePath);
                    FrontMatter frontMatter = parsed.frontMatter;
                    Tsonic.CSharp.Js.Date modifiedAt = new Tsonic.CSharp.Js.Date(Tsonic.CSharp.Node.fs.statSync(filePath).mtimeMs);
                    PageFile file = createPageFile(directory, fileName, filePath);
                    if (isBranchIndexFile(fileName))
                    {
                        if (listPagesByRoute.has(directory))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_CONTENT_ROUTE_CONFLICT", $"Multiple branch indexes map to '{directory}'", filePath);
                        }
                        assertUniqueOutput(outputs, Build_siteRoutes.siteOutputPath(directorySegments), filePath);
                        listPagesByRoute.set(directory, new ListPageSource(frontMatter.title, parsed.body, frontMatter.description ?? "", frontMatter.type, frontMatter.layout, frontMatter.Params, Tsonic.CSharp.Node.path.dirname(filePath), file));
                        continue;
                    }
                    string section = directorySegments.length > 0 ? directorySegments[0] : "";
                    string? configuredType = frontMatter.type;
                    string pageType = configuredType is null || Tsonic.CSharp.Js.String.trim(configuredType) == "" ? section != "" ? section : "page" : configuredType;
                    if (frontMatter.draft && !buildDrafts)
                    {
                        continue;
                    }
                    bool isLeafBundle = isLeafBundleIndexFile(fileName) && directorySegments.length > 0;
                    string defaultLeafName = isLeafBundle ? directorySegments[directorySegments.length - 1] : Build_siteRoutes.withoutMarkdownExtension(fileName);
                    string slug = frontMatter.slug ?? Utils_text.slugify(defaultLeafName);
                    Build_siteRoutes.assertSiteRouteSegment(slug, filePath);
                    Tsonic.CSharp.Js.JSArray<string> routeSegments = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                    int directoryCount = isLeafBundle ? directorySegments.length - 1 : directorySegments.length;
                    for (int index_2 = 0; index_2 < directoryCount; index_2++)
                    {
                        routeSegments.push(directorySegments[index_2]);
                    }
                    routeSegments.push(slug);
                    string outputRelPath = Build_siteRoutes.siteOutputPath(routeSegments);
                    assertUniqueOutput(outputs, outputRelPath, filePath);
                    ContentPageSource page = new ContentPageSource(filePath, section, pageType, slug, frontMatter.title ?? Utils_text.humanizeSlug(defaultLeafName), frontMatter.date ?? modifiedAt, (frontMatter.date ?? modifiedAt).toISOString(), modifiedAt.toISOString(), frontMatter.draft, isLeafBundle, frontMatter.description ?? "", frontMatter.tags, frontMatter.categories, frontMatter.Params, parsed.body, "/" + Build_siteRoutes.joinSitePath(routeSegments) + "/", outputRelPath, frontMatter.layout, file, frontMatter.menus);
                    pages.push(page);
                }
                pages.sort((ContentPageSource left, ContentPageSource right) => compareContentPages(left, right));
                return new ContentInventory(pages, listPagesByRoute);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
