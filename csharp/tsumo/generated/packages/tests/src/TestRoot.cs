using System;

namespace Tsumo.Tests
{
    public static class TestRoot
    {
        public static Func<string, string> createTestDirectory
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Action<string> createDirectory
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Action<string, string> writeTextFile
        {
            get;
            private set;
        } = default(Action<string, string>)!;
        public static Func<string, string> readTextFile
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, bool> pathExists
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> directoryExists
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> fileExists
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Action<string, string> createSymbolicLink
        {
            get;
            private set;
        } = default(Action<string, string>)!;
        public static Action<string> deleteTestDirectory
        {
            get;
            private set;
        } = default(Action<string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            createTestDirectory = (string name) =>
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
            };
            createDirectory = (string path) =>
            {
                Tsonic.CSharp.Node.fs.mkdirSync(path, new Tsonic.CSharp.Node.MakeDirectoryOptions
                {
                    recursive = true,
                });
            };
            writeTextFile = (string path, string content) =>
            {
                Tsonic.CSharp.Node.fs.writeFileSync(path, content, "utf8");
            };
            readTextFile = (string path) => Tsonic.CSharp.Node.fs.readFileSync(path, "utf8");
            pathExists = (string path) => Tsonic.CSharp.Node.fs.existsSync(path);
            directoryExists = (string path) => Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsDirectory();
            fileExists = (string path) => Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsFile();
            createSymbolicLink = (string target, string path) =>
            {
                Tsonic.CSharp.Node.fs.symlinkSync(target, path);
            };
            deleteTestDirectory = (string path) =>
            {
                Tsonic.CSharp.Node.fs.rmSync(path, new Tsonic.CSharp.Node.RmOptions
                {
                    recursive = true,
                    force = true,
                });
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
