using System;

namespace Tsumo.Engine
{
    public static class Docs_searchIndex
    {
        public static Func<string, string> escapeJsonString
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<SearchDocument, SearchDocument, double> compareSearchDocuments
        {
            get;
            private set;
        } = default(Func<SearchDocument, SearchDocument, double>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<SearchDocument>, string> renderSearchIndexJson
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<SearchDocument>, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_textBuilder.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            escapeJsonString = (string input) =>
            {
                string value = input;
                value = Utils_strings.replaceText(value, "\\", "\\\\");
                value = Utils_strings.replaceText(value, "\"", "\\\"");
                value = Utils_strings.replaceText(value, "\r", "\\r");
                value = Utils_strings.replaceText(value, "\n", "\\n");
                value = Utils_strings.replaceText(value, "\t", "\\t");
                return value;
            };
            compareSearchDocuments = (SearchDocument left, SearchDocument right) =>
            {
                int url = Utils_strings.compareText(left.url, right.url);
                if (url != 0)
                {
                    return url;
                }
                int mount = Utils_strings.compareText(left.mount, right.mount);
                return mount != 0 ? mount : Utils_strings.compareText(left.title, right.title);
            };
            renderSearchIndexJson = (Tsonic.CSharp.Js.JSArray<SearchDocument> documents) =>
            {
                Tsonic.CSharp.Js.JSArray<SearchDocument> ordered = new Tsonic.CSharp.Js.JSArray<SearchDocument>(new SearchDocument[] { });
                for (int index = 0; index < documents.length; index++)
                {
                    ordered.push(documents[index]);
                }
                ordered.sort((SearchDocument left, SearchDocument right) => compareSearchDocuments(left, right));
                TextBuilder output = new TextBuilder();
                output.append("[");
                for (int index_1 = 0; index_1 < ordered.length; index_1++)
                {
                    SearchDocument document = ordered[index_1];
                    if (index_1 > 0)
                    {
                        output.append(",");
                    }
                    output.append("{\"title\":\"");
                    output.append(escapeJsonString(document.title));
                    output.append("\",\"url\":\"");
                    output.append(escapeJsonString(document.url));
                    output.append("\",\"mount\":\"");
                    output.append(escapeJsonString(document.mount));
                    output.append("\",\"text\":\"");
                    output.append(escapeJsonString(document.text));
                    output.append("\"}");
                }
                output.append("]");
                return output.toString();
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class SearchDocument
    {
        public string title;
        public string url;
        public string mount;
        public string text;
        public SearchDocument(string title, string url, string mount, string text)
        {
            this.title = title;
            this.url = url;
            this.mount = mount;
            this.text = text;
        }
    }
}
