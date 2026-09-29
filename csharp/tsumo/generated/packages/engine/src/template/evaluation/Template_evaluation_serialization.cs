namespace Tsumo.Engine
{
    public static class Template_evaluation_serialization
    {
        public static string getPathExtension(string path)
        {
            int lastDot = Tsonic.CSharp.Js.String.lastIndexOf(path, ".");
            double lastSlash = Tsonic.CSharp.Js.Math.max(Tsonic.CSharp.Js.String.lastIndexOf(path, "/"), Tsonic.CSharp.Js.String.lastIndexOf(path, "\\"));
            if (lastDot < 0 || lastDot <= lastSlash)
            {
                return "";
            }
            return Utils_strings.substringFrom(path, lastDot);
        }
        public static string toJson(TemplateValue value)
        {
            if ((object?)value is NilValue)
            {
                return "null";
            }
            if ((object?)value is BoolValue)
            {
                return ((BoolValue)value).value ? "true" : "false";
            }
            if ((object?)value is NumberValue)
            {
                return $"{((NumberValue)value).value}";
            }
            if ((object?)value is StringValue)
            {
                return toJsonString(((StringValue)value).value);
            }
            if ((object?)value is DateValue)
            {
                return toJsonString(((DateValue)value).value);
            }
            if ((object?)value is HtmlValue)
            {
                return toJsonString(((HtmlValue)value).value.value);
            }
            if ((object?)value is AnyArrayValue)
            {
                Tsonic.CSharp.Js.JSArray<TemplateValue> items = ((AnyArrayValue)value).value;
                TextBuilder sb = new TextBuilder();
                sb.append("[");
                bool first = true;
                for (int i = 0; i < items.length; i++)
                {
                    if (!first)
                    {
                        sb.append(",");
                    }
                    first = false;
                    sb.append(toJson(items[i]));
                }
                sb.append("]");
                return sb.toString();
            }
            if ((object?)value is DictValue)
            {
                TextBuilder sb_1 = new TextBuilder();
                sb_1.append("{");
                bool first_1 = true;
                foreach (string k in ((DictValue)value).value.keys())
                {
                    TemplateValue? v = Tsonic.CSharp.Js.Map.getOptional<string, TemplateValue>(((DictValue)value).value, k);
                    if (v is null)
                    {
                        continue;
                    }
                    if (!first_1)
                    {
                        sb_1.append(",");
                    }
                    first_1 = false;
                    sb_1.append(toJsonString(k));
                    sb_1.append(":");
                    sb_1.append(toJson(v));
                }
                sb_1.append("}");
                return sb_1.toString();
            }
            return "null";
        }
        public static string toJsonString(string value)
        {
            TextBuilder sb = new TextBuilder();
            sb.append("\"");
            for (int i = 0; i < value.Length; i++)
            {
                string ch = Utils_strings.substringCount(value, i, 1);
                if (ch == "\\")
                {
                    sb.append("\\\\");
                }
                else
                {
                    if (ch == "\"")
                    {
                        sb.append("\\\"");
                    }
                    else
                    {
                        if (ch == "\n")
                        {
                            sb.append("\\n");
                        }
                        else
                        {
                            if (ch == "\r")
                            {
                                sb.append("\\r");
                            }
                            else
                            {
                                if (ch == "\t")
                                {
                                    sb.append("\\t");
                                }
                                else
                                {
                                    sb.append(ch);
                                }
                            }
                        }
                    }
                }
            }
            sb.append("\"");
            return sb.toString();
        }
        public static ParsedUrl parseUrl(string value)
        {
            string trimmed = Tsonic.CSharp.Js.String.trim(value);
            if (Tsonic.CSharp.Js.String.includes(trimmed, "\0"))
            {
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_URL_INVALID", $"Invalid URL: {value}");
            }
            return new ParsedUrl(trimmed, Tsonic.CSharp.Node.url.parse(trimmed));
        }
        public static string trimStartCharacter(string value, string ch)
        {
            int start = 0;
            while (start < value.Length && Utils_strings.substringCount(value, start, 1) == ch)
            {
                start++;
            }
            return Utils_strings.substringFrom(value, start);
        }
        public static string trimEndCharacter(string value, string ch)
        {
            int end = value.Length;
            while (end > 0 && Utils_strings.substringCount(value, end - 1, 1) == ch)
            {
                end--;
            }
            return Utils_strings.substringCount(value, 0, end);
        }
        public static string trimSlashes(string value)
        {
            string withoutLeading = trimStartCharacter(value, "/");
            return trimEndCharacter(withoutLeading, "/");
        }
        public static string trimRightWhitespace(string s)
        {
            return Tsonic.CSharp.Js.String.trimEnd(s);
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_values_url.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
