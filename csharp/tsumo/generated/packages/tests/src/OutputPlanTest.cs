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
                catch (System.Exception __tsonic_catch0)
                {
                    Tsonic.CSharp.Runtime.TsValue error = Tsonic.CSharp.Runtime.TsThrownValueException.toValue(__tsonic_catch0);
                    if (Tsonic.CSharp.Runtime.TsValue.IsDynamicInstanceOf<TsumoError>(error))
                    {
                        return Tsonic.CSharp.Runtime.TsValue.CastDynamic<TsumoError>(error).diagnostic.code;
                    }
                    throw;
                }
                throw new Tsonic.CSharp.Runtime.Error("Expected an output-plan diagnostic");
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
            string theme = Tsonic.CSharp.Node.path.join(root, "theme");
            string site = Tsonic.CSharp.Node.path.join(root, "site");
            string output = Tsonic.CSharp.Node.path.join(root, "output");
            try
            {
                TestRoot.createDirectory(theme);
                TestRoot.createDirectory(site);
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(theme, "style.css"), "theme");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(theme, "robots.txt"), "theme robots");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(site, "style.css"), "site");
                TestRoot.writeTextFile(Tsonic.CSharp.Node.path.join(site, "robots.txt"), "site robots");
                SiteOutputPlan plan = new SiteOutputPlan();
                plan.addDirectory(theme, "", "theme static", "theme-static");
                plan.addDirectory(site, "", "site static", "site-static");
                plan.addDefaultText("robots.txt", "generated robots", "generated robots");
                plan.addText("index.html", "home", "home");
                Xunit.Assert.Equal<double>(1, plan.generatedOutputCount());
                plan.render(output);
                Xunit.Assert.Equal("site", TestRoot.readTextFile(Tsonic.CSharp.Node.path.join(output, "style.css")));
                Xunit.Assert.Equal("site robots", TestRoot.readTextFile(Tsonic.CSharp.Node.path.join(output, "robots.txt")));
                Xunit.Assert.Equal("home", TestRoot.readTextFile(Tsonic.CSharp.Node.path.join(output, "index.html")));
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
                string asset = Tsonic.CSharp.Node.path.join(root, "index.html");
                TestRoot.writeTextFile(asset, "asset");
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
            string output = Tsonic.CSharp.Node.path.join(root, "output");
            try
            {
                SiteOutputPlan plan = new SiteOutputPlan();
                plan.addText("first.html", "before:<deferred-token>:after", "first page");
                plan.addText("second.html", "unchanged", "second page");
                Tsonic.CSharp.Js.Map<string, string> results = new Tsonic.CSharp.Js.Map<string, string>();
                results.set("<deferred-token>", "ready");
                plan.applyDeferredTemplateResults(results);
                plan.render(output);
                Xunit.Assert.Equal("before:ready:after", TestRoot.readTextFile(Tsonic.CSharp.Node.path.join(output, "first.html")));
                Xunit.Assert.Equal("unchanged", TestRoot.readTextFile(Tsonic.CSharp.Node.path.join(output, "second.html")));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
