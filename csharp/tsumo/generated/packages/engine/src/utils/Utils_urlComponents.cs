using System;

namespace Tsumo.Engine
{
    public static class Utils_urlComponents
    {
        public static System.Text.UTF8Encoding strictUtf8
        {
            get;
            private set;
        } = default(System.Text.UTF8Encoding)!;
        public static Func<string, int> hexValue
        {
            get;
            private set;
        } = default(Func<string, int>)!;
        public static Action<System.Collections.Generic.List<byte>, string> appendUtf8
        {
            get;
            private set;
        } = default(Action<System.Collections.Generic.List<byte>, string>)!;
        public static Func<string, string> decodeUrlComponent
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            strictUtf8 = new System.Text.UTF8Encoding(false, true);
            hexValue = (string value) =>
            {
                double code = Tsonic.CSharp.Js.String.charCodeAt(value, 0);
                if (code >= 48 && code <= 57)
                {
                    return (int)(code - 48);
                }
                if (code >= 65 && code <= 70)
                {
                    return (int)(code - 55);
                }
                if (code >= 97 && code <= 102)
                {
                    return (int)(code - 87);
                }
                return -1;
            };
            appendUtf8 = (System.Collections.Generic.List<byte> output, string value) =>
            {
                byte[] encoded = strictUtf8.GetBytes(value);
                for (int index = 0; index < encoded.Length; index++)
                {
                    output.Add(encoded[index]);
                }
            };
            decodeUrlComponent = (string value) =>
            {
                System.Collections.Generic.List<byte> decoded = new System.Collections.Generic.List<byte>();
                int literalStart = 0;
                int index = 0;
                while (index < value.Length)
                {
                    if (Utils_strings.substringCount(value, index, 1) != "%")
                    {
                        index++;
                        continue;
                    }
                    if (literalStart < index)
                    {
                        appendUtf8(decoded, Utils_strings.substringCount(value, literalStart, index - literalStart));
                    }
                    if (index + 2 >= value.Length)
                    {
                        throw new System.Exception("URL component contains an incomplete percent escape");
                    }
                    int high = hexValue(Utils_strings.substringCount(value, index + 1, 1));
                    int low = hexValue(Utils_strings.substringCount(value, index + 2, 1));
                    if (high < 0 || low < 0)
                    {
                        throw new System.Exception("URL component contains an invalid percent escape");
                    }
                    decoded.Add((byte)(high * 16 + low));
                    index += 3;
                    literalStart = index;
                }
                if (literalStart < value.Length)
                {
                    appendUtf8(decoded, Utils_strings.substringCount(value, literalStart, value.Length - literalStart));
                }
                return strictUtf8.GetString(decoded.ToArray());
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
