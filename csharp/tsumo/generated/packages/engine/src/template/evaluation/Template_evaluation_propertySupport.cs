namespace Tsumo.Engine
{
    public static class Template_evaluation_propertySupport
    {
        public static Tsonic.CSharp.Js.Map<PageContext, ScratchStore> pageStores
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.Map<PageContext, ScratchStore>)!;
        public static Tsonic.CSharp.Js.Map<SiteContext, ScratchStore> siteStores
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.Map<SiteContext, ScratchStore>)!;
        public static AnyArrayValue taxonomyTermsByCount(Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<PageContext>> terms)
        {
            Tsonic.CSharp.Js.JSArray<string> names = Tsonic.CSharp.Js.JSArrayStatics.from<string>(terms.keys());
            names.sort((string left, string right) =>
            {
                int leftCount = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(terms, left)?.length ?? 0;
                int rightCount = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(terms, right)?.length ?? 0;
                return leftCount > rightCount ? -1 : leftCount < rightCount ? 1 : Utils_strings.compareText(left, right);
            });
            Tsonic.CSharp.Js.JSArray<TemplateValue> values = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            for (int index = 0; index < names.length; index++)
            {
                string name = names[index];
                Tsonic.CSharp.Js.JSArray<PageContext>? pages = Tsonic.CSharp.Js.Map.getOptional<string, Tsonic.CSharp.Js.JSArray<PageContext>>(terms, name);
                if (pages is null)
                {
                    continue;
                }
                Tsonic.CSharp.Js.Map<string, TemplateValue> fields = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                fields.set("Name", new StringValue(name));
                fields.set("Count", new NumberValue(pages.length));
                fields.set("Pages", new PageArrayValue(pages));
                values.push(new DictValue(fields));
            }
            return new AnyArrayValue(values);
        }
        public static DictValue wrapParamDict(Tsonic.CSharp.Js.Map<string, ParamValue> dict)
        {
            Tsonic.CSharp.Js.Map<string, TemplateValue> mapped = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
            foreach (string key in dict.keys())
            {
                ParamValue? value = Tsonic.CSharp.Js.Map.getOptional<string, ParamValue>(dict, key);
                if (value is null)
                {
                    continue;
                }
                TemplateValue wrapped = new StringValue(value.stringValue);
                if (value.kind == ParamKind.Bool)
                {
                    wrapped = new BoolValue(value.boolValue);
                }
                if (value.kind == ParamKind.Number)
                {
                    wrapped = new NumberValue(value.numberValue);
                }
                mapped.set(key, wrapped);
            }
            return new DictValue(mapped);
        }
        public static AnyArrayValue wrapLanguages(Tsonic.CSharp.Js.JSArray<LanguageContext> languages)
        {
            Tsonic.CSharp.Js.JSArray<TemplateValue> items = Tsonic.CSharp.Js.JSArray<TemplateValue>.of([]);
            for (int index = 0; index < languages.length; index++)
            {
                items.push(new LanguageValue(languages[index]));
            }
            return new AnyArrayValue(items);
        }
        public static MediaTypeValue wrapMediaType(MediaType mediaType)
        {
            return new MediaTypeValue(mediaType);
        }
        public static ScratchStore getPageStore(PageContext page)
        {
            ScratchStore? existing = Tsonic.CSharp.Js.Map.getOptional<PageContext, ScratchStore>(pageStores, page);
            if (existing is not null)
            {
                return existing;
            }
            ScratchStore store = new ScratchStore();
            pageStores.set(page, store);
            return store;
        }
        public static ScratchStore getSiteStore(SiteContext site)
        {
            ScratchStore? existing = Tsonic.CSharp.Js.Map.getOptional<SiteContext, ScratchStore>(siteStores, site);
            if (existing is not null)
            {
                return existing;
            }
            ScratchStore store = new ScratchStore();
            siteStores.set(site, store);
            return store;
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Params.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            pageStores = new Tsonic.CSharp.Js.Map<PageContext, ScratchStore>();
            siteStores = new Tsonic.CSharp.Js.Map<SiteContext, ScratchStore>();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
