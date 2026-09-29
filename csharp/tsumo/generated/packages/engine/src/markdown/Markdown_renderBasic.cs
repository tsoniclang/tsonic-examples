namespace Tsumo.Engine
{
    public static class Markdown_renderBasic
    {
        public static string normalizeNewlines(string text)
        {
            return Utils_strings.replaceLineEndings(text, "\n");
        }
        public static string summaryMarker
        {
            get;
            private set;
        } = default(string)!;
        public static int summaryMarkerLength
        {
            get;
            private set;
        } = default(int)!;
        public static int findSummaryDividerIndex(string markdown)
        {
            return Utils_strings.indexOfTextIgnoreCase(markdown, summaryMarker);
        }
        public static string firstBlock(string markdown)
        {
            string text = Tsonic.CSharp.Js.String.trim(markdown);
            if (text == "")
            {
                return "";
            }
            int idx = Utils_strings.indexOfText(text, "\n\n");
            return idx >= 0 ? Utils_strings.substringCount(text, 0, idx) : text;
        }
        public static MarkdownResult renderMarkdown(string markdownRaw)
        {
            string markdown = normalizeNewlines(markdownRaw);
            int moreIndex = findSummaryDividerIndex(markdown);
            string toc = Markdown_toc.generateTableOfContents(markdown);
            if (moreIndex >= 0)
            {
                string before = Utils_strings.substringCount(markdown, 0, moreIndex);
                string after = Utils_strings.substringFrom(markdown, moreIndex + summaryMarkerLength);
                string full = before + after;
                return new MarkdownResult(Markdig.Markdown.ToHtml(full, Markdown_pipeline.markdownPipeline), Tsonic.CSharp.Js.String.trim(Markdig.Markdown.ToHtml(before, Markdown_pipeline.markdownPipeline)), Markdig.Markdown.ToPlainText(full, Markdown_pipeline.markdownPipeline), toc);
            }
            string html = Markdig.Markdown.ToHtml(markdown, Markdown_pipeline.markdownPipeline);
            string plainText = Markdig.Markdown.ToPlainText(markdown, Markdown_pipeline.markdownPipeline);
            string summarySource = firstBlock(markdown);
            string summaryHtml = summarySource == "" ? "" : Tsonic.CSharp.Js.String.trim(Markdig.Markdown.ToHtml(summarySource, Markdown_pipeline.markdownPipeline));
            return new MarkdownResult(html, summaryHtml, plainText, toc);
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            Markdown_pipeline.__tsonic_module_init();
            Markdown_toc.__tsonic_module_init();
            summaryMarker = "<!--more-->";
            summaryMarkerLength = summaryMarker.Length;
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
