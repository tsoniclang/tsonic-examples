using System;

namespace Tsumo.Engine
{
    public static class Build_planHomeOutput
    {
        public static Action<StandardPageGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>> planHomeOutput
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
            Build_standardPageGraph.__tsonic_module_init();
            Build_standardTemplates.__tsonic_module_init();
            planHomeOutput = (StandardPageGraph graph, BuildEnvironment environment, StandardTemplates templates, SiteOutputPlan outputPlan, Tsonic.CSharp.Js.Map<string, bool> sitemapUrls) =>
            {
                outputPlan.addText("index.html", Build_layout.renderWithBase(environment, templates.@base, templates.home, graph.home), "home page");
                sitemapUrls.set("/", true);
                string? bundleSource = Tsonic.CSharp.Js.Map.getReference<PageContext, string>(graph.bundleSourceByPage, graph.home);
                if (bundleSource is not null)
                {
                    Build_bundleResources.addBundleResources(bundleSource, "", "home bundle", outputPlan);
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
