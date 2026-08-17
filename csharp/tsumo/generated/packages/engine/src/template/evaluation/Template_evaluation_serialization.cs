using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_serialization
    {
        public static Func<string, string> getPathExtension
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<TemplateValue, string> toJson
        {
            get;
            private set;
        } = default(Func<TemplateValue, string>)!;
        public static Func<string, string> toJsonString
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, System.Uri> parseUrl
        {
            get;
            private set;
        } = default(Func<string, System.Uri>)!;
        public static Func<string, string, string> trimStartCharacter
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        public static Func<string, string, string> trimEndCharacter
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        public static Func<string, string> trimSlashes
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> trimRightWhitespace
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            getPathExtension = (string path) =>
            {
                int lastDot = Tsonic.CSharp.Js.String.lastIndexOf(path, ".");
                double lastSlash = Tsonic.CSharp.Js.Math.max(Tsonic.CSharp.Js.String.lastIndexOf(path, "/"), Tsonic.CSharp.Js.String.lastIndexOf(path, "\\"));
                if (lastDot < 0 || lastDot <= lastSlash)
                {
                    return "";
                }
                return Utils_strings.substringFrom(path, lastDot);
            };
            toJson = (TemplateValue value) =>
            {
                if (value is NilValue)
                {
                    return "null";
                }
                if (value is BoolValue)
                {
                    return ((BoolValue)value).value ? "true" : "false";
                }
                if (value is NumberValue)
                {
                    return $"{((NumberValue)value).value}";
                }
                if (value is StringValue)
                {
                    return toJsonString(((StringValue)value).value);
                }
                if (value is DateValue)
                {
                    return toJsonString(((DateValue)value).value);
                }
                if (value is HtmlValue)
                {
                    return toJsonString(((HtmlValue)value).value.value);
                }
                if (value is AnyArrayValue)
                {
                    Tsonic.CSharp.Js.JSArray<TemplateValue> items = ((AnyArrayValue)value).value;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("[");
                    bool first = true;
                    for (int i = 0; i < items.length; i++)
                    {
                        if (!first)
                        {
                            sb.Append(",");
                        }
                        first = false;
                        sb.Append(toJson(items[i]));
                    }
                    sb.Append("]");
                    return sb.ToString();
                }
                if (value is DictValue)
                {
                    System.Text.StringBuilder sb_1 = new System.Text.StringBuilder();
                    sb_1.Append("{");
                    bool first_1 = true;
                    foreach (string k in ((DictValue)value).value.keys())
                    {
                        TemplateValue? v = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(((DictValue)value).value, k);
                        if (v is null)
                        {
                            continue;
                        }
                        if (!first_1)
                        {
                            sb_1.Append(",");
                        }
                        first_1 = false;
                        sb_1.Append(toJsonString(k));
                        sb_1.Append(":");
                        sb_1.Append(toJson(v));
                    }
                    sb_1.Append("}");
                    return sb_1.ToString();
                }
                return "null";
            };
            toJsonString = (string value) =>
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("\"");
                for (int i = 0; i < value.Length; i++)
                {
                    string ch = Utils_strings.substringCount(value, i, 1);
                    if (ch == "\\")
                    {
                        sb.Append("\\\\");
                    }
                    else
                    {
                        if (ch == "\"")
                        {
                            sb.Append("\\\"");
                        }
                        else
                        {
                            if (ch == "\n")
                            {
                                sb.Append("\\n");
                            }
                            else
                            {
                                if (ch == "\r")
                                {
                                    sb.Append("\\r");
                                }
                                else
                                {
                                    if (ch == "\t")
                                    {
                                        sb.Append("\\t");
                                    }
                                    else
                                    {
                                        sb.Append(ch);
                                    }
                                }
                            }
                        }
                    }
                }
                sb.Append("\"");
                return sb.ToString();
            };
            parseUrl = (string value) =>
            {
                string trimmed = Tsonic.CSharp.Js.String.trim(value);
                try
                {
                    return new System.Uri(trimmed, System.UriKind.RelativeOrAbsolute);
                }
                catch
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_URL_INVALID", $"Invalid URL: {value}");
                }
            };
            trimStartCharacter = (string value, string ch) =>
            {
                int start = 0;
                while (start < value.Length && Utils_strings.substringCount(value, start, 1) == ch)
                {
                    start++;
                }
                return Utils_strings.substringFrom(value, start);
            };
            trimEndCharacter = (string value, string ch) =>
            {
                int end = value.Length;
                while (end > 0 && Utils_strings.substringCount(value, end - 1, 1) == ch)
                {
                    end--;
                }
                return Utils_strings.substringCount(value, 0, end);
            };
            trimSlashes = (string value) =>
            {
                string withoutLeading = trimStartCharacter(value, "/");
                return trimEndCharacter(withoutLeading, "/");
            };
            trimRightWhitespace = (string s) =>
            {
                return Tsonic.CSharp.Js.String.trimEnd(s);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
