namespace Tsumo.Tests
{
    public static class Index
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            ScaffoldAndBuildTest.__tsonic_module_init();
            InputBoundariesTest.__tsonic_module_init();
            FilesystemBoundariesTest.__tsonic_module_init();
            ContentAndMenuTest.__tsonic_module_init();
            DocsDomainTest.__tsonic_module_init();
            OutputPlanTest.__tsonic_module_init();
            ResourcePipelineTest.__tsonic_module_init();
            TemplatePageContextTest.__tsonic_module_init();
            TemplateFunctionSemanticsTest.__tsonic_module_init();
            TemplateRuntimeTest.__tsonic_module_init();
            ThemeCompatibilityTest.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
