using System;

namespace Tsumo.Engine
{
    public static class Docs_markdown
    {
        public static Func<string, string> normalizeSlashes
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, bool> isExternalUrl
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> isUnsafeUrl
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> isMarkdownLink
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, string, string?> normalizeRelativePath
        {
            get;
            private set;
        } = default(Func<string, string, string?>)!;
        public static Func<DocsMountConfig, string, string?> computeGitHubBlobUrl
        {
            get;
            private set;
        } = default(Func<DocsMountConfig, string, string?>)!;
        public static Func<string?, DocsLinkRewriteContext, string?> maybeRewriteUrl
        {
            get;
            private set;
        } = default(Func<string?, DocsLinkRewriteContext, string?>)!;
        public static Action<Markdig.Syntax.Inlines.ContainerInline, DocsLinkRewriteContext> rewriteInInlines
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.Inlines.ContainerInline, DocsLinkRewriteContext>)!;
        public static Action<Markdig.Syntax.Block, DocsLinkRewriteContext> rewriteInBlock
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.Block, DocsLinkRewriteContext>)!;
        public static Action<Markdig.Syntax.MarkdownDocument, DocsLinkRewriteContext> rewriteLinks
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.MarkdownDocument, DocsLinkRewriteContext>)!;
        public static Func<string, string> normalizeNewlines
        {
            get;
            private set;
        } = default(Func<string, string>)!;
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
        public static Func<string, int> findSummaryDividerIndex
        {
            get;
            private set;
        } = default(Func<string, int>)!;
        public static Func<string, string> firstBlock
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, DocsLinkRewriteContext, string> renderWithRewrites
        {
            get;
            private set;
        } = default(Func<string, DocsLinkRewriteContext, string>)!;
        public static Func<string, DocsLinkRewriteContext, MarkdownResult> renderDocsMarkdown
        {
            get;
            private set;
        } = default(Func<string, DocsLinkRewriteContext, MarkdownResult>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Markdown.__tsonic_module_init();
            Diagnostics.__tsonic_module_init();
            Docs_models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Docs_url.__tsonic_module_init();
            normalizeSlashes = (string path) => Tsonic.CSharp.Js.String.replaceAll(path, "\\", "/");
            isExternalUrl = (string url) =>
            {
                string lower = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(url));
                return (Tsonic.CSharp.Js.String.startsWith(lower, "http://") || Tsonic.CSharp.Js.String.startsWith(lower, "https://") || Tsonic.CSharp.Js.String.startsWith(lower, "mailto:") || Tsonic.CSharp.Js.String.startsWith(lower, "tel:") || Tsonic.CSharp.Js.String.startsWith(lower, "//"));
            };
            isUnsafeUrl = (string url) =>
            {
                string lower = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(url));
                return Tsonic.CSharp.Js.String.startsWith(lower, "javascript:") || Tsonic.CSharp.Js.String.startsWith(lower, "data:") || Tsonic.CSharp.Js.String.startsWith(lower, "vbscript:");
            };
            isMarkdownLink = (string path) =>
            {
                string lower = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(path));
                return Tsonic.CSharp.Js.String.endsWith(lower, ".md") || Tsonic.CSharp.Js.String.endsWith(lower, ".markdown");
            };
            normalizeRelativePath = (string baseDirKey, string targetPath) =>
            {
                string @base = Tsonic.CSharp.Js.String.trim(baseDirKey);
                Tsonic.CSharp.Js.JSArray<string> start = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                if (@base != "")
                {
                    Tsonic.CSharp.Js.JSArray<string> baseParts = Tsonic.CSharp.Js.String.split(@base, "/");
                    for (int i = 0; i < baseParts.length; i++)
                    {
                        string seg = Tsonic.CSharp.Js.String.trim(baseParts[i]);
                        if (seg != "")
                        {
                            start.push(seg);
                        }
                    }
                }
                string target = normalizeSlashes(Tsonic.CSharp.Js.String.trim(targetPath));
                Tsonic.CSharp.Js.JSArray<string> parts = Tsonic.CSharp.Js.String.split(target, "/");
                for (int i_1 = 0; i_1 < parts.length; i_1++)
                {
                    string raw = parts[i_1];
                    string seg_1 = Tsonic.CSharp.Js.String.trim(raw);
                    if (seg_1 == "" || seg_1 == ".")
                    {
                        continue;
                    }
                    if (seg_1 == "..")
                    {
                        if (start.length == 0)
                        {
                            return null;
                        }
                        Tsonic.CSharp.Js.Array.popReference(start);
                        continue;
                    }
                    start.push(seg_1);
                }
                Tsonic.CSharp.Js.JSArray<string> arr = start;
                if (arr.length == 0)
                {
                    return "";
                }
                string @out = arr[0];
                for (int i_2 = 1; i_2 < arr.length; i_2++)
                {
                    @out += "/" + arr[i_2];
                }
                return @out;
            };
            computeGitHubBlobUrl = (DocsMountConfig mount, string repoRelPath) =>
            {
                string? repoUrl = mount.repoUrl;
                if (repoUrl is null)
                {
                    return null;
                }
                string slash = "/";
                string repo = Utils_strings.trimEndChar(Tsonic.CSharp.Js.String.trim(repoUrl), slash);
                if (repo == "")
                {
                    return null;
                }
                string branch = Tsonic.CSharp.Js.String.trim(mount.repoBranch) == "" ? "main" : Tsonic.CSharp.Js.String.trim(mount.repoBranch);
                string rel = Utils_strings.trimStartChar(Tsonic.CSharp.Js.String.trim(repoRelPath), slash);
                if (rel == "")
                {
                    return null;
                }
                return $"{repo}/blob/{branch}/{rel}";
            };
            maybeRewriteUrl = (string? urlValue, DocsLinkRewriteContext ctx) =>
            {
                string? urlRaw = urlValue;
                if (urlRaw is null)
                {
                    return null;
                }
                string url = Tsonic.CSharp.Js.String.trim(urlRaw);
                if (isUnsafeUrl(url))
                {
                    throw Diagnostics.createTsumoError("TSUMO_DOCS_LINK_UNSAFE", $"Unsafe docs link: {url}", ctx.sourcePath);
                }
                if (url == "" || Tsonic.CSharp.Js.String.startsWith(url, "#") || isExternalUrl(url))
                {
                    return null;
                }
                UrlSuffixSplit split = Docs_url.splitUrlSuffix(url);
                string pathPart = Tsonic.CSharp.Js.String.trim(split.path);
                string suffix = split.suffix;
                if (pathPart == "")
                {
                    return null;
                }
                string slash = "/";
                string mountPrefixLower = Tsonic.CSharp.Js.String.toLowerCase(ctx.mount.urlPrefix);
                string pathLower = Tsonic.CSharp.Js.String.toLowerCase(pathPart);
                string? resolvedRel = null;
                bool escaped = false;
                if (Tsonic.CSharp.Js.String.startsWith(pathPart, "/"))
                {
                    if (mountPrefixLower == "/")
                    {
                        resolvedRel = Utils_strings.trimStartChar(pathPart, slash);
                    }
                    else
                    {
                        if (Tsonic.CSharp.Js.String.startsWith(pathLower, mountPrefixLower))
                        {
                            resolvedRel = Utils_strings.trimStartChar(Utils_strings.substringFrom(pathPart, ctx.mount.urlPrefix.Length), slash);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    resolvedRel = normalizeRelativePath(ctx.currentDirKey, pathPart);
                    escaped = resolvedRel is null;
                }
                if (escaped)
                {
                    if (ctx.strictLinks)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_DOCS_LINK_ESCAPES_MOUNT", $"Out-of-mount link from {ctx.mount.name}: {url}", ctx.sourcePath);
                    }
                    string? repoPathRaw = ctx.mount.repoPath;
                    if (repoPathRaw is null || Tsonic.CSharp.Js.String.trim(repoPathRaw) == "")
                    {
                        return null;
                    }
                    string repoPath = Utils_strings.trimEndChar(Utils_strings.trimStartChar(Tsonic.CSharp.Js.String.trim(repoPathRaw), slash), slash);
                    string baseDir = Tsonic.CSharp.Js.String.trim(ctx.currentDirKey) == "" ? repoPath : $"{repoPath}/{ctx.currentDirKey}";
                    string? repoRel = normalizeRelativePath(baseDir, pathPart);
                    if (repoRel is null)
                    {
                        return null;
                    }
                    string? gh = computeGitHubBlobUrl(ctx.mount, repoRel);
                    return gh is not null ? gh + suffix : null;
                }
                if (resolvedRel is null)
                {
                    return null;
                }
                if (!isMarkdownLink(resolvedRel))
                {
                    return null;
                }
                string key = Tsonic.CSharp.Js.String.toLowerCase(resolvedRel);
                string? mapped = Tsonic.CSharp.Js.Map.getReference<string, string>(ctx.relPermalinkByRelPathLower, key);
                if (mapped is not null)
                {
                    return mapped + suffix;
                }
                if (ctx.strictLinks)
                {
                    throw Diagnostics.createTsumoError("TSUMO_DOCS_LINK_UNRESOLVED", $"Unresolved docs link from {ctx.mount.name}: {url}", ctx.sourcePath);
                }
                return null;
            };
            rewriteInInlines = (Markdig.Syntax.Inlines.ContainerInline container, DocsLinkRewriteContext ctx) =>
            {
                Markdig.Syntax.Inlines.ContainerInline.Enumerator it = container.GetEnumerator();
                while (it.MoveNext())
                {
                    Markdig.Syntax.Inlines.Inline inline = it.Current;
                    if (inline is Markdig.Syntax.Inlines.LinkInline)
                    {
                        Markdig.Syntax.Inlines.LinkInline link = (Markdig.Syntax.Inlines.LinkInline)inline;
                        string? updated = maybeRewriteUrl(link.Url, ctx);
                        if (updated is not null)
                        {
                            link.Url = updated;
                        }
                    }
                    if (inline is Markdig.Syntax.Inlines.ContainerInline)
                    {
                        rewriteInInlines((Markdig.Syntax.Inlines.ContainerInline)inline, ctx);
                    }
                }
                it.Dispose();
            };
            rewriteInBlock = (Markdig.Syntax.Block block, DocsLinkRewriteContext ctx) =>
            {
                if (block is Markdig.Syntax.LeafBlock)
                {
                    Markdig.Syntax.LeafBlock leaf = (Markdig.Syntax.LeafBlock)block;
                    Markdig.Syntax.Inlines.ContainerInline? inline = leaf.Inline;
                    if (inline is not null)
                    {
                        rewriteInInlines(inline, ctx);
                    }
                    if ((Markdig.Syntax.LeafBlock)block is Markdig.Syntax.LinkReferenceDefinition)
                    {
                        Markdig.Syntax.LinkReferenceDefinition def = (Markdig.Syntax.LinkReferenceDefinition)block;
                        string? updated = maybeRewriteUrl(def.Url, ctx);
                        if (updated is not null)
                        {
                            def.Url = updated;
                        }
                    }
                }
                if (block is Markdig.Syntax.ContainerBlock)
                {
                    Markdig.Syntax.ContainerBlock container = (Markdig.Syntax.ContainerBlock)block;
                    Markdig.Syntax.ContainerBlock.Enumerator it = container.GetEnumerator();
                    while (it.MoveNext())
                    {
                        rewriteInBlock(it.Current, ctx);
                    }
                    it.Dispose();
                }
            };
            rewriteLinks = (Markdig.Syntax.MarkdownDocument document, DocsLinkRewriteContext ctx) =>
            {
                rewriteInBlock(document, ctx);
            };
            normalizeNewlines = (string text) => Utils_strings.replaceLineEndings(text, "\n");
            summaryMarker = "<!--more-->";
            summaryMarkerLength = summaryMarker.Length;
            findSummaryDividerIndex = (string markdown) => Utils_strings.indexOfTextIgnoreCase(markdown, summaryMarker);
            firstBlock = (string markdown) =>
            {
                string text = Tsonic.CSharp.Js.String.trim(markdown);
                if (text == "")
                {
                    return "";
                }
                int idx = Utils_strings.indexOfText(text, "\n\n");
                return idx >= 0 ? Utils_strings.substringCount(text, 0, idx) : text;
            };
            renderWithRewrites = (string markdown, DocsLinkRewriteContext ctx) =>
            {
                Markdig.Syntax.MarkdownDocument doc = Markdig.Markdown.Parse(markdown, Markdown_pipeline.markdownPipeline);
                rewriteLinks(doc, ctx);
                return Markdig.Markdown.ToHtml(doc, Markdown_pipeline.markdownPipeline);
            };
            renderDocsMarkdown = (string markdownRaw, DocsLinkRewriteContext ctx) =>
            {
                string markdown = normalizeNewlines(markdownRaw);
                int moreIndex = findSummaryDividerIndex(markdown);
                if (moreIndex >= 0)
                {
                    string before = Utils_strings.substringCount(markdown, 0, moreIndex);
                    string after = Utils_strings.substringFrom(markdown, moreIndex + summaryMarkerLength);
                    string full = before + after;
                    return new MarkdownResult(renderWithRewrites(full, ctx), Tsonic.CSharp.Js.String.trim(renderWithRewrites(before, ctx)), Markdig.Markdown.ToPlainText(full, Markdown_pipeline.markdownPipeline), "");
                }
                string html = renderWithRewrites(markdown, ctx);
                string summarySource = firstBlock(markdown);
                string summaryHtml = summarySource == "" ? "" : Tsonic.CSharp.Js.String.trim(renderWithRewrites(summarySource, ctx));
                return new MarkdownResult(html, summaryHtml, Markdig.Markdown.ToPlainText(markdown, Markdown_pipeline.markdownPipeline), "");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DocsLinkRewriteContext
    {
        public DocsMountConfig mount;
        public string sourcePath;
        public string currentDirKey;
        public Tsonic.CSharp.Js.Map<string, string> relPermalinkByRelPathLower;
        public bool strictLinks;
        public DocsLinkRewriteContext(DocsMountConfig mount, string sourcePath, string currentDirKey, Tsonic.CSharp.Js.Map<string, string> relPermalinkByRelPathLower, bool strictLinks)
        {
            this.mount = mount;
            this.sourcePath = sourcePath;
            this.currentDirKey = currentDirKey;
            this.relPermalinkByRelPathLower = relPermalinkByRelPathLower;
            this.strictLinks = strictLinks;
        }
    }
}
