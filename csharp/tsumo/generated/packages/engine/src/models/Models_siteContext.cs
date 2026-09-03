namespace Tsumo.Engine
{
    public static class Models_siteContext
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Models_menuEntry.__tsonic_module_init();
            Models_siteConfig.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class SiteContext
    {
        public string title;
        public string baseURL;
        public string languageCode;
        public string copyright;
        public LanguageContext Language;
        public Tsonic.CSharp.Js.JSArray<LanguageContext> Languages;
        public bool IsMultiLingual;
        public string LanguagePrefix;
        public Tsonic.CSharp.Js.Map<string, ParamValue> Params;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<MenuEntry>> Menus;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>> Taxonomies;
        public Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, PageContext>> taxonomyTermPages;
        public ScratchStore? store;
        public Tsonic.CSharp.Js.JSArray<PageContext> pages;
        public Tsonic.CSharp.Js.JSArray<PageContext> allPages;
        public PageContext? home;
        public Tsonic.CSharp.Js.JSArray<DocsMountContext> docsMounts;
        public Tsonic.CSharp.Js.JSArray<SiteContext> Sites;
        public int paginationSize;
        public SiteContext(SiteConfig config, Tsonic.CSharp.Js.JSArray<PageContext> pages, LanguageConfig? languageRaw, Tsonic.CSharp.Js.JSArray<LanguageContext>? allLanguagesRaw)
        {
            this.title = config.title;
            this.baseURL = config.baseURL;
            this.copyright = config.copyright ?? "";
            LanguageConfig? language = languageRaw;
            if (language is not null)
            {
                this.Language = new LanguageContext(language.lang, language.languageName, language.languageDirection);
                this.languageCode = language.lang;
            }
            else
            {
                string lang = config.languages.length > 0 ? config.languages[0].lang : (Tsonic.CSharp.Js.String.trim(config.languageCode) == "" ? "en" : config.languageCode);
                string name = config.languages.length > 0 ? config.languages[0].languageName : lang;
                string dir = config.languages.length > 0 ? config.languages[0].languageDirection : "ltr";
                this.Language = new LanguageContext(lang, name, dir);
                this.languageCode = lang;
            }
            Tsonic.CSharp.Js.JSArray<LanguageContext>? allLanguages = allLanguagesRaw;
            if (allLanguages is not null && allLanguages.length > 0)
            {
                this.Languages = allLanguages;
            }
            else
            {
                Tsonic.CSharp.Js.JSArray<LanguageContext> langs = new Tsonic.CSharp.Js.JSArray<LanguageContext>(new LanguageContext[] { this.Language });
                this.Languages = langs;
            }
            this.IsMultiLingual = false;
            this.LanguagePrefix = "";
            this.Params = config.Params;
            this.Menus = config.Menus;
            this.Taxonomies = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>>>();
            this.taxonomyTermPages = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.Map<string, PageContext>>();
            this.store = null;
            this.pages = pages;
            this.allPages = pages;
            this.home = null;
            Tsonic.CSharp.Js.JSArray<DocsMountContext> empty = new Tsonic.CSharp.Js.JSArray<DocsMountContext>(new DocsMountContext[] { });
            this.docsMounts = empty;
            Tsonic.CSharp.Js.JSArray<SiteContext> emptySites = new Tsonic.CSharp.Js.JSArray<SiteContext>(new SiteContext[] { });
            this.Sites = emptySites;
            this.paginationSize = 10;
        }
        public Tsonic.CSharp.Js.JSArray<OutputFormat> getOutputFormats()
        {
            OutputFormat rss = new OutputFormat("alternate", "application/rss+xml", this.baseURL + "index.xml");
            Tsonic.CSharp.Js.JSArray<OutputFormat> formats = new Tsonic.CSharp.Js.JSArray<OutputFormat>(new OutputFormat[] { rss });
            return formats;
        }
    }
}
