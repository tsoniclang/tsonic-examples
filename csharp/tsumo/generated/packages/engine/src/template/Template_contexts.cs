using System;

namespace Tsumo.Engine
{
    public static class Template_contexts
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Params.__tsonic_module_init();
            Shortcode.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ShortcodeContext
    {
        public string name;
        public PageContext Page;
        public SiteContext Site;
        public Tsonic.CSharp.Js.Map<string, ParamValue> Params;
        public Tsonic.CSharp.Js.JSArray<string> positionalParams;
        public bool IsNamedParams;
        public string Inner;
        public string InnerDeindent;
        public int Ordinal;
        public ShortcodeContext? Parent;
        public ShortcodeContext(string name, PageContext page, SiteContext site, Tsonic.CSharp.Js.Map<string, ParamValue> @params, Tsonic.CSharp.Js.JSArray<string> positionalParams, bool isNamedParams, string inner, int ordinal, ShortcodeContext? parent)
        {
            this.name = name;
            this.Page = page;
            this.Site = site;
            this.Params = @params;
            this.positionalParams = positionalParams;
            this.IsNamedParams = isNamedParams;
            this.Inner = inner;
            this.InnerDeindent = Shortcode.innerDeindent(inner);
            this.Ordinal = ordinal;
            this.Parent = parent;
        }
        public ParamValue? Get(string keyOrIndex)
        {
            if (this.IsNamedParams)
            {
                return Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(this.Params, keyOrIndex);
            }
            int? idx = Utils_int32.parseInt32(keyOrIndex);
            if (idx is not null && idx.Value >= 0 && idx.Value < this.positionalParams.length)
            {
                return ParamValue.@string(this.positionalParams[idx.Value]);
            }
            return null;
        }
    }
    public class ShortcodeValue : TemplateValue
    {
        public ShortcodeContext value;
        public ShortcodeValue(ShortcodeContext value) : base()
        {
            this.value = value;
        }
    }
    public class LinkHookContext
    {
        public string Destination;
        public string Text;
        public string Title;
        public string PlainText;
        public PageContext Page;
        public PageContext PageInner;
        public PageContext PageOuter;
        public LinkHookContext(string destination, string text, string title, string plainText, PageContext pageInner, PageContext pageOuter)
        {
            this.Destination = destination;
            this.Text = text;
            this.Title = title;
            this.PlainText = plainText;
            this.Page = pageInner;
            this.PageInner = pageInner;
            this.PageOuter = pageOuter;
        }
    }
    public class LinkHookValue : TemplateValue
    {
        public LinkHookContext value;
        public LinkHookValue(LinkHookContext value) : base()
        {
            this.value = value;
        }
    }
    public class ImageHookContext
    {
        public string Destination;
        public string Text;
        public string Title;
        public string PlainText;
        public PageContext Page;
        public PageContext PageInner;
        public PageContext PageOuter;
        public ImageHookContext(string destination, string text, string title, string plainText, PageContext pageInner, PageContext pageOuter)
        {
            this.Destination = destination;
            this.Text = text;
            this.Title = title;
            this.PlainText = plainText;
            this.Page = pageInner;
            this.PageInner = pageInner;
            this.PageOuter = pageOuter;
        }
    }
    public class ImageHookValue : TemplateValue
    {
        public ImageHookContext value;
        public ImageHookValue(ImageHookContext value) : base()
        {
            this.value = value;
        }
    }
    public class HeadingHookContext
    {
        public int Level;
        public string Text;
        public string PlainText;
        public string Anchor;
        public PageContext Page;
        public PageContext PageInner;
        public PageContext PageOuter;
        public HeadingHookContext(int level, string text, string plainText, string anchor, PageContext pageInner, PageContext pageOuter)
        {
            this.Level = level;
            this.Text = text;
            this.PlainText = plainText;
            this.Anchor = anchor;
            this.Page = pageInner;
            this.PageInner = pageInner;
            this.PageOuter = pageOuter;
        }
    }
    public class HeadingHookValue : TemplateValue
    {
        public HeadingHookContext value;
        public HeadingHookValue(HeadingHookContext value) : base()
        {
            this.value = value;
        }
    }
}
