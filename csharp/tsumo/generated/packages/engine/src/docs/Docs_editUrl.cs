namespace Tsumo.Engine
{
    public static class Docs_editUrl
    {
        public static string? createDocsEditUrl(DocsMountConfig mount, string relativePath)
        {
            string? repoUrl = mount.repoUrl;
            if (repoUrl is null)
            {
                return null;
            }
            string repository = Utils_strings.trimEndChar(Tsonic.CSharp.Js.String.trim(repoUrl), "/");
            if (repository == "")
            {
                return null;
            }
            string branch = Tsonic.CSharp.Js.String.trim(mount.repoBranch) == "" ? "main" : Tsonic.CSharp.Js.String.trim(mount.repoBranch);
            string sourcePath = Utils_strings.trimStartChar(relativePath, "/");
            string? configuredRepoPath = mount.repoPath;
            if (configuredRepoPath is null || Tsonic.CSharp.Js.String.trim(configuredRepoPath) == "")
            {
                return $"{repository}/blob/{branch}/{sourcePath}";
            }
            string repoPath = Utils_strings.trimEndChar(Utils_strings.trimStartChar(Tsonic.CSharp.Js.String.trim(configuredRepoPath), "/"), "/");
            return $"{repository}/blob/{branch}/{repoPath}/{sourcePath}";
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
