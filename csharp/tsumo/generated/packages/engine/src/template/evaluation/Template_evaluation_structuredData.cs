using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_structuredData
    {
        public static Func<string, string?, int, TsumoError> yamlError
        {
            get;
            private set;
        } = default(Func<string, string?, int, TsumoError>)!;
        public static Func<string, string?, int, int> yamlSourceIndentation
        {
            get;
            private set;
        } = default(Func<string, string?, int, int>)!;
        public static Func<string, int> yamlMappingSeparator
        {
            get;
            private set;
        } = default(Func<string, int>)!;
        public static Func<string, int?> yamlQuotedScalarStart
        {
            get;
            private set;
        } = default(Func<string, int?>)!;
        public static Func<string, int, string, YamlQuoteScan> scanYamlQuotedScalar
        {
            get;
            private set;
        } = default(Func<string, int, string, YamlQuoteScan>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<string>, int, int, string?, YamlLogicalLine> readYamlLogicalLine
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<string>, int, int, string?, YamlLogicalLine>)!;
        public static Func<JsonValue, TemplateValue> jsonToTemplateValue
        {
            get;
            private set;
        } = default(Func<JsonValue, TemplateValue>)!;
        public static Func<TemplateValue, StructuredInput> inputFromValue
        {
            get;
            private set;
        } = default(Func<TemplateValue, StructuredInput>)!;
        public static Func<DictValue, string, string?> optionValue
        {
            get;
            private set;
        } = default(Func<DictValue, string, string?>)!;
        public static Func<string?, StructuredInput, string> normalizeFormat
        {
            get;
            private set;
        } = default(Func<string?, StructuredInput, string>)!;
        public static Func<string, string, string?, TemplateValue> parseTemplateDataText
        {
            get;
            private set;
        } = default(Func<string, string, string?, TemplateValue>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateValue> unmarshalTemplateData
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<TemplateValue>, TemplateValue>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Utils_json.__tsonic_module_init();
            Utils_structuredScalars.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_values.__tsonic_module_init();
            Template_runtimeHelpers.__tsonic_module_init();
            Template_evaluation_tomlData.__tsonic_module_init();
            yamlError = (string message, string? sourcePath, int line) => Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_YAML_INVALID", message, sourcePath, line, 1);
            yamlSourceIndentation = (string raw, string? sourcePath, int line) =>
            {
                int indentation = 0;
                while (indentation < raw.Length && raw.Substring(indentation, 1) == " ")
                {
                    indentation++;
                }
                if (indentation < raw.Length && raw.Substring(indentation, 1) == "\t")
                {
                    throw yamlError("YAML indentation cannot contain tabs", sourcePath, line);
                }
                return indentation;
            };
            yamlMappingSeparator = (string value) =>
            {
                string quote = "";
                bool escaped = false;
                for (int index = 0; index < value.Length; index++)
                {
                    string character = value.Substring(index, 1);
                    if (escaped)
                    {
                        escaped = false;
                        continue;
                    }
                    if (quote == "\"" && character == "\\")
                    {
                        escaped = true;
                        continue;
                    }
                    if (character == "\"" || character == "'")
                    {
                        if (quote == "")
                        {
                            quote = character;
                        }
                        else
                        {
                            if (quote == character)
                            {
                                quote = "";
                            }
                        }
                        continue;
                    }
                    if (quote == "" && character == ":" && (index + 1 == value.Length || value.Substring(index + 1, 1) == " " || value.Substring(index + 1, 1) == "\t"))
                    {
                        return index;
                    }
                }
                return -1;
            };
            yamlQuotedScalarStart = (string content) =>
            {
                int start = 0;
                if (Tsonic.CSharp.Js.String.startsWith(content, "- "))
                {
                    start = 2;
                }
                while (start < content.Length && (content.Substring(start, 1) == " " || content.Substring(start, 1) == "\t"))
                {
                    start++;
                }
                string candidate = Utils_strings.substringFrom(content, start);
                int separator = yamlMappingSeparator(candidate);
                if (separator >= 0)
                {
                    start += separator + 1;
                    while (start < content.Length && (content.Substring(start, 1) == " " || content.Substring(start, 1) == "\t"))
                    {
                        start++;
                    }
                }
                if (start >= content.Length || (content.Substring(start, 1) != "\"" && content.Substring(start, 1) != "'"))
                {
                    return null;
                }
                return start;
            };
            scanYamlQuotedScalar = (string content, int quoteStart, string quote) =>
            {
                for (int index = quoteStart + 1; index < content.Length; index++)
                {
                    string character = content.Substring(index, 1);
                    if (quote == "\"" && character == "\\")
                    {
                        if (index + 1 >= content.Length)
                        {
                            return new YamlQuoteScan(false, true);
                        }
                        index++;
                        continue;
                    }
                    if (character != quote)
                    {
                        continue;
                    }
                    if (quote == "'" && index + 1 < content.Length && content.Substring(index + 1, 1) == "'")
                    {
                        index++;
                        continue;
                    }
                    return new YamlQuoteScan(true, false);
                }
                return new YamlQuoteScan(false, false);
            };
            readYamlLogicalLine = (Tsonic.CSharp.Js.JSArray<string> sourceLines, int sourceIndex, int indent, string? sourcePath) =>
            {
                string raw = sourceLines[sourceIndex];
                string content = Tsonic.CSharp.Js.String.trimEnd(Utils_structuredScalars.stripStructuredComment(Utils_strings.substringFrom(raw, indent), "yaml"));
                int? quoteStart = yamlQuotedScalarStart(content);
                if (quoteStart is null)
                {
                    return new YamlLogicalLine(content, sourceIndex + 1);
                }
                string quote = content.Substring(quoteStart.Value, 1);
                YamlQuoteScan scan = scanYamlQuotedScalar(content, quoteStart.Value, quote);
                if (scan.closed)
                {
                    return new YamlLogicalLine(content, sourceIndex + 1);
                }
                int minimumContinuationIndent = indent;
                int nextSourceIndex = sourceIndex + 1;
                int blankLineCount = 0;
                while (!scan.closed)
                {
                    if (scan.escapedLineBreak)
                    {
                        content = Tsonic.CSharp.Js.String.slice(content, 0, content.Length - 1);
                    }
                    if (nextSourceIndex >= sourceLines.length)
                    {
                        throw yamlError("String has mismatched quotes", sourcePath, sourceIndex + 1);
                    }
                    string continuationRaw = sourceLines[nextSourceIndex];
                    int continuationIndent = yamlSourceIndentation(continuationRaw, sourcePath, nextSourceIndex + 1);
                    string continuation = Tsonic.CSharp.Js.String.trim(continuationRaw);
                    nextSourceIndex++;
                    if (continuation == "")
                    {
                        blankLineCount++;
                        continue;
                    }
                    if (continuationIndent < minimumContinuationIndent)
                    {
                        throw yamlError("Multiline YAML scalar indentation is inconsistent", sourcePath, nextSourceIndex);
                    }
                    if (!scan.escapedLineBreak)
                    {
                        content += blankLineCount == 0 ? " " : Tsonic.CSharp.Js.String.repeat("\n", blankLineCount);
                    }
                    else
                    {
                        if (blankLineCount > 0)
                        {
                            content += Tsonic.CSharp.Js.String.repeat("\n", blankLineCount);
                        }
                    }
                    content += continuation;
                    blankLineCount = 0;
                    scan = scanYamlQuotedScalar(content, quoteStart.Value, quote);
                }
                return new YamlLogicalLine(Tsonic.CSharp.Js.String.trimEnd(Utils_structuredScalars.stripStructuredComment(content, "yaml")), nextSourceIndex);
            };
            jsonToTemplateValue = (JsonValue value) =>
            {
                if (value is JsonNull)
                {
                    return Template_runtimeHelpers.nil;
                }
                if (value is JsonBool)
                {
                    return new BoolValue(((JsonBool)value).value);
                }
                if (value is JsonNumber)
                {
                    if (!Tsonic.CSharp.Js.Number.isInteger(((JsonNumber)value).value) || ((JsonNumber)value).value < -2147483648 || ((JsonNumber)value).value > 2147483647)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_NUMBER_UNSUPPORTED", "Structured template data currently requires 32-bit integer numbers", null, ((JsonNumber)value).line, ((JsonNumber)value).column);
                    }
                    return new NumberValue((int)((JsonNumber)value).value);
                }
                if (value is JsonString)
                {
                    return new StringValue(((JsonString)value).value);
                }
                if (value is JsonArray)
                {
                    Tsonic.CSharp.Js.JSArray<TemplateValue> items = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
                    for (int index = 0; index < ((JsonArray)value).items.length; index++)
                    {
                        items.push(jsonToTemplateValue(((JsonArray)value).items[index]));
                    }
                    return new AnyArrayValue(items);
                }
                if (value is JsonObject)
                {
                    Tsonic.CSharp.Js.Map<string, TemplateValue> fields = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                    for (int index_1 = 0; index_1 < ((JsonObject)value).properties.length; index_1++)
                    {
                        JsonProperty property = ((JsonObject)value).properties[index_1];
                        fields.set(property.key, jsonToTemplateValue(property.value));
                    }
                    return new DictValue(fields);
                }
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_VALUE_INVALID", "Structured data contains an unknown value kind");
            };
            inputFromValue = (TemplateValue value) =>
            {
                if (value is ResourceValue)
                {
                    Resource resource = ((ResourceValue)value).value;
                    string? sourcePath = resource.sourcePath;
                    string? formatHint = sourcePath is null ? null : Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Node.path.extname(sourcePath));
                    return new StructuredInput(Resources_text.readResourceText(resource, "transform.Unmarshal"), sourcePath, formatHint);
                }
                if (value is StringValue)
                {
                    return new StructuredInput(((StringValue)value).value);
                }
                if (value is HtmlValue)
                {
                    return new StructuredInput(((HtmlValue)value).value.value);
                }
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_INPUT_INVALID", "transform.Unmarshal requires a string or resource input");
            };
            optionValue = (DictValue options, string name) =>
            {
                TemplateValue? exact = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(options.value, name);
                if (exact is not null)
                {
                    return Template_runtimeHelpers.toPlainString(exact);
                }
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(name);
                foreach (string key in options.value.keys())
                {
                    if (Tsonic.CSharp.Js.String.toLowerCase(key) == normalized)
                    {
                        TemplateValue? value = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(options.value, key);
                        return value is null ? null : Template_runtimeHelpers.toPlainString(value);
                    }
                }
                return null;
            };
            normalizeFormat = (string? requested, StructuredInput input) =>
            {
                string? @explicit = null;
                if (requested is not null)
                {
                    string selected = requested;
                    @explicit = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(selected));
                }
                if (@explicit is not null && @explicit != "")
                {
                    return @explicit == "yml" ? "yaml" : @explicit;
                }
                string? hint = input.formatHint;
                if (hint == ".json")
                {
                    return "json";
                }
                if (hint == ".yaml" || hint == ".yml")
                {
                    return "yaml";
                }
                if (hint == ".toml")
                {
                    return "toml";
                }
                string trimmed = Tsonic.CSharp.Js.String.trimStart(input.text);
                return Tsonic.CSharp.Js.String.startsWith(trimmed, "{") || Tsonic.CSharp.Js.String.startsWith(trimmed, "[") ? "json" : "yaml";
            };
            parseTemplateDataText = (string text, string formatRaw, string? sourcePath) =>
            {
                string format = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(formatRaw));
                if (format == "json")
                {
                    return jsonToTemplateValue(Utils_json.parseJson(text, sourcePath));
                }
                if (format == "yaml" || format == "yml")
                {
                    return new YamlTemplateParser(text, sourcePath).parse();
                }
                if (format == "toml")
                {
                    return Template_evaluation_tomlData.parseTomlTemplateData(text, sourcePath);
                }
                throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_FORMAT_UNSUPPORTED", $"transform.Unmarshal format '{format}' is not supported by the current template data contract", sourcePath);
            };
            unmarshalTemplateData = (Tsonic.CSharp.Js.JSArray<TemplateValue> args) =>
            {
                if (args.length == 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_INPUT_MISSING", "transform.Unmarshal requires an input");
                }
                string? requestedFormat = null;
                if (args.length >= 2)
                {
                    TemplateValue options = args[0];
                    if (!(options is DictValue))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_OPTIONS_INVALID", "transform.Unmarshal options must be a dictionary");
                    }
                    requestedFormat = optionValue((DictValue)options, "format");
                }
                StructuredInput input = inputFromValue(args[args.length - 1]);
                string format = normalizeFormat(requestedFormat, input);
                return parseTemplateDataText(input.text, format, input.sourcePath);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class StructuredInput
    {
        public string text;
        public string? sourcePath;
        public string? formatHint;
        public StructuredInput(string text, string? sourcePath = null, string? formatHint = null)
        {
            this.text = text;
            this.sourcePath = sourcePath;
            this.formatHint = formatHint;
        }
    }
    public class YamlLine
    {
        public int indent;
        public string content;
        public int lineNumber;
        public YamlLine(int indent, string content, int lineNumber)
        {
            this.indent = indent;
            this.content = content;
            this.lineNumber = lineNumber;
        }
    }
    public class YamlLogicalLine
    {
        public string content;
        public int nextSourceIndex;
        public YamlLogicalLine(string content, int nextSourceIndex)
        {
            this.content = content;
            this.nextSourceIndex = nextSourceIndex;
        }
    }
    public class YamlQuoteScan
    {
        public bool closed;
        public bool escapedLineBreak;
        public YamlQuoteScan(bool closed, bool escapedLineBreak)
        {
            this.closed = closed;
            this.escapedLineBreak = escapedLineBreak;
        }
    }
    public class YamlParseResult
    {
        public TemplateValue value;
        public int nextIndex;
        public YamlParseResult(TemplateValue value, int nextIndex)
        {
            this.value = value;
            this.nextIndex = nextIndex;
        }
    }
    public class YamlBlockScalarHeader
    {
        public bool folded;
        public string chomping;
        public int? indentation;
        public YamlBlockScalarHeader(bool folded, string chomping, int? indentation)
        {
            this.folded = folded;
            this.chomping = chomping;
            this.indentation = indentation;
        }
    }
    public class YamlTemplateParser
    {
        public Tsonic.CSharp.Js.JSArray<YamlLine> lines;
        public Tsonic.CSharp.Js.JSArray<string> sourceLines;
        public string? sourcePath;
        public YamlTemplateParser(string text, string? sourcePath = null)
        {
            this.lines = new Tsonic.CSharp.Js.JSArray<YamlLine>(new YamlLine[] { });
            this.sourcePath = sourcePath;
            string normalized = Tsonic.CSharp.Js.String.replaceAll(Tsonic.CSharp.Js.String.replaceAll(text, "\r\n", "\n"), "\r", "\n");
            Tsonic.CSharp.Js.JSArray<string> sourceLines = Tsonic.CSharp.Js.String.split(normalized, "\n");
            this.sourceLines = sourceLines;
            int index = 0;
            while (index < sourceLines.length)
            {
                string raw = sourceLines[index];
                int indent = Template_evaluation_structuredData.yamlSourceIndentation(raw, sourcePath, index + 1);
                YamlLogicalLine logical = Template_evaluation_structuredData.readYamlLogicalLine(sourceLines, index, indent, sourcePath);
                string content = logical.content;
                int lineNumber = index + 1;
                index = logical.nextSourceIndex;
                if (Tsonic.CSharp.Js.String.trim(content) == "" || Tsonic.CSharp.Js.String.trim(content) == "---" || Tsonic.CSharp.Js.String.trim(content) == "...")
                {
                    continue;
                }
                this.lines.push(new YamlLine(indent, content, lineNumber));
            }
        }
        public TemplateValue parse()
        {
            if (this.lines.length == 0)
            {
                return Template_runtimeHelpers.nil;
            }
            YamlParseResult result = this.parseBlock(0, this.lines[0].indent);
            if (result.nextIndex != this.lines.length)
            {
                YamlLine line = this.lines[result.nextIndex];
                throw this.error("YAML indentation does not belong to the preceding value", line.lineNumber);
            }
            return result.value;
        }
        public YamlParseResult parseBlock(int index, int indent)
        {
            YamlLine line = this.lines[index];
            if (line.indent != indent)
            {
                throw this.error("YAML block indentation is inconsistent", line.lineNumber);
            }
            if (line.content == "-" || Tsonic.CSharp.Js.String.startsWith(line.content, "- "))
            {
                return this.parseSequence(index, indent);
            }
            if (Template_evaluation_structuredData.yamlMappingSeparator(line.content) >= 0)
            {
                return this.parseMapping(index, indent);
            }
            return new YamlParseResult(this.parseScalar(line.content, line.lineNumber), index + 1);
        }
        public YamlParseResult parseSequence(int index, int indent)
        {
            Tsonic.CSharp.Js.JSArray<TemplateValue> values = new Tsonic.CSharp.Js.JSArray<TemplateValue>(new TemplateValue[] { });
            int current = index;
            while (current < this.lines.length)
            {
                YamlLine line = this.lines[current];
                if (line.indent < indent)
                {
                    break;
                }
                if (line.indent != indent || (line.content != "-" && !Tsonic.CSharp.Js.String.startsWith(line.content, "- ")))
                {
                    throw this.error("YAML sequence entries must use the same indentation and '-' marker", line.lineNumber);
                }
                string item = line.content == "-" ? "" : Tsonic.CSharp.Js.String.trim(Utils_strings.substringFrom(line.content, 2));
                current++;
                if (item != "")
                {
                    int separator = Template_evaluation_structuredData.yamlMappingSeparator(item);
                    if (separator >= 0)
                    {
                        string key = Tsonic.CSharp.Js.String.trim(Tsonic.CSharp.Js.String.slice(item, 0, separator));
                        if (key == "")
                        {
                            throw this.error("YAML mapping key cannot be empty", line.lineNumber);
                        }
                        string valueText = Tsonic.CSharp.Js.String.trim(Utils_strings.substringFrom(item, separator + 1));
                        if (valueText == "")
                        {
                            throw this.error("A YAML sequence mapping must begin with a scalar-valued field", line.lineNumber);
                        }
                        Tsonic.CSharp.Js.Map<string, TemplateValue> fields = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
                        YamlBlockScalarHeader? blockHeader = this.parseBlockScalarHeader(valueText, line.lineNumber);
                        if (blockHeader is not null)
                        {
                            YamlParseResult block = this.parseBlockScalar(blockHeader, indent, line.lineNumber, current);
                            fields.set(key, block.value);
                            current = block.nextIndex;
                        }
                        else
                        {
                            fields.set(key, this.parseScalar(valueText, line.lineNumber));
                        }
                        if (current < this.lines.length && this.lines[current].indent > indent)
                        {
                            YamlParseResult continuation = this.parseBlock(current, this.lines[current].indent);
                            if (!(continuation.value is DictValue))
                            {
                                throw this.error("A YAML sequence mapping continuation must be a mapping", this.lines[current].lineNumber);
                            }
                            Tsonic.CSharp.Js.Map<string, TemplateValue> continuationFields = ((DictValue)continuation.value).value;
                            foreach (string continuationKey in continuationFields.keys())
                            {
                                if (fields.has(continuationKey))
                                {
                                    throw this.error($"YAML mapping key '{continuationKey}' is declared more than once", this.lines[current].lineNumber);
                                }
                                TemplateValue? continuationValue = Tsonic.CSharp.Js.Map.getReference<string, TemplateValue>(continuationFields, continuationKey);
                                if (continuationValue is null)
                                {
                                    throw this.error($"YAML mapping key '{continuationKey}' disappeared", this.lines[current].lineNumber);
                                }
                                fields.set(continuationKey, continuationValue);
                            }
                            current = continuation.nextIndex;
                        }
                        values.push(new DictValue(fields));
                        continue;
                    }
                    values.push(this.parseScalar(item, line.lineNumber));
                    if (current < this.lines.length && this.lines[current].indent > indent)
                    {
                        throw this.error("A scalar YAML sequence entry cannot own an indented block", this.lines[current].lineNumber);
                    }
                    continue;
                }
                if (current >= this.lines.length || this.lines[current].indent <= indent)
                {
                    values.push(Template_runtimeHelpers.nil);
                    continue;
                }
                YamlParseResult nested = this.parseBlock(current, this.lines[current].indent);
                values.push(nested.value);
                current = nested.nextIndex;
            }
            return new YamlParseResult(new AnyArrayValue(values), current);
        }
        public YamlParseResult parseMapping(int index, int indent)
        {
            Tsonic.CSharp.Js.Map<string, TemplateValue> fields = new Tsonic.CSharp.Js.Map<string, TemplateValue>();
            int current = index;
            while (current < this.lines.length)
            {
                YamlLine line = this.lines[current];
                if (line.indent < indent)
                {
                    break;
                }
                if (line.indent != indent || line.content == "-" || Tsonic.CSharp.Js.String.startsWith(line.content, "- "))
                {
                    throw this.error("YAML mapping entries must use consistent indentation", line.lineNumber);
                }
                int separator = Template_evaluation_structuredData.yamlMappingSeparator(line.content);
                if (separator < 0)
                {
                    throw this.error("YAML mapping entry requires a ':' separator", line.lineNumber);
                }
                string key = Tsonic.CSharp.Js.String.trim(Tsonic.CSharp.Js.String.slice(line.content, 0, separator));
                if (key == "")
                {
                    throw this.error("YAML mapping key cannot be empty", line.lineNumber);
                }
                if (fields.has(key))
                {
                    throw this.error($"YAML mapping key '{key}' is declared more than once", line.lineNumber);
                }
                string valueText = Tsonic.CSharp.Js.String.trim(Utils_strings.substringFrom(line.content, separator + 1));
                current++;
                if (valueText != "")
                {
                    YamlBlockScalarHeader? blockHeader = this.parseBlockScalarHeader(valueText, line.lineNumber);
                    if (blockHeader is not null)
                    {
                        YamlParseResult block = this.parseBlockScalar(blockHeader, indent, line.lineNumber, current);
                        fields.set(key, block.value);
                        current = block.nextIndex;
                        continue;
                    }
                    fields.set(key, this.parseScalar(valueText, line.lineNumber));
                    if (current < this.lines.length && this.lines[current].indent > indent)
                    {
                        throw this.error("A scalar YAML mapping value cannot own an indented block", this.lines[current].lineNumber);
                    }
                    continue;
                }
                if (current >= this.lines.length || this.lines[current].indent <= indent)
                {
                    fields.set(key, Template_runtimeHelpers.nil);
                    continue;
                }
                YamlParseResult nested = this.parseBlock(current, this.lines[current].indent);
                fields.set(key, nested.value);
                current = nested.nextIndex;
            }
            return new YamlParseResult(new DictValue(fields), current);
        }
        public YamlBlockScalarHeader? parseBlockScalarHeader(string value, int line)
        {
            if (!Tsonic.CSharp.Js.String.startsWith(value, "|") && !Tsonic.CSharp.Js.String.startsWith(value, ">"))
            {
                return null;
            }
            string chomping = "clip";
            int? indentation = null;
            for (int index = 1; index < value.Length; index++)
            {
                string character = value.Substring(index, 1);
                if (character == "-" || character == "+")
                {
                    if (chomping != "clip")
                    {
                        throw this.error("YAML block scalar has more than one chomping indicator", line);
                    }
                    chomping = character == "-" ? "strip" : "keep";
                    continue;
                }
                int? parsedIndentation = Utils_int32.parseInt32(character);
                if (parsedIndentation is null || parsedIndentation.Value < 1 || parsedIndentation.Value > 9)
                {
                    throw this.error($"YAML block scalar indicator '{value}' is invalid", line);
                }
                if (indentation is not null)
                {
                    throw this.error("YAML block scalar has more than one indentation indicator", line);
                }
                indentation = parsedIndentation.Value;
            }
            return new YamlBlockScalarHeader(Tsonic.CSharp.Js.String.startsWith(value, ">"), chomping, indentation);
        }
        public YamlParseResult parseBlockScalar(YamlBlockScalarHeader header, int parentIndent, int headerLine, int nextParsedIndex)
        {
            int sourceStart = headerLine;
            int sourceEnd = sourceStart;
            while (sourceEnd < this.sourceLines.length)
            {
                string raw = this.sourceLines[sourceEnd];
                if (Tsonic.CSharp.Js.String.trim(raw) == "")
                {
                    sourceEnd++;
                    continue;
                }
                int indentation = Template_evaluation_structuredData.yamlSourceIndentation(raw, this.sourcePath, sourceEnd + 1);
                if (indentation <= parentIndent)
                {
                    break;
                }
                sourceEnd++;
            }
            int parsedIndex = nextParsedIndex;
            while (parsedIndex < this.lines.length && this.lines[parsedIndex].lineNumber <= sourceEnd)
            {
                parsedIndex++;
            }
            int? contentIndent = null;
            int? explicitIndentation = header.indentation;
            if (explicitIndentation is not null)
            {
                contentIndent = parentIndent + explicitIndentation.Value;
            }
            if (contentIndent is null)
            {
                for (int sourceIndex = sourceStart; sourceIndex < sourceEnd; sourceIndex++)
                {
                    string raw_1 = this.sourceLines[sourceIndex];
                    if (Tsonic.CSharp.Js.String.trim(raw_1) == "")
                    {
                        continue;
                    }
                    contentIndent = Template_evaluation_structuredData.yamlSourceIndentation(raw_1, this.sourcePath, sourceIndex + 1);
                    break;
                }
            }
            int selectedIndent = contentIndent is not null ? contentIndent.Value : parentIndent + 1;
            Tsonic.CSharp.Js.JSArray<string> values = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
            Tsonic.CSharp.Js.JSArray<int> indentations = new Tsonic.CSharp.Js.JSArray<int>(new int[] { });
            for (int sourceIndex_1 = sourceStart; sourceIndex_1 < sourceEnd; sourceIndex_1++)
            {
                string raw_2 = this.sourceLines[sourceIndex_1];
                if (Tsonic.CSharp.Js.String.trim(raw_2) == "")
                {
                    values.push("");
                    indentations.push(selectedIndent);
                    continue;
                }
                int indentation_1 = Template_evaluation_structuredData.yamlSourceIndentation(raw_2, this.sourcePath, sourceIndex_1 + 1);
                if (indentation_1 < selectedIndent)
                {
                    throw this.error("YAML block scalar indentation is inconsistent", sourceIndex_1 + 1);
                }
                values.push(Utils_strings.substringFrom(raw_2, selectedIndent));
                indentations.push(indentation_1);
            }
            int lastContentIndex = values.length - 1;
            while (lastContentIndex >= 0 && values[lastContentIndex] == "")
            {
                lastContentIndex--;
            }
            string rendered = "";
            for (int index = 0; index <= lastContentIndex; index++)
            {
                rendered += values[index];
                if (index >= lastContentIndex)
                {
                    continue;
                }
                if (!header.folded || values[index] == "" || values[index + 1] == "" || indentations[index] > selectedIndent || indentations[index + 1] > selectedIndent)
                {
                    rendered += "\n";
                }
                else
                {
                    rendered += " ";
                }
            }
            if (header.chomping == "clip")
            {
                rendered += "\n";
            }
            if (header.chomping == "keep")
            {
                int trailingLineCount = values.length - lastContentIndex;
                rendered += Tsonic.CSharp.Js.String.repeat("\n", trailingLineCount);
            }
            return new YamlParseResult(new StringValue(rendered), parsedIndex);
        }
        public TemplateValue parseScalar(string value, int line)
        {
            string normalized = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(value));
            if (normalized == "null" || normalized == "~")
            {
                return Template_runtimeHelpers.nil;
            }
            if (Tsonic.CSharp.Js.String.startsWith(value, "[") || Tsonic.CSharp.Js.String.startsWith(value, "{"))
            {
                throw this.error("YAML flow collections are not supported by the current template data contract", line);
            }
            string? sourcePath = this.sourcePath;
            ParamValue parsed = Utils_structuredScalars.parseStructuredScalar(value, "yaml", (string message) => Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_YAML_INVALID", message, sourcePath, line, 1));
            if (parsed.kind == ParamKind.Bool)
            {
                return new BoolValue(parsed.boolValue);
            }
            if (parsed.kind == ParamKind.Number)
            {
                return new NumberValue(parsed.numberValue);
            }
            return new StringValue(parsed.stringValue);
        }
        public TsumoError error(string message, int line)
        {
            return Diagnostics.createTsumoError("TSUMO_TEMPLATE_UNMARSHAL_YAML_INVALID", message, this.sourcePath, line, 1);
        }
    }
}
