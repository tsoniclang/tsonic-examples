namespace Tsumo.Engine
{
    public static class Build_siteRoutes
    {
        public static string normalizeSitePath(string path)
        {
            return Tsonic.CSharp.Js.String.replaceAll(path, "\\", "/");
        }
        public static Tsonic.CSharp.Js.JSArray<string> splitSitePath(string path)
        {
            return Tsonic.CSharp.Js.String.split(normalizeSitePath(path), "/");
        }
        public static string joinSitePath(Tsonic.CSharp.Js.JSArray<string> segments)
        {
            return Tsonic.CSharp.Js.Array.join(segments, "/");
        }
        public static string withoutMarkdownExtension(string fileName)
        {
            return Tsonic.CSharp.Js.String.endsWith(Tsonic.CSharp.Js.String.toLowerCase(fileName), ".md") ? Utils_strings.substringCount(fileName, 0, fileName.Length - 3) : fileName;
        }
        public static string siteOutputPath(Tsonic.CSharp.Js.JSArray<string> routeSegments)
        {
            return routeSegments.length == 0 ? "index.html" : joinSitePath(routeSegments) + "/index.html";
        }
        public static void assertSiteRouteSegment(string segment, string sourcePath)
        {
            if (segment == "" || segment == "." || segment == ".." || Tsonic.CSharp.Js.String.includes(segment, "/") || Tsonic.CSharp.Js.String.includes(segment, "\\") || Tsonic.CSharp.Js.String.includes(segment, ":"))
            {
                throw Diagnostics.createTsumoError("TSUMO_CONTENT_ROUTE_SEGMENT_INVALID", $"Content route segment is invalid: {segment}", sourcePath);
            }
        }
        public static double compareSitePaths(string left, string right)
        {
            return Utils_strings.compareText(normalizeSitePath(left), normalizeSitePath(right));
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_strings.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
