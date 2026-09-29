namespace Tsumo.Tests
{
    public static class TestRoot
    {
        public static string createTestDirectory(string name)
        {
            string? configuredRoot = Tsonic.CSharp.Node.process.env["TSUMO_TEST_ROOT"];
            if (configuredRoot is null || Tsonic.CSharp.Js.String.trim(configuredRoot) == "")
            {
                throw new Tsonic.CSharp.Runtime.Error("TSUMO_TEST_ROOT must name the test-owned scratch directory");
            }
            string root = Tsonic.CSharp.Node.path.resolve(configuredRoot);
            Tsonic.CSharp.Node.fs.mkdirSync(root, new Tsonic.CSharp.Node.MakeDirectoryOptions
            {
                recursive = true,
            });
            return Tsonic.CSharp.Node.fs.mkdtempSync(Tsonic.CSharp.Node.path.join(root, $"{name}-"));
        }
        public static void createDirectory(string path)
        {
            Tsonic.CSharp.Node.fs.mkdirSync(path, new Tsonic.CSharp.Node.MakeDirectoryOptions
            {
                recursive = true,
            });
        }
        public static void writeTextFile(string path, string content)
        {
            Tsonic.CSharp.Node.fs.writeFileSync(path, content, "utf8");
        }
        public static string readTextFile(string path)
        {
            return Tsonic.CSharp.Node.fs.readFileSync(path, "utf8");
        }
        public static bool pathExists(string path)
        {
            return Tsonic.CSharp.Node.fs.existsSync(path);
        }
        public static bool directoryExists(string path)
        {
            return Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsDirectory();
        }
        public static bool fileExists(string path)
        {
            return Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsFile();
        }
        public static void createSymbolicLink(string target, string path)
        {
            Tsonic.CSharp.Node.fs.symlinkSync(target, path);
        }
        public static void deleteTestDirectory(string path)
        {
            Tsonic.CSharp.Node.fs.rmSync(path, new Tsonic.CSharp.Node.RmOptions
            {
                recursive = true,
                force = true,
            });
        }
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
}
