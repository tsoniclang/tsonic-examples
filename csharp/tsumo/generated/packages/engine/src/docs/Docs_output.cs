namespace Tsumo.Engine
{
    public static class Docs_output
    {
        public static string resolveDocsOutputPath(string outputRoot, string relativePath)
        {
            string normalized = Tsonic.CSharp.Js.String.replaceAll(relativePath, "\\", "/");
            if (Tsonic.CSharp.Js.String.startsWith(normalized, "/") || (normalized.Length >= 2 && normalized.Substring(1, 1) == ":"))
            {
                throw Diagnostics.createTsumoError("TSUMO_DOCS_OUTPUT_PATH_ABSOLUTE", $"Docs output path must be relative: {relativePath}");
            }
            string root = Tsonic.CSharp.Node.path.resolve(outputRoot);
            string candidate = Tsonic.CSharp.Node.path.resolve(root, normalized);
            if (!Utils_paths.pathContainsOrEquals(root, candidate))
            {
                throw Diagnostics.createTsumoError("TSUMO_DOCS_OUTPUT_PATH_ESCAPES_ROOT", $"Docs output path escapes its root: {relativePath}");
            }
            return candidate;
        }
        public static string docsOutputPathForPermalink(string permalink)
        {
            string normalized = Tsonic.CSharp.Js.String.replaceAll(permalink, "\\", "/");
            string trimmed = Tsonic.CSharp.Js.String.startsWith(normalized, "/") ? Tsonic.CSharp.Js.String.substring(normalized, 1) : normalized;
            string withoutTrailingSlash = Tsonic.CSharp.Js.String.endsWith(trimmed, "/") ? Tsonic.CSharp.Js.String.substring(trimmed, 0, trimmed.Length - 1) : trimmed;
            return withoutTrailingSlash == "" ? "index.html" : withoutTrailingSlash + "/index.html";
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Utils_paths.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class DocsOutputClaims
    {
        public Tsonic.CSharp.Js.Map<string, string> sourcesByOutputPath;
        public DocsOutputClaims()
        {
            this.sourcesByOutputPath = new Tsonic.CSharp.Js.Map<string, string>();
        }
        public void add(string outputRelPath, string sourcePath)
        {
            string key = Tsonic.CSharp.Js.String.toLowerCase(outputRelPath);
            string? previous = Tsonic.CSharp.Js.Map.getOptional<string, string>(this.sourcesByOutputPath, key);
            if (previous is not null)
            {
                throw Diagnostics.createTsumoError("TSUMO_DOCS_ROUTE_CONFLICT", $"Docs sources '{previous}' and '{sourcePath}' both map to '{outputRelPath}'");
            }
            this.sourcesByOutputPath.set(key, sourcePath);
        }
    }
}
