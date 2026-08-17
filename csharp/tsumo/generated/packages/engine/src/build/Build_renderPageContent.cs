using System;

namespace Tsumo.Engine
{
    public static class Build_renderPageContent
    {
        public static Action<StandardPageGraph, BuildEnvironment> renderStandardPageContent
        {
            get;
            private set;
        } = default(Action<StandardPageGraph, BuildEnvironment>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Env.__tsonic_module_init();
            Markdown.__tsonic_module_init();
            Utils_html.__tsonic_module_init();
            Build_standardPageGraph.__tsonic_module_init();
            renderStandardPageContent = (StandardPageGraph graph, BuildEnvironment environment) =>
            {
                foreach ((PageContext, string) entry in graph.rawBodyByPage.entries())
                {
                    PageContext page = entry.Item1;
                    string rawBody = entry.Item2;
                    if (rawBody == "")
                    {
                        continue;
                    }
                    MarkdownResult rendered = Markdown_renderWithShortcodes.renderMarkdownWithShortcodes(rawBody, page, graph.site, environment);
                    page.content = new HtmlString(rendered.html);
                    page.summary = new HtmlString(rendered.summaryHtml);
                    page.tableOfContents = new HtmlString(rendered.tableOfContents);
                    page.plain = rendered.plainText;
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
