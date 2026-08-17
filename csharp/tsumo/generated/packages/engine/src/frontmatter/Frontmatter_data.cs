using System;

namespace Tsumo.Engine
{
    public static class Frontmatter_data
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Frontmatter_menu.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class FrontMatter
    {
        public string? title;
        public Tsonic.CSharp.Js.Date? date;
        public bool draft;
        public Tsonic.CSharp.Js.JSArray<string> tags;
        public Tsonic.CSharp.Js.JSArray<string> categories;
        public string? description;
        public string? slug;
        public string? layout;
        public string? type;
        public Tsonic.CSharp.Js.Map<string, ParamValue> Params;
        public Tsonic.CSharp.Js.JSArray<FrontMatterMenu> menus;
        public FrontMatter()
        {
            this.title = null;
            this.date = null;
            this.draft = false;
            Tsonic.CSharp.Js.JSArray<string> emptyStrings = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
            this.tags = emptyStrings;
            this.categories = emptyStrings;
            this.description = null;
            this.slug = null;
            this.layout = null;
            this.type = null;
            this.Params = new Tsonic.CSharp.Js.Map<string, ParamValue>();
            Tsonic.CSharp.Js.JSArray<FrontMatterMenu> emptyMenus = new Tsonic.CSharp.Js.JSArray<FrontMatterMenu>(new FrontMatterMenu[] { });
            this.menus = emptyMenus;
        }
    }
}
