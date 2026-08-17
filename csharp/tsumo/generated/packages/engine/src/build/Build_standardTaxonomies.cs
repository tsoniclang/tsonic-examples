using System;

namespace Tsumo.Engine
{
    public static class Build_standardTaxonomies
    {
        public static Func<StandardPageGraph, string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>, StandardTaxonomy> createTaxonomyPage
        {
            get;
            private set;
        } = default(Func<StandardPageGraph, string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>, StandardTaxonomy>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<PageContext>, Func<PageContext, Tsonic.CSharp.Js.JSArray<string>>, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>> collectTerms
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<PageContext>, Func<PageContext, Tsonic.CSharp.Js.JSArray<string>>, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>>)!;
        public static Func<StandardPageGraph, StandardTaxonomyGraph> createStandardTaxonomies
        {
            get;
            private set;
        } = default(Func<StandardPageGraph, StandardTaxonomyGraph>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Params.__tsonic_module_init();
            Utils_html.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_urlPath.__tsonic_module_init();
            Build_standardPageGraph.__tsonic_module_init();
            createTaxonomyPage = (StandardPageGraph graph, string taxonomy, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> pagesByTerm) =>
            {
                Tsonic.CSharp.Js.JSArray<string> emptyStrings = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                Tsonic.CSharp.Js.JSArray<PageContext> emptyPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                HtmlString emptyHtml = new HtmlString("");
                Tsonic.CSharp.Js.Map<string, ParamValue> taxonomyParameters = new Tsonic.CSharp.Js.Map<string, ParamValue>();
                taxonomyParameters.set("taxonomy", ParamValue.@string(taxonomy));
                PageContext root = new PageContext(Utils_text.humanizeSlug(taxonomy), "", "", false, "taxonomy", taxonomy, taxonomy, taxonomy, Utils_urlPath.combineUrlPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { taxonomy })), "", emptyHtml, emptyHtml, emptyHtml, "", emptyStrings, emptyStrings, taxonomyParameters, null, graph.site.Language, emptyPages, null, graph.site, emptyPages, graph.home, new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { graph.home }), null);
                Tsonic.CSharp.Js.JSArray<string> termSlugs = Tsonic.CSharp.Js.JSArrayStatics.from<string>(pagesByTerm.keys());
                termSlugs.sort((string left, string right) => Utils_strings.compareText(left, right));
                Tsonic.CSharp.Js.JSArray<PageContext> terms = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                for (int index = 0; index < termSlugs.length; index++)
                {
                    string termSlug = termSlugs[index];
                    Tsonic.CSharp.Js.JSArray<PageContext>? termPages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(pagesByTerm, termSlug);
                    if (termPages is null)
                    {
                        continue;
                    }
                    Tsonic.CSharp.Js.Map<string, ParamValue> parameters = new Tsonic.CSharp.Js.Map<string, ParamValue>();
                    parameters.set("term", ParamValue.@string(termSlug));
                    parameters.set("taxonomy", ParamValue.@string(taxonomy));
                    PageContext term = new PageContext(Utils_text.humanizeSlug(termSlug), "", "", false, "term", taxonomy, taxonomy, termSlug, Utils_urlPath.combineUrlPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { taxonomy, termSlug })), "", new HtmlString(""), new HtmlString(""), new HtmlString(""), "", emptyStrings, emptyStrings, parameters, null, graph.site.Language, emptyPages, null, graph.site, termPages, root, new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { graph.home, root }), null);
                    terms.push(term);
                }
                root.pages = terms;
                Tsonic.CSharp.Js.Map<string, PageContext> termPages_1 = new Tsonic.CSharp.Js.Map<string, PageContext>();
                for (int index_1 = 0; index_1 < terms.length; index_1++)
                {
                    PageContext term_1 = terms[index_1];
                    termPages_1.set(term_1.slug, term_1);
                }
                graph.site.taxonomyTermPages.set(taxonomy, termPages_1);
                return new StandardTaxonomy(taxonomy, root, terms);
            };
            collectTerms = (Tsonic.CSharp.Js.JSArray<PageContext> pages, Func<PageContext, Tsonic.CSharp.Js.JSArray<string>> selectTerms) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> pagesByTerm = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>();
                for (int pageIndex = 0; pageIndex < pages.length; pageIndex++)
                {
                    PageContext page = pages[pageIndex];
                    Tsonic.CSharp.Js.JSArray<string> terms = selectTerms(page);
                    for (int termIndex = 0; termIndex < terms.length; termIndex++)
                    {
                        string termText = Tsonic.CSharp.Js.String.trim(terms[termIndex]);
                        if (termText == "")
                        {
                            continue;
                        }
                        string termSlug = Utils_text.slugify(termText);
                        if (termSlug == "")
                        {
                            continue;
                        }
                        Tsonic.CSharp.Js.JSArray<PageContext> termPages = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<PageContext>>(pagesByTerm, termSlug) ?? new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                        termPages.push(page);
                        pagesByTerm.set(termSlug, termPages);
                    }
                }
                return pagesByTerm;
            };
            createStandardTaxonomies = (StandardPageGraph graph) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> tagsByTerm = collectTerms(graph.contentPages, (PageContext page) => page.tags);
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> categoriesByTerm = collectTerms(graph.contentPages, (PageContext page) => page.categories);
                graph.site.Taxonomies.set("tags", tagsByTerm);
                graph.site.Taxonomies.set("categories", categoriesByTerm);
                Tsonic.CSharp.Js.JSArray<StandardTaxonomy> taxonomies = new Tsonic.CSharp.Js.JSArray<StandardTaxonomy>(new StandardTaxonomy[] { createTaxonomyPage(graph, "tags", tagsByTerm), createTaxonomyPage(graph, "categories", categoriesByTerm) });
                Tsonic.CSharp.Js.JSArray<PageContext> allPages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
                for (int index = 0; index < graph.site.allPages.length; index++)
                {
                    allPages.push(graph.site.allPages[index]);
                }
                for (int taxonomyIndex = 0; taxonomyIndex < taxonomies.length; taxonomyIndex++)
                {
                    StandardTaxonomy taxonomy = taxonomies[taxonomyIndex];
                    allPages.push(taxonomy.root);
                    for (int termIndex = 0; termIndex < taxonomy.terms.length; termIndex++)
                    {
                        allPages.push(taxonomy.terms[termIndex]);
                    }
                }
                graph.site.allPages = allPages;
                return new StandardTaxonomyGraph(taxonomies);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class StandardTaxonomy
    {
        public string name;
        public PageContext root;
        public Tsonic.CSharp.Js.JSArray<PageContext> terms;
        public StandardTaxonomy(string name, PageContext root, Tsonic.CSharp.Js.JSArray<PageContext> terms)
        {
            this.name = name;
            this.root = root;
            this.terms = terms;
        }
    }
    public class StandardTaxonomyGraph
    {
        public Tsonic.CSharp.Js.JSArray<StandardTaxonomy> taxonomies;
        public StandardTaxonomyGraph(Tsonic.CSharp.Js.JSArray<StandardTaxonomy> taxonomies)
        {
            this.taxonomies = taxonomies;
        }
    }
}
