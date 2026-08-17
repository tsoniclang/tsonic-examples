using System;

namespace Tsumo.Engine
{
    public static class Scaffold_initSite
    {
        public static Action<string> ensureEmptyDir
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Func<string, string> defaultConfigToml
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string> defaultArchetype
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> baseofHtml
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> partialHeader
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> partialFooter
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> singleHtml
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> listHtml
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> termsHtml
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> taxonomyHtml
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<string> indexMd
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Func<Tsonic.CSharp.Js.Date, string> helloWorldMd
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Date, string>)!;
        public static Func<string> styleCss
        {
            get;
            private set;
        } = default(Func<string>)!;
        public static Action<string, Tsonic.CSharp.Js.Date?> initSite
        {
            get;
            private set;
        } = default(Action<string, Tsonic.CSharp.Js.Date?>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Diagnostics.__tsonic_module_init();
            ensureEmptyDir = (string path) =>
            {
                if (!Fs.dirExists(path))
                {
                    Fs.ensureDir(path);
                    return;
                }
                Fs.rejectFilesystemLink(path);
                if (Tsonic.CSharp.Node.fs.readdirSync(path).Length > 0)
                {
                    throw Diagnostics.createTsumoError("TSUMO_SCAFFOLD_DESTINATION_NOT_EMPTY", $"Directory not empty: {path}", path);
                }
            };
            defaultConfigToml = (string title) => $"baseURL = \"http://localhost:1313/\"\nlanguageCode = \"en-us\"\ntitle = \"{title}\"\n";
            defaultArchetype = () => "---\ntitle: \"{{ .Title }}\"\ndate: \"{{ .Date }}\"\ndraft: true\ndescription: \"\"\ntags: []\ncategories: []\n---\n\nWrite your post here.\n";
            baseofHtml = () => "<!doctype html>\n<html lang=\"{{ .Site.LanguageCode }}\">\n  <head>\n    <meta charset=\"utf-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\n    <title>{{ .Title }} | {{ .Site.Title }}</title>\n    <meta name=\"description\" content=\"{{ default .Site.Title .Description }}\" />\n    <link rel=\"stylesheet\" href=\"{{ relURL \"style.css\" }}\" />\n    <link rel=\"alternate\" type=\"application/rss+xml\" href=\"{{ relURL \"/index.xml\" }}\" title=\"{{ .Site.Title }}\" />\n  </head>\n  <body>\n    {{ partial \"header.html\" . }}\n    <main class=\"container\">\n      {{ block \"main\" . }}{{ end }}\n    </main>\n    {{ partial \"footer.html\" . }}\n  </body>\n</html>\n";
            partialHeader = () => "<header class=\"container\">\n  <h1><a href=\"{{ relURL \"/\" }}\">{{ .Site.Title }}</a></h1>\n  <nav>\n    <a href=\"{{ relURL \"/\" }}\">Home</a>\n    <a href=\"{{ relURL \"/posts/\" }}\">Posts</a>\n    <a href=\"{{ relURL \"/tags/\" }}\">Tags</a>\n    <a href=\"{{ relURL \"/categories/\" }}\">Categories</a>\n  </nav>\n</header>\n";
            partialFooter = () => "<footer class=\"container\">\n  <p class=\"muted\">Built with tsumo</p>\n</footer>\n";
            singleHtml = () => "{{ define \"main\" }}\n<article>\n  <h2>{{ .Title }}</h2>\n  <p class=\"muted\">\n    {{ dateFormat \"Jan 2, 2006\" .Date }}\n    {{ with .Categories }}\n      · Categories:\n      {{ range . }}\n        <a href=\"{{ . | urlize | printf \"/categories/%s/\" | relURL }}\">{{ . }}</a>\n      {{ end }}\n    {{ end }}\n    {{ with .Tags }}\n      · Tags:\n      {{ range . }}\n        <a href=\"{{ . | urlize | printf \"/tags/%s/\" | relURL }}\">{{ . }}</a>\n      {{ end }}\n    {{ end }}\n  </p>\n  <div class=\"content\">\n    {{ .Content }}\n  </div>\n</article>\n{{ end }}\n";
            listHtml = () => "{{ define \"main\" }}\n<section>\n  <h2>{{ .Title }}</h2>\n  <div class=\"content\">{{ .Content }}</div>\n  <ul class=\"post-list\">\n    {{ range .Pages }}\n      <li>\n        <div>\n          <a href=\"{{ .RelPermalink }}\">{{ .Title }}</a>\n          {{ with .Summary }}<div class=\"summary\">{{ . }}</div>{{ end }}\n        </div>\n        <span class=\"muted\">{{ dateFormat \"Jan 2, 2006\" .Date }}</span>\n      </li>\n    {{ end }}\n  </ul>\n</section>\n{{ end }}\n";
            termsHtml = () => "{{ define \"main\" }}\n<section>\n  <h2>{{ .Title }}</h2>\n  <ul class=\"post-list\">\n    {{ range .Pages }}\n      <li>\n        <a href=\"{{ .RelPermalink }}\">{{ .Title }}</a>\n        <span class=\"muted\">{{ len .Pages }}</span>\n      </li>\n    {{ end }}\n  </ul>\n</section>\n{{ end }}\n";
            taxonomyHtml = () => "{{ define \"main\" }}\n<section>\n  <h2>{{ .Title }}</h2>\n  <ul class=\"post-list\">\n    {{ range .Pages }}\n      <li>\n        <a href=\"{{ .RelPermalink }}\">{{ .Title }}</a>\n        <span class=\"muted\">{{ dateFormat \"Jan 2, 2006\" .Date }}</span>\n      </li>\n    {{ end }}\n  </ul>\n</section>\n{{ end }}\n";
            indexMd = () => "---\ntitle: \"Home\"\ndescription: \"Example site for tsumo.\"\n---\n\nWelcome to your new site.\n";
            helloWorldMd = (Tsonic.CSharp.Js.Date creationTime) => $"---\ntitle: \"Hello World\"\ndate: \"{creationTime.toISOString()}\"\ndraft: false\ndescription: \"An end-to-end demo of tsumo with GFM markdown.\"\ntags: [\"hello\", \"tsumo\", \"gfm\"]\ncategories: [\"meta\"]\n---\n\nThis is your first post.\n\n<!--more-->\n\n```\ntsumo build\ntsumo server\n```\n";
            styleCss = () => ":root { color-scheme: light dark; }\nbody { font-family: system-ui, -apple-system, Segoe UI, Roboto, sans-serif; margin: 0; line-height: 1.5; }\na { color: inherit; }\n.container { max-width: 860px; margin: 0 auto; padding: 1.25rem; }\n.muted { color: #777; }\nnav { display: flex; gap: 1rem; flex-wrap: wrap; }\n.post-list { list-style: none; padding: 0; }\n.post-list li { display: flex; justify-content: space-between; gap: 1rem; padding: 0.25rem 0; }\n.summary { margin-top: 0.25rem; }\n.summary p { margin: 0; }\n.content pre { padding: 0.75rem 1rem; background: rgba(127,127,127,0.15); overflow: auto; border-radius: 10px; }\n";
            initSite = (string targetDir, Tsonic.CSharp.Js.Date? creationTime) =>
            {
                string dir = Tsonic.CSharp.Node.path.resolve(targetDir);
                Tsonic.CSharp.Js.Date scaffoldTime = creationTime ?? new Tsonic.CSharp.Js.Date();
                ensureEmptyDir(dir);
                string @base = Tsonic.CSharp.Node.path.basename(dir);
                string title = Utils_text.humanizeSlug(@base == "" ? "Tsumo Site" : @base);
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "content"));
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "content", "posts"));
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default"));
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "layouts", "partials"));
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "static"));
                Fs.ensureDir(Tsonic.CSharp.Node.path.join(dir, "archetypes"));
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "hugo.toml"), defaultConfigToml(title));
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "archetypes", "default.md"), defaultArchetype());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default", "baseof.html"), baseofHtml());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default", "single.html"), singleHtml());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default", "list.html"), listHtml());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default", "terms.html"), termsHtml());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "_default", "taxonomy.html"), taxonomyHtml());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "partials", "header.html"), partialHeader());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "layouts", "partials", "footer.html"), partialFooter());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "static", "style.css"), styleCss());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "content", "_index.md"), indexMd());
                Fs.writeTextFile(Tsonic.CSharp.Node.path.join(dir, "content", "posts", "hello-world.md"), helloWorldMd(scaffoldTime));
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
