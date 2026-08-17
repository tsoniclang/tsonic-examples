using System;

namespace Tsumo.Engine
{
    public static class Utils_structuredScalars
    {
        public static Func<string, int> hexValue
        {
            get;
            private set;
        } = default(Func<string, int>)!;
        public static Func<string, int, int, Func<string, TsumoError>, string> decodeHexEscape
        {
            get;
            private set;
        } = default(Func<string, int, int, Func<string, TsumoError>, string>)!;
        public static Func<string, string, Func<string, TsumoError>, string> decodeSingleQuoted
        {
            get;
            private set;
        } = default(Func<string, string, Func<string, TsumoError>, string>)!;
        public static Func<string, Func<string, TsumoError>, string> decodeDoubleQuoted
        {
            get;
            private set;
        } = default(Func<string, Func<string, TsumoError>, string>)!;
        public static Func<string, string, Func<string, TsumoError>, string?> decodeQuoted
        {
            get;
            private set;
        } = default(Func<string, string, Func<string, TsumoError>, string?>)!;
        public static Func<string, Func<string, TsumoError>, ParamValue?> parseInteger
        {
            get;
            private set;
        } = default(Func<string, Func<string, TsumoError>, ParamValue?>)!;
        public static Func<string, string, Func<string, TsumoError>, ParamValue> parseStructuredScalar
        {
            get;
            private set;
        } = default(Func<string, string, Func<string, TsumoError>, ParamValue>)!;
        public static Func<string, string, string> stripStructuredComment
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Params.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            hexValue = (string character) => Utils_strings.indexOfText("0123456789abcdef", Tsonic.CSharp.Js.String.toLowerCase(character));
            decodeHexEscape = (string source, int start, int count, Func<string, TsumoError> invalid) =>
            {
                if (start + count > source.Length)
                {
                    throw invalid($"String escape requires {count} hexadecimal digits");
                }
                int value = 0;
                for (int offset = 0; offset < count; offset++)
                {
                    int digit = hexValue(source.Substring(start + offset, 1));
                    if (digit < 0)
                    {
                        throw invalid("String escape contains a non-hexadecimal digit");
                    }
                    value = value * 16 + digit;
                }
                if (value > 1114111 || (value >= 55296 && value <= 57343))
                {
                    throw invalid("String escape does not name a Unicode scalar value");
                }
                return System.Char.ConvertFromUtf32(value);
            };
            decodeSingleQuoted = (string inner, string format, Func<string, TsumoError> invalid) =>
            {
                string result = "";
                for (int index = 0; index < inner.Length; index++)
                {
                    string current = inner.Substring(index, 1);
                    if (current != "'")
                    {
                        result += current;
                        continue;
                    }
                    if (format == "yaml" && index + 1 < inner.Length && inner.Substring(index + 1, 1) == "'")
                    {
                        result += "'";
                        index++;
                        continue;
                    }
                    throw invalid("Single-quoted string contains an unescaped quote");
                }
                return result;
            };
            decodeDoubleQuoted = (string inner, Func<string, TsumoError> invalid) =>
            {
                string result = "";
                for (int index = 0; index < inner.Length; index++)
                {
                    string current = inner.Substring(index, 1);
                    if (current == "\"")
                    {
                        throw invalid("Double-quoted string contains an unescaped quote");
                    }
                    if (current != "\\")
                    {
                        result += current;
                        continue;
                    }
                    if (index + 1 >= inner.Length)
                    {
                        throw invalid("String ends with an incomplete escape");
                    }
                    string escaped = inner.Substring(++index, 1);
                    if (escaped == "\"" || escaped == "\\" || escaped == "/")
                    {
                        result += escaped;
                    }
                    else
                    {
                        if (escaped == "b")
                        {
                            result += "\b";
                        }
                        else
                        {
                            if (escaped == "t")
                            {
                                result += "\t";
                            }
                            else
                            {
                                if (escaped == "n")
                                {
                                    result += "\n";
                                }
                                else
                                {
                                    if (escaped == "f")
                                    {
                                        result += "\f";
                                    }
                                    else
                                    {
                                        if (escaped == "r")
                                        {
                                            result += "\r";
                                        }
                                        else
                                        {
                                            if (escaped == "u")
                                            {
                                                result += decodeHexEscape(inner, index + 1, 4, invalid);
                                                index += 4;
                                            }
                                            else
                                            {
                                                if (escaped == "U")
                                                {
                                                    result += decodeHexEscape(inner, index + 1, 8, invalid);
                                                    index += 8;
                                                }
                                                else
                                                {
                                                    throw invalid($"Unsupported string escape '\\{escaped}'");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return result;
            };
            decodeQuoted = (string value, string format, Func<string, TsumoError> invalid) =>
            {
                string first = value.Length == 0 ? "" : value.Substring(0, 1);
                string last = value.Length == 0 ? "" : value.Substring(value.Length - 1, 1);
                bool startsQuoted = first == "\"" || first == "'";
                bool endsQuoted = last == "\"" || last == "'";
                if (!startsQuoted && !endsQuoted)
                {
                    return null;
                }
                if (!startsQuoted)
                {
                    if (format == "yaml")
                    {
                        return null;
                    }
                    throw invalid("String has mismatched quotes");
                }
                if (first != last || value.Length < 2)
                {
                    throw invalid("String has mismatched quotes");
                }
                string inner = Utils_strings.substringCount(value, 1, value.Length - 2);
                return first == "'" ? decodeSingleQuoted(inner, format, invalid) : decodeDoubleQuoted(inner, invalid);
            };
            parseInteger = (string value, Func<string, TsumoError> invalid) =>
            {
                bool integerLike = new Tsonic.CSharp.Js.RegExp("^[+-]?[0-9_]+$", "").test(value);
                if (!integerLike)
                {
                    return null;
                }
                if (!new Tsonic.CSharp.Js.RegExp("^[+-]?(?:0|[1-9](?:_?[0-9])*)$", "").test(value))
                {
                    throw invalid("Integer has invalid leading zeroes or underscore placement");
                }
                string normalized = Tsonic.CSharp.Js.String.replaceAll(value, "_", "");
                if (Tsonic.CSharp.Js.String.startsWith(normalized, "+"))
                {
                    normalized = Utils_strings.substringFrom(normalized, 1);
                }
                int? parsed = Utils_int32.parseInt32(normalized);
                if (parsed is null)
                {
                    throw invalid("Integer is outside the supported 32-bit range");
                }
                return ParamValue.number(parsed.Value);
            };
            parseStructuredScalar = (string value, string format, Func<string, TsumoError> invalid) =>
            {
                string trimmed = Tsonic.CSharp.Js.String.trim(value);
                string? quoted = decodeQuoted(trimmed, format, invalid);
                if (quoted is not null)
                {
                    return ParamValue.@string(quoted);
                }
                if (format == "toml")
                {
                    if (trimmed == "true")
                    {
                        return ParamValue.@bool(true);
                    }
                    if (trimmed == "false")
                    {
                        return ParamValue.@bool(false);
                    }
                }
                else
                {
                    string normalized = Tsonic.CSharp.Js.String.toLowerCase(trimmed);
                    if (normalized == "true")
                    {
                        return ParamValue.@bool(true);
                    }
                    if (normalized == "false")
                    {
                        return ParamValue.@bool(false);
                    }
                }
                ParamValue? integer = parseInteger(trimmed, invalid);
                if (integer is not null)
                {
                    return integer;
                }
                if (format == "toml")
                {
                    throw invalid("TOML string values must be quoted");
                }
                return ParamValue.@string(trimmed);
            };
            stripStructuredComment = (string line, string format) =>
            {
                string quote = "";
                bool escaped = false;
                for (int index = 0; index < line.Length; index++)
                {
                    string current = line.Substring(index, 1);
                    if (escaped)
                    {
                        escaped = false;
                        continue;
                    }
                    if (quote == "\"" && current == "\\")
                    {
                        escaped = true;
                        continue;
                    }
                    if (current == "\"" || current == "'")
                    {
                        if (quote == "")
                        {
                            quote = current;
                        }
                        else
                        {
                            if (quote == current)
                            {
                                if (quote == "'" && format == "yaml" && index + 1 < line.Length && line.Substring(index + 1, 1) == "'")
                                {
                                    index++;
                                }
                                else
                                {
                                    quote = "";
                                }
                            }
                        }
                        continue;
                    }
                    bool yamlComment = format == "yaml" && current == "#" && (index == 0 || new Tsonic.CSharp.Js.RegExp("\\s", "").test(line.Substring(index - 1, 1)));
                    if ((format == "toml" && current == "#" && quote == "") || (yamlComment && quote == ""))
                    {
                        return Tsonic.CSharp.Js.String.trimEnd(Utils_strings.substringCount(line, 0, index));
                    }
                }
                return line;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
