using System;

namespace Tsumo.Engine
{
    public static class Frontmatter_parsedContent
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Frontmatter_data.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ParsedContent
    {
        public FrontMatter frontMatter;
        public string body;
        public ParsedContent(FrontMatter frontMatter, string body)
        {
            this.frontMatter = frontMatter;
            this.body = body;
        }
    }
}
