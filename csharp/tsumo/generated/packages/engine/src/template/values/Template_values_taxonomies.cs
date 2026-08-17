using System;

namespace Tsumo.Engine
{
    public static class Template_values_taxonomies
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Template_values_base.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class TaxonomiesValue : TemplateValue
    {
        public SiteContext site;
        public TaxonomiesValue(SiteContext site) : base()
        {
            this.site = site;
        }
    }
    public class TaxonomyTermsValue : TemplateValue
    {
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> terms;
        public SiteContext site;
        public TaxonomyTermsValue(Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> terms, SiteContext site) : base()
        {
            this.terms = terms;
            this.site = site;
        }
    }
}
