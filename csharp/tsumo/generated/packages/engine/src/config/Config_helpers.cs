namespace Tsumo.Engine
{
    public static class Config_helpers
    {
        public static string? tryGetFirstExisting(Tsonic.CSharp.Js.JSArray<string> paths)
        {
            for (int i = 0; i < paths.length; i++)
            {
                string p = paths[i];
                if (Fs.fileExists(p))
                {
                    return p;
                }
            }
            return null;
        }
        public static Tsonic.CSharp.Js.JSArray<LanguageConfig> sortLanguages(Tsonic.CSharp.Js.JSArray<LanguageConfig> langs)
        {
            Tsonic.CSharp.Js.JSArray<LanguageConfig> copy = Tsonic.CSharp.Js.JSArray<LanguageConfig>.of([]);
            for (int i = 0; i < langs.length; i++)
            {
                copy.push(langs[i]);
            }
            return copy.sort((LanguageConfig a, LanguageConfig b) =>
            {
                int byWeight = a.weight - b.weight;
                return byWeight != 0 ? byWeight : Utils_strings.compareText(a.lang, b.lang);
            });
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
