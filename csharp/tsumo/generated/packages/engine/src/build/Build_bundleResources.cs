using System;

namespace Tsumo.Engine
{
    public static class Build_bundleResources
    {
        public static Action<string, string, string, SiteOutputPlan> addBundleResources
        {
            get;
            private set;
        } = default(Action<string, string, string, SiteOutputPlan>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Resources_pageBundle.__tsonic_module_init();
            Build_outputPlan.__tsonic_module_init();
            addBundleResources = (string sourceDir, string outputPrefix, string owner, SiteOutputPlan outputPlan) =>
            {
                Tsonic.CSharp.Js.JSArray<PageBundleResourceFile> files = Resources_pageBundle.discoverPageBundleResourceFiles(sourceDir);
                for (int index = 0; index < files.length; index++)
                {
                    PageBundleResourceFile file = files[index];
                    string outputPath = outputPrefix == "" ? file.relativePath : $"{outputPrefix}/{file.relativePath}";
                    outputPlan.addAsset(outputPath, file.sourcePath, owner, "bundle");
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
