namespace Tsumo.Engine
{
    public static class Template_values_output
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
    public class OutputFormatsValue : TemplateValue
    {
        public SiteContext site;
        public OutputFormatsValue(SiteContext site) : base()
        {
            this.site = site;
        }
    }
    public class OutputFormatValue : TemplateValue
    {
        public OutputFormat value;
        public OutputFormatValue(OutputFormat value) : base()
        {
            this.value = value;
        }
    }
    public class OutputFormatsGetValue : TemplateValue
    {
        public SiteContext site;
        public OutputFormatsGetValue(SiteContext site) : base()
        {
            this.site = site;
        }
    }
}
