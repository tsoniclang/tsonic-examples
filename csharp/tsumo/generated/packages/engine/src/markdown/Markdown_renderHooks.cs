using System;

namespace Tsumo.Engine
{
    public static class Markdown_renderHooks
    {
        public static Markdig.Parsers.HtmlBlockParser? sharedHtmlBlockParser
        {
            get;
            internal set;
        } = default(Markdig.Parsers.HtmlBlockParser?)!;
        public static Func<Markdig.Parsers.HtmlBlockParser> getHtmlBlockParser
        {
            get;
            private set;
        } = default(Func<Markdig.Parsers.HtmlBlockParser>)!;
        public static Func<Markdig.Syntax.Inlines.ContainerInline, string> renderInlineChildrenToHtml
        {
            get;
            private set;
        } = default(Func<Markdig.Syntax.Inlines.ContainerInline, string>)!;
        public static Func<string, string> stripHtmlTags
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<Template, LinkHookValue, SiteContext, TemplateEnvironment, string> renderLinkHookTemplate
        {
            get;
            private set;
        } = default(Func<Template, LinkHookValue, SiteContext, TemplateEnvironment, string>)!;
        public static Func<Template, ImageHookValue, SiteContext, TemplateEnvironment, string> renderImageHookTemplate
        {
            get;
            private set;
        } = default(Func<Template, ImageHookValue, SiteContext, TemplateEnvironment, string>)!;
        public static Func<Template, HeadingHookValue, SiteContext, TemplateEnvironment, string> renderHeadingHookTemplate
        {
            get;
            private set;
        } = default(Func<Template, HeadingHookValue, SiteContext, TemplateEnvironment, string>)!;
        public static Action<Markdig.Syntax.Inlines.ContainerInline, RenderHookContext> rewriteInlinesForHooks
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.Inlines.ContainerInline, RenderHookContext>)!;
        public static Action<Markdig.Syntax.ContainerBlock, RenderHookContext> rewriteBlocksForHooks
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.ContainerBlock, RenderHookContext>)!;
        public static Action<Markdig.Syntax.MarkdownDocument, RenderHookContext> applyRenderHooksToAst
        {
            get;
            private set;
        } = default(Action<Markdig.Syntax.MarkdownDocument, RenderHookContext>)!;
        public static Func<string, RenderHookContext, string> renderMarkdownWithHooks
        {
            get;
            private set;
        } = default(Func<string, RenderHookContext, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Template_contexts.__tsonic_module_init();
            Models.__tsonic_module_init();
            Markdown_pipeline.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            sharedHtmlBlockParser = null;
            getHtmlBlockParser = () =>
            {
                Markdig.Parsers.HtmlBlockParser? existing = sharedHtmlBlockParser;
                if (existing is not null)
                {
                    return existing;
                }
                Markdig.Parsers.HtmlBlockParser created = new Markdig.Parsers.HtmlBlockParser();
                sharedHtmlBlockParser = created;
                return created;
            };
            renderInlineChildrenToHtml = (Markdig.Syntax.Inlines.ContainerInline container) =>
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                Markdig.Renderers.HtmlRenderer renderer = new Markdig.Renderers.HtmlRenderer(writer);
                Markdown_pipeline.setupRenderer(renderer);
                renderer.WriteChildren(container);
                return writer.ToString();
            };
            stripHtmlTags = (string html) =>
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                bool inTag = false;
                for (int i = 0; i < html.Length; i++)
                {
                    string c = Utils_strings.substringCount(html, i, 1);
                    if (c == "<")
                    {
                        inTag = true;
                        continue;
                    }
                    if (c == ">")
                    {
                        inTag = false;
                        continue;
                    }
                    if (!inTag)
                    {
                        result.Append(c);
                    }
                }
                return result.ToString();
            };
            renderLinkHookTemplate = (Template template, LinkHookValue hookValue, SiteContext site, TemplateEnvironment env) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> emptyOverrides = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>();
                return env.renderTemplate(template, hookValue, site, emptyOverrides);
            };
            renderImageHookTemplate = (Template template, ImageHookValue hookValue, SiteContext site, TemplateEnvironment env) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> emptyOverrides = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>();
                return env.renderTemplate(template, hookValue, site, emptyOverrides);
            };
            renderHeadingHookTemplate = (Template template, HeadingHookValue hookValue, SiteContext site, TemplateEnvironment env) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>> emptyOverrides = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<TemplateNode>>();
                return env.renderTemplate(template, hookValue, site, emptyOverrides);
            };
            rewriteInlinesForHooks = (Markdig.Syntax.Inlines.ContainerInline container, RenderHookContext hookCtx) =>
            {
                Tsonic.CSharp.Js.JSArray<Markdig.Syntax.Inlines.LinkInline> linksToRewrite = new Tsonic.CSharp.Js.JSArray<Markdig.Syntax.Inlines.LinkInline>(new Markdig.Syntax.Inlines.LinkInline[] { });
                Markdig.Syntax.Inlines.ContainerInline.Enumerator it = container.GetEnumerator();
                while (it.MoveNext())
                {
                    Markdig.Syntax.Inlines.Inline inline = it.Current;
                    if (inline is Markdig.Syntax.Inlines.LinkInline)
                    {
                        Markdig.Syntax.Inlines.LinkInline link = (Markdig.Syntax.Inlines.LinkInline)(Markdig.Syntax.Inlines.LinkInline)inline;
                        bool isImage = link.IsImage;
                        bool hasHook = isImage ? hookCtx.imageHook is not null : hookCtx.linkHook is not null;
                        if (hasHook)
                        {
                            linksToRewrite.push(link);
                        }
                    }
                    if (inline is Markdig.Syntax.Inlines.ContainerInline)
                    {
                        rewriteInlinesForHooks((Markdig.Syntax.Inlines.ContainerInline)(Markdig.Syntax.Inlines.ContainerInline)inline, hookCtx);
                    }
                }
                it.Dispose();
                Tsonic.CSharp.Js.JSArray<Markdig.Syntax.Inlines.LinkInline> linkArr = linksToRewrite;
                for (int i = 0; i < linkArr.length; i++)
                {
                    Markdig.Syntax.Inlines.LinkInline link_1 = linkArr[i];
                    bool isImage_1 = link_1.IsImage;
                    Template? imageHook = hookCtx.imageHook;
                    Template? linkHook = hookCtx.linkHook;
                    if (isImage_1 && imageHook is not null)
                    {
                        string altHtml = renderInlineChildrenToHtml(link_1);
                        string alt = stripHtmlTags(altHtml);
                        string title = link_1.Title ?? "";
                        string url = link_1.Url ?? "";
                        ImageHookContext ctx = new ImageHookContext(url, alt, title, alt, hookCtx.page, hookCtx.page);
                        ImageHookValue hookValue = new ImageHookValue(ctx);
                        string hookHtml = renderImageHookTemplate(imageHook, hookValue, hookCtx.site, hookCtx.env);
                        Markdig.Syntax.Inlines.HtmlInline htmlInline = new Markdig.Syntax.Inlines.HtmlInline(hookHtml);
                        link_1.ReplaceBy(htmlInline, false);
                    }
                    else
                    {
                        if (!isImage_1 && linkHook is not null)
                        {
                            string innerHtml = renderInlineChildrenToHtml(link_1);
                            string plainText = stripHtmlTags(innerHtml);
                            string title_1 = link_1.Title ?? "";
                            string url_1 = link_1.Url ?? "";
                            LinkHookContext ctx_1 = new LinkHookContext(url_1, innerHtml, title_1, plainText, hookCtx.page, hookCtx.page);
                            LinkHookValue hookValue_1 = new LinkHookValue(ctx_1);
                            string hookHtml_1 = renderLinkHookTemplate(linkHook, hookValue_1, hookCtx.site, hookCtx.env);
                            Markdig.Syntax.Inlines.HtmlInline htmlInline_1 = new Markdig.Syntax.Inlines.HtmlInline(hookHtml_1);
                            link_1.ReplaceBy(htmlInline_1, false);
                        }
                    }
                }
            };
            rewriteBlocksForHooks = (Markdig.Syntax.ContainerBlock containerBlock, RenderHookContext hookCtx) =>
            {
                Tsonic.CSharp.Js.JSArray<Markdig.Syntax.HeadingBlock> headingsToRewrite = new Tsonic.CSharp.Js.JSArray<Markdig.Syntax.HeadingBlock>(new Markdig.Syntax.HeadingBlock[] { });
                Tsonic.CSharp.Js.JSArray<int> headingIndices = new Tsonic.CSharp.Js.JSArray<int>(new int[] { });
                Markdig.Syntax.ContainerBlock.Enumerator blockIt = containerBlock.GetEnumerator();
                int idx = 0;
                while (blockIt.MoveNext())
                {
                    Markdig.Syntax.Block block = blockIt.Current;
                    if (block is Markdig.Syntax.HeadingBlock && hookCtx.headingHook is not null)
                    {
                        Markdig.Syntax.HeadingBlock heading = (Markdig.Syntax.HeadingBlock)(Markdig.Syntax.HeadingBlock)block;
                        headingsToRewrite.push(heading);
                        headingIndices.push(idx);
                    }
                    if (block is Markdig.Syntax.LeafBlock)
                    {
                        Markdig.Syntax.LeafBlock leaf = (Markdig.Syntax.LeafBlock)(Markdig.Syntax.LeafBlock)block;
                        Markdig.Syntax.Inlines.ContainerInline? inline = leaf.Inline;
                        if (inline is not null)
                        {
                            rewriteInlinesForHooks(inline, hookCtx);
                        }
                    }
                    if (block is Markdig.Syntax.ContainerBlock)
                    {
                        rewriteBlocksForHooks((Markdig.Syntax.ContainerBlock)(Markdig.Syntax.ContainerBlock)block, hookCtx);
                    }
                    idx = idx + 1;
                }
                blockIt.Dispose();
                Template? headingHookTemplate = hookCtx.headingHook;
                if (headingHookTemplate is null)
                {
                    return;
                }
                Tsonic.CSharp.Js.JSArray<Markdig.Syntax.HeadingBlock> headingArr = headingsToRewrite;
                Tsonic.CSharp.Js.JSArray<int> indexArr = headingIndices;
                for (int i = headingArr.length - 1; i >= 0; i--)
                {
                    Markdig.Syntax.HeadingBlock heading_1 = headingArr[i];
                    int headingIdx = indexArr[i];
                    Markdig.Renderers.Html.HtmlAttributes? existingAttrs = Markdig.Renderers.Html.HtmlAttributesExtensions.TryGetAttributes(heading_1);
                    string anchor = existingAttrs?.Id ?? "";
                    Markdig.Syntax.Inlines.ContainerInline? inline_1 = heading_1.Inline;
                    string innerHtml = inline_1 is not null ? renderInlineChildrenToHtml(inline_1) : "";
                    string plainText = stripHtmlTags(innerHtml);
                    HeadingHookContext ctx = new HeadingHookContext(heading_1.Level, innerHtml, plainText, anchor, hookCtx.page, hookCtx.page);
                    HeadingHookValue hookValue = new HeadingHookValue(ctx);
                    string hookHtml = renderHeadingHookTemplate(headingHookTemplate, hookValue, hookCtx.site, hookCtx.env);
                    Markdig.Parsers.HtmlBlockParser parser = getHtmlBlockParser();
                    Markdig.Syntax.HtmlBlock htmlBlock = new Markdig.Syntax.HtmlBlock(parser);
                    htmlBlock.Lines = new Markdig.Helpers.StringLineGroup(hookHtml);
                    containerBlock.RemoveAt(headingIdx);
                    containerBlock.Insert(headingIdx, htmlBlock);
                }
            };
            applyRenderHooksToAst = (Markdig.Syntax.MarkdownDocument document, RenderHookContext hookCtx) =>
            {
                if (!hookCtx.hasAnyHooks())
                {
                    return;
                }
                rewriteBlocksForHooks(document, hookCtx);
            };
            renderMarkdownWithHooks = (string markdown, RenderHookContext hookCtx) =>
            {
                Markdig.Syntax.MarkdownDocument document = Markdig.Markdown.Parse(markdown, Markdown_pipeline.markdownPipeline);
                applyRenderHooksToAst(document, hookCtx);
                return Markdig.Markdown.ToHtml(document, Markdown_pipeline.markdownPipeline);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class RenderHookContext
    {
        public PageContext page;
        public SiteContext site;
        public TemplateEnvironment env;
        public Template? linkHook;
        public Template? imageHook;
        public Template? headingHook;
        public RenderHookContext(PageContext page, SiteContext site, TemplateEnvironment env)
        {
            this.page = page;
            this.site = site;
            this.env = env;
            this.linkHook = env.getRenderHookTemplate("render-link");
            this.imageHook = env.getRenderHookTemplate("render-image");
            this.headingHook = env.getRenderHookTemplate("render-heading");
        }
        public bool hasAnyHooks()
        {
            return this.linkHook is not null || this.imageHook is not null || this.headingHook is not null;
        }
    }
}
