using System;

namespace Tsumo.Tests
{
    public static class ScaffoldAndBuildTest
    {
        public static Func<Action, string> captureScaffoldDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_index.__tsonic_module_init();
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureScaffoldDiagnostic = (Action operation) =>
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
                        return Tsonic.CSharp.Runtime.TsValue.CastDynamic<TsumoError>(error).diagnostic.code;
                    }
                    throw;
                }
                throw new Tsonic.CSharp.Runtime.Error("Expected a scaffold diagnostic");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ScaffoldAndBuildTests
    {
        [Xunit.FactAttribute]
        public void scaffold_then_build()
        {
            string siteDir = TestRoot.createTestDirectory("site");
            string outDir = TestRoot.createTestDirectory("out");
            try
            {
                Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(siteDir, null);
                BuildRequest req = new BuildRequest(siteDir);
                req.destinationDir = outDir;
                req.cleanDestinationDir = true;
                BuildResult result = Node_modules_Tsumo_engine_src_buildSite.buildSite(req);
                Xunit.Assert.True(TestRoot.directoryExists(outDir));
                Xunit.Assert.True(TestRoot.fileExists(Tsonic.CSharp.Node.path.join(outDir, "index.html")));
                Xunit.Assert.True(TestRoot.fileExists(Tsonic.CSharp.Node.path.join(outDir, "posts", "hello-world", "index.html")));
                Xunit.Assert.Equal<double>(12, result.pagesBuilt);
                Xunit.Assert.Equal<double>(13, Node_modules_Tsumo_engine_src_fs.listFilesRecursive(outDir, "*").length);
            }
            finally
            {
                TestRoot.deleteTestDirectory(outDir);
                TestRoot.deleteTestDirectory(siteDir);
            }
        }
        [Xunit.FactAttribute]
        public void drafts_skipped_by_default()
        {
            string siteDir = TestRoot.createTestDirectory("site");
            string outDir = TestRoot.createTestDirectory("out");
            try
            {
                Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(siteDir, null);
                Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(siteDir, "posts/my-draft.md", null);
                BuildRequest req = new BuildRequest(siteDir);
                req.destinationDir = outDir;
                req.cleanDestinationDir = true;
                req.buildDrafts = false;
                Node_modules_Tsumo_engine_src_buildSite.buildSite(req);
                Xunit.Assert.True(!TestRoot.fileExists(Tsonic.CSharp.Node.path.join(outDir, "posts", "my-draft", "index.html")));
            }
            finally
            {
                TestRoot.deleteTestDirectory(outDir);
                TestRoot.deleteTestDirectory(siteDir);
            }
        }
        [Xunit.FactAttribute]
        public void new_content_then_build()
        {
            string siteDir = TestRoot.createTestDirectory("site");
            string outDir = TestRoot.createTestDirectory("out");
            try
            {
                Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(siteDir, null);
                Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(siteDir, "posts/my-post.md", null);
                BuildRequest req = new BuildRequest(siteDir);
                req.destinationDir = outDir;
                req.cleanDestinationDir = true;
                req.buildDrafts = true;
                Node_modules_Tsumo_engine_src_buildSite.buildSite(req);
                Xunit.Assert.True(TestRoot.fileExists(Tsonic.CSharp.Node.path.join(outDir, "posts", "my-post", "index.html")));
            }
            finally
            {
                TestRoot.deleteTestDirectory(outDir);
                TestRoot.deleteTestDirectory(siteDir);
            }
        }
        [Xunit.FactAttribute]
        public void scaffold_boundaries_fail_closed_with_exact_diagnostics()
        {
            string root = TestRoot.createTestDirectory("scaffold-boundaries");
            try
            {
                string occupied = Tsonic.CSharp.Node.path.join(root, "occupied");
                TestRoot.createDirectory(occupied);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(occupied, "keep.txt"), "keep");
                Xunit.Assert.Equal("TSUMO_SCAFFOLD_DESTINATION_NOT_EMPTY", ScaffoldAndBuildTest.captureScaffoldDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(occupied, null);
                }));
                string site = Tsonic.CSharp.Node.path.join(root, "site");
                Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(site, null);
                Xunit.Assert.Equal("TSUMO_SCAFFOLD_CONTENT_PATH_ESCAPES_ROOT", ScaffoldAndBuildTest.captureScaffoldDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(site, "../outside.md", null);
                }));
                Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(site, "posts/exact.md", null);
                Xunit.Assert.Equal("TSUMO_SCAFFOLD_CONTENT_EXISTS", ScaffoldAndBuildTest.captureScaffoldDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_scaffold_newContent.newContent(site, "posts/exact.md", null);
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
