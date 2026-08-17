using System;

namespace Tsumo.Engine
{
    public static class Docs_directoryGraph
    {
        public static Action<string, Tsonic.CSharp.Js.Map<string, bool>> addDocsDirectoryWithParents
        {
            get;
            private set;
        } = default(Action<string, Tsonic.CSharp.Js.Map<string, bool>>)!;
        public static Func<string, int> docsDirectoryDepth
        {
            get;
            private set;
        } = default(Func<string, int>)!;
        public static Func<string, string> docsParentDirectory
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> docsDirectoryName
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Action<PageContext, PageContext?, Tsonic.CSharp.Js.JSArray<PageContext>> assignDocsPageAncestry
        {
            get;
            private set;
        } = default(Action<PageContext, PageContext?, Tsonic.CSharp.Js.JSArray<PageContext>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            addDocsDirectoryWithParents = (string directory, Tsonic.CSharp.Js.Map<string, bool> directories) =>
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
            };
            docsDirectoryDepth = (string directory) =>
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
            };
            docsParentDirectory = (string directory) =>
            {
                int separator = Tsonic.CSharp.Js.String.lastIndexOf(directory, "/");
                return separator < 0 ? "" : Utils_strings.substringCount(directory, 0, separator);
            };
            docsDirectoryName = (string directory) =>
            {
                int separator = Tsonic.CSharp.Js.String.lastIndexOf(directory, "/");
                return separator < 0 ? directory : Utils_strings.substringFrom(directory, separator + 1);
            };
            assignDocsPageAncestry = (PageContext page, PageContext? parent, Tsonic.CSharp.Js.JSArray<PageContext> ancestors) =>
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
                    Tsonic.CSharp.Js.JSArray<PageContext> childAncestors = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                    for (int ancestorIndex = 0; ancestorIndex < ancestors.length; ancestorIndex++)
                    {
                        childAncestors.push(ancestors[ancestorIndex]);
                    }
                    childAncestors.push(page);
                    assignDocsPageAncestry(child, page, childAncestors);
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
