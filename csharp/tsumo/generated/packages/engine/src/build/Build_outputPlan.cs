using System;

namespace Tsumo.Engine
{
    public static class Build_outputPlan
    {
        public static Func<string, string> normalizeOutputPath
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string, string> combineOutputPath
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        public static Func<string, string, string> resolveOutputPath
        {
            get;
            private set;
        } = default(Func<string, string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Utils_paths.__tsonic_module_init();
            Build_siteRoutes.__tsonic_module_init();
            normalizeOutputPath = (string relativePath) =>
            {
                string normalized = Build_siteRoutes.normalizeSitePath(relativePath);
                if (normalized == "" || Tsonic.CSharp.Js.String.startsWith(normalized, "/") || Tsonic.CSharp.Node.path.isAbsolute(normalized) || (normalized.Length >= 2 && normalized.Substring(1, 1) == ":"))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PATH_ABSOLUTE", $"Site output path must be relative: {relativePath}");
                }
                Tsonic.CSharp.Js.JSArray<string> segments = Build_siteRoutes.splitSitePath(normalized);
                for (int index = 0; index < segments.length; index++)
                {
                    string segment = segments[index];
                    if (segment == "" || segment == "." || segment == "..")
                    {
                        throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PATH_ESCAPES_ROOT", $"Site output path is not canonical: {relativePath}");
                    }
                }
                return Build_siteRoutes.joinSitePath(segments);
            };
            combineOutputPath = (string prefix, string relativePath) =>
            {
                string normalizedRelativePath = normalizeOutputPath(relativePath);
                if (Tsonic.CSharp.Js.String.trim(prefix) == "")
                {
                    return normalizedRelativePath;
                }
                return normalizeOutputPath(Build_siteRoutes.normalizeSitePath(prefix) + "/" + normalizedRelativePath);
            };
            resolveOutputPath = (string outputRoot, string relativePath) =>
            {
                string root = Tsonic.CSharp.Node.path.resolve(outputRoot);
                string candidate = Tsonic.CSharp.Node.path.resolve(root, normalizeOutputPath(relativePath));
                if (!Utils_paths.pathContainsOrEquals(root, candidate))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PATH_ESCAPES_ROOT", $"Site output path escapes its root: {relativePath}");
                }
                return candidate;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class OutputClaim
    {
        public string relativePath;
        public string owner;
        public string? assetLayer;
        public OutputClaim(string relativePath, string owner, string? assetLayer)
        {
            this.relativePath = relativePath;
            this.owner = owner;
            this.assetLayer = assetLayer;
        }
    }
    public class FileSiteOutput
    {
        public string sourcePath;
        public FileSiteOutput(string sourcePath)
        {
            this.sourcePath = sourcePath;
        }
    }
    public class SiteOutputPlan
    {
        public Tsonic.CSharp.Js.Map<string, OutputClaim> claimsByPath;
        public Tsonic.CSharp.Js.Map<string, string> textByPath;
        public Tsonic.CSharp.Js.Map<string, FileSiteOutput> filesByPath;
        public SiteOutputPlan()
        {
            this.claimsByPath = new Tsonic.CSharp.Js.Map<string, OutputClaim>();
            this.textByPath = new Tsonic.CSharp.Js.Map<string, string>();
            this.filesByPath = new Tsonic.CSharp.Js.Map<string, FileSiteOutput>();
        }
        public void addText(string relativePath, string content, string owner)
        {
            string outputPath = Build_outputPlan.normalizeOutputPath(relativePath);
            string key = Tsonic.CSharp.Js.String.toLowerCase(outputPath);
            OutputClaim? previous = Tsonic.CSharp.Js.Map.getReference<string, OutputClaim>(this.claimsByPath, key);
            if (previous is not null)
            {
                this.throwConflict(outputPath, owner, previous);
            }
            this.claimsByPath.set(key, new OutputClaim(outputPath, owner, null));
            this.textByPath.set(key, content);
        }
        public void addDefaultText(string relativePath, string content, string owner)
        {
            string outputPath = Build_outputPlan.normalizeOutputPath(relativePath);
            OutputClaim? previous = Tsonic.CSharp.Js.Map.getReference<string, OutputClaim>(this.claimsByPath, Tsonic.CSharp.Js.String.toLowerCase(outputPath));
            if (previous is null)
            {
                this.addText(outputPath, content, owner);
                return;
            }
            if (previous.assetLayer == "theme-static" || previous.assetLayer == "site-static")
            {
                return;
            }
            this.throwConflict(outputPath, owner, previous);
        }
        public void addAsset(string relativePath, string sourcePath, string owner, string layer)
        {
            string outputPath = Build_outputPlan.normalizeOutputPath(relativePath);
            string key = Tsonic.CSharp.Js.String.toLowerCase(outputPath);
            OutputClaim? previous = Tsonic.CSharp.Js.Map.getReference<string, OutputClaim>(this.claimsByPath, key);
            if (previous is null)
            {
                this.claimsByPath.set(key, new OutputClaim(outputPath, owner, layer));
                this.filesByPath.set(key, new FileSiteOutput(sourcePath));
                return;
            }
            if (previous.assetLayer == "theme-static" && layer == "site-static")
            {
                this.claimsByPath.set(key, new OutputClaim(outputPath, owner, layer));
                this.filesByPath.set(key, new FileSiteOutput(sourcePath));
                return;
            }
            this.throwConflict(outputPath, owner, previous);
        }
        public void addDirectory(string sourceRoot, string outputPrefix, string owner, string layer)
        {
            Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesRecursive(sourceRoot, "*");
            files.sort((string left, string right) => Build_siteRoutes.compareSitePaths(left, right));
            for (int index = 0; index < files.length; index++)
            {
                string sourcePath = files[index];
                string relativePath = Build_siteRoutes.normalizeSitePath(Tsonic.CSharp.Node.path.relative(sourceRoot, sourcePath));
                this.addAsset(Build_outputPlan.combineOutputPath(outputPrefix, relativePath), sourcePath, owner, layer);
            }
        }
        public int generatedOutputCount()
        {
            int count = 0;
            foreach (string unused in this.textByPath.values())
            {
                count++;
            }
            return count;
        }
        public void applyDeferredTemplateResults(Tsonic.CSharp.Js.Map<string, string> results)
        {
            if (results.size == 0)
            {
                return;
            }
            Tsonic.CSharp.Js.Set<string> resolvedPlacements = new Tsonic.CSharp.Js.Set<string>();
            Tsonic.CSharp.Js.JSArray<string> outputPaths = Tsonic.CSharp.Js.JSArrayStatics.from<string>(this.textByPath.keys());
            for (int outputIndex = 0; outputIndex < outputPaths.length; outputIndex++)
            {
                string key = outputPaths[outputIndex];
                string? content = Tsonic.CSharp.Js.Map.getReference<string, string>(this.textByPath, key);
                if (content is null)
                {
                    continue;
                }
                foreach (string token in results.keys())
                {
                    string? replacement = Tsonic.CSharp.Js.Map.getReference<string, string>(results, token);
                    if (replacement is null)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_RESULT_INVALID", "A deferred-template replacement disappeared");
                    }
                    int first = Tsonic.CSharp.Js.String.indexOf(content, token);
                    if (first >= 0)
                    {
                        if (resolvedPlacements.has(token) || Tsonic.CSharp.Js.String.indexOf(content, token, first + token.Length) >= 0)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_PLACEMENT_INVALID", "Each deferred-template placement must occur exactly once in planned output");
                        }
                        resolvedPlacements.add(token);
                        content = Tsonic.CSharp.Js.String.replaceAll(content, token, replacement);
                    }
                }
                this.textByPath.set(key, content);
            }
            foreach (string token_1 in results.keys())
            {
                if (!resolvedPlacements.has(token_1))
                {
                    throw Diagnostics.createTsumoError("TSUMO_TEMPLATE_DEFER_PLACEMENT_INVALID", "Each deferred-template placement must occur exactly once in planned output");
                }
            }
        }
        public void render(string outputRoot)
        {
            Tsonic.CSharp.Js.JSArray<string> keys = Tsonic.CSharp.Js.JSArrayStatics.from<string>(this.claimsByPath.keys());
            keys.sort((string left, string right) => Build_siteRoutes.compareSitePaths(left, right));
            for (int index = 0; index < keys.length; index++)
            {
                string key = keys[index];
                OutputClaim? claim = Tsonic.CSharp.Js.Map.getReference<string, OutputClaim>(this.claimsByPath, key);
                if (claim is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PLAN_INCONSISTENT", $"Output claim '{key}' disappeared before rendering");
                }
                string destination = Build_outputPlan.resolveOutputPath(outputRoot, claim.relativePath);
                string? text = Tsonic.CSharp.Js.Map.getReference<string, string>(this.textByPath, key);
                if (text is not null)
                {
                    Fs.writeTextFile(destination, text);
                    continue;
                }
                FileSiteOutput? file = Tsonic.CSharp.Js.Map.getReference<string, FileSiteOutput>(this.filesByPath, key);
                if (file is null)
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PLAN_INCONSISTENT", $"Output claim '{key}' has no planned content");
                }
                Fs.ensureDir(Tsonic.CSharp.Node.path.dirname(destination));
                Tsonic.CSharp.Node.fs.copyFileSync(file.sourcePath, destination);
            }
        }
        public void throwConflict(string relativePath, string owner, OutputClaim previous)
        {
            throw Diagnostics.createTsumoError("TSUMO_OUTPUT_PATH_CONFLICT", $"Output '{relativePath}' is claimed by both '{previous.owner}' and '{owner}'");
        }
    }
}
