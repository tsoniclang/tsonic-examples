using System;

namespace Tsumo.Engine
{
    public static class Build_planTaxonomyOutputs
    {
        public static Action<StandardTaxonomyGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>> planTaxonomyOutputs
        {
            get;
            private set;
        } = default(Action<StandardTaxonomyGraph, BuildEnvironment, StandardTemplates, SiteOutputPlan, Tsonic.CSharp.Js.Map<string, bool>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Env.__tsonic_module_init();
            Build_layout.__tsonic_module_init();
            Build_outputPlan.__tsonic_module_init();
            Build_siteRoutes.__tsonic_module_init();
            Build_standardTaxonomies.__tsonic_module_init();
            Build_standardTemplates.__tsonic_module_init();
            planTaxonomyOutputs = (StandardTaxonomyGraph taxonomies, BuildEnvironment environment, StandardTemplates templates, SiteOutputPlan outputPlan, Tsonic.CSharp.Js.Map<string, bool> sitemapUrls) =>
            {
                for (int taxonomyIndex = 0; taxonomyIndex < taxonomies.taxonomies.length; taxonomyIndex++)
                {
                    StandardTaxonomy taxonomy = taxonomies.taxonomies[taxonomyIndex];
                    for (int termIndex = 0; termIndex < taxonomy.terms.length; termIndex++)
                    {
                        PageContext term = taxonomy.terms[termIndex];
                        string main = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{taxonomy.name}/taxonomy.html", "taxonomy/taxonomy.html", "_default/taxonomy.html", "_default/list.html" })) ?? templates.list;
                        string? @base = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{taxonomy.name}/baseof.html", "taxonomy/baseof.html", "_default/baseof.html" })) ?? templates.@base;
                        outputPlan.addText(Build_siteRoutes.siteOutputPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { taxonomy.name, term.slug })), Build_layout.renderWithBase(environment, @base, main, term), $"taxonomy term '{taxonomy.name}/{term.slug}'");
                        sitemapUrls.set(term.relPermalink, true);
                    }
                    PageContext root = taxonomy.root;
                    string main_1 = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{taxonomy.name}/terms.html", "taxonomy/terms.html", "_default/terms.html", "_default/list.html" })) ?? templates.list;
                    string? base_1 = Build_layout.selectTemplate(environment, new Tsonic.CSharp.Js.JSArray<string>(new string[] { $"{taxonomy.name}/baseof.html", "taxonomy/baseof.html", "_default/baseof.html" })) ?? templates.@base;
                    outputPlan.addText(Build_siteRoutes.siteOutputPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { taxonomy.name })), Build_layout.renderWithBase(environment, base_1, main_1, root), $"taxonomy '{taxonomy.name}'");
                    sitemapUrls.set(root.relPermalink, true);
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
