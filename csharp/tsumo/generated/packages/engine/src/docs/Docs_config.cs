using System;

namespace Tsumo.Engine
{
    public static class Docs_config
    {
        public static Func<string, string, string, TsumoError> docsConfigError
        {
            get;
            private set;
        } = default(Func<string, string, string, TsumoError>)!;
        public static Action<JsonObject, string, string> assertUniqueProperties
        {
            get;
            private set;
        } = default(Action<JsonObject, string, string>)!;
        public static Func<JsonObject, string, string, string, string?> optionalString
        {
            get;
            private set;
        } = default(Func<JsonObject, string, string, string, string?>)!;
        public static Func<JsonObject, string, string, string, bool?> optionalBool
        {
            get;
            private set;
        } = default(Func<JsonObject, string, string, string, bool?>)!;
        public static Func<JsonObject, string, string, string, string> requiredString
        {
            get;
            private set;
        } = default(Func<JsonObject, string, string, string, string>)!;
        public static Action<JsonObject, Tsonic.CSharp.Js.JSArray<string>, string, string> rejectUnknownProperties
        {
            get;
            private set;
        } = default(Action<JsonObject, Tsonic.CSharp.Js.JSArray<string>, string, string>)!;
        public static Func<string, string> normalizePrefix
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string, string, string> resolveSourceDir
        {
            get;
            private set;
        } = default(Func<string, string, string, string>)!;
        public static Func<string, JsonValue, double, string, DocsMountConfig> parseMount
        {
            get;
            private set;
        } = default(Func<string, JsonValue, double, string, DocsMountConfig>)!;
        public static Func<string, JsonObject, string, Tsonic.CSharp.Js.JSArray<DocsMountConfig>> parseMounts
        {
            get;
            private set;
        } = default(Func<string, JsonObject, string, Tsonic.CSharp.Js.JSArray<DocsMountConfig>>)!;
        public static Func<string, LoadedDocsConfig?> loadDocsConfig
        {
            get;
            private set;
        } = default(Func<string, LoadedDocsConfig?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Utils_json.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            docsConfigError = (string code, string message, string path) => Diagnostics.createTsumoError(code, message, path);
            assertUniqueProperties = (JsonObject value, string context, string path) =>
            {
                Tsonic.CSharp.Js.Map<string, string> seen = new Tsonic.CSharp.Js.Map<string, string>();
                for (int index = 0; index < value.properties.length; index++)
                {
                    JsonProperty property = value.properties[index];
                    string key = Tsonic.CSharp.Js.String.toLowerCase(property.key);
                    string? previous = Tsonic.CSharp.Js.Map.getReference<string, string>(seen, key);
                    if (previous is not null)
                    {
                        throw docsConfigError("TSUMO_DOCS_CONFIG_DUPLICATE_PROPERTY", $"{context} contains duplicate properties '{previous}' and '{property.key}'", path);
                    }
                    seen.set(key, property.key);
                }
            };
            optionalString = (JsonObject root, string name, string context, string path) =>
            {
                JsonValue? value = root.getCaseInsensitive(name);
                if (value is null)
                {
                    return null;
                }
                if (!(value is JsonString))
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_TYPE", $"{context}.{name} must be a string", path);
                }
                return ((JsonString)value).value;
            };
            optionalBool = (JsonObject root, string name, string context, string path) =>
            {
                JsonValue? value = root.getCaseInsensitive(name);
                if (value is null)
                {
                    return null;
                }
                if (!(value is JsonBool))
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_TYPE", $"{context}.{name} must be a boolean", path);
                }
                return ((JsonBool)value).value;
            };
            requiredString = (JsonObject root, string name, string context, string path) =>
            {
                string? value = optionalString(root, name, context, path);
                if (value is null)
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_REQUIRED", $"{context}.{name} is required", path);
                }
                return value;
            };
            rejectUnknownProperties = (JsonObject root, Tsonic.CSharp.Js.JSArray<string> allowedNames, string context, string path) =>
            {
                Tsonic.CSharp.Js.Map<string, bool> allowed = new Tsonic.CSharp.Js.Map<string, bool>();
                for (int index = 0; index < allowedNames.length; index++)
                {
                    allowed.set(Tsonic.CSharp.Js.String.toLowerCase(allowedNames[index]), true);
                }
                for (int index_1 = 0; index_1 < root.properties.length; index_1++)
                {
                    string name = root.properties[index_1].key;
                    if (allowed.has(Tsonic.CSharp.Js.String.toLowerCase(name)))
                    {
                        continue;
                    }
                    throw docsConfigError("TSUMO_DOCS_CONFIG_UNKNOWN_PROPERTY", $"{context} contains unknown property '{name}'", path);
                }
            };
            normalizePrefix = (string raw) => Utils_text.ensureTrailingSlash(Utils_text.ensureLeadingSlash(Tsonic.CSharp.Js.String.trim(raw)));
            resolveSourceDir = (string siteDir, string raw, string path) =>
            {
                if (Tsonic.CSharp.Js.String.trim(raw) == "")
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_SOURCE_EMPTY", "Docs mount source cannot be empty", path);
                }
                return Tsonic.CSharp.Node.path.isAbsolute(raw) ? Tsonic.CSharp.Node.path.resolve(raw) : Tsonic.CSharp.Node.path.resolve(Tsonic.CSharp.Node.path.join(siteDir, raw));
            };
            parseMount = (string siteDir, JsonValue value, double index, string path) =>
            {
                string context = $"mounts[{index}]";
                if (!(value is JsonObject))
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_TYPE", $"{context} must be an object", path);
                }
                JsonObject @object = (JsonObject)value;
                assertUniqueProperties(@object, context, path);
                rejectUnknownProperties(@object, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "name", "source", "prefix", "repoUrl", "repoBranch", "repoPath", "navPath" }), context, path);
                string sourceDir = resolveSourceDir(siteDir, requiredString(@object, "source", context, path), path);
                string urlPrefix = normalizePrefix(requiredString(@object, "prefix", context, path));
                string? configuredName = optionalString(@object, "name", context, path);
                string fallbackName = urlPrefix == "/" ? "Docs" : Utils_strings.trimEndChar(Utils_strings.trimStartChar(urlPrefix, "/"), "/");
                string name = configuredName is null || Tsonic.CSharp.Js.String.trim(configuredName) == "" ? fallbackName : Tsonic.CSharp.Js.String.trim(configuredName);
                string? repoUrl = optionalString(@object, "repoUrl", context, path);
                string repoBranch = optionalString(@object, "repoBranch", context, path) ?? "main";
                string? repoPath = optionalString(@object, "repoPath", context, path);
                string? navPath = optionalString(@object, "navPath", context, path);
                return new DocsMountConfig(name, sourceDir, urlPrefix, repoUrl, repoBranch, repoPath, navPath);
            };
            parseMounts = (string siteDir, JsonObject root, string path) =>
            {
                JsonValue? value = root.getCaseInsensitive("mounts");
                if (!(value is JsonArray))
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_TYPE", "mounts must be an array", path);
                }
                JsonArray array = (JsonArray)value;
                if (array.items.length == 0)
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_REQUIRED", "mounts must contain at least one mount", path);
                }
                Tsonic.CSharp.Js.JSArray<DocsMountConfig> mounts = new Tsonic.CSharp.Js.JSArray<DocsMountConfig>(new DocsMountConfig[] { });
                Tsonic.CSharp.Js.Map<string, bool> names = new Tsonic.CSharp.Js.Map<string, bool>();
                Tsonic.CSharp.Js.Map<string, bool> prefixes = new Tsonic.CSharp.Js.Map<string, bool>();
                for (int index = 0; index < array.items.length; index++)
                {
                    DocsMountConfig mount = parseMount(siteDir, array.items[index], index, path);
                    string nameKey = Tsonic.CSharp.Js.String.toLowerCase(mount.name);
                    if (names.has(nameKey))
                    {
                        throw docsConfigError("TSUMO_DOCS_CONFIG_DUPLICATE_MOUNT", $"Duplicate docs mount name: {mount.name}", path);
                    }
                    string prefixKey = Tsonic.CSharp.Js.String.toLowerCase(mount.urlPrefix);
                    if (prefixes.has(prefixKey))
                    {
                        throw docsConfigError("TSUMO_DOCS_CONFIG_DUPLICATE_MOUNT", $"Duplicate docs mount prefix: {mount.urlPrefix}", path);
                    }
                    names.set(nameKey, true);
                    prefixes.set(prefixKey, true);
                    mounts.push(mount);
                }
                return mounts;
            };
            loadDocsConfig = (string siteDir) =>
            {
                string candidate = Tsonic.CSharp.Node.path.join(siteDir, "tsumo.docs.json");
                if (!Fs.fileExists(candidate))
                {
                    return null;
                }
                JsonObject? parsedRoot = Utils_json.jsonObject(Utils_json.parseJson(Fs.readTextFile(candidate), candidate));
                if (parsedRoot is null)
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_TYPE", "tsumo.docs.json root must be an object", candidate);
                }
                JsonObject root = parsedRoot;
                assertUniqueProperties(root, "tsumo.docs.json", candidate);
                rejectUnknownProperties(root, new Tsonic.CSharp.Js.JSArray<string>(new string[] { "siteName", "homeMount", "strictLinks", "search", "searchFile", "mounts" }), "tsumo.docs.json", candidate);
                Tsonic.CSharp.Js.JSArray<DocsMountConfig> mounts = parseMounts(siteDir, root, candidate);
                bool generateSearchIndex = optionalBool(root, "search", "tsumo.docs.json", candidate) ?? true;
                string searchIndexFileName = optionalString(root, "searchFile", "tsumo.docs.json", candidate) ?? "search.json";
                if (generateSearchIndex && Tsonic.CSharp.Js.String.trim(searchIndexFileName) == "")
                {
                    throw docsConfigError("TSUMO_DOCS_CONFIG_SEARCH_FILE_EMPTY", "searchFile cannot be empty when search is enabled", candidate);
                }
                DocsSiteConfig config = new DocsSiteConfig(mounts, optionalBool(root, "strictLinks", "tsumo.docs.json", candidate) ?? false, generateSearchIndex, searchIndexFileName, optionalString(root, "homeMount", "tsumo.docs.json", candidate), optionalString(root, "siteName", "tsumo.docs.json", candidate) ?? "Docs");
                return new LoadedDocsConfig(candidate, config);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class LoadedDocsConfig
    {
        public string path;
        public DocsSiteConfig config;
        public LoadedDocsConfig(string path, DocsSiteConfig config)
        {
            this.path = path;
            this.config = config;
        }
    }
}
