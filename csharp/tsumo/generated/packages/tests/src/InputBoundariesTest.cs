using System;

namespace Tsumo.Tests
{
    public static class InputBoundariesTest
    {
        public static Func<Action, TsumoDiagnostic> captureDiagnostic
        {
            get;
            private set;
        } = default(Func<Action, TsumoDiagnostic>)!;
        public static Action<string> assertFrontMatterModel
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Action<string, string, bool, string> assertConfigModel
        {
            get;
            private set;
        } = default(Action<string, string, bool, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Node_modules_Tsumo_engine_src_testing.__tsonic_module_init();
            TestRoot.__tsonic_module_init();
            captureDiagnostic = (Action operation) =>
            {
                try
                {
                    operation();
                }
                catch (System.Exception error)
                {
                    if (error is TsumoError)
                    {
                        return ((TsumoError)error).diagnostic;
                    }
                    throw;
                }
                throw new System.Exception("Expected a Tsumo diagnostic");
            };
            assertFrontMatterModel = (string source) =>
            {
                ParsedContent parsed = Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent(source, "content/post.md");
                Xunit.Assert.Equal("Café 🚀", parsed.frontMatter.title);
                Xunit.Assert.True(parsed.frontMatter.date is not null);
                Xunit.Assert.True(!parsed.frontMatter.draft);
                Xunit.Assert.Equal<double>(2, parsed.frontMatter.tags.length);
                Xunit.Assert.Equal("alpha", parsed.frontMatter.tags[0]);
                Xunit.Assert.Equal("beta", parsed.frontMatter.tags[1]);
                ParamValue? featured = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(parsed.frontMatter.Params, "featured");
                Xunit.Assert.True(featured is not null && featured.boolValue);
                Xunit.Assert.Equal<double>(1, parsed.frontMatter.menus.length);
                Xunit.Assert.Equal("main", parsed.frontMatter.menus[0].menu);
                Xunit.Assert.Equal<double>(2, parsed.frontMatter.menus[0].weight);
                Xunit.Assert.Equal("Body", parsed.body);
            };
            assertConfigModel = (string title, string baseURL, bool featured, string menuName) =>
            {
                Xunit.Assert.Equal("Café", title);
                Xunit.Assert.Equal("https://example.test/", baseURL);
                Xunit.Assert.True(featured);
                Xunit.Assert.Equal("Home", menuName);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class InputBoundaryTests
    {
        [Xunit.FactAttribute]
        public void json_tree_preserves_unicode_kinds_and_source_locations()
        {
            JsonValue value = Node_modules_Tsumo_engine_src_utils_json.parseJson("{\n  \"title\": \"Caf\\u00e9 \\ud83d\\ude80\"\n}", "config.json");
            Xunit.Assert.True(value is JsonObject);
            if (!(value is JsonObject))
            {
                throw new System.Exception("Expected JSON object");
            }
            JsonValue? title = ((JsonObject)value).get("title");
            Xunit.Assert.True(title is JsonString);
            if (!(title is JsonString))
            {
                throw new System.Exception("Expected JSON string");
            }
            Xunit.Assert.Equal("Café 🚀", ((JsonString)title).value);
            Xunit.Assert.Equal<double>(2, ((JsonString)title).line);
            Xunit.Assert.Equal<double>(12, ((JsonString)title).column);
        }
        [Xunit.FactAttribute]
        public void json_tree_handles_large_indexed_inputs()
        {
            Tsonic.CSharp.Js.JSArray<string> entries = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
            for (double index = 0; index < 27000; index++)
            {
                entries.push("0");
            }
            JsonValue value = Node_modules_Tsumo_engine_src_utils_json.parseJson($"[{Tsonic.CSharp.Js.Array.join(entries, ",")}]", "large.json");
            Xunit.Assert.True(value is JsonArray);
            if (!(value is JsonArray))
            {
                throw new System.Exception("Expected JSON array");
            }
            Xunit.Assert.Equal<double>(27000, ((JsonArray)value).items.length);
        }
        [Xunit.FactAttribute]
        public void json_tree_rejects_ambiguous_and_malformed_inputs_exactly()
        {
            TsumoDiagnostic leadingZero = InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_utils_json.parseJson("{\n  \"value\": 01\n}", "bad.json");
            });
            Xunit.Assert.Equal("TSUMO_JSON_SYNTAX_INVALID", leadingZero.code);
            Xunit.Assert.Equal("bad.json", leadingZero.file);
            Xunit.Assert.Equal<double?>(2, leadingZero.line);
            Xunit.Assert.Equal<double?>(13, leadingZero.column);
            TsumoDiagnostic duplicate = InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_utils_json.parseJson("{\n  \"value\": 1,\n  \"value\": 2\n}", "duplicate.json");
            });
            Xunit.Assert.Equal("TSUMO_JSON_DUPLICATE_PROPERTY", duplicate.code);
            Xunit.Assert.Equal<double?>(3, duplicate.line);
            Xunit.Assert.Equal<double?>(3, duplicate.column);
            Xunit.Assert.Equal("TSUMO_JSON_SYNTAX_INVALID", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_utils_json.parseJson("{\"value\": \"\\ud800\"}", "surrogate.json");
            }).code);
            string deeplyNested = "";
            for (double index = 0; index < 257; index++)
            {
                deeplyNested += "[";
            }
            for (double index_1 = 0; index_1 < 257; index_1++)
            {
                deeplyNested += "]";
            }
            Xunit.Assert.Equal("TSUMO_JSON_DEPTH_EXCEEDED", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_utils_json.parseJson(deeplyNested, "deep.json");
            }).code);
        }
        [Xunit.FactAttribute]
        public void all_front_matter_formats_create_one_closed_model()
        {
            InputBoundariesTest.assertFrontMatterModel(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "---", "title: 'Café 🚀'", "date: '2026-01-02T00:00:00Z'", "draft: false", "tags: ['alpha', 'beta']", "params:", "  featured: true", "menu:", "  main:", "    name: Home", "    weight: 2", "---", "Body" }), "\n"));
            InputBoundariesTest.assertFrontMatterModel(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "+++", "title = 'Café 🚀'", "date = '2026-01-02T00:00:00Z'", "draft = false", "tags = ['alpha', 'beta']", "[params]", "featured = true", "[[menu.main]]", "name = 'Home'", "weight = 2", "+++", "Body" }), "\n"));
            InputBoundariesTest.assertFrontMatterModel(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "{", "  \"title\": \"Caf\\u00e9 \\ud83d\\ude80\",", "  \"date\": \"2026-01-02T00:00:00Z\",", "  \"draft\": false,", "  \"tags\": [\"alpha\", \"beta\"],", "  \"params\": { \"featured\": true },", "  \"menu\": { \"main\": { \"name\": \"Home\", \"weight\": 2 } }", "}", "Body" }), "\n"));
        }
        [Xunit.FactAttribute]
        public void front_matter_rejects_invalid_shapes_with_exact_locations()
        {
            TsumoDiagnostic invalidDate = InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("---\ndate: not-a-date\n---\nBody", "date.md");
            });
            Xunit.Assert.Equal("TSUMO_FRONTMATTER_INVALID_DATE", invalidDate.code);
            Xunit.Assert.Equal("date.md", invalidDate.file);
            Xunit.Assert.Equal<double?>(2, invalidDate.line);
            Xunit.Assert.Equal("TSUMO_FRONTMATTER_INVALID_BOOL", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("+++\ndraft = 'false'\n+++", "draft.md");
            }).code);
            Xunit.Assert.Equal("TSUMO_FRONTMATTER_FIELD_INVALID", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("{\"tags\": [\"ok\", 1]}", "tags.md");
            }).code);
            Xunit.Assert.Equal("TSUMO_FRONTMATTER_FIELD_DUPLICATE", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("---\ntitle: First\nTitle: Second\n---", "duplicate.md");
            }).code);
            Xunit.Assert.Equal("TSUMO_FRONTMATTER_DELIMITER_UNCLOSED", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("---\ntitle: Missing", "unclosed.md");
            }).code);
        }
        [Xunit.FactAttribute]
        public void all_configuration_formats_create_one_closed_model()
        {
            SiteConfig toml = Node_modules_Tsumo_engine_src_config_toml.parseTomlConfig(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "title = 'Café'", "baseURL = 'https://example.test'", "[params]", "featured = true", "[[menu.main]]", "name = 'Home'" }), "\n"), "hugo.toml");
            SiteConfig yaml = Node_modules_Tsumo_engine_src_config_yaml.parseYamlConfig(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "title: Café", "baseURL: https://example.test", "params:", "  featured: true", "menu:", "  main:", "    - name: Home" }), "\n"), "hugo.yaml");
            SiteConfig json = Node_modules_Tsumo_engine_src_config_json.parseJsonConfig("{\"title\":\"Caf\\u00e9\",\"baseURL\":\"https://example.test\",\"params\":{\"featured\":true},\"menu\":{\"main\":[{\"name\":\"Home\"}]}}", "hugo.json");
            ParamValue? tomlFeatured = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(toml.Params, "featured");
            ParamValue? yamlFeatured = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(yaml.Params, "featured");
            ParamValue? jsonFeatured = Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(json.Params, "featured");
            Tsonic.CSharp.Js.JSArray<MenuEntry>? tomlMenu = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<MenuEntry>>(toml.Menus, "main");
            Tsonic.CSharp.Js.JSArray<MenuEntry>? yamlMenu = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<MenuEntry>>(yaml.Menus, "main");
            Tsonic.CSharp.Js.JSArray<MenuEntry>? jsonMenu = Tsonic.CSharp.Js.Map.getReference<string, Tsonic.CSharp.Js.JSArray<MenuEntry>>(json.Menus, "main");
            Xunit.Assert.True(tomlMenu is not null && yamlMenu is not null && jsonMenu is not null);
            if (tomlMenu is null || yamlMenu is null || jsonMenu is null)
            {
                throw new System.Exception("Expected main menus");
            }
            InputBoundariesTest.assertConfigModel(toml.title, toml.baseURL, tomlFeatured is not null && tomlFeatured.boolValue, tomlMenu[0].name);
            InputBoundariesTest.assertConfigModel(yaml.title, yaml.baseURL, yamlFeatured is not null && yamlFeatured.boolValue, yamlMenu[0].name);
            InputBoundariesTest.assertConfigModel(json.title, json.baseURL, jsonFeatured is not null && jsonFeatured.boolValue, jsonMenu[0].name);
        }
        [Xunit.FactAttribute]
        public void configuration_rejects_unknown_malformed_and_mistyped_fields()
        {
            TsumoDiagnostic json = InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_json.parseJsonConfig("{\n  \"title\": 42\n}", "hugo.json");
            });
            Xunit.Assert.Equal("TSUMO_CONFIG_INVALID_FIELD", json.code);
            Xunit.Assert.Equal<double?>(2, json.line);
            Xunit.Assert.Equal("TSUMO_CONFIG_UNKNOWN_FIELD", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_yaml.parseYamlConfig("unsupported: value", "hugo.yaml");
            }).code);
            Xunit.Assert.Equal("TSUMO_CONFIG_INVALID_FIELD", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_yaml.parseYamlConfig("title: true", "typed.yaml");
            }).code);
            Xunit.Assert.Equal("TSUMO_CONFIG_DUPLICATE_FIELD", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_yaml.parseYamlConfig("title: First\nTitle: Second", "duplicate.yaml");
            }).code);
            Xunit.Assert.Equal("TSUMO_CONFIG_INVALID_FIELD", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_toml.parseTomlConfig("title = 42", "typed.toml");
            }).code);
            Xunit.Assert.Equal("TSUMO_CONFIG_TABLE_UNSUPPORTED", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_toml.parseTomlConfig("[unsupported]\nvalue = 1", "hugo.toml");
            }).code);
            Xunit.Assert.Equal("TSUMO_CONFIG_SYNTAX_INVALID", InputBoundariesTest.captureDiagnostic(() =>
            {
                Node_modules_Tsumo_engine_src_config_toml.parseTomlConfig("title = bare", "bare.toml");
            }).code);
        }
        [Xunit.FactAttribute]
        public void structured_scalars_decode_strings_and_comments()
        {
            SiteConfig toml = Node_modules_Tsumo_engine_src_config_toml.parseTomlConfig(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "title = \"Caf\\u00e9 # retained\" # removed", "[params]", "message = 'literal # retained' # removed", "count = 1_024" }), "\n"), "scalars.toml");
            Xunit.Assert.Equal("Café # retained", toml.title);
            Xunit.Assert.Equal("literal # retained", Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(toml.Params, "message")?.stringValue);
            Xunit.Assert.Equal<double?>(1024, Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(toml.Params, "count")?.numberValue);
            SiteConfig yaml = Node_modules_Tsumo_engine_src_config_yaml.parseYamlConfig(Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "title: \"Caf\\u00e9 # retained\" # removed", "copyright: 'Tsumo''s docs' # removed", "params:", "  address: value#fragment # removed" }), "\n"), "scalars.yaml");
            Xunit.Assert.Equal("Café # retained", yaml.title);
            Xunit.Assert.Equal("Tsumo's docs", yaml.copyright);
            Xunit.Assert.Equal("value#fragment", Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(yaml.Params, "address")?.stringValue);
            ParsedContent frontMatter = Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent("---\ntitle: 'Tsumo''s \\u263a' # removed\n---\nBody", "frontmatter.md");
            Xunit.Assert.Equal("Tsumo's \\u263a", frontMatter.frontMatter.title);
            string leadingJson = " \n{\"title\":\"Not front matter\"}";
            ParsedContent content = Node_modules_Tsumo_engine_src_frontmatter_parse.parseContent(leadingJson, "leading-json.md");
            Xunit.Assert.True(content.frontMatter.title is null);
            Xunit.Assert.Equal(leadingJson, content.body);
        }
        [Xunit.FactAttribute]
        public void split_configuration_has_one_deterministic_merge_contract()
        {
            string site = TestRoot.createTestDirectory("split-config");
            try
            {
                string configDir = System.IO.Path.Combine(site, "config", "_default");
                System.IO.Directory.CreateDirectory(configDir);
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "hugo.toml"), "title = 'Example'\nbaseURL = 'https://example.test'");
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "params.yaml"), "message: \"Hello # retained\" # removed");
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "languages.toml"), Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "[en]", "languageName = 'English'", "languageDirection = 'rtl'", "contentDir = 'content/custom'", "weight = 4" }), "\n"));
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "languages.en.toml"), "weight = 1");
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "module.toml"), Tsonic.CSharp.Js.Array.join(new Tsonic.CSharp.Js.JSArray<string>(new string[] { "[[mounts]]", "source = 'shared'", "target = 'content'" }), "\n"));
                SiteConfig loaded = Node_modules_Tsumo_engine_src_config_loader.loadSiteConfig(site).config;
                Xunit.Assert.Equal("Example", loaded.title);
                Xunit.Assert.Equal("https://example.test/", loaded.baseURL);
                Xunit.Assert.Equal("Hello # retained", Tsonic.CSharp.Js.Map.getReference<string, ParamValue>(loaded.Params, "message")?.stringValue);
                Xunit.Assert.Equal<double>(1, loaded.languages.length);
                Xunit.Assert.Equal("English", loaded.languages[0].languageName);
                Xunit.Assert.Equal("rtl", loaded.languages[0].languageDirection);
                Xunit.Assert.Equal("content/custom", loaded.languages[0].contentDir);
                Xunit.Assert.Equal<double>(1, loaded.languages[0].weight);
                Xunit.Assert.Equal<double>(1, loaded.moduleMounts.length);
                Xunit.Assert.Equal("shared", loaded.moduleMounts[0].source);
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "params.yaml"), "message: first\nMessage: second");
                Xunit.Assert.Equal("TSUMO_CONFIG_DUPLICATE_FIELD", InputBoundariesTest.captureDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_config_loader.loadSiteConfig(site);
                }).code);
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "params.yaml"), "message: first");
                System.IO.File.WriteAllText(System.IO.Path.Combine(configDir, "config.yaml"), "title: Other");
                Xunit.Assert.Equal("TSUMO_CONFIG_FILE_AMBIGUOUS", InputBoundariesTest.captureDiagnostic(() =>
                {
                    Node_modules_Tsumo_engine_src_config_loader.loadSiteConfig(site);
                }).code);
            }
            finally
            {
                TestRoot.deleteTestDirectory(site);
            }
        }
        [Xunit.FactAttribute]
        public void content_types_are_exact_and_fail_to_binary_by_default()
        {
            Xunit.Assert.Equal("text/html; charset=utf-8", Node_modules_Tsumo_engine_src_utils_mime.contentTypeForPath("INDEX.HTML"));
            Xunit.Assert.Equal("application/json; charset=utf-8", Node_modules_Tsumo_engine_src_utils_mime.contentTypeForPath("data.json"));
            Xunit.Assert.Equal("image/png", Node_modules_Tsumo_engine_src_utils_mime.contentTypeForPath("image.png"));
            Xunit.Assert.Equal("font/woff2", Node_modules_Tsumo_engine_src_utils_mime.contentTypeForPath("font.woff2"));
            Xunit.Assert.Equal("application/octet-stream", Node_modules_Tsumo_engine_src_utils_mime.contentTypeForPath("archive.unknown"));
        }
    }
}
