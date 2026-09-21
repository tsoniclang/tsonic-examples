using System;

namespace Tsumo.Engine
{
    public static class Build_standardTemplates
    {
        public static Func<BuildEnvironment, StandardTemplates> selectStandardTemplates
        {
            get;
            private set;
        } = default(Func<BuildEnvironment, StandardTemplates>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Env.__tsonic_module_init();
            Build_layout.__tsonic_module_init();
            selectStandardTemplates = (BuildEnvironment environment) =>
            {
                Tsonic.CSharp.Js.JSArray<string> baseCandidates = Tsonic.CSharp.Js.JSArray<string>.of(["_default/baseof.html", "baseof.html"]);
                Tsonic.CSharp.Js.JSArray<string> homeCandidates = Tsonic.CSharp.Js.JSArray<string>.of(["index.html", "home.html", "_default/home.html", "_default/list.html", "list.html"]);
                Tsonic.CSharp.Js.JSArray<string> listCandidates = Tsonic.CSharp.Js.JSArray<string>.of(["list.html", "_default/list.html"]);
                Tsonic.CSharp.Js.JSArray<string> singleCandidates = Tsonic.CSharp.Js.JSArray<string>.of(["single.html", "_default/single.html"]);
                string list = Build_layout.selectTemplate(environment, listCandidates) ?? listCandidates[0];
                return new StandardTemplates(Build_layout.selectTemplate(environment, baseCandidates), Build_layout.selectTemplate(environment, homeCandidates) ?? list, list, Build_layout.selectTemplate(environment, singleCandidates) ?? singleCandidates[0]);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class StandardTemplates
    {
        public string? @base;
        public string home;
        public string list;
        public string single;
        public StandardTemplates(string? @base, string home, string list, string single)
        {
            this.@base = @base;
            this.home = home;
            this.list = list;
            this.single = single;
        }
    }
}
