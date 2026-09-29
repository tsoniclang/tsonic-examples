namespace Tsumo.Engine
{
    public static class Docs_directoryGraph
    {
        public static void addDocsDirectoryWithParents(string directory, Tsonic.CSharp.Js.Map<string, bool> directories)
        {
            string current = Tsonic.CSharp.Js.String.trim(directory);
            while (true)
            {
                directories.set(current, true);
                if (current == "")
                {
                    return;
                }
                int separator = Tsonic.CSharp.Js.String.lastIndexOf(current, "/");
                current = separator < 0 ? "" : Utils_strings.substringCount(current, 0, separator);
            }
        }
        public static int docsDirectoryDepth(string directory)
        {
            if (directory == "")
            {
                return 0;
            }
            int depth = 1;
            int position = 0;
            while (true)
            {
                int separator = Tsonic.CSharp.Js.String.indexOf(directory, "/", position);
                if (separator < 0)
                {
                    return depth;
                }
                depth++;
                position = separator + 1;
            }
        }
        public static string docsParentDirectory(string directory)
        {
            int separator = Tsonic.CSharp.Js.String.lastIndexOf(directory, "/");
            return separator < 0 ? "" : Utils_strings.substringCount(directory, 0, separator);
        }
        public static string docsDirectoryName(string directory)
        {
            int separator = Tsonic.CSharp.Js.String.lastIndexOf(directory, "/");
            return separator < 0 ? directory : Utils_strings.substringFrom(directory, separator + 1);
        }
        public static void assignDocsPageAncestry(PageContext page, PageContext? parent, Tsonic.CSharp.Js.JSArray<PageContext> ancestors)
        {
            page.parent = parent;
            page.ancestors = ancestors;
            if (page.kind == "page")
            {
                return;
            }
            for (int index = 0; index < page.pages.length; index++)
            {
                PageContext child = page.pages[index];
                Tsonic.CSharp.Js.JSArray<PageContext> childAncestors = Tsonic.CSharp.Js.JSArray<PageContext>.of([]);
                for (int ancestorIndex = 0; ancestorIndex < ancestors.length; ancestorIndex++)
                {
                    childAncestors.push(ancestors[ancestorIndex]);
                }
                childAncestors.push(page);
                assignDocsPageAncestry(child, page, childAncestors);
            }
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
