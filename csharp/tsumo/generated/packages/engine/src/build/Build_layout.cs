using System;

namespace Tsumo.Engine
{
    public static class Build_layout
    {
        public static Func<string, SiteConfig, string?, string?> resolveThemeDir
        {
            get;
            private set;
        } = default(Func<string, SiteConfig, string?, string?>)!;
        public static Func<LayoutEnvironment, Tsonic.CSharp.Js.JSArray<string>, string?> selectTemplate
        {
            get;
            private set;
        } = default(Func<LayoutEnvironment, Tsonic.CSharp.Js.JSArray<string>, string?>)!;
        public static Func<LayoutEnvironment, string?, string, PageContext, string> renderWithBase
        {
            get;
            private set;
        } = default(Func<LayoutEnvironment, string?, string, PageContext, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Layouts.__tsonic_module_init();
            Models.__tsonic_module_init();
            resolveThemeDir = (string siteDir, SiteConfig config, string? themesDirRaw) =>
            {
                string? configTheme = config.theme;
                if (configTheme is null)
                {
                    return null;
                }
                string themeName = Tsonic.CSharp.Js.String.trim(configTheme);
                if (themeName == "")
                {
                    return null;
                }
                string? themesDir = themesDirRaw;
                string customThemesDir = themesDir is not null ? Tsonic.CSharp.Js.String.trim(themesDir) : "";
                if (customThemesDir != "")
                {
                    string themesBase = Tsonic.CSharp.Node.path.isAbsolute(customThemesDir) ? customThemesDir : Tsonic.CSharp.Node.path.join(siteDir, customThemesDir);
                    string candidate = Tsonic.CSharp.Node.path.join(themesBase, themeName);
                    if (Fs.dirExists(candidate))
                    {
                        return candidate;
                    }
                }
                string themeDir = Tsonic.CSharp.Node.path.join(siteDir, "themes", themeName);
                return Fs.dirExists(themeDir) ? themeDir : null;
            };
            selectTemplate = (LayoutEnvironment env, Tsonic.CSharp.Js.JSArray<string> candidates) =>
            {
                for (int i = 0; i < candidates.length; i++)
                {
                    string candidate = candidates[i];
                    if (env.getTemplate(candidate) is not null)
                    {
                        return candidate;
                    }
                }
                return null;
            };
            renderWithBase = (LayoutEnvironment env, string? basePathRaw, string mainPath, PageContext ctx) =>
            {
                Template? main = env.getTemplate(mainPath);
                if (main is null)
                {
                    return "";
                }
                string? basePath = basePathRaw;
                if (basePath is not null)
                {
                    Template? @base = env.getTemplate(basePath);
                    if (@base is not null)
                    {
                        return @base.render(ctx, env, main.defines);
                    }
                }
                return main.render(ctx, env);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
