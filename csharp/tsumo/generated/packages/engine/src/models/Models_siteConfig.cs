namespace Tsumo.Engine
{
    public static class Models_siteConfig
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Models_menuEntry.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ModuleMount
    {
        public string source;
        public string target;
        public ModuleMount(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
    }
    public class SiteConfig
    {
        public string title;
        public string baseURL;
        public string languageCode;
        public string contentDir;
        public Tsonic.CSharp.Js.JSArray<LanguageConfig> languages;
        public string? theme;
        public string? copyright;
        public Tsonic.CSharp.Js.Map<string, ParamValue> Params;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<MenuEntry>> Menus;
        public Tsonic.CSharp.Js.JSArray<ModuleMount> moduleMounts;
        public SiteConfig(string title, string baseURL, string languageCode, string? theme, string? copyright)
        {
            this.title = title;
            this.baseURL = baseURL;
            this.languageCode = languageCode;
            this.contentDir = "content";
            Tsonic.CSharp.Js.JSArray<LanguageConfig> empty = new Tsonic.CSharp.Js.JSArray<LanguageConfig>(new LanguageConfig[] { });
            this.languages = empty;
            this.theme = theme;
            this.copyright = copyright;
            this.Params = new Tsonic.CSharp.Js.Map<string, ParamValue>();
            this.Menus = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<MenuEntry>>();
            Tsonic.CSharp.Js.JSArray<ModuleMount> emptyMounts = new Tsonic.CSharp.Js.JSArray<ModuleMount>(new ModuleMount[] { });
            this.moduleMounts = emptyMounts;
        }
    }
}
