using System;

namespace Tsumo.Engine
{
    public static class Build_standardSite
    {
        public static Func<BuildRequest, string, string, int> buildStandardSite
        {
            get;
            private set;
        } = default(Func<BuildRequest, string, string, int>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Config.__tsonic_module_init();
            Env.__tsonic_module_init();
            Models.__tsonic_module_init();
            Outputs.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Build_discoverContent.__tsonic_module_init();
            Build_layout.__tsonic_module_init();
            Build_outputPlan.__tsonic_module_init();
            Build_planContentOutputs.__tsonic_module_init();
            Build_planHomeOutput.__tsonic_module_init();
            Build_planListOutputs.__tsonic_module_init();
            Build_planTaxonomyOutputs.__tsonic_module_init();
            Build_renderPageContent.__tsonic_module_init();
            Build_siteRoutes.__tsonic_module_init();
            Build_standardPageGraph.__tsonic_module_init();
            Build_standardTaxonomies.__tsonic_module_init();
            Build_standardTemplates.__tsonic_module_init();
            buildStandardSite = (BuildRequest request, string siteDir, string outDir) =>
            {
                SiteConfig config = Config_loader.loadSiteConfig(siteDir).config;
                string? requestedBaseUrl = request.baseURL;
                if (requestedBaseUrl is not null && Tsonic.CSharp.Js.String.trim(requestedBaseUrl) != "")
                {
                    config.baseURL = Utils_text.ensureTrailingSlash(Tsonic.CSharp.Js.String.trim(requestedBaseUrl));
                }
                string? themeDir = Build_layout.resolveThemeDir(siteDir, config, request.themesDir);
                BuildEnvironment environment = new BuildEnvironment(siteDir, themeDir, outDir, config.moduleMounts, request.buildTime);
                SiteOutputPlan outputPlan = new SiteOutputPlan();
                if (themeDir is not null)
                {
                    outputPlan.addDirectory(System.IO.Path.Combine(themeDir, "static"), "", "theme static files", "theme-static");
                }
                outputPlan.addDirectory(System.IO.Path.Combine(siteDir, "static"), "", "site static files", "site-static");
                ContentInventory inventory = Build_discoverContent.discoverContent(System.IO.Path.Combine(siteDir, config.contentDir), request.buildDrafts);
                StandardPageGraph pageGraph = Build_standardPageGraph.createStandardPageGraph(config, inventory);
                StandardTaxonomyGraph taxonomies = Build_standardTaxonomies.createStandardTaxonomies(pageGraph);
                Build_renderPageContent.renderStandardPageContent(pageGraph, environment);
                StandardTemplates templates = Build_standardTemplates.selectStandardTemplates(environment);
                Tsonic.CSharp.Js.Map<string, bool> sitemapUrls = new Tsonic.CSharp.Js.Map<string, bool>();
                Build_planHomeOutput.planHomeOutput(pageGraph, environment, templates, outputPlan, sitemapUrls);
                Build_planListOutputs.planListOutputs(pageGraph, environment, templates, outputPlan, sitemapUrls);
                Build_planTaxonomyOutputs.planTaxonomyOutputs(taxonomies, environment, templates, outputPlan, sitemapUrls);
                Build_planContentOutputs.planContentOutputs(pageGraph, environment, templates, outputPlan, sitemapUrls);
                Tsonic.CSharp.Js.JSArray<string> orderedSitemapUrls = Tsonic.CSharp.Js.JSArrayStatics.from<string>(sitemapUrls.keys());
                orderedSitemapUrls.sort((string left, string right) => Build_siteRoutes.compareSitePaths(left, right));
                outputPlan.addDefaultText("sitemap.xml", Outputs.renderSitemap(config, orderedSitemapUrls, request.buildTime), "generated sitemap");
                outputPlan.addDefaultText("index.xml", Outputs.renderRss(config, pageGraph.contentPages, request.buildTime), "generated RSS");
                outputPlan.addDefaultText("robots.txt", Outputs.renderRobotsTxt(config), "generated robots policy");
                outputPlan.applyDeferredTemplateResults(environment.finalizeDeferredTemplates());
                outputPlan.render(outDir);
                return outputPlan.generatedOutputCount();
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
