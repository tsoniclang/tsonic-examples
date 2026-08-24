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
                catch (System.Exception __tsonic_catch0)
                {
                    Tsonic.CSharp.Runtime.TsValue error = Tsonic.CSharp.Runtime.TsThrownValueException.toValue(__tsonic_catch0);
                    if (Tsonic.CSharp.Runtime.TsValue.IsDynamicInstanceOf<TsumoError>(error))
                    {
                        return Tsonic.CSharp.Runtime.TsValue.CastDynamic<TsumoError>(error);
                    }
                    throw;
                }
                throw new Tsonic.CSharp.Runtime.Error("Expected a Tsumo error");
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
                string source = Tsonic.CSharp.Node.path.join(root, "source");
                string nested = Tsonic.CSharp.Node.path.join(source, "a");
                string outside = Tsonic.CSharp.Node.path.join(root, "outside");
                TestRoot.createDirectory(nested);
                TestRoot.createDirectory(outside);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(source, "z.txt"), "z");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(nested, "b.txt"), "b");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(nested, "a.txt"), "a");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(outside, "outside.txt"), "outside");
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { Tsonic.CSharp.Node.path.join(nested, "a.txt"), Tsonic.CSharp.Node.path.join(nested, "b.txt"), Tsonic.CSharp.Node.path.join(source, "z.txt") }), Node_modules_Tsumo_engine_src_fs.listFilesRecursive(source, "*.txt"));
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { Tsonic.CSharp.Node.path.join(source, "z.txt") }), Node_modules_Tsumo_engine_src_fs.listFilesTopDirectory(source, "*.txt"));
                Xunit.Assert.Equal<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>(new string[] { nested }), Node_modules_Tsumo_engine_src_fs.listDirectoriesTopDirectory(source));
                string link = Tsonic.CSharp.Node.path.join(source, "linked-directory");
                TestRoot.createSymbolicLink(outside, link);
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
                string watched = Tsonic.CSharp.Node.path.join(root, "watched");
                TestRoot.createDirectory(watched);
                string file = Tsonic.CSharp.Node.path.join(watched, "page.md");
                TestRoot.writeTextFile(file, "before");
                Tsonic.CSharp.Js.Map<string, WatchEntryState> initial = Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }));
                Xunit.Assert.True(Node_modules_Tsumo_engine_src_watchSnapshot.watchSnapshotsEqual(initial, Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }))));
                TestRoot.writeTextFile(file, "after with a different size");
                Xunit.Assert.False(Node_modules_Tsumo_engine_src_watchSnapshot.watchSnapshotsEqual(initial, Node_modules_Tsumo_engine_src_watchSnapshot.createWatchSnapshot(new Tsonic.CSharp.Js.JSArray<string>(new string[] { watched }))));
                string link = Tsonic.CSharp.Node.path.join(watched, "linked-file.md");
                TestRoot.createSymbolicLink(file, link);
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
