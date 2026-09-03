namespace Tsumo.Engine
{
    public static class Config_loadedConfig
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
    public class LoadedConfig
    {
        public string? path;
        public SiteConfig config;
        public LoadedConfig(string? path, SiteConfig config)
        {
            this.path = path;
            this.config = config;
        }
    }
}
