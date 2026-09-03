using System;

namespace Tsumo.Engine
{
    public static class Template_functions_scalarFunctions
    {
        public static Func<TemplateValue, string, int> requireSubstringInteger
        {
            get;
            private set;
        } = default(Func<TemplateValue, string, int>)!;
        public static Func<TemplateValue, string> templateValueTypeName
        {
            get;
            private set;
        } = default(Func<TemplateValue, string>)!;
        public static Func<TemplateValue, string, string> formatTemplateValue
        {
            get;
            private set;
        } = default(Func<TemplateValue, string, string>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?> callScalarFunction
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateFunctionContext, TemplateValue?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_regularExpressions.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Utils_urlComponents.__tsonic_module_init();
            Markdown.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Utils_html.__tsonic_module_init();
            Template_evaluation_scalarSemantics.__tsonic_module_init();
            Template_evaluation_serialization.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_functions_textCompatibility.__tsonic_module_init();
            requireSubstringInteger = (TemplateValue value, string name) =>
            {
                int? result = Utils_int32.parseInt32(Template_runtimeHelpers.toPlainString(value));
                if (result is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_SUBSTRING_ARGUMENT_INVALID", $"substr {name} must be a 32-bit integer");
                }
                return result.Value;
            };
            templateValueTypeName = (TemplateValue value) =>
            {
                if (value is NilValue)
                {
                    return "<nil>";
                }
                if (value is BoolValue)
                {
                    return "bool";
                }
                if (value is NumberValue)
                {
                    return "int";
                }
                if (value is StringValue)
                {
                    return "string";
                }
                if (value is HtmlValue)
                {
                    return "template.HTML";
                }
                if (value is DateValue)
                {
                    return "time.Time";
                }
                if (value is StringArrayValue)
                {
                    return "[]string";
                }
                if (value is AnyArrayValue || value is PageArrayValue)
                {
                    return "[]interface {}";
                }
                if (value is DictValue)
                {
                    return "map[string]interface {}";
                }
                if (value is PageValue)
                {
                    return "*hugolib.pageState";
                }
                if (value is ResourceValue)
                {
                    return "resource.Resource";
                }
                if (value is UrlValue)
                {
                    return "*url.URL";
                }
                if (value is UrlQueryValue)
                {
                    return "url.Values";
                }
                return "interface {}";
            };
            formatTemplateValue = (TemplateValue value, string verb) =>
            {
                if (verb == "T")
                {
                    return templateValueTypeName(value);
                }
                if (verb == "q")
                {
                    return Template_evaluation_serialization.toJson(new StringValue(Template_runtimeHelpers.toPlainString(value)));
                }
                if (verb == "#v")
                {
                    return Template_evaluation_serialization.toJson(value);
                }
                return Template_runtimeHelpers.toPlainString(value);
            };
            callScalarFunction = (string name, Tsonic.CSharp.Js.JSArray<TemplateValue> args, TemplateFunctionContext context) =>
            {
                RenderScope scope = context.scope;
                if (name == "reflect.ismap" && args.length >= 1)
                {
                    return new BoolValue(Template_runtimeHelpers.isTemplateMap(args[0]));
                }
                if (name == "reflect.isslice" && args.length >= 1)
                {
                    return new BoolValue(Template_runtimeHelpers.isTemplateSlice(args[0]));
                }
                if (name == "add" && args.length >= 2)
                {
                    int sum = 0;
                    for (int i = 0; i < args.length; i++)
                    {
                        TemplateValue v = args[i];
                        string s = Template_runtimeHelpers.toPlainString(v);
                        sum += Utils_int32.parseInt32(s) ?? 0;
                    }
                    return new NumberValue(sum);
                }
                if (name == "sub" && args.length >= 2)
                {
                    int a = Template_runtimeHelpers.toNumber(args[0]);
                    int b = Template_runtimeHelpers.toNumber(args[1]);
                    return new NumberValue(a - b);
                }
                if (name == "mul" && args.length >= 2)
                {
                    int a_1 = Template_runtimeHelpers.toNumber(args[0]);
                    int b_1 = Template_runtimeHelpers.toNumber(args[1]);
                    return new NumberValue(a_1 * b_1);
                }
                if (name == "div" && args.length >= 2)
                {
                    int a_2 = Template_runtimeHelpers.toNumber(args[0]);
                    int b_2 = Template_runtimeHelpers.toNumber(args[1]);
                    if (b_2 == 0)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DIVIDE_BY_ZERO", "Template division by zero is not valid");
                    }
                    return new NumberValue(a_2 / b_2);
                }
                if (name == "mod" && args.length >= 2)
                {
                    int a_3 = Template_runtimeHelpers.toNumber(args[0]);
                    int b_3 = Template_runtimeHelpers.toNumber(args[1]);
                    if (b_3 == 0)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_MODULO_BY_ZERO", "Template modulo by zero is not valid");
                    }
                    return new NumberValue(a_3 % b_3);
                }
                if (name == "ceil" && args.length >= 1 && args[0] is NumberValue)
                {
                    return args[0];
                }
                if ((name == "min" || name == "max") && args.length >= 1)
                {
                    int selected = Template_runtimeHelpers.toNumber(args[0]);
                    for (int index = 1; index < args.length; index++)
                    {
                        int candidate = Template_runtimeHelpers.toNumber(args[index]);
                        if (name == "min" ? candidate < selected : candidate > selected)
                        {
                            selected = candidate;
                        }
                    }
                    return new NumberValue(selected);
                }
                if (name == "round" && args.length >= 1 && args[0] is NumberValue)
                {
                    return args[0];
                }
                if (name == "int" && args.length == 1)
                {
                    TemplateValue value = args[0];
                    if (value is NumberValue)
                    {
                        return (NumberValue)value;
                    }
                    int? parsed = Utils_int32.parseInt32(Template_runtimeHelpers.toPlainString(value));
                    if (parsed is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_INTEGER_CONVERSION_INVALID", $"Template value '{Template_runtimeHelpers.toPlainString(value)}' is not a 32-bit integer");
                    }
                    return new NumberValue(parsed.Value);
                }
                if (name == "string" && args.length == 1)
                {
                    return new StringValue(Template_runtimeHelpers.toPlainString(args[0]));
                }
                if ((name == "time" || name == "time.astime") && args.length == 1)
                {
                    TemplateValue value_1 = args[0];
                    if (value_1 is DateValue)
                    {
                        return (DateValue)value_1;
                    }
                    string text = Template_runtimeHelpers.toPlainString(value_1);
                    if (Tsonic.CSharp.Js.Number.isNaN(Tsonic.CSharp.Js.Date.parse(text)))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_TIME_INVALID", $"Template value '{text}' is not a valid date or time");
                    }
                    return new DateValue(text);
                }
                if (name == "newscratch")
                {
                    return new ScratchValue(new ScratchStore());
                }
                if (name == "encoding.jsonify" || name == "jsonify")
                {
                    TemplateValue v_1 = args.length >= 1 ? args[0] : Template_runtimeHelpers.nil;
                    return new StringValue(Template_evaluation_serialization.toJson(v_1));
                }
                if (name == "crypto.sha1" && args.length >= 1)
                {
                    Tsonic.CSharp.Node.Buffer bytes = Tsonic.CSharp.Node.Buffer.from(Template_runtimeHelpers.toPlainString(args[0]), "utf8");
                    return new StringValue(Tsonic.CSharp.Node.crypto.createHash("sha1").update(bytes).digest("hex"));
                }
                if (name == "md5" && args.length >= 1)
                {
                    Tsonic.CSharp.Node.Buffer bytes_1 = Tsonic.CSharp.Node.Buffer.from(Template_runtimeHelpers.toPlainString(args[0]), "utf8");
                    return new StringValue(Tsonic.CSharp.Node.crypto.createHash("md5").update(bytes_1).digest("hex"));
                }
                if (name == "urls.parse" && args.length >= 1)
                {
                    string s_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    return new UrlValue(Template_evaluation_serialization.parseUrl(s_1));
                }
                if (name == "urls.joinpath" && args.length >= 1)
                {
                    Tsonic.CSharp.Js.JSArray<string> parts = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                    for (int i_1 = 0; i_1 < args.length; i_1++)
                    {
                        parts.push(Template_runtimeHelpers.toPlainString(args[i_1]));
                    }
                    Tsonic.CSharp.Js.JSArray<string> arr = parts;
                    string @out = "";
                    for (int i_2 = 0; i_2 < arr.length; i_2++)
                    {
                        string p = arr[i_2];
                        @out = @out == "" ? Template_evaluation_serialization.trimSlashes(p) : Template_evaluation_serialization.trimEndCharacter(@out, "/") + "/" + Template_evaluation_serialization.trimStartCharacter(p, "/");
                    }
                    return new StringValue(@out);
                }
                if (name == "strings.contains" && args.length >= 2)
                {
                    string s_2 = Template_runtimeHelpers.toPlainString(args[0]);
                    string sub = Template_runtimeHelpers.toPlainString(args[1]);
                    return new BoolValue(Tsonic.CSharp.Js.String.includes(s_2, sub));
                }
                if (name == "strings.repeat" && args.length >= 2)
                {
                    int count = Template_runtimeHelpers.toNumber(args[0]);
                    if (count < 0)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_STRING_REPEAT_INVALID", "strings.Repeat requires a non-negative repetition count");
                    }
                    return new StringValue(Tsonic.CSharp.Js.String.repeat(Template_runtimeHelpers.toPlainString(args[1]), count));
                }
                if (name == "strings.hasprefix" && args.length >= 2)
                {
                    string s_3 = Template_runtimeHelpers.toPlainString(args[0]);
                    string prefix = Template_runtimeHelpers.toPlainString(args[1]);
                    return new BoolValue(Tsonic.CSharp.Js.String.startsWith(s_3, prefix));
                }
                if (name == "strings.hassuffix" && args.length >= 2)
                {
                    string s_4 = Template_runtimeHelpers.toPlainString(args[0]);
                    string suffix = Template_runtimeHelpers.toPlainString(args[1]);
                    return new BoolValue(Tsonic.CSharp.Js.String.endsWith(s_4, suffix));
                }
                if (name == "strings.trimprefix" && args.length >= 2)
                {
                    string prefix_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    string s_5 = Template_runtimeHelpers.toPlainString(args[1]);
                    return new StringValue(Tsonic.CSharp.Js.String.startsWith(s_5, prefix_1) ? Utils_strings.substringFrom(s_5, prefix_1.Length) : s_5);
                }
                if (name == "strings.trimsuffix" && args.length >= 2)
                {
                    string suffix_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    string s_6 = Template_runtimeHelpers.toPlainString(args[1]);
                    return new StringValue(Tsonic.CSharp.Js.String.endsWith(s_6, suffix_1) ? Utils_strings.substringCount(s_6, 0, s_6.Length - suffix_1.Length) : s_6);
                }
                if (name == "strings.trim" && args.length >= 2)
                {
                    string value_2 = Template_runtimeHelpers.toPlainString(args[0]);
                    string cutset = Template_runtimeHelpers.toPlainString(args[1]);
                    return new StringValue(Utils_strings.trimCodePoints(value_2, cutset));
                }
                if (name == "strings.trimleft" && args.length >= 2)
                {
                    return new StringValue(Utils_strings.trimStartCodePoints(Template_runtimeHelpers.toPlainString(args[1]), Template_runtimeHelpers.toPlainString(args[0])));
                }
                if (name == "strings.trimright" && args.length >= 2)
                {
                    return new StringValue(Utils_strings.trimEndCodePoints(Template_runtimeHelpers.toPlainString(args[1]), Template_runtimeHelpers.toPlainString(args[0])));
                }
                if (name == "strings.trimspace" && args.length >= 1)
                {
                    return new StringValue(Utils_strings.trimUnicodeSpace(Template_runtimeHelpers.toPlainString(args[0])));
                }
                if (name == "substr" && (args.length == 2 || args.length == 3))
                {
                    string source = Template_runtimeHelpers.toPlainString(args[0]);
                    int sourceLength = Utils_strings.codePointLength(source);
                    if (sourceLength == 0)
                    {
                        return new StringValue("");
                    }
                    int start = requireSubstringInteger(args[1], "start");
                    if (start < 0)
                    {
                        start += sourceLength;
                    }
                    if (start < 0)
                    {
                        start = 0;
                    }
                    if (start >= sourceLength)
                    {
                        return new StringValue("");
                    }
                    int end = sourceLength;
                    if (args.length == 3)
                    {
                        int length = requireSubstringInteger(args[2], "length");
                        if (length == 0)
                        {
                            return new StringValue("");
                        }
                        end = length < 0 ? sourceLength + length : start + length;
                    }
                    if (start >= end || end < 0)
                    {
                        return new StringValue("");
                    }
                    if (end > sourceLength)
                    {
                        end = sourceLength;
                    }
                    return new StringValue(Utils_strings.substringCodePoints(source, start, end - start));
                }
                if (name == "urlize" && args.length >= 1)
                {
                    TemplateValue v_2 = args[0];
                    return new StringValue(Utils_text.slugify(Template_runtimeHelpers.toPlainString(v_2)));
                }
                if (name == "anchorize" && args.length >= 1)
                {
                    return new StringValue(Template_functions_textCompatibility.anchorizeText(Template_runtimeHelpers.toPlainString(args[0])));
                }
                if (name == "emojify" && args.length >= 1)
                {
                    return new HtmlValue(new HtmlString(Template_functions_textCompatibility.emojifyText(Template_runtimeHelpers.toPlainString(args[0]))));
                }
                if (name == "humanize" && args.length >= 1)
                {
                    TemplateValue v_3 = args[0];
                    return new StringValue(Utils_text.humanizeSlug(Template_runtimeHelpers.toPlainString(v_3)));
                }
                if (name == "lower" && args.length >= 1)
                {
                    TemplateValue v_4 = args[0];
                    return new StringValue(Tsonic.CSharp.Js.String.toLowerCase(Template_runtimeHelpers.toPlainString(v_4)));
                }
                if (name == "upper" && args.length >= 1)
                {
                    TemplateValue v_5 = args[0];
                    return new StringValue(Tsonic.CSharp.Js.String.toUpperCase(Template_runtimeHelpers.toPlainString(v_5)));
                }
                if (name == "trim" && args.length >= 1)
                {
                    TemplateValue v_6 = args[0];
                    return new StringValue(Tsonic.CSharp.Js.String.trim(Template_runtimeHelpers.toPlainString(v_6)));
                }
                if (name == "chomp" && args.length >= 1)
                {
                    string value_3 = Template_runtimeHelpers.toPlainString(args[0]);
                    while (Tsonic.CSharp.Js.String.endsWith(value_3, "\n") || Tsonic.CSharp.Js.String.endsWith(value_3, "\r"))
                    {
                        value_3 = Utils_strings.substringCount(value_3, 0, value_3.Length - 1);
                    }
                    return new StringValue(value_3);
                }
                if (name == "replace" && args.length >= 3)
                {
                    string s_7 = Template_runtimeHelpers.toPlainString(args[0]);
                    string oldStr = Template_runtimeHelpers.toPlainString(args[1]);
                    string newStr = Template_runtimeHelpers.toPlainString(args[2]);
                    return new StringValue(Tsonic.CSharp.Js.String.replaceAll(s_7, oldStr, newStr));
                }
                if (name == "replacere" && args.length >= 3)
                {
                    string pattern = Template_runtimeHelpers.toPlainString(args[0]);
                    string replacement = Template_runtimeHelpers.toPlainString(args[1]);
                    string s_8 = Template_runtimeHelpers.toPlainString(args[2]);
                    int limit = args.length >= 4 ? Template_runtimeHelpers.toNumber(args[3]) : -1;
                    return new StringValue(Utils_regularExpressions.replaceRegularExpression(pattern, replacement, s_8, limit));
                }
                if (name == "findre" && args.length >= 2)
                {
                    string pattern_1 = Template_runtimeHelpers.toPlainString(args[0]);
                    string input = Template_runtimeHelpers.toPlainString(args[1]);
                    int limit_1 = args.length >= 3 ? Template_runtimeHelpers.toNumber(args[2]) : -1;
                    return new StringArrayValue(Utils_regularExpressions.findRegularExpressionMatches(pattern_1, input, limit_1));
                }
                if (name == "findresubmatch" && args.length >= 2)
                {
                    string pattern_2 = Template_runtimeHelpers.toPlainString(args[0]);
                    string input_1 = Template_runtimeHelpers.toPlainString(args[1]);
                    int limit_2 = args.length >= 3 ? Template_runtimeHelpers.toNumber(args[2]) : -1;
                    Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<string>> matches = Utils_regularExpressions.findRegularExpressionSubmatches(pattern_2, input_1, limit_2);
                    Tsonic.CSharp.Js.JSArray<TemplateValue> result = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
                    for (int matchIndex = 0; matchIndex < matches.length; matchIndex++)
                    {
                        result.push(new StringArrayValue(matches[matchIndex]));
                    }
                    return new AnyArrayValue(result);
                }
                if (name == "truncate" && args.length >= 2)
                {
                    int length_1 = Template_runtimeHelpers.toNumber(args[0]);
                    string s_9 = Template_runtimeHelpers.toPlainString(args[1]);
                    string ellipsis = args.length >= 3 ? Template_runtimeHelpers.toPlainString(args[2]) : "...";
                    if (s_9.Length <= length_1)
                    {
                        return new StringValue(s_9);
                    }
                    int truncLen = length_1 - ellipsis.Length;
                    if (truncLen <= 0)
                    {
                        return new StringValue(Utils_strings.substringCount(ellipsis, 0, length_1));
                    }
                    return new StringValue(Utils_strings.substringCount(s_9, 0, truncLen) + ellipsis);
                }
                if (name == "markdownify" && args.length >= 1)
                {
                    string s_10 = Template_runtimeHelpers.toPlainString(args[0]);
                    MarkdownResult md = Markdown_renderBasic.renderMarkdown(s_10);
                    string html = Tsonic.CSharp.Js.String.trim(md.html);
                    if (Tsonic.CSharp.Js.String.startsWith(html, "<p>") && Tsonic.CSharp.Js.String.endsWith(html, "</p>"))
                    {
                        html = Utils_strings.substringCount(html, 3, html.Length - 4);
                    }
                    return new HtmlValue(new HtmlString(html));
                }
                if (name == "relurl" && args.length >= 1)
                {
                    TemplateValue v_7 = args[0];
                    string s_11 = Template_runtimeHelpers.toPlainString(v_7);
                    return new StringValue(Tsonic.CSharp.Js.String.startsWith(s_11, "/") ? s_11 : "/" + s_11);
                }
                if (name == "absurl" && args.length >= 1)
                {
                    TemplateValue v_8 = args[0];
                    string s_12 = Template_runtimeHelpers.toPlainString(v_8);
                    string rel = Tsonic.CSharp.Js.String.startsWith(s_12, "/") ? Utils_strings.substringFrom(s_12, 1) : s_12;
                    return new StringValue(Utils_text.ensureTrailingSlash(scope.site.baseURL) + rel);
                }
                if (name == "abslangurl" && args.length >= 1)
                {
                    TemplateValue v_9 = args[0];
                    string s_13 = Template_runtimeHelpers.toPlainString(v_9);
                    string lang = scope.site.Language.Lang;
                    string langPrefix = scope.site.Languages.length > 1 ? lang + "/" : "";
                    string rel_1 = Tsonic.CSharp.Js.String.startsWith(s_13, "/") ? Utils_strings.substringFrom(s_13, 1) : s_13;
                    return new StringValue(Utils_text.ensureTrailingSlash(scope.site.baseURL) + langPrefix + rel_1);
                }
                if (name == "rellangurl" && args.length >= 1)
                {
                    TemplateValue v_10 = args[0];
                    string s_14 = Template_runtimeHelpers.toPlainString(v_10);
                    string lang_1 = scope.site.Language.Lang;
                    string langPrefix_1 = scope.site.Languages.length > 1 ? "/" + lang_1 : "";
                    string path = Tsonic.CSharp.Js.String.startsWith(s_14, "/") ? s_14 : "/" + s_14;
                    return new StringValue(langPrefix_1 + path);
                }
                if (name == "urlquery" && args.length >= 1)
                {
                    TemplateValue v_11 = args[0];
                    string s_15 = Template_runtimeHelpers.toPlainString(v_11);
                    return new StringValue(Utils_urlComponents.encodeUrlComponent(s_15));
                }
                if (name == "querify" && args.length >= 2)
                {
                    return new StringValue(Utils_urlComponents.encodeUrlComponent(Template_runtimeHelpers.toPlainString(args[0])) + "=" + Utils_urlComponents.encodeUrlComponent(Template_runtimeHelpers.toPlainString(args[1])));
                }
                if (name == "default" && args.length == 1)
                {
                    return args[0];
                }
                if (name == "default" && args.length == 2)
                {
                    TemplateValue fallback = args[0];
                    TemplateValue v_12 = args[1];
                    return Template_runtimeHelpers.isDefaultSet(v_12) ? v_12 : fallback;
                }
                if (name == "len" && args.length >= 1)
                {
                    TemplateValue v_13 = args[0];
                    if (v_13 is StringValue)
                    {
                        int l = ((StringValue)v_13).value.Length;
                        return new NumberValue(l);
                    }
                    if (v_13 is HtmlValue)
                    {
                        int l_1 = ((HtmlValue)v_13).value.value.Length;
                        return new NumberValue(l_1);
                    }
                    if (v_13 is PageArrayValue)
                    {
                        int l_2 = ((PageArrayValue)v_13).value.length;
                        return new NumberValue(l_2);
                    }
                    if (v_13 is StringArrayValue)
                    {
                        int l_3 = ((StringArrayValue)v_13).value.length;
                        return new NumberValue(l_3);
                    }
                    if (v_13 is SitesArrayValue)
                    {
                        int l_4 = ((SitesArrayValue)v_13).value.length;
                        return new NumberValue(l_4);
                    }
                    if (v_13 is DocsMountArrayValue)
                    {
                        int l_5 = ((DocsMountArrayValue)v_13).value.length;
                        return new NumberValue(l_5);
                    }
                    if (v_13 is NavArrayValue)
                    {
                        int l_6 = ((NavArrayValue)v_13).value.length;
                        return new NumberValue(l_6);
                    }
                    if (v_13 is DictValue)
                    {
                        return new NumberValue(((DictValue)v_13).value.size);
                    }
                    if (v_13 is AnyArrayValue)
                    {
                        return new NumberValue(((AnyArrayValue)v_13).value.length);
                    }
                    return new NumberValue(0);
                }
                if (name == "dateformat" && args.length >= 2)
                {
                    string layout = Template_runtimeHelpers.toPlainString(args[0]);
                    string s_16 = Template_runtimeHelpers.toPlainString(args[1]);
                    return new StringValue(Template_evaluation_scalarSemantics.formatDateTime(s_16, layout) ?? "");
                }
                if (name == "print" && args.length >= 1)
                {
                    TextBuilder sb = new TextBuilder();
                    for (int i_3 = 0; i_3 < args.length; i_3++)
                    {
                        sb.append(Template_runtimeHelpers.toPlainString(args[i_3]));
                    }
                    return new StringValue(sb.toString());
                }
                if (name == "printf" && args.length >= 1)
                {
                    string fmt = Template_runtimeHelpers.toPlainString(args[0]);
                    Tsonic.CSharp.Js.JSArray<TemplateValue> values = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
                    for (int argumentIndex = 1; argumentIndex < args.length; argumentIndex++)
                    {
                        values.push(args[argumentIndex]);
                    }
                    TextBuilder sb_1 = new TextBuilder();
                    int pos = 0;
                    int valueIndex = 0;
                    while (pos < fmt.Length)
                    {
                        string ch = Utils_strings.substringCount(fmt, pos, 1);
                        if (ch == "%" && pos + 1 < fmt.Length)
                        {
                            string next = Utils_strings.substringCount(fmt, pos + 1, 1);
                            if (next == "%")
                            {
                                sb_1.append("%");
                                pos += 2;
                                continue;
                            }
                            string verb = next;
                            int width = 2;
                            if (next == "#" && pos + 2 < fmt.Length && Utils_strings.substringCount(fmt, pos + 2, 1) == "v")
                            {
                                verb = "#v";
                                width = 3;
                            }
                            if (verb == "s" || verb == "d" || verb == "t" || verb == "v" || verb == "q" || verb == "T" || verb == "#v")
                            {
                                if (valueIndex < values.length)
                                {
                                    sb_1.append(formatTemplateValue(values[valueIndex], verb));
                                }
                                valueIndex++;
                                pos += width;
                                continue;
                            }
                        }
                        sb_1.append(ch);
                        pos++;
                    }
                    return new StringValue(sb_1.toString());
                }
                if (args.length >= 2)
                {
                    bool isCompare = name == "eq" || name == "ne" || name == "lt" || name == "le" || name == "gt" || name == "ge";
                    if (isCompare)
                    {
                        TemplateValue a_4 = args[0];
                        TemplateValue b_4 = args[1];
                        double cmp = 0;
                        if (a_4 is VersionStringValue || b_4 is VersionStringValue)
                        {
                            string av = Template_runtimeHelpers.toPlainString(a_4);
                            string bv = Template_runtimeHelpers.toPlainString(b_4);
                            cmp = VersionStringValue.compare(av, bv);
                        }
                        else
                        {
                            if (a_4 is NumberValue)
                            {
                                if (b_4 is NumberValue)
                                {
                                    int av_1 = ((NumberValue)a_4).value;
                                    int bv_1 = ((NumberValue)b_4).value;
                                    cmp = av_1 < bv_1 ? -1 : av_1 > bv_1 ? 1 : 0;
                                }
                                else
                                {
                                    string av_2 = Template_runtimeHelpers.toPlainString((NumberValue)a_4);
                                    string bv_2 = Template_runtimeHelpers.toPlainString(b_4);
                                    cmp = Utils_strings.compareText(av_2, bv_2);
                                }
                            }
                            else
                            {
                                string av_3 = Template_runtimeHelpers.toPlainString(a_4);
                                string bv_3 = Template_runtimeHelpers.toPlainString(b_4);
                                cmp = Utils_strings.compareText(av_3, bv_3);
                            }
                        }
                        if (name == "eq")
                        {
                            return new BoolValue(cmp == 0);
                        }
                        if (name == "ne")
                        {
                            return new BoolValue(cmp != 0);
                        }
                        if (name == "lt")
                        {
                            return new BoolValue(cmp < 0);
                        }
                        if (name == "le")
                        {
                            return new BoolValue(cmp <= 0);
                        }
                        if (name == "gt")
                        {
                            return new BoolValue(cmp > 0);
                        }
                        return new BoolValue(cmp >= 0);
                    }
                }
                if (name == "not" && args.length >= 1)
                {
                    return new BoolValue(!Template_runtimeHelpers.isTruthy(args[0]));
                }
                if (name == "and" && args.length >= 1)
                {
                    TemplateValue cur = args[0];
                    for (int i_4 = 0; i_4 < args.length; i_4++)
                    {
                        cur = args[i_4];
                        if (!Template_runtimeHelpers.isTruthy(cur))
                        {
                            return cur;
                        }
                    }
                    return cur;
                }
                if (name == "or" && args.length >= 1)
                {
                    for (int i_5 = 0; i_5 < args.length; i_5++)
                    {
                        TemplateValue cur_1 = args[i_5];
                        if (Template_runtimeHelpers.isTruthy(cur_1))
                        {
                            return cur_1;
                        }
                    }
                    return args[args.length - 1];
                }
                return null;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
