using System;

namespace Tsumo.Engine
{
    public static class BuildSite
    {
        public static Func<BuildRequest, BuildResult> buildSite
        {
            get;
            private set;
        } = default(Func<BuildRequest, BuildResult>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Docs_config.__tsonic_module_init();
            Docs_builder.__tsonic_module_init();
            OutputPublicationModule.__tsonic_module_init();
            Build_standardSite.__tsonic_module_init();
            buildSite = (BuildRequest request) =>
            {
                string siteDir = Tsonic.CSharp.Node.path.resolve(request.siteDir);
                LoadedDocsConfig? docs = Docs_config.loadDocsConfig(siteDir);
                OutputPublication publication = OutputPublicationModule.beginOutputPublication(siteDir, request.destinationDir, !request.cleanDestinationDir);
                try
                {
                    int pagesBuilt = docs is null ? Build_standardSite.buildStandardSite(request, siteDir, publication.stagingDir) : Docs_builder.buildDocsSite(request, docs, publication.stagingDir);
                    publication.publish();
                    return new BuildResult(publication.destinationDir, pagesBuilt);
                }
                catch
                {
                    publication.abort();
                    throw;
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
