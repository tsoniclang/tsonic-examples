using System;

namespace Tsumo.Engine
{
    public static class Utils_paths
    {
        public static Func<string, string, bool> pathContainsOrEquals
        {
            get;
            private set;
        } = default(Func<string, string, bool>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            pathContainsOrEquals = (string root, string candidate) =>
            {
                string rel = Tsonic.CSharp.Node.path.relative(root, candidate);
                return rel == "" || (!Tsonic.CSharp.Node.path.isAbsolute(rel) && rel != ".." && !Tsonic.CSharp.Js.String.startsWith(rel, $"..{Tsonic.CSharp.Node.path.sep}"));
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
