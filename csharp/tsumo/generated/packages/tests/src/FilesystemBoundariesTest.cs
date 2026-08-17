using System;

namespace Tsumo.Tests
{
    public static class FilesystemBoundariesTest
    {
        public static Func<Action, TsumoError> captureTsumoError
        {
            get;
            private set;
        } = default(Func<Action, TsumoError>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureTsumoError = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception error)
                {
                    if (error is TsumoError)
                    {
                        return (TsumoError)error;
                    }
                    throw;
                }
                throw new System.Exception("Expected a Tsumo error");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class FilesystemBoundaryTests
    {
        [Xunit.FactAttribute]
        public void recursive_discovery_is_sorted_and_rejects_links()
        {
            string root = TestRoot.createTestDirectory("filesystem-discovery");
            try
            {
                string source = System.IO.Path.Combine(root, "source");
                string nested = System.IO.Path.Combine(source, "a");
                string outside = System.IO.Path.Combine(root, "outside");
                System.IO.Directory.CreateDirectory(nested);
                System.IO.Directory.CreateDirectory(outside);
                System.IO.File.WriteAllText(System.IO.Path.Combine(source, "z.txt"), "z");
                System.IO.File.WriteAllText(System.IO.Path.Combine(nested, "b.txt"), "b");
                System.IO.File.WriteAllText(System.IO.Path.Combine(nested, "a.txt"), "a");
                System.IO.File.WriteAllText(System.IO.Path.Combine(outside, "outside.txt"), "outside");
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { System.IO.Path.Combine(nested, "a.txt"), System.IO.Path.Combine(nested, "b.txt"), System.IO.Path.Combine(source, "z.txt") }), Node_modules_Tsumo_engine_src_fs.listFilesRecursive(source, "*.txt"));
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { System.IO.Path.Combine(source, "z.txt") }), Node_modules_Tsumo_engine_src_fs.listFilesTopDirectory(source, "*.txt"));
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { nested }), Node_modules_Tsumo_engine_src_fs.listDirectoriesTopDirectory(source));
                string link = System.IO.Path.Combine(source, "linked-directory");
                System.IO.Directory.CreateSymbolicLink(link, outside);
                TsumoError error = FilesystemBoundariesTest.captureTsumoError(() =>
                {
                    Node_modules_Tsumo_engine_src_fs.listFilesRecursive(source, "*");
                });
                Xunit.Assert.Equal("TSUMO_FILESYSTEM_LINK_UNSUPPORTED", error.diagnostic.code);
                Xunit.Assert.Equal(link, error.diagnostic.file);
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void watch_snapshots_detect_file_changes_and_use_link_policy()
        {
            string root = TestRoot.createTestDirectory("watch-snapshot");
            try
            {
                string watched = System.IO.Path.Combine(root, "watched");
                System.IO.Directory.CreateDirectory(watched);
                string file = System.IO.Path.Combine(watched, "page.md");
                System.IO.File.WriteAllText(file, "before");
                Tsonic.CSharp.Js.Map<string, WatchEntryState> initial = Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }));
                Xunit.Assert.True(Node_modules_Tsumo_engine_src_watchSnapshot.watchSnapshotsEqual(initial, Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }))));
                System.IO.File.WriteAllText(file, "after with a different size");
                Xunit.Assert.False(Node_modules_Tsumo_engine_src_watchSnapshot.watchSnapshotsEqual(initial, Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }))));
                string link = System.IO.Path.Combine(watched, "linked-file.md");
                System.IO.File.CreateSymbolicLink(link, file);
                Xunit.Assert.Equal("TSUMO_FILESYSTEM_LINK_UNSUPPORTED", FilesystemBoundariesTest.captureTsumoError(() =>
                {
                    Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }));
                }).diagnostic.code);
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
