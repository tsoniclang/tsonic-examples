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
            TestRoot.__tsonic_module_init();
            captureScaffoldDiagnostic = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception error)
                {
                    if (error is TsumoError)
                    {
                        return ((TsumoError)error).diagnostic.code;
                    }
                    throw;
                }
                throw new System.Exception("Expected a scaffold diagnostic");
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
                Xunit.Assert.True(System.IO.Directory.Exists(outDir));
                Xunit.Assert.True(System.IO.File.Exists(System.IO.Path.Combine(outDir, "index.html")));
                Xunit.Assert.True(System.IO.File.Exists(System.IO.Path.Combine(outDir, "posts", "hello-world", "index.html")));
                Xunit.Assert.Equal<double>(12, result.pagesBuilt);
                Xunit.Assert.Equal<double>(13, System.IO.Directory.GetFiles(outDir, "*", System.IO.SearchOption.AllDirectories).Length);
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
                Xunit.Assert.True(!System.IO.File.Exists(System.IO.Path.Combine(outDir, "posts", "my-draft", "index.html")));
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
                Xunit.Assert.True(System.IO.File.Exists(System.IO.Path.Combine(outDir, "posts", "my-post", "index.html")));
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
                string occupied = System.IO.Path.Combine(root, "occupied");
                System.IO.Directory.CreateDirectory(occupied);
                System.IO.File.WriteAllText(System.IO.Path.Combine(occupied, "keep.txt"), "keep");
                Xunit.Assert.Equal("TSUMO_SCAFFOLD_DESTINATION_NOT_EMPTY", ScaffoldAndBuildTest.captureScaffoldDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_scaffold_initSite.initSite(occupied, null);
                }));
                string site = System.IO.Path.Combine(root, "site");
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
