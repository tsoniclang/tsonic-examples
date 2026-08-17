using System;

namespace Tsumo.Engine
{
    public static class Docs_models
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
    public class DocsMountConfig
    {
        public string name;
        public string sourceDir;
        public string urlPrefix;
        public string? repoUrl;
        public string repoBranch;
        public string? repoPath;
        public string? navPath;
        public DocsMountConfig(string name, string sourceDir, string urlPrefix, string? repoUrl, string repoBranch, string? repoPath, string? navPath)
        {
            this.name = name;
            this.sourceDir = sourceDir;
            this.urlPrefix = urlPrefix;
            this.repoUrl = repoUrl;
            this.repoBranch = repoBranch;
            this.repoPath = repoPath;
            this.navPath = navPath;
        }
    }
    public class DocsSiteConfig
    {
        public Tsonic.CSharp.Js.JSArray<DocsMountConfig> mounts;
        public bool strictLinks;
        public bool generateSearchIndex;
        public string searchIndexFileName;
        public string? homeMount;
        public string siteName;
        public DocsSiteConfig(Tsonic.CSharp.Js.JSArray<DocsMountConfig> mounts, bool strictLinks, bool generateSearchIndex, string searchIndexFileName, string? homeMount, string siteName)
        {
            this.mounts = mounts;
            this.strictLinks = strictLinks;
            this.generateSearchIndex = generateSearchIndex;
            this.searchIndexFileName = searchIndexFileName;
            this.homeMount = homeMount;
            this.siteName = siteName;
        }
    }
    public class NavItem
    {
        public string title;
        public string url;
        public Tsonic.CSharp.Js.JSArray<NavItem> children;
        public bool isSection;
        public bool isCurrent;
        public int order;
        public NavItem(string title, string url, Tsonic.CSharp.Js.JSArray<NavItem> children, bool isSection, bool isCurrent, int order)
        {
            this.title = title;
            this.url = url;
            this.children = children;
            this.isSection = isSection;
            this.isCurrent = isCurrent;
            this.order = order;
        }
    }
    public class DocsMountContext
    {
        public string name;
        public string urlPrefix;
        public Tsonic.CSharp.Js.JSArray<NavItem> nav;
        public DocsMountContext(string name, string urlPrefix, Tsonic.CSharp.Js.JSArray<NavItem> nav)
        {
            this.name = name;
            this.urlPrefix = urlPrefix;
            this.nav = nav;
        }
    }
}
