using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_urlQuerySemantics
    {
        public static Func<string, bool> isHexDigit
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, string> decodeQueryComponent
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, UrlQueryValue> parseUrlQuery
        {
            get;
            private set;
        } = default(Func<string, UrlQueryValue>)!;
        public static Func<Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>>, string, string?> getUrlQueryValue
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>>, string, string?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_urlComponents.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            isHexDigit = (string value) =>
            {
                double code = Tsonic.CSharp.Js.String.charCodeAt(value, 0);
                return (code >= 48 && code <= 57) || (code >= 65 && code <= 70) || (code >= 97 && code <= 102);
            };
            decodeQueryComponent = (string value) =>
            {
                for (int index = 0; index < value.Length; index++)
                {
                    if (Utils_strings.substringCount(value, index, 1) != "%")
                    {
                        continue;
                    }
                    if (index + 2 >= value.Length || !isHexDigit(Utils_strings.substringCount(value, index + 1, 1)) || !isHexDigit(Utils_strings.substringCount(value, index + 2, 1)))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_URL_QUERY_INVALID", "URL query contains an invalid percent escape");
                    }
                    index += 2;
                }
                try
                {
                    return Utils_urlComponents.decodeUrlComponent(Utils_strings.replaceText(value, "+", " "));
                }
                catch
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_URL_QUERY_INVALID", "URL query contains invalid UTF-8 data");
                }
            };
            parseUrlQuery = (string rawQuery) =>
            {
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>> values = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>>();
                if (rawQuery == "")
                {
                    return new UrlQueryValue(values);
                }
                Tsonic.CSharp.Js.JSArray<string> fields = Tsonic.CSharp.Js.String.split(rawQuery, "&");
                for (int index = 0; index < fields.length; index++)
                {
                    string field = fields[index];
                    if (field == "")
                    {
                        continue;
                    }
                    int separator = Tsonic.CSharp.Js.String.indexOf(field, "=");
                    string rawName = separator < 0 ? field : Utils_strings.substringCount(field, 0, separator);
                    string rawValue = separator < 0 ? "" : Utils_strings.substringFrom(field, separator + 1);
                    string name = decodeQueryComponent(rawName);
                    string value = decodeQueryComponent(rawValue);
                    Tsonic.CSharp.Js.JSArray<string>? existing = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<string>>(values, name);
                    if (existing is null)
                    {
                        values.set(name, new Tsonic.CSharp.Js.JSArray<string>(new string[] { value }));
                    }
                    else
                    {
                        existing.push(value);
                    }
                }
                return new UrlQueryValue(values);
            };
            getUrlQueryValue = (Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<string>> query, string name) =>
            {
                Tsonic.CSharp.Js.JSArray<string>? values = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<string>>(query, name);
                return values is null || values.length == 0 ? null : values[0];
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
