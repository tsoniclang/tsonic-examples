namespace Tsumo.Engine
{
    public static class Template_values_menus
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
    public class MenuEntryValue : TemplateValue
    {
        public MenuEntry value;
        public SiteContext site;
        public MenuEntryValue(MenuEntry value, SiteContext site) : base()
        {
            this.value = value;
            this.site = site;
        }
    }
    public class MenuArrayValue : TemplateValue
    {
        public Tsonic.CSharp.Js.JSArray<MenuEntry> value;
        public SiteContext site;
        public MenuArrayValue(Tsonic.CSharp.Js.JSArray<MenuEntry> value, SiteContext site) : base()
        {
            this.value = value;
            this.site = site;
        }
    }
    public class MenusValue : TemplateValue
    {
        public SiteContext site;
        public MenusValue(SiteContext site) : base()
        {
            this.site = site;
        }
    }
}
