using System;

namespace Tsumo.Engine
{
    public static class Scaffold_newContent
    {
        public static Func<string> defaultArchetype
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string, string, Tsonic.CSharp.Js.Date?, string> newContent
        {
            get;
            private set;
        } = default(Func<string, string, Tsonic.CSharp.Js.Date?, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Utils_paths.__tsonic_module_init();
            defaultArchetype = () => """
            ---
            title: "{{ .Title }}"
            date: "{{ .Date }}"
            draft: true
            description: ""
            tags: []
            categories: []
            ---

            Write your post here.

            """;
            newContent = (string siteDir, string contentPathRaw, Tsonic.CSharp.Js.Date? creationTime) =>
            {
                string dir = Tsonic.CSharp.Node.path.resolve(siteDir);
                string contentDir = Tsonic.CSharp.Node.path.resolve(dir, "content");
                string rel = Utils_strings.replaceText(Tsonic.CSharp.Js.String.trim(contentPathRaw), "\\", "/");
                if (rel == "" || Tsonic.CSharp.Node.path.isAbsolute(rel))
                {
                    throw Diagnostics.createTsumoError("TSUMO_SCAFFOLD_CONTENT_PATH_INVALID", $"Content path must be relative to the site's content directory: {contentPathRaw}");
                }
                string withExt = Tsonic.CSharp.Js.String.endsWith(Tsonic.CSharp.Js.String.toLowerCase(rel), ".md") ? rel : rel + ".md";
                string dest = Tsonic.CSharp.Node.path.resolve(contentDir, withExt);
                if (!Utils_paths.pathContainsOrEquals(contentDir, dest) || dest == contentDir)
                {
                    throw Diagnostics.createTsumoError("TSUMO_SCAFFOLD_CONTENT_PATH_ESCAPES_ROOT", $"Content path escapes the site's content directory: {contentPathRaw}", dest);
                }
                if (Fs.fileExists(dest))
                {
                    throw Diagnostics.createTsumoError("TSUMO_SCAFFOLD_CONTENT_EXISTS", $"File already exists: {dest}", dest);
                }
                string archetypePath = Tsonic.CSharp.Node.path.join(dir, "archetypes", "default.md");
                string template = Fs.fileExists(archetypePath) ? Fs.readTextFile(archetypePath) : defaultArchetype();
                string baseName = Tsonic.CSharp.Node.path.basename(withExt);
                string fileName = baseName != "" ? baseName : withExt;
                string slug = Utils_text.slugify(Tsonic.CSharp.Js.String.endsWith(Tsonic.CSharp.Js.String.toLowerCase(fileName), ".md") ? Utils_strings.substringCount(fileName, 0, fileName.Length - 3) : fileName);
                string title = Utils_text.humanizeSlug(slug);
                string date = (creationTime ?? new Tsonic.CSharp.Js.Date()).toISOString();
                string content = template;
                content = Utils_strings.replaceText(content, "{{ .Title }}", title);
                content = Utils_strings.replaceText(content, "{{ .Date }}", date);
                Fs.writeTextFile(dest, content);
                return dest;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
