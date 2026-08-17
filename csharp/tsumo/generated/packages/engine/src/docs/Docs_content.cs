using System;

namespace Tsumo.Engine
{
    public static class Docs_content
    {
        public static Func<Tsonic.CSharp.Js.JSArray<DocsMarkdownRoute>, bool, DocsContentInventory> loadDocsContent
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<DocsMarkdownRoute>, bool, DocsContentInventory>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Frontmatter.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Docs_routes.__tsonic_module_init();
            loadDocsContent = (Tsonic.CSharp.Js.JSArray<DocsMarkdownRoute> routes, bool buildDrafts) =>
            {
                Tsonic.CSharp.Js.Map<string, DocsContentRoute> indexByDirectory = new Tsonic.CSharp.Js.Map<string, DocsContentRoute>();
                Tsonic.CSharp.Js.JSArray<DocsContentRoute> leaves = new Tsonic.CSharp.Js.JSArray<DocsContentRoute>(new DocsContentRoute[] { });
                Tsonic.CSharp.Js.Map<string, string> permalinkByRelativePath = new Tsonic.CSharp.Js.Map<string, string>();
                for (int index = 0; index < routes.length; index++)
                {
                    DocsMarkdownRoute route = routes[index];
                    ParsedContent parsed = Frontmatter_parse.parseContent(Fs.readTextFile(route.sourcePath), route.sourcePath);
                    DocsContentRoute content = new DocsContentRoute(route, parsed, Tsonic.CSharp.Node.fs.statSync(route.sourcePath).mtime);
                    if (route.isIndex)
                    {
                        indexByDirectory.set(route.dirKey, content);
                        permalinkByRelativePath.set(Tsonic.CSharp.Js.String.toLowerCase(route.relPath), route.relPermalink);
                        continue;
                    }
                    if (parsed.frontMatter.draft && !buildDrafts)
                    {
                        continue;
                    }
                    leaves.push(content);
                    permalinkByRelativePath.set(Tsonic.CSharp.Js.String.toLowerCase(route.relPath), route.relPermalink);
                }
                return new DocsContentInventory(indexByDirectory, leaves, permalinkByRelativePath);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DocsContentRoute
    {
        public DocsMarkdownRoute route;
        public ParsedContent parsed;
        public Tsonic.CSharp.Js.Date modifiedAt;
        public DocsContentRoute(DocsMarkdownRoute route, ParsedContent parsed, Tsonic.CSharp.Js.Date modifiedAt)
        {
            this.route = route;
            this.parsed = parsed;
            this.modifiedAt = modifiedAt;
        }
    }
    public class DocsContentInventory
    {
        public Tsonic.CSharp.Js.Map<string, DocsContentRoute> indexByDirectory;
        public Tsonic.CSharp.Js.JSArray<DocsContentRoute> leaves;
        public Tsonic.CSharp.Js.Map<string, string> permalinkByRelativePath;
        public DocsContentInventory(Tsonic.CSharp.Js.Map<string, DocsContentRoute> indexByDirectory, Tsonic.CSharp.Js.JSArray<DocsContentRoute> leaves, Tsonic.CSharp.Js.Map<string, string> permalinkByRelativePath)
        {
            this.indexByDirectory = indexByDirectory;
            this.leaves = leaves;
            this.permalinkByRelativePath = permalinkByRelativePath;
        }
    }
}
