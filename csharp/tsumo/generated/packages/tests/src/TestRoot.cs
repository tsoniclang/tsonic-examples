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
                string? configuredRoot = System.Environment.GetEnvironmentVariable("TSUMO_TEST_ROOT");
                if (configuredRoot is null || Tsonic.CSharp.Js.String.trim(configuredRoot) == "")
                {
                    throw new System.Exception("TSUMO_TEST_ROOT must name the test-owned scratch directory");
                }
                string root = System.IO.Path.GetFullPath(configuredRoot);
                System.IO.Directory.CreateDirectory(root);
                string testDirectory = System.IO.Path.Combine(root, $"{name}-{System.Guid.NewGuid().ToString("n")}");
                System.IO.Directory.CreateDirectory(testDirectory);
                return testDirectory;
            };
            deleteTestDirectory = (string path) =>
            {
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
