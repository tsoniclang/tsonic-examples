namespace Tsumo.Engine
{
    public static class Template_evaluation_pageResourceSemantics
    {
        internal static Tsonic.CSharp.Js.JSArray<TemplateValue> pageResourceTemplateValues(Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries)
        {
            Tsonic.CSharp.Js.JSArray<TemplateValue> values = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            for (int index = 0; index < entries.length; index++)
            {
                values.push(entries[index].value);
            }
            return values;
        }
        internal static Tsonic.CSharp.Js.JSArray<PageResourceEntry> pageResourceEntries(PageResourcesValue resources)
        {
            string? sourceDirectory = resources.page.resourceSourceDir;
            if (sourceDirectory is null)
            {
                return Tsonic.CSharp.Js.JSArray<PageResourceEntry>.of([]);
            }
            Tsonic.CSharp.Js.JSArray<PageBundleResourceFile> files = Resources_pageBundle.discoverPageBundleResourceFiles(sourceDirectory);
            Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries = Tsonic.CSharp.Js.JSArray<PageResourceEntry>.of([]);
            string @base = Template_evaluation_serialization.trimSlashes(resources.page.relPermalink);
            for (int index = 0; index < files.length; index++)
            {
                PageBundleResourceFile file = files[index];
                string outputPath = @base == "" ? file.relativePath : $"{Template_evaluation_serialization.trimEndCharacter(@base, "/")}/{file.relativePath}";
                string identity = $"page-resource:{resources.page.relPermalink}:{file.relativePath}";
                entries.push(new PageResourceEntry(file.relativePath, new ResourceValue(resources.manager, resources.manager.loadFile(identity, file.sourcePath, outputPath))));
            }
            return entries;
        }
        public static TemplateValue getPageResource(PageResourcesValue resources, string pathRaw)
        {
            string path = Resources_paths.normalizeResourceRelativePath(pathRaw);
            if (path == "")
            {
                return Template_runtimeHelpers.nil;
            }
            Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries = pageResourceEntries(resources);
            for (int index = 0; index < entries.length; index++)
            {
                PageResourceEntry entry = entries[index];
                if (entry.relativePath == path)
                {
                    return entry.value;
                }
            }
            return Template_runtimeHelpers.nil;
        }
        public static TemplateValue getMatchingPageResource(PageResourcesValue resources, string pattern)
        {
            return getMatchingPageResourceFromCollection(new PageResourceCollectionValue(pageResourceEntries(resources)), pattern);
        }
        public static TemplateValue getMatchingPageResourceFromCollection(PageResourceCollectionValue resources, string pattern)
        {
            Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries = resources.entries;
            for (int index = 0; index < entries.length; index++)
            {
                PageResourceEntry entry = entries[index];
                if (Resources_glob.resourceGlobMatches(pattern, entry.relativePath))
                {
                    return entry.value;
                }
            }
            return Template_runtimeHelpers.nil;
        }
        public static PageResourceCollectionValue getMatchingPageResources(PageResourcesValue resources, string pattern)
        {
            return getMatchingPageResourcesFromCollection(new PageResourceCollectionValue(pageResourceEntries(resources)), pattern);
        }
        public static PageResourceCollectionValue getMatchingPageResourcesFromCollection(PageResourceCollectionValue resources, string pattern)
        {
            Tsonic.CSharp.Js.JSArray<PageResourceEntry> selected = Tsonic.CSharp.Js.JSArray<PageResourceEntry>.of([]);
            for (int index = 0; index < resources.entries.length; index++)
            {
                PageResourceEntry entry = resources.entries[index];
                if (Resources_glob.resourceGlobMatches(pattern, entry.relativePath))
                {
                    selected.push(entry);
                }
            }
            return new PageResourceCollectionValue(selected);
        }
        internal static PageResourceCollectionValue filterPageResourcesByType(Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries, string mediaType)
        {
            Tsonic.CSharp.Js.JSArray<PageResourceEntry> selected = Tsonic.CSharp.Js.JSArray<PageResourceEntry>.of([]);
            for (int index = 0; index < entries.length; index++)
            {
                PageResourceEntry entry = entries[index];
                if (Resources_mediaTypes.resourceMatchesMediaType(entry.value.value.mediaType, mediaType))
                {
                    selected.push(entry);
                }
            }
            return new PageResourceCollectionValue(selected);
        }
        public static PageResourceCollectionValue getPageResourcesByType(PageResourcesValue resources, string mediaType)
        {
            return filterPageResourcesByType(pageResourceEntries(resources), mediaType);
        }
        public static PageResourceCollectionValue getPageResourceCollectionByType(PageResourceCollectionValue resources, string mediaType)
        {
            return filterPageResourcesByType(resources.entries, mediaType);
        }
        public static TemplateValue? callPageResourcesMethod(PageResourcesValue resources, string methodName, Tsonic.CSharp.Js.JSArray<TemplateValue> args)
        {
            string method = Tsonic.CSharp.Js.String.toLowerCase(methodName);
            if (method == "get" && args.length >= 1)
            {
                return getPageResource(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            if (method == "getmatch" && args.length >= 1)
            {
                return getMatchingPageResource(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            if (method == "match" && args.length >= 1)
            {
                return getMatchingPageResources(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            if (method == "bytype" && args.length >= 1)
            {
                return getPageResourcesByType(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            return null;
        }
        public static TemplateValue? callPageResourceCollectionMethod(PageResourceCollectionValue resources, string methodName, Tsonic.CSharp.Js.JSArray<TemplateValue> args)
        {
            string method = Tsonic.CSharp.Js.String.toLowerCase(methodName);
            if (method == "getmatch" && args.length >= 1)
            {
                return getMatchingPageResourceFromCollection(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            if (method == "match" && args.length >= 1)
            {
                return getMatchingPageResourcesFromCollection(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            if (method == "bytype" && args.length >= 1)
            {
                return getPageResourceCollectionByType(resources, Template_runtimeHelpers.toPlainString(args[0]));
            }
            return null;
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Resources_pageBundle.__tsonic_module_init();
            Resources_glob.__tsonic_module_init();
            Resources_mediaTypes.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_evaluation_serialization.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class PageResourceEntry
    {
        public string relativePath;
        public ResourceValue value;
        public PageResourceEntry(string relativePath, ResourceValue value)
        {
            this.relativePath = relativePath;
            this.value = value;
        }
    }
    public class PageResourceCollectionValue : AnyArrayValue
    {
        public Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries;
        public PageResourceCollectionValue(Tsonic.CSharp.Js.JSArray<PageResourceEntry> entries) : base(Template_evaluation_pageResourceSemantics.pageResourceTemplateValues(entries))
        {
            this.entries = entries;
        }
    }
}
