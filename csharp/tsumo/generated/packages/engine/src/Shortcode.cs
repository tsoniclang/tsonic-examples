using System;

namespace Tsumo.Engine
{
    public static class Shortcode
    {
        public static Func<ParseState, string?, int, int, string> parseQuotedString
        {
            get;
            private set;
        } = default(Func<ParseState, string?, int, int, string>)!;
        public static Func<ParseState, string> parseUnquotedValue
        {
            get;
            private set;
        } = default(Func<ParseState, string>)!;
        public static Func<string, string?, int, int, __TsonicShape_c0627f65328ba94ab7816ed39f682bb5c70c61f24a74af557f1bcdfd06234f9c> parseParams
        {
            get;
            private set;
        } = default(Func<string, string?, int, int, __TsonicShape_c0627f65328ba94ab7816ed39f682bb5c70c61f24a74af557f1bcdfd06234f9c>)!;
        public static Func<string, string, int, bool, __TsonicShape_2945f73fec05dfd8cf3c4e3fb122c5df8b275c740c90075bf46e4ec6a92b5239?> findClosingTag
        {
            get;
            private set;
        } = default(Func<string, string, int, bool, __TsonicShape_2945f73fec05dfd8cf3c4e3fb122c5df8b275c740c90075bf46e4ec6a92b5239?>)!;
        public static Func<string, string?, Tsonic.CSharp.Js.JSArray<ShortcodeCall>> parseShortcodes
        {
            get;
            private set;
        } = default(Func<string, string?, Tsonic.CSharp.Js.JSArray<ShortcodeCall>>)!;
        public static Func<string, string?, Tsonic.CSharp.Js.Map<string, bool>> collectShortcodeNames
        {
            get;
            private set;
        } = default(Func<string, string?, Tsonic.CSharp.Js.Map<string, bool>>)!;
        public static Func<string, string> innerDeindent
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Params.__tsonic_module_init();
            parseQuotedString = (ParseState state, string? sourcePath, int line, int column) =>
            {
                string quote = state.peek(0);
                if (quote != "\"" && quote != "'")
                {
                    return "";
                }
                state.advance(1);
                string result = "";
                bool closed = false;
                while (!state.atEnd())
                {
                    string c = state.peek(0);
                    if (c == quote)
                    {
                        state.advance(1);
                        closed = true;
                        break;
                    }
                    if (c == "\\" && !state.atEnd())
                    {
                        state.advance(1);
                        result += state.peek(0);
                        state.advance(1);
                        continue;
                    }
                    result += c;
                    state.advance(1);
                }
                if (!closed)
                {
                    throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_STRING_UNCLOSED", $"Shortcode string opened with {quote} but is not closed", sourcePath, line, column);
                }
                return result;
            };
            parseUnquotedValue = (ParseState state) =>
            {
                string result = "";
                while (!state.atEnd())
                {
                    string c = state.peek(0);
                    if (c == " " || c == "\t" || c == "\n" || c == "\r" || c == ">" || c == "%" || c == "/")
                    {
                        break;
                    }
                    result += c;
                    state.advance(1);
                }
                return result;
            };
            parseParams = (string argsText, string? sourcePath, int line, int column) =>
            {
                Tsonic.CSharp.Js.Map<string, ParamValue> @params = new Tsonic.CSharp.Js.Map<string, ParamValue>();
                Tsonic.CSharp.Js.JSArray<string> positional = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                bool isNamed = false;
                ParseState state = new ParseState(Tsonic.CSharp.Js.String.trim(argsText));
                while (!state.atEnd())
                {
                    state.skipWhitespace();
                    if (state.atEnd())
                    {
                        break;
                    }
                    string peek2 = state.peekString(2);
                    if (peek2 == ">}" || peek2 == "%}" || peek2 == "/>" || peek2 == "/%")
                    {
                        break;
                    }
                    string key = "";
                    string value = "";
                    bool foundEquals = false;
                    while (!state.atEnd())
                    {
                        string c = state.peek(0);
                        if (c == "=" && state.peek(1) != "=")
                        {
                            foundEquals = true;
                            state.advance(1);
                            break;
                        }
                        if (c == " " || c == "\t" || c == "\n" || c == "\r" || c == ">" || c == "%" || c == "/")
                        {
                            break;
                        }
                        if (c == "\"" || c == "'")
                        {
                            break;
                        }
                        key += c;
                        state.advance(1);
                    }
                    if (foundEquals)
                    {
                        if (key == "")
                        {
                            throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_PARAMETER_INVALID", "Shortcode named parameters require a name", sourcePath, line, column);
                        }
                        if (@params.has(key))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_PARAMETER_DUPLICATE", $"Shortcode parameter '{key}' is declared more than once", sourcePath, line, column);
                        }
                        isNamed = true;
                        state.skipWhitespace();
                        if (state.atEnd())
                        {
                            throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_PARAMETER_INVALID", $"Shortcode parameter '{key}' requires a value", sourcePath, line, column);
                        }
                        string q = state.peek(0);
                        bool quoted = q == "\"" || q == "'";
                        if (quoted)
                        {
                            value = parseQuotedString(state, sourcePath, line, column);
                        }
                        else
                        {
                            value = parseUnquotedValue(state);
                        }
                        if (!quoted && value == "")
                        {
                            throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_PARAMETER_INVALID", $"Shortcode parameter '{key}' requires a value", sourcePath, line, column);
                        }
                        @params.set(key, quoted ? ParamValue.@string(value) : ParamValue.parseScalar(value));
                    }
                    else
                    {
                        if (key == "")
                        {
                            string q_1 = state.peek(0);
                            if (q_1 == "\"" || q_1 == "'")
                            {
                                key = parseQuotedString(state, sourcePath, line, column);
                            }
                        }
                        if (key != "")
                        {
                            positional.push(key);
                        }
                    }
                }
                if (isNamed && positional.length > 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_PARAMETER_STYLE_MIXED", "Shortcode parameters cannot mix named and positional forms", sourcePath, line, column);
                }
                return new __TsonicShape_c0627f65328ba94ab7816ed39f682bb5c70c61f24a74af557f1bcdfd06234f9c
                {
                    __tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3 = @params,
                    positional = positional,
                    isNamed = isNamed,
                };
            };
            findClosingTag = (string text, string name, int startPos, bool isMarkdown) =>
            {
                string openTag = isMarkdown ? "{{%" : "{{<";
                string closeTagPrefix = isMarkdown ? $"{{{{% /{name}" : $"{{{{< /{name}";
                string closeTagPrefix2 = isMarkdown ? $"{{{{% / {name}" : $"{{{{< / {name}";
                int depth = 1;
                int pos = startPos;
                int innerStart = startPos;
                while (pos < text.Length)
                {
                    string remaining = Utils_strings.substringFrom(text, pos);
                    if (Tsonic.CSharp.Js.String.startsWith(remaining, openTag))
                    {
                        string afterOpen = Tsonic.CSharp.Js.String.trimStart(Utils_strings.substringFrom(text, pos + openTag.Length));
                        if (Tsonic.CSharp.Js.String.startsWith(afterOpen, name + " ") || Tsonic.CSharp.Js.String.startsWith(afterOpen, name + ">") || Tsonic.CSharp.Js.String.startsWith(afterOpen, name + "%"))
                        {
                            depth++;
                        }
                    }
                    if (Tsonic.CSharp.Js.String.startsWith(remaining, closeTagPrefix) || Tsonic.CSharp.Js.String.startsWith(remaining, closeTagPrefix2))
                    {
                        depth--;
                        if (depth == 0)
                        {
                            string inner = Utils_strings.substringCount(text, innerStart, pos - innerStart);
                            string endSuffix = isMarkdown ? "%}}" : ">}}";
                            int closeEnd = Utils_strings.indexOfTextFrom(text, endSuffix, pos);
                            if (closeEnd < 0)
                            {
                                return null;
                            }
                            return new __TsonicShape_2945f73fec05dfd8cf3c4e3fb122c5df8b275c740c90075bf46e4ec6a92b5239
                            {
                                inner = inner,
                                endPos = closeEnd + endSuffix.Length,
                            };
                        }
                    }
                    pos++;
                }
                return null;
            };
            parseShortcodes = (string text, string? sourcePath) =>
            {
                Tsonic.CSharp.Js.JSArray<ShortcodeCall> results = new Tsonic.CSharp.Js.JSArray<ShortcodeCall>(new ShortcodeCall[] { });
                ShortcodeSourceMap sourceMap = new ShortcodeSourceMap(text);
                int pos = 0;
                while (pos < text.Length)
                {
                    int openAngle = Utils_strings.indexOfTextFrom(text, "{{<", pos);
                    int openPercent = Utils_strings.indexOfTextFrom(text, "{{%", pos);
                    int openPos = -1;
                    bool isMarkdown = false;
                    if (openAngle >= 0)
                    {
                        if (openPercent < 0 || openAngle <= openPercent)
                        {
                            openPos = openAngle;
                            isMarkdown = false;
                        }
                    }
                    if (openPos < 0 && openPercent >= 0)
                    {
                        openPos = openPercent;
                        isMarkdown = true;
                    }
                    if (openPos < 0)
                    {
                        break;
                    }
                    if (sourceMap.isInCodeBlock(openPos))
                    {
                        pos = openPos + 3;
                        continue;
                    }
                    string closeSuffix = isMarkdown ? "%}}" : ">}}";
                    int closePos = Utils_strings.indexOfTextFrom(text, closeSuffix, openPos + 3);
                    if (closePos < 0)
                    {
                        ShortcodePosition position = sourceMap.positionAt(openPos);
                        throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_ACTION_UNCLOSED", $"Shortcode action opened with '{(isMarkdown ? "{{%" : "{{<")}' but is not closed", sourcePath, position.line, position.column);
                    }
                    string content = Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(text, openPos + 3, closePos - (openPos + 3)));
                    bool isSelfClosing = Tsonic.CSharp.Js.String.endsWith(content, "/");
                    string tagContent = isSelfClosing ? Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(content, 0, content.Length - 1)) : content;
                    if (Tsonic.CSharp.Js.String.startsWith(tagContent, "/*"))
                    {
                        pos = closePos + closeSuffix.Length;
                        continue;
                    }
                    int firstSpace = Tsonic.CSharp.Js.String.indexOf(tagContent, " ");
                    string name = firstSpace >= 0 ? Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(tagContent, 0, firstSpace)) : Tsonic.CSharp.Js.String.trim(tagContent);
                    string argsText = firstSpace >= 0 ? Utils_strings.substringFrom(tagContent, firstSpace + 1) : "";
                    if (name == "" || Tsonic.CSharp.Js.String.startsWith(name, "/"))
                    {
                        if (Tsonic.CSharp.Js.String.startsWith(name, "/"))
                        {
                            ShortcodePosition position_1 = sourceMap.positionAt(openPos);
                            throw Diagnostics.createTsumoError("TSUMO_SHORTCODE_CLOSE_UNEXPECTED", $"Unexpected shortcode closing action '{name}'", sourcePath, position_1.line, position_1.column);
                        }
                        pos = closePos + closeSuffix.Length;
                        continue;
                    }
                    ShortcodePosition position_2 = sourceMap.positionAt(openPos);
                    __TsonicShape_c0627f65328ba94ab7816ed39f682bb5c70c61f24a74af557f1bcdfd06234f9c parsed = parseParams(argsText, sourcePath, position_2.line, position_2.column);
                    if (isSelfClosing == true)
                    {
                        ShortcodeCall call = new ShortcodeCall(name, parsed.__tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3, parsed.positional, parsed.isNamed, "", isMarkdown, true, openPos, closePos + closeSuffix.Length, sourcePath, position_2.line, position_2.column);
                        results.push(call);
                        pos = closePos + closeSuffix.Length;
                        continue;
                    }
                    int tagEndPos = closePos + closeSuffix.Length;
                    __TsonicShape_2945f73fec05dfd8cf3c4e3fb122c5df8b275c740c90075bf46e4ec6a92b5239? closeResult = findClosingTag(text, name, tagEndPos, isMarkdown);
                    if (closeResult is not null)
                    {
                        ShortcodeCall call_1 = new ShortcodeCall(name, parsed.__tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3, parsed.positional, parsed.isNamed, closeResult.inner, isMarkdown, false, openPos, closeResult.endPos, sourcePath, position_2.line, position_2.column);
                        results.push(call_1);
                        pos = closeResult.endPos;
                    }
                    else
                    {
                        ShortcodeCall call_2 = new ShortcodeCall(name, parsed.__tsonic_member_a20b52fae57cc7a99c9651f1b573950fd211823e3ace3bb9c273c06430f24cd3, parsed.positional, parsed.isNamed, "", isMarkdown, true, openPos, tagEndPos, sourcePath, position_2.line, position_2.column);
                        results.push(call_2);
                        pos = tagEndPos;
                    }
                }
                return results;
            };
            collectShortcodeNames = (string text, string? sourcePath) =>
            {
                Tsonic.CSharp.Js.Map<string, bool> names = new Tsonic.CSharp.Js.Map<string, bool>();
                Tsonic.CSharp.Js.JSArray<string> pending = new Tsonic.CSharp.Js.JSArray<string>(new string[] { text });
                for (int pendingIndex = 0; pendingIndex < pending.length; pendingIndex++)
                {
                    Tsonic.CSharp.Js.JSArray<ShortcodeCall> calls = parseShortcodes(pending[pendingIndex], sourcePath);
                    for (int callIndex = 0; callIndex < calls.length; callIndex++)
                    {
                        ShortcodeCall call = calls[callIndex];
                        names.set(call.name, true);
                        if (call.inner != "")
                        {
                            pending.push(call.inner);
                        }
                    }
                }
                return names;
            };
            innerDeindent = (string inner) =>
            {
                Tsonic.CSharp.Js.JSArray<string> lines = Tsonic.CSharp.Js.String.split(inner, "\n");
                if (lines.length == 0)
                {
                    return inner;
                }
                int minIndent = -1;
                for (int i = 0; i < lines.length; i++)
                {
                    string line = lines[i];
                    if (Tsonic.CSharp.Js.String.trim(line) == "")
                    {
                        continue;
                    }
                    int indent = 0;
                    for (int j = 0; j < line.Length; j++)
                    {
                        string c = Utils_strings.substringCount(line, j, 1);
                        if (c == " ")
                        {
                            indent++;
                        }
                        else
                        {
                            if (c == "\t")
                            {
                                indent += 4;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (minIndent < 0 || indent < minIndent)
                    {
                        minIndent = indent;
                    }
                }
                if (minIndent <= 0)
                {
                    return inner;
                }
                Tsonic.CSharp.Js.JSArray<string> result = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int i_1 = 0; i_1 < lines.length; i_1++)
                {
                    string line_1 = lines[i_1];
                    if (Tsonic.CSharp.Js.String.trim(line_1) == "")
                    {
                        result.push(line_1);
                        continue;
                    }
                    int removed = 0;
                    int startIdx = 0;
                    for (int j_1 = 0; j_1 < line_1.Length && removed < minIndent; j_1++)
                    {
                        string c_1 = Utils_strings.substringCount(line_1, j_1, 1);
                        if (c_1 == " ")
                        {
                            removed++;
                            startIdx++;
                        }
                        else
                        {
                            if (c_1 == "\t")
                            {
                                removed += 4;
                                startIdx++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    result.push(Utils_strings.substringFrom(line_1, startIdx));
                }
                Tsonic.CSharp.Js.JSArray<string> arr = result;
                string @out = "";
                for (int i_2 = 0; i_2 < arr.length; i_2++)
                {
                    if (i_2 > 0)
                    {
                        @out += "\n";
                    }
                    @out += arr[i_2];
                }
                return @out;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ShortcodeCall
    {
        public string name;
        public Tsonic.CSharp.Js.Map<string, ParamValue> @params;
        public Tsonic.CSharp.Js.JSArray<string> positionalParams;
        public bool isNamedParams;
        public string inner;
        public bool isMarkdown;
        public bool isSelfClosing;
        public int startIndex;
        public int endIndex;
        public string? sourcePath;
        public int line;
        public int column;
        public ShortcodeCall(string name, Tsonic.CSharp.Js.Map<string, ParamValue> @params, Tsonic.CSharp.Js.JSArray<string> positionalParams, bool isNamedParams, string inner, bool isMarkdown, bool isSelfClosing, int startIndex, int endIndex, string? sourcePath, int line, int column)
        {
            this.name = name;
            this.@params = @params;
            this.positionalParams = positionalParams;
            this.isNamedParams = isNamedParams;
            this.inner = inner;
            this.isMarkdown = isMarkdown;
            this.isSelfClosing = isSelfClosing;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.sourcePath = sourcePath;
            this.line = line;
            this.column = column;
        }
    }
    public class ParseState
    {
        public string text;
        public int pos;
        public ParseState(string text)
        {
            this.text = text;
            this.pos = 0;
        }
        public string peek(int offset)
        {
            int idx = this.pos + offset;
            return idx < this.text.Length ? Utils_strings.substringCount(this.text, idx, 1) : "";
        }
        public string peekString(int length)
        {
            int remaining = this.text.Length - this.pos;
            if (remaining <= 0)
            {
                return "";
            }
            int sliceLength = length < remaining ? length : remaining;
            return Utils_strings.substringCount(this.text, this.pos, sliceLength);
        }
        public void advance(int count)
        {
            this.pos += count;
        }
        public bool atEnd()
        {
            return this.pos >= this.text.Length;
        }
        public void skipWhitespace()
        {
            while (!this.atEnd())
            {
                string c = this.peek(0);
                if (c != " " && c != "\t" && c != "\n" && c != "\r")
                {
                    break;
                }
                this.advance(1);
            }
        }
    }
    public class ShortcodePosition
    {
        public int line;
        public int column;
        public ShortcodePosition(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
    public class ShortcodeRange
    {
        public int start;
        public int end;
        public ShortcodeRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
    public class ShortcodeSourceMap
    {
        public Tsonic.CSharp.Js.JSArray<int> lineStarts;
        public Tsonic.CSharp.Js.JSArray<ShortcodeRange> codeFences;
        public ShortcodeSourceMap(string text)
        {
            this.lineStarts = new Tsonic.CSharp.Js.JSArray<int>(new int[] { 0 });
            for (int index = 0; index < text.Length; index++)
            {
                string current = text.Substring(index, 1);
                if (current == "\r")
                {
                    if (index + 1 < text.Length && text.Substring(index + 1, 1) == "\n")
                    {
                        index++;
                    }
                    this.lineStarts.push(index + 1);
                }
                else
                {
                    if (current == "\n")
                    {
                        this.lineStarts.push(index + 1);
                    }
                }
            }
            this.codeFences = new Tsonic.CSharp.Js.JSArray<ShortcodeRange>(new ShortcodeRange[] { });
            int fenceStart = -1;
            string fenceCharacter = "";
            int fenceLength = 0;
            int position = 0;
            while (position < text.Length)
            {
                string current_1 = text.Substring(position, 1);
                if (fenceStart < 0 && (current_1 == "`" || current_1 == "~"))
                {
                    int length = 1;
                    while (position + length < text.Length && text.Substring(position + length, 1) == current_1)
                    {
                        length++;
                    }
                    if (length >= 3)
                    {
                        fenceStart = position;
                        fenceCharacter = current_1;
                        fenceLength = length;
                        position += length;
                        while (position < text.Length && text.Substring(position, 1) != "\n")
                        {
                            position++;
                        }
                        continue;
                    }
                }
                else
                {
                    if (fenceStart >= 0 && current_1 == fenceCharacter)
                    {
                        int length_1 = 1;
                        while (position + length_1 < text.Length && text.Substring(position + length_1, 1) == current_1)
                        {
                            length_1++;
                        }
                        if (length_1 >= fenceLength)
                        {
                            this.codeFences.push(new ShortcodeRange(fenceStart, position + length_1));
                            fenceStart = -1;
                            fenceCharacter = "";
                            fenceLength = 0;
                            position += length_1;
                            continue;
                        }
                    }
                }
                position++;
            }
            if (fenceStart >= 0)
            {
                this.codeFences.push(new ShortcodeRange(fenceStart, text.Length));
            }
        }
        public ShortcodePosition positionAt(int offset)
        {
            int low = 0;
            int high = this.lineStarts.length - 1;
            while (low <= high)
            {
                int middle = (int)(low + Tsonic.CSharp.Js.Math.floor((high - low) / 2));
                if (this.lineStarts[middle] <= offset)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle - 1;
                }
            }
            int lineIndex = high < 0 ? 0 : high;
            return new ShortcodePosition(lineIndex + 1, offset - this.lineStarts[lineIndex] + 1);
        }
        public bool isInCodeBlock(int offset)
        {
            int low = 0;
            int high = this.codeFences.length - 1;
            while (low <= high)
            {
                int middle = (int)(low + Tsonic.CSharp.Js.Math.floor((high - low) / 2));
                ShortcodeRange range = this.codeFences[middle];
                if (offset < range.start)
                {
                    high = middle - 1;
                }
                else
                {
                    if (offset >= range.end)
                    {
                        low = middle + 1;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
