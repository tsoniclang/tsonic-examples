using System;

namespace Tsumo.Engine
{
    public static class Build
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class BuildRequest
    {
        public string siteDir;
        public string destinationDir;
        public string? baseURL;
        public string? themesDir;
        public bool buildDrafts;
        public bool cleanDestinationDir;
        public Tsonic.CSharp.Js.Date buildTime;
        public BuildRequest(string siteDir)
        {
            this.siteDir = siteDir;
            this.destinationDir = "public";
            this.baseURL = null;
            this.themesDir = null;
            this.buildDrafts = false;
            this.cleanDestinationDir = true;
            this.buildTime = new Tsonic.CSharp.Js.Date();
        }
    }
    public class ServeRequest : BuildRequest
    {
        public string host;
        public int port;
        public bool watch;
        public ServeRequest(string siteDir) : base(siteDir)
        {
            this.host = "localhost";
            this.port = 1313;
            this.watch = true;
        }
    }
    public class BuildResult
    {
        public string outputDir;
        public int pagesBuilt;
        public BuildResult(string outputDir, int pagesBuilt)
        {
            this.outputDir = outputDir;
            this.pagesBuilt = pagesBuilt;
        }
    }
}
