using System;

namespace Tsumo.Tests
{
    public static class ResourcePipelineTest
    {
        public static Func<Action, string> captureResourceDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureResourceDiagnostic = (Action operation) =>
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
                throw new System.Exception("Expected a resource diagnostic");
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ResourcePipelineTests
    {
        [Xunit.FactAttribute]
        public void relative_path_policy_rejects_every_escape_form()
        {
            Xunit.Assert.Equal("TSUMO_RESOURCE_PATH_ESCAPES_ROOT", ResourcePipelineTest.captureResourceDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_resources_paths.normalizeResourceRelativePath("../secret.txt");
            }));
            Xunit.Assert.Equal("TSUMO_RESOURCE_PATH_ESCAPES_ROOT", ResourcePipelineTest.captureResourceDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_resources_paths.normalizeResourceRelativePath("assets/../../secret.txt");
            }));
            Xunit.Assert.Equal("TSUMO_RESOURCE_PATH_ABSOLUTE", ResourcePipelineTest.captureResourceDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_resources_paths.normalizeResourceRelativePath("C:\\secret.txt");
            }));
            Xunit.Assert.Equal("images/logo.png", Node_modules_Tsumo_engine_src_resources_paths.normalizeResourceRelativePath("/images/./logo.png"));
        }
        [Xunit.FactAttribute]
        public void glob_matching_is_segment_exact()
        {
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("images/**/*.png", "images/icons/logo.png"));
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("*.css", "site.css"));
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("{*cover*,*thumbnail*}", "article-cover.png"));
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("{*cover*,*thumbnail*}", "article-thumbnail.png"));
            Xunit.Assert.True(!Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("{*cover*,*thumbnail*}", "article-logo.png"));
            Xunit.Assert.True(!Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("*.css", "nested/site.css"));
            Xunit.Assert.True(!Node_modules_Tsumo_engine_src_resources_glob.resourceGlobMatches("images/*.png", "images/icons/logo.png"));
        }
        [Xunit.FactAttribute]
        public void image_dimensions_are_read_from_exact_file_signatures()
        {
            Tsonic.CSharp.Node.Buffer png = Tsonic.CSharp.Node.Buffer.from(new int[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 2, 0, 0, 0, 3 });
            ImageDimensions? dimensions = Node_modules_Tsumo_engine_src_resources_imageDimensions.parseImageDimensions(png);
            Xunit.Assert.True(dimensions is not null && dimensions.width == 2 && dimensions.height == 3);
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_imageDimensions.parseImageDimensions(Tsonic.CSharp.Node.Buffer.from(new int[] { 1, 2, 3 })) is null);
        }
        [Xunit.FactAttribute]
        public void utf8_validation_accepts_scalars_and_rejects_malformed_sequences()
        {
            Xunit.Assert.True(Node_modules_Tsumo_engine_src_resources_text.isValidUtf8(Tsonic.CSharp.Node.Buffer.from(new int[] { 65, 194, 162, 226, 130, 172, 240, 159, 152, 128 })));
            Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Node.Buffer> malformed = new Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Node.Buffer>(new Tsonic.CSharp.Node.Buffer[] { Tsonic.CSharp.Node.Buffer.from(new int[] { 128 }), Tsonic.CSharp.Node.Buffer.from(new int[] { 192, 128 }), Tsonic.CSharp.Node.Buffer.from(new int[] { 224, 128, 128 }), Tsonic.CSharp.Node.Buffer.from(new int[] { 237, 160, 128 }), Tsonic.CSharp.Node.Buffer.from(new int[] { 244, 144, 128, 128 }), Tsonic.CSharp.Node.Buffer.from(new int[] { 240, 159, 146 }) });
            for (int index = 0; index < malformed.length; index++)
            {
                Xunit.Assert.True(!Node_modules_Tsumo_engine_src_resources_text.isValidUtf8(malformed[index]));
            }
        }
        [Xunit.FactAttribute]
        public void file_resources_publish_raw_bytes_and_decode_only_for_text_operations()
        {
            string root = TestRoot.createTestDirectory("resource-bytes");
            string siteDir = System.IO.Path.Combine(root, "site");
            string outputDir = System.IO.Path.Combine(root, "output");
            try
            {
                string assetsDir = System.IO.Path.Combine(siteDir, "assets");
                System.IO.Directory.CreateDirectory(assetsDir);
                Tsonic.CSharp.Node.Buffer sourceBytes = Tsonic.CSharp.Node.Buffer.from(new int[] { 97, 160, 98 });
                Tsonic.CSharp.Node.fs.writeFileSync(System.IO.Path.Combine(assetsDir, "legacy.js"), sourceBytes);
                ResourceManager manager = new ResourceManager(siteDir, null, outputDir);
                Resource? resource = manager.get("legacy.js");
                Xunit.Assert.True(resource is not null && resource.text is null);
                if (resource is null)
                {
                    throw new System.Exception("Expected legacy.js resource");
                }
                manager.ensurePublished(resource);
                Tsonic.CSharp.Node.Buffer published = Tsonic.CSharp.Node.fs.readFileSync(System.IO.Path.Combine(outputDir, "legacy.js"));
                Xunit.Assert.Equal<double>(3, published.length);
                Xunit.Assert.Equal<double>(160, published.readUInt8(1));
                Xunit.Assert.Equal("TSUMO_RESOURCE_TEXT_ENCODING_INVALID", ResourcePipelineTest.captureResourceDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_resources_text.readResourceText(resource, "Resource.Content");
                }));
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
        [Xunit.FactAttribute]
        public void transform_identity_and_metadata_are_content_exact()
        {
            Resource first = Node_modules_Tsumo_engine_src_resources_transforms.createStringResource("style.css", "a {}");
            Resource second = Node_modules_Tsumo_engine_src_resources_transforms.createStringResource("style.css", "b {}");
            Xunit.Assert.True(first.id != second.id);
            Xunit.Assert.True(first.publishable);
            Xunit.Assert.Equal("style.css", first.outputRelPath);
            Xunit.Assert.Equal("text/css", first.mediaType);
            Resource source = new Resource("source", null, true, "css/site.css", Tsonic.CSharp.Node.Buffer.from("body {}", "utf8"), "body {}", new ResourceData(""), "text/css", 10, 20);
            Resource fingerprinted = Node_modules_Tsumo_engine_src_resources_transforms.fingerprintResource(source);
            Xunit.Assert.Equal("text/css", fingerprinted.mediaType);
            Xunit.Assert.Equal<double>(10, fingerprinted.width);
            Xunit.Assert.Equal<double>(20, fingerprinted.height);
            string expectedHash = Tsonic.CSharp.Js.String.slice(Tsonic.CSharp.Node.crypto.createHash("sha256").update(source.bytes).digest("hex"), 0, 16);
            Xunit.Assert.True(fingerprinted.outputRelPath == $"css/site.{expectedHash}.css");
            Xunit.Assert.True(Tsonic.CSharp.Js.String.startsWith(fingerprinted.Data.Integrity, "sha256-"));
        }
        [Xunit.FactAttribute]
        public void resource_lookup_is_sorted_and_site_assets_override_theme_assets()
        {
            string root = TestRoot.createTestDirectory("resources");
            string siteDir = System.IO.Path.Combine(root, "site");
            string themeDir = System.IO.Path.Combine(root, "theme");
            string outputDir = System.IO.Path.Combine(root, "output");
            try
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(siteDir, "assets"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(themeDir, "assets"));
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDir, "assets", "z.txt"), "site-z");
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDir, "assets", "a.txt"), "site-a");
                System.IO.File.WriteAllText(System.IO.Path.Combine(siteDir, "assets", "main.ts"), "export const value = 1;");
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDir, "assets", "a.txt"), "theme-a");
                System.IO.File.WriteAllText(System.IO.Path.Combine(themeDir, "assets", "m.txt"), "theme-m");
                ResourceManager manager = new ResourceManager(siteDir, themeDir, outputDir);
                Tsonic.CSharp.Js.JSArray<Resource> matched = manager.match("*.txt");
                Xunit.Assert.Equal<double>(3, matched.length);
                Xunit.Assert.True(matched[0].outputRelPath == "a.txt");
                Xunit.Assert.True(matched[1].outputRelPath == "m.txt");
                Xunit.Assert.True(matched[2].outputRelPath == "z.txt");
                Xunit.Assert.True(matched[0].text is null);
                Xunit.Assert.Equal("site-a", Node_modules_Tsumo_engine_src_resources_text.readResourceText(matched[0], "test"));
                Xunit.Assert.Equal<double>(4, manager.byType("text").length);
                Resource? typescript = manager.get("main.ts");
                Xunit.Assert.True(typescript is not null && typescript.text is null);
                Xunit.Assert.True(typescript is not null && Node_modules_Tsumo_engine_src_resources_text.readResourceText(typescript, "test") == "export const value = 1;");
                Xunit.Assert.True(typescript is not null && typescript.mediaType == "text/typescript");
            }
            finally
            {
                TestRoot.deleteTestDirectory(root);
            }
        }
    }
}
