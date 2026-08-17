using System;

namespace Tsumo.Engine
{
    public static class Docs_url
    {
        public static Func<string, UrlSuffixSplit> splitUrlSuffix
        {
            get;
            private set;
        } = default(Func<string, UrlSuffixSplit>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            splitUrlSuffix = (string url) =>
            {
                int q = Tsonic.CSharp.Js.String.indexOf(url, "?");
                int h = Tsonic.CSharp.Js.String.indexOf(url, "#");
                int cut = -1;
                if (q >= 0 && h >= 0)
                {
                    cut = q < h ? q : h;
                }
                else
                {
                    if (q >= 0)
                    {
                        cut = q;
                    }
                    else
                    {
                        if (h >= 0)
                        {
                            cut = h;
                        }
                    }
                }
                if (cut < 0)
                {
                    return new UrlSuffixSplit(url, "");
                }
                return new UrlSuffixSplit(Utils_strings.substringCount(url, 0, cut), Utils_strings.substringFrom(url, cut));
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class UrlSuffixSplit
    {
        public string path;
        public string suffix;
        public UrlSuffixSplit(string path, string suffix)
        {
            this.path = path;
            this.suffix = suffix;
        }
    }
}
