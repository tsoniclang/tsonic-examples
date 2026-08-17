using System;

namespace Tsumo.Engine
{
    public static class Markdown_result
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class MarkdownResult
    {
        public string html;
        public string summaryHtml;
        public string plainText;
        public string tableOfContents;
        public MarkdownResult(string html, string summaryHtml, string plainText, string tableOfContents)
        {
            this.html = html;
            this.summaryHtml = summaryHtml;
            this.plainText = plainText;
            this.tableOfContents = tableOfContents;
        }
    }
}
