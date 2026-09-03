namespace Tsumo.Engine
{
    public static class Template_values_site
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class SiteValue : TemplateValue
    {
        public SiteContext value;
        public SiteValue(SiteContext value) : base()
        {
            this.value = value;
        }
    }
    public class LanguageValue : TemplateValue
    {
        public LanguageContext value;
        public LanguageValue(LanguageContext value) : base()
        {
            this.value = value;
        }
    }
    public class SitesValue : TemplateValue
    {
        public SiteContext value;
        public SitesValue(SiteContext value) : base()
        {
            this.value = value;
        }
    }
    public class SitesArrayValue : TemplateValue
    {
        public Tsonic.CSharp.Js.JSArray<SiteContext> value;
        public SitesArrayValue(Tsonic.CSharp.Js.JSArray<SiteContext> value) : base()
        {
            this.value = value;
        }
    }
}
