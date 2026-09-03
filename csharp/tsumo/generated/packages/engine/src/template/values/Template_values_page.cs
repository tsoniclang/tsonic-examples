namespace Tsumo.Engine
{
    public static class Template_values_page
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
    public class PageValue : TemplateValue
    {
        public PageContext value;
        public PageValue(PageContext value) : base()
        {
            this.value = value;
        }
    }
    public class FileValue : TemplateValue
    {
        public PageFile value;
        public FileValue(PageFile value) : base()
        {
            this.value = value;
        }
    }
    public class PageArrayValue : TemplateValue
    {
        public Tsonic.CSharp.Js.JSArray<PageContext> value;
        public PageArrayValue(Tsonic.CSharp.Js.JSArray<PageContext> value) : base()
        {
            this.value = value;
        }
    }
    public class PageGroupValue : TemplateValue
    {
        public TemplateValue key;
        public Tsonic.CSharp.Js.JSArray<PageContext> pages;
        public PageGroupValue(TemplateValue key, Tsonic.CSharp.Js.JSArray<PageContext> pages) : base()
        {
            this.key = key;
            this.pages = pages;
        }
    }
    public class PageDataValue : TemplateValue
    {
        public PageContext page;
        public PageDataValue(PageContext page) : base()
        {
            this.page = page;
        }
    }
    public class PageResourcesValue : TemplateValue
    {
        public PageContext page;
        public ResourceManager manager;
        public PageResourcesValue(PageContext page, ResourceManager manager) : base()
        {
            this.page = page;
            this.manager = manager;
        }
    }
}
