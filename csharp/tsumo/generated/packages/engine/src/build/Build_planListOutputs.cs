using System;

namespace Tsumo.Engine
{
    public static class Build_planListOutputs
    {
        public static Action<StandardPageGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>> planListOutputs
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
            planListOutputs = (StandardPageGraph graph, BuildEnvironment environment, StandardTemplates templates, SiteOutputPlan outputPlan, Tsonic.CSharp.Js.Map<string, bool> sitemapUrls) =>
            {
                for (int index = 0; index < graph.listRoutes.length; index++)
                {
                    string route = graph.listRoutes[index];
                    if (route == "")
                    {
                        continue;
                    }
                    PageContext? page = Tsonic.CSharp.Js.Map.getReference<string, PageContext>(graph.listPagesByRoute, route);
                    if (page is null)
                    {
                        continue;
                    }
                    string main = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{page.type}/list.html", $"{page.section}/list.html", "_default/list.html" })) ?? templates.list;
                    string? @base = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{page.type}/baseof.html", $"{page.section}/baseof.html", "_default/baseof.html" })) ?? templates.@base;
                    outputPlan.addText(Build_siteRoutes.siteOutputPath(Build_siteRoutes.splitSitePath(route)), Build_layout.renderWithBase(environment, @base, main, page), $"section '{route}'");
                    sitemapUrls.set(page.relPermalink, true);
                    string? bundleSource = Tsonic.CSharp.Js.Map.getReference<PageContext, string>(graph.bundleSourceByPage, page);
                    if (bundleSource is not null)
                    {
                        Build_bundleResources.addBundleResources(bundleSource, route, $"section bundle '{route}'", outputPlan);
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
