using System;

namespace Tsumo.Engine
{
    public static class Models_mediaType
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
    public class MediaType
    {
        public string Type;
        public MediaType(string type)
        {
            this.Type = type;
        }
    }
}
