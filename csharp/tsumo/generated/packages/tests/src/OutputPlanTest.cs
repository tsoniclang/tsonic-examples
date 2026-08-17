using System;

namespace Tsumo.Tests
{
    public static class OutputPlanTest
    {
        public static Func<Action, string> captureOutputDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureOutputDiagnostic = (Action operation) =>
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
                throw new System.Exception("Expected an output-plan diagnostic");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class OutputPlanTests
    {
        [Xunit.FactAttribute]
        public void paths_and_collisions_fail_before_rendering()
        {
            SiteOutputPlan plan = new SiteOutputPlan();
            Xunit.Assert.Equal("TSUMO_OUTPUT_PATH_ESCAPES_ROOT", OutputPlanTest.captureOutputDiagnostic(() =>
            {
                plan.addText("../outside.html", "outside", "escape");
            }));
            plan.addText("pages/index.html", "first", "first page");
            Xunit.Assert.Equal("TSUMO_OUTPUT_PATH_CONFLICT", OutputPlanTest.captureOutputDiagnostic(() =>
            {
                plan.addText("PAGES/index.html", "second", "second page");
            }));
        }
        [Xunit.FactAttribute]
        public void static_layers_have_one_explicit_precedence_policy()
        {
            string root = TestRoot.createTestDirectory("output-plan-static");
            string theme = System.IO.Path.Combine(root, "theme");
            string site = System.IO.Path.Combine(root, "site");
            string output = System.IO.Path.Combine(root, "output");
            try
            {
                System.IO.Directory.CreateDirectory(theme);
                System.IO.Directory.CreateDirectory(site);
                System.IO.File.WriteAllText(System.IO.Path.Combine(theme, "style.css"), "theme");
                System.IO.File.WriteAllText(System.IO.Path.Combine(theme, "robots.txt"), "theme robots");
                System.IO.File.WriteAllText(System.IO.Path.Combine(site, "style.css"), "site");
                System.IO.File.WriteAllText(System.IO.Path.Combine(site, "robots.txt"), "site robots");
                SiteOutputPlan plan = new SiteOutputPlan();
                plan.addDirectory(theme, "", "theme static", "theme-static");
                plan.addDirectory(site, "", "site static", "site-static");
                plan.addDefaultText("robots.txt", "generated robots", "generated robots");
                plan.addText("index.html", "home", "home");
                Xunit.Assert.Equal<double>(1, plan.generatedOutputCount());
                plan.render(output);
                Xunit.Assert.Equal("site", System.IO.File.ReadAllText(System.IO.Path.Combine(output, "style.css")));
                Xunit.Assert.Equal("site robots", System.IO.File.ReadAllText(System.IO.Path.Combine(output, "robots.txt")));
                Xunit.Assert.Equal("home", System.IO.File.ReadAllText(System.IO.Path.Combine(output, "index.html")));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void bundle_assets_cannot_overwrite_generated_routes()
        {
            string root = TestRoot.createTestDirectory("output-plan-bundle");
            try
            {
                string asset = System.IO.Path.Combine(root, "index.html");
                System.IO.File.WriteAllText(asset, "asset");
                SiteOutputPlan plan = new SiteOutputPlan();
                plan.addText("index.html", "generated", "home");
                Xunit.Assert.Equal("TSUMO_OUTPUT_PATH_CONFLICT", OutputPlanTest.captureOutputDiagnostic(() =>
                {
                    plan.addAsset("index.html", asset, "bundle", "bundle");
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void deferred_replacements_snapshot_outputs_before_mutation()
        {
            string root = TestRoot.createTestDirectory("output-plan-deferred");
            string output = System.IO.Path.Combine(root, "output");
            try
            {
                SiteOutputPlan plan = new SiteOutputPlan();
                plan.addText("first.html", "before:<deferred-token>:after", "first page");
                plan.addText("second.html", "unchanged", "second page");
                Tsonic.CSharp.Js.Map<string, string> results = new Tsonic.CSharp.Js.Map<string, string>();
                results.set("<deferred-token>", "ready");
                plan.applyDeferredTemplateResults(results);
                plan.render(output);
                Xunit.Assert.Equal("before:ready:after", System.IO.File.ReadAllText(System.IO.Path.Combine(output, "first.html")));
                Xunit.Assert.Equal("unchanged", System.IO.File.ReadAllText(System.IO.Path.Combine(output, "second.html")));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
