using System;

namespace Tsumo.Engine
{
    public static class Resources_pageBundle
    {
        public static Action<Tsonic.CSharp.Js.JSArray<string>> sortPaths
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Func<string, bool> isNestedBundle
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Action<string, string, Tsonic.CSharp.Js.JSArray<PageBundleResourceFile>> collectPageBundleResourceFiles
        {
            get;
            private set;
        } = default(Action<string, string, Tsonic.CSharp.Js.JSArray<PageBundleResourceFile>>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<PageBundleResourceFile>> discoverPageBundleResourceFiles
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<PageBundleResourceFile>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            sortPaths = (Tsonic.CSharp.Js.JSArray<string> paths) =>
            {
                paths.sort((string left, string right) => Utils_strings.compareText(left, right));
            };
            isNestedBundle = (string directory) => Fs.fileExists(Tsonic.CSharp.Node.path.join(directory, "index.md")) || Fs.fileExists(Tsonic.CSharp.Node.path.join(directory, "_index.md"));
            collectPageBundleResourceFiles = (string directory, string prefix, Tsonic.CSharp.Js.JSArray<PageBundleResourceFile> result) =>
            {
                Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesTopDirectory(directory, "*");
                sortPaths(files);
                for (int index = 0; index < files.length; index++)
                {
                    string sourcePath = files[index];
                    if (Tsonic.CSharp.Js.String.endsWith(Tsonic.CSharp.Js.String.toLowerCase(sourcePath), ".md"))
                    {
                        continue;
                    }
                    string name = Tsonic.CSharp.Node.path.basename(sourcePath);
                    if (name == "")
                    {
                        continue;
                    }
                    string relativePath = prefix == "" ? name : $"{prefix}/{name}";
                    result.push(new PageBundleResourceFile(sourcePath, relativePath));
                }
                Tsonic.CSharp.Js.JSArray<string> directories = Fs.listDirectoriesTopDirectory(directory);
                sortPaths(directories);
                for (int index_1 = 0; index_1 < directories.length; index_1++)
                {
                    string child = directories[index_1];
                    if (isNestedBundle(child) || Fs.listFilesTopDirectory(child, "*.md").length > 0)
                    {
                        continue;
                    }
                    string name_1 = Tsonic.CSharp.Node.path.basename(child);
                    if (name_1 == "")
                    {
                        continue;
                    }
                    collectPageBundleResourceFiles(child, prefix == "" ? name_1 : $"{prefix}/{name_1}", result);
                }
            };
            discoverPageBundleResourceFiles = (string directory) =>
            {
                Tsonic.CSharp.Js.JSArray<PageBundleResourceFile> result = new Tsonic.CSharp.Js.JSArray<PageBundleResourceFile>(new PageBundleResourceFile[] { });
                collectPageBundleResourceFiles(directory, "", result);
                return result;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class PageBundleResourceFile
    {
        public string sourcePath;
        public string relativePath;
        public PageBundleResourceFile(string sourcePath, string relativePath)
        {
            this.sourcePath = sourcePath;
            this.relativePath = relativePath;
        }
    }
}
