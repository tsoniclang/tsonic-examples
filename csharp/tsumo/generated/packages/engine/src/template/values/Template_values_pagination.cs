using System;

namespace Tsumo.Engine
{
    public static class Template_values_pagination
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_urlPath.__tsonic_module_init();
            Template_values_base.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class PaginatorValue : TemplateValue
    {
        public Tsonic.CSharp.Js.JSArray<PageContext> sourcePages;
        public int pageSize;
        public int pageNumber;
        public string basePath;
        public PaginatorValue(Tsonic.CSharp.Js.JSArray<PageContext> sourcePages, int pageSize, int pageNumber, string basePath) : base()
        {
            this.sourcePages = sourcePages;
            this.pageSize = pageSize > 0 ? pageSize : 1;
            this.pageNumber = pageNumber > 0 ? pageNumber : 1;
            this.basePath = basePath;
        }
        public int totalPages()
        {
            if (this.sourcePages.length == 0)
            {
                return 1;
            }
            return (int)Tsonic.CSharp.Js.Math.ceil(this.sourcePages.length / this.pageSize);
        }
        public Tsonic.CSharp.Js.JSArray<PageContext> pages()
        {
            int start = (this.pageNumber - 1) * this.pageSize;
            int end = (int)Tsonic.CSharp.Js.Math.min(start + this.pageSize, this.sourcePages.length);
            Tsonic.CSharp.Js.JSArray<PageContext> pages = new Tsonic.CSharp.Js.JSArray<PageContext>(new PageContext[] { });
            for (int index = start; index < end; index++)
            {
                pages.push(this.sourcePages[index]);
            }
            return pages;
        }
        public string url()
        {
            return this.pageNumber <= 1 ? Utils_urlPath.combineUrlPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { this.basePath })) : Utils_urlPath.combineUrlPath(new Tsonic.CSharp.Js.JSArray<string>(new string[] { this.basePath, "page", $"{this.pageNumber}" }));
        }
        public PaginatorValue withPageNumber(int pageNumber)
        {
            return new PaginatorValue(this.sourcePages, this.pageSize, pageNumber, this.basePath);
        }
        public bool hasSameSource(PaginatorValue other)
        {
            if (this.pageSize != other.pageSize || this.basePath != other.basePath || this.sourcePages.length != other.sourcePages.length)
            {
                return false;
            }
            for (int index = 0; index < this.sourcePages.length; index++)
            {
                if (this.sourcePages[index] != other.sourcePages[index])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
