using System;

namespace Tsumo.Engine
{
    public static class Template_functions_textCompatibility
    {
        public static Func<Tsonic.CSharp.Js.Map<string, string>> createEmojiShortcodes
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Map<string, string>>)!;
        public static Tsonic.CSharp.Js.Map<string, string> emojiByShortcode
        {
            get;
            private set;
        } = default(Tsonic.CSharp.Js.Map<string, string>)!;
        public static Func<string, bool> isAsciiLetterOrDigit
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> isAsciiWhitespace
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, string> anchorizeText
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string> emojifyText
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            createEmojiShortcodes = () =>
            {
                Tsonic.CSharp.Js.Map<string, string> result = new Tsonic.CSharp.Js.Map<string, string>();
                result.set("heart", "❤️");
                result.set("red_heart", "❤️");
                result.set("smile", "😄");
                result.set("grinning", "😀");
                result.set("joy", "😂");
                result.set("tada", "🎉");
                result.set("rocket", "🚀");
                result.set("warning", "⚠️");
                result.set("wave", "👋");
                result.set("fire", "🔥");
                result.set("sparkles", "✨");
                return result;
            };
            emojiByShortcode = createEmojiShortcodes();
            isAsciiLetterOrDigit = (string character) =>
            {
                double code = Tsonic.CSharp.Js.String.charCodeAt(character, 0);
                return ((code >= 48 && code <= 57) || (code >= 65 && code <= 90) || (code >= 97 && code <= 122));
            };
            isAsciiWhitespace = (string character) => character == " " || character == "\t" || character == "\n" || character == "\r";
            anchorizeText = (string input) =>
            {
                string lower = Tsonic.CSharp.Js.String.toLowerCase(input);
                Tsonic.CSharp.Js.JSArray<string> result = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int index = 0; index < lower.Length; index++)
                {
                    string character = lower.Substring(index, 1);
                    if (isAsciiWhitespace(character))
                    {
                        result.push("-");
                        continue;
                    }
                    if (isAsciiLetterOrDigit(character) || character == "-" || character == "_" || Tsonic.CSharp.Js.String.charCodeAt(character, 0) >= 128)
                    {
                        result.push(character);
                    }
                }
                return Tsonic.CSharp.Js.Array.join(result, "");
            };
            emojifyText = (string input) =>
            {
                Tsonic.CSharp.Js.JSArray<string> result = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                int cursor = 0;
                while (cursor < input.Length)
                {
                    int opening = Tsonic.CSharp.Js.String.indexOf(input, ":", cursor);
                    if (opening < 0)
                    {
                        result.push(Tsonic.CSharp.Js.String.substring(input, cursor));
                        break;
                    }
                    result.push(Tsonic.CSharp.Js.String.substring(input, cursor, opening));
                    int closing = Tsonic.CSharp.Js.String.indexOf(input, ":", opening + 1);
                    if (closing < 0)
                    {
                        result.push(Tsonic.CSharp.Js.String.substring(input, opening));
                        break;
                    }
                    string shortcode = Tsonic.CSharp.Js.String.substring(input, opening + 1, closing);
                    string? emoji = Tsonic.CSharp.Js.Map.getReference<string, string>(emojiByShortcode, shortcode);
                    if (emoji is null)
                    {
                        result.push(Tsonic.CSharp.Js.String.substring(input, opening, closing + 1));
                    }
                    else
                    {
                        result.push(emoji);
                    }
                    cursor = closing + 1;
                }
                return Tsonic.CSharp.Js.Array.join(result, "");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
