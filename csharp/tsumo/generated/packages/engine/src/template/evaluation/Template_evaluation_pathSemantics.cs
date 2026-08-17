using System;

namespace Tsumo.Engine
{
    public static class Template_evaluation_pathSemantics
    {
        public static Func<string, string> normalizeRelPath
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, string, bool> segmentMatch
        {
            get;
            private set;
        } = default(Func<string, string, bool>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<string>> splitGlobSegments
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<string>, Tsonic.CSharp.Js.JSArray<string>, int, int, bool> globMatchAt
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<string>, Tsonic.CSharp.Js.JSArray<string>, int, int, bool>)!;
        public static Func<string, string, bool> globMatch
        {
            get;
            private set;
        } = default(Func<string, string, bool>)!;
        public static Func<PageContext, string, string> resolvePageRef
        {
            get;
            private set;
        } = default(Func<PageContext, string, string>)!;
        public static Func<SiteContext, string, PageContext?> tryGetPage
        {
            get;
            private set;
        } = default(Func<SiteContext, string, PageContext?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Template_evaluation_serialization.__tsonic_module_init();
            normalizeRelPath = (string raw) =>
            {
                string normalized = Utils_strings.replaceText(raw, "\\", "/");
                Tsonic.CSharp.Js.JSArray<string> parts = Tsonic.CSharp.Js.String.split(normalized, "/");
                Tsonic.CSharp.Js.JSArray<string> outParts = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int i = 0; i < parts.length; i++)
                {
                    string p = Tsonic.CSharp.Js.String.trim(parts[i]);
                    if (p == "" || p == ".")
                    {
                        continue;
                    }
                    if (p == "..")
                    {
                        if (outParts.length > 0)
                        {
                            Tsonic.CSharp.Js.Array.popReference(outParts);
                        }
                        continue;
                    }
                    outParts.push(p);
                }
                Tsonic.CSharp.Js.JSArray<string> arr = outParts;
                string @out = "";
                for (int i_1 = 0; i_1 < arr.length; i_1++)
                {
                    @out = @out == "" ? arr[i_1] : @out + "/" + arr[i_1];
                }
                return @out;
            };
            segmentMatch = (string pattern, string segment) =>
            {
                if (pattern == "*")
                {
                    return true;
                }
                int star = Tsonic.CSharp.Js.String.indexOf(pattern, "*");
                if (star < 0)
                {
                    return pattern == segment;
                }
                Tsonic.CSharp.Js.JSArray<string> parts = Tsonic.CSharp.Js.String.split(pattern, "*");
                int pos = 0;
                for (int i = 0; i < parts.length; i++)
                {
                    string p = parts[i];
                    if (p == "")
                    {
                        continue;
                    }
                    int idx = Tsonic.CSharp.Js.String.indexOf(segment, p, pos);
                    if (idx < 0)
                    {
                        return false;
                    }
                    if (i == 0 && !Tsonic.CSharp.Js.String.startsWith(pattern, "*") && idx != 0)
                    {
                        return false;
                    }
                    pos = idx + p.Length;
                }
                if (!Tsonic.CSharp.Js.String.endsWith(pattern, "*") && pos != segment.Length)
                {
                    return false;
                }
                return true;
            };
            splitGlobSegments = (string raw) =>
            {
                string slash = "/";
                string normalized = Template_evaluation_serialization.trimStartCharacter(Utils_strings.replaceText(Tsonic.CSharp.Js.String.trim(raw), "\\", "/"), slash);
                if (normalized == "")
                {
                    Tsonic.CSharp.Js.JSArray<string> empty = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                    return empty;
                }
                return Tsonic.CSharp.Js.String.split(normalized, "/");
            };
            globMatchAt = (Tsonic.CSharp.Js.JSArray<string> patSegs, Tsonic.CSharp.Js.JSArray<string> pathSegs, int pi, int si) =>
            {
                if (pi >= patSegs.length)
                {
                    return si >= pathSegs.length;
                }
                string p = patSegs[pi];
                if (p == "**")
                {
                    for (int i = si; i <= pathSegs.length; i++)
                    {
                        if (globMatchAt(patSegs, pathSegs, pi + 1, i))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                if (si >= pathSegs.length)
                {
                    return false;
                }
                if (!segmentMatch(p, pathSegs[si]))
                {
                    return false;
                }
                return globMatchAt(patSegs, pathSegs, pi + 1, si + 1);
            };
            globMatch = (string patternRaw, string pathRaw) =>
            {
                Tsonic.CSharp.Js.JSArray<string> patSegs = splitGlobSegments(patternRaw);
                Tsonic.CSharp.Js.JSArray<string> pathSegs = splitGlobSegments(pathRaw);
                return globMatchAt(patSegs, pathSegs, 0, 0);
            };
            resolvePageRef = (PageContext page, string @ref) =>
            {
                string raw = Tsonic.CSharp.Js.String.trim(@ref);
                if (raw == "" || raw == "/")
                {
                    return "";
                }
                if (Tsonic.CSharp.Js.String.startsWith(raw, "/"))
                {
                    return Template_evaluation_serialization.trimSlashes(raw);
                }
                PageFile? pageFile = page.File;
                string @base = pageFile is not null ? pageFile.Dir : Template_evaluation_serialization.trimSlashes(page.relPermalink);
                string combined = @base == "" ? raw : Template_evaluation_serialization.trimEndCharacter(@base, "/") + "/" + Template_evaluation_serialization.trimStartCharacter(raw, "/");
                return normalizeRelPath(combined);
            };
            tryGetPage = (SiteContext site, string pathRaw) =>
            {
                string trimmed = Tsonic.CSharp.Js.String.trim(pathRaw);
                if (trimmed == "" || trimmed == "/")
                {
                    return site.home;
                }
                string needle = Template_evaluation_serialization.trimSlashes(trimmed);
                if (needle == "")
                {
                    return site.home;
                }
                Tsonic.CSharp.Js.JSArray<PageContext> candidates = site.pages;
                if (site.allPages.length > 0)
                {
                    candidates = site.allPages;
                }
                for (int i = 0; i < candidates.length; i++)
                {
                    PageContext p = candidates[i];
                    if (Template_evaluation_serialization.trimSlashes(p.relPermalink) == needle)
                    {
                        return p;
                    }
                    if (p.slug == needle)
                    {
                        return p;
                    }
                }
                return null;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
