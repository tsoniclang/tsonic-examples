using System;

namespace Tsumo.Engine
{
    public static class Env
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Layouts.__tsonic_module_init();
            Resources.__tsonic_module_init();
            Models.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Resources_paths.__tsonic_module_init();
            Template_dataLoader.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class BuildEnvironment : LayoutEnvironment
    {
        public string siteDir;
        public string? themeDir;
        public string outputDir;
        public ResourceManager resources;
        public BuildEnvironment(string siteDir, string? themeDir, string outputDir, Tsonic.CSharp.Js.JSArray<ModuleMount>? mounts = null, Tsonic.CSharp.Js.Date? buildTime = null) : base(siteDir, themeDir, mounts, buildTime, Template_dataLoader.loadSiteData(siteDir, themeDir, mounts))
        {
            this.siteDir = siteDir;
            this.themeDir = themeDir;
            this.outputDir = outputDir;
            this.resources = new ResourceManager(siteDir, themeDir, outputDir);
        }
        public override ResourceManager? getResourceManager()
        {
            return this.resources;
        }
        public override string? getEnvironmentVariable(string name)
        {
            return Tsonic.CSharp.Node.process.env[name];
        }
        public override bool sourceFileExists(string path)
        {
            return Fs.fileExists(Resources_paths.resolveContainedResourcePath(this.siteDir, path));
        }
    }
}
