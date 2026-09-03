namespace Tsumo.Engine
{
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
