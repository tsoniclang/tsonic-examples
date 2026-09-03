using System;

namespace Tsumo.Engine
{
    public static class Markdown_renderWithShortcodes
    {
        public static Func<string, Tsonic.CSharp.Js.JSArray<ShortcodeCall>, PageContext, SiteContext, TemplateEnvironment, ShortcodeOrdinalTracker, Tsonic.CSharp.Js.Map<string, bool>, ProtectedShortcodeSource> protectStandardShortcodes
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<ShortcodeCall>, PageContext, SiteContext, TemplateEnvironment, ShortcodeOrdinalTracker, Tsonic.CSharp.Js.Map<string, bool>, ProtectedShortcodeSource>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<ProtectedShortcode>, string> restoreStandardShortcodes
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<ProtectedShortcode>, string>)!;
        public static Func<string, PageContext, SiteContext, TemplateEnvironment, MarkdownResult> renderMarkdownWithShortcodes
        {
            get;
            private set;
        } = default(Func<string, PageContext, SiteContext, TemplateEnvironment, MarkdownResult>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Shortcode.__tsonic_module_init();
            Template_environment.__tsonic_module_init();
            Models.__tsonic_module_init();
            Markdown_pipeline.__tsonic_module_init();
            Markdown_toc.__tsonic_module_init();
            Markdown_renderHooks.__tsonic_module_init();
            Markdown_shortcodes.__tsonic_module_init();
            Markdown_renderBasic.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            protectStandardShortcodes = (string text, Tsonic.CSharp.Js.JSArray<ShortcodeCall> calls, PageContext page, SiteContext site, TemplateEnvironment env, ShortcodeOrdinalTracker ordinalTracker, Tsonic.CSharp.Js.Map<string, bool> recursionGuard) =>
            {
                Tsonic.CSharp.Js.JSArray<string> outputs = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int i = 0; i < calls.length; i++)
                {
                    outputs.push(Markdown_shortcodes.renderShortcode(calls[i], page, site, env, ordinalTracker, null, recursionGuard));
                }
                string markerPrefix = "tsumo-shortcode-output";
                bool markerPrefixTaken = true;
                while (markerPrefixTaken)
                {
                    markerPrefixTaken = Tsonic.CSharp.Js.String.includes(text, $"<!--{markerPrefix}-");
                    for (int i_1 = 0; i_1 < outputs.length && !markerPrefixTaken; i_1++)
                    {
                        markerPrefixTaken = Tsonic.CSharp.Js.String.includes(outputs[i_1], $"<!--{markerPrefix}-");
                    }
                    if (markerPrefixTaken)
                    {
                        markerPrefix += "-x";
                    }
                }
                Tsonic.CSharp.Js.JSArray<ProtectedShortcode> replacements = new Tsonic.CSharp.Js.JSArray<ProtectedShortcode>(new ProtectedShortcode[] { });
                for (int i_2 = 0; i_2 < calls.length; i_2++)
                {
                    replacements.push(new ProtectedShortcode($"<!--{markerPrefix}-{i_2}-->", outputs[i_2]));
                }
                string source = text;
                for (int i_3 = calls.length - 1; i_3 >= 0; i_3--)
                {
                    ShortcodeCall call = calls[i_3];
                    source = Utils_strings.substringCount(source, 0, call.startIndex) + replacements[i_3].marker + Utils_strings.substringFrom(source, call.endIndex);
                }
                return new ProtectedShortcodeSource(source, replacements);
            };
            restoreStandardShortcodes = (string html, Tsonic.CSharp.Js.JSArray<ProtectedShortcode> replacements) =>
            {
                string result = html;
                for (int i = 0; i < replacements.length; i++)
                {
                    ProtectedShortcode replacement = replacements[i];
                    result = Tsonic.CSharp.Js.String.replace(result, replacement.marker, replacement.output);
                }
                return result;
            };
            renderMarkdownWithShortcodes = (string markdownRaw, PageContext page, SiteContext site, TemplateEnvironment env) =>
            {
                string markdown = Markdown_renderBasic.normalizeNewlines(markdownRaw);
                ShortcodeOrdinalTracker ordinalTracker = Markdown_shortcodes.createOrdinalTracker();
                Tsonic.CSharp.Js.Map<string, bool> recursionGuard = new Tsonic.CSharp.Js.Map<string, bool>();
                Tsonic.CSharp.Js.JSArray<ShortcodeCall> calls = Shortcode.parseShortcodes(markdown, page.File?.Filename);
                string textAfterMarkdownShortcodes = markdown;
                Tsonic.CSharp.Js.JSArray<ShortcodeCall> mdCalls = new Tsonic.CSharp.Js.JSArray<ShortcodeCall>(new ShortcodeCall[] { });
                for (int i = 0; i < calls.length; i++)
                {
                    ShortcodeCall call = calls[i];
                    if (call.isMarkdown)
                    {
                        mdCalls.push(call);
                    }
                }
                if (mdCalls.length > 0)
                {
                    textAfterMarkdownShortcodes = Markdown_shortcodes.processShortcodeCalls(markdown, mdCalls, page, site, env, ordinalTracker, null, recursionGuard);
                }
                Tsonic.CSharp.Js.JSArray<ShortcodeCall> parsedStandardCalls = Shortcode.parseShortcodes(textAfterMarkdownShortcodes, page.File?.Filename);
                Tsonic.CSharp.Js.JSArray<ShortcodeCall> standardCalls = new Tsonic.CSharp.Js.JSArray<ShortcodeCall>(new ShortcodeCall[] { });
                for (int i_1 = 0; i_1 < parsedStandardCalls.length; i_1++)
                {
                    ShortcodeCall call_1 = parsedStandardCalls[i_1];
                    if (!call_1.isMarkdown)
                    {
                        standardCalls.push(call_1);
                    }
                }
                ProtectedShortcodeSource protectedStandard = protectStandardShortcodes(textAfterMarkdownShortcodes, standardCalls, page, site, env, ordinalTracker, recursionGuard);
                string markdownSource = protectedStandard.source;
                string toc = Markdown_toc.generateTableOfContents(markdownSource);
                RenderHookContext hookCtx = new RenderHookContext(page, site, env);
                int moreIndex = Markdown_renderBasic.findSummaryDividerIndex(markdownSource);
                string html = default(string)!;
                string summaryHtml = default(string)!;
                string plainText = default(string)!;
                if (moreIndex >= 0)
                {
                    string before = Utils_strings.substringCount(markdownSource, 0, moreIndex);
                    string after = Utils_strings.substringFrom(markdownSource, moreIndex + Markdown_renderBasic.summaryMarkerLength);
                    string full = before + after;
                    if (hookCtx.hasAnyHooks())
                    {
                        html = Markdown_renderHooks.renderMarkdownWithHooks(full, hookCtx);
                        summaryHtml = Tsonic.CSharp.Js.String.trim(Markdown_renderHooks.renderMarkdownWithHooks(before, hookCtx));
                    }
                    else
                    {
                        html = Markdig.Markdown.ToHtml(full, Markdown_pipeline.markdownPipeline);
                        summaryHtml = Tsonic.CSharp.Js.String.trim(Markdig.Markdown.ToHtml(before, Markdown_pipeline.markdownPipeline));
                    }
                    plainText = Markdig.Markdown.ToPlainText(full, Markdown_pipeline.markdownPipeline);
                }
                else
                {
                    if (hookCtx.hasAnyHooks())
                    {
                        html = Markdown_renderHooks.renderMarkdownWithHooks(markdownSource, hookCtx);
                    }
                    else
                    {
                        html = Markdig.Markdown.ToHtml(markdownSource, Markdown_pipeline.markdownPipeline);
                    }
                    plainText = Markdig.Markdown.ToPlainText(markdownSource, Markdown_pipeline.markdownPipeline);
                    string summarySource = Markdown_renderBasic.firstBlock(markdownSource);
                    if (summarySource == "")
                    {
                        summaryHtml = "";
                    }
                    else
                    {
                        if (hookCtx.hasAnyHooks())
                        {
                            summaryHtml = Tsonic.CSharp.Js.String.trim(Markdown_renderHooks.renderMarkdownWithHooks(summarySource, hookCtx));
                        }
                        else
                        {
                            summaryHtml = Tsonic.CSharp.Js.String.trim(Markdig.Markdown.ToHtml(summarySource, Markdown_pipeline.markdownPipeline));
                        }
                    }
                }
                html = restoreStandardShortcodes(html, protectedStandard.replacements);
                summaryHtml = restoreStandardShortcodes(summaryHtml, protectedStandard.replacements);
                return new MarkdownResult(html, summaryHtml, plainText, toc);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ProtectedShortcode
    {
        public string marker;
        public string output;
        public ProtectedShortcode(string marker, string output)
        {
            this.marker = marker;
            this.output = output;
        }
    }
    public class ProtectedShortcodeSource
    {
        public string source;
        public Tsonic.CSharp.Js.JSArray<ProtectedShortcode> replacements;
        public ProtectedShortcodeSource(string source, Tsonic.CSharp.Js.JSArray<ProtectedShortcode> replacements)
        {
            this.source = source;
            this.replacements = replacements;
        }
    }
}
