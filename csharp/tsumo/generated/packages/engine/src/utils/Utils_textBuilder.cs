using System;

namespace Tsumo.Engine
{
    public static class Utils_textBuilder
    {
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class TextBuilder
    {
        private System.Text.StringBuilder __tsonic_private_c76097e219622eeef9ec1e4e5e5fbd4a57a585e024a0b0827b4eb26c81103456;
        public TextBuilder()
        {
            this.__tsonic_private_c76097e219622eeef9ec1e4e5e5fbd4a57a585e024a0b0827b4eb26c81103456 = new System.Text.StringBuilder();
        }
        public int length
        {
            get
            {
                return this.__tsonic_private_c76097e219622eeef9ec1e4e5e5fbd4a57a585e024a0b0827b4eb26c81103456.Length;
            }
        }
        public void append(string text)
        {
            this.__tsonic_private_c76097e219622eeef9ec1e4e5e5fbd4a57a585e024a0b0827b4eb26c81103456.Append(text);
        }
        public string toString()
        {
            return this.__tsonic_private_c76097e219622eeef9ec1e4e5e5fbd4a57a585e024a0b0827b4eb26c81103456.ToString();
        }
    }
}
