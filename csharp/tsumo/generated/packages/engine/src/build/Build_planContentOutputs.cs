using System;

namespace Tsumo.Engine
{
    public static class Build_planContentOutputs
    {
        public static Func<string, string> outputDirectory
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Action<StandardPageGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>> planContentOutputs
        {
            get;
            private set;
        } = default(Action<StandardPageGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Env.__tsonic_module_init();
            Build_bundleResources.__tsonic_module_init();
            Build_layout.__tsonic_module_init();
            Build_outputPlan.__tsonic_module_init();
            Build_siteRoutes.__tsonic_module_init();
            Build_standardPageGraph.__tsonic_module_init();
            Build_standardTemplates.__tsonic_module_init();
            outputDirectory = (string relativePath) =>
            {
                Tsonic.CSharp.Js.JSArray<string> segments = Build_siteRoutes.splitSitePath(relativePath);
                Tsonic.CSharp.Js.JSArray<string> directorySegments = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int index = 0; index < segments.length - 1; index++)
                {
                    directorySegments.push(segments[index]);
                }
                return Build_siteRoutes.joinSitePath(directorySegments);
            };
            planContentOutputs = (StandardPageGraph graph, BuildEnvironment environment, StandardTemplates templates, SiteOutputPlan outputPlan, Tsonic.CSharp.Js.Map<string, bool> sitemapUrls) =>
            {
                for (int index = 0; index < graph.pageSources.length; index++)
                {
                    ContentPageSource source = graph.pageSources[index];
                    PageContext page = graph.contentPages[index];
                    string templateType = source.type != "" ? source.type : source.section;
                    string? layout = source.layout;
                    Tsonic.CSharp.Js.JSArray<string> candidates = layout is not null && Tsonic.CSharp.Js.String.trim(layout) != "" ? new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{templateType}/{layout}.html", $"{source.section}/{layout}.html", $"_default/{layout}.html", $"{layout}.html", $"{templateType}/single.html", $"{source.section}/single.html", "_default/single.html" }) : new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{templateType}/single.html", source.section != "" ? $"{source.section}/single.html" : "_default/single.html", "_default/single.html" });
                    string main = Build_layout.selectTemplate(environment, candidates) ?? templates.single;
                    string? @base = Build_layout.selectTemplate(environment, templateType != "" ? new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{templateType}/baseof.html", $"{source.section}/baseof.html", "_default/baseof.html", "baseof.html" }) : new Tsonic.CSharp.Js.JSArray<string>(new string[] { "_default/baseof.html", "baseof.html" })) ?? templates.@base;
                    outputPlan.addText(source.outputRelPath, Build_layout.renderWithBase(environment, @base, main, page), $"content page '{source.sourcePath}'");
                    sitemapUrls.set(page.relPermalink, true);
                    string? bundleSource = Tsonic.CSharp.Js.Map.getReference<PageContext, string>(graph.bundleSourceByPage, page);
                    if (bundleSource is not null)
                    {
                        Build_bundleResources.addBundleResources(bundleSource, outputDirectory(source.outputRelPath), $"leaf bundle '{source.sourcePath}'", outputPlan);
                    }
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
