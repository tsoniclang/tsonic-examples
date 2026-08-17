using System;

namespace Tsumo.Engine
{
    public static class Config_json
    {
        public static TsumoError invalidField(string field, string expected, JsonValue value, string? sourcePath = null)
        {
            return Diagnostics.createTsumoError("TSUMO_CONFIG_INVALID_FIELD", $"Configuration field '{field}' requires {expected}", sourcePath, value.line, value.column);
        }
        public static Func<string, JsonValue, string?, string> requireString
        {
            get;
            private set;
        } = default(Func<string, JsonValue, string?, string>)!;
        public static Func<string, JsonValue, string?, int> requireInt
        {
            get;
            private set;
        } = default(Func<string, JsonValue, string?, int>)!;
        public static Func<string, JsonValue, string?, JsonObject> requireObject
        {
            get;
            private set;
        } = default(Func<string, JsonValue, string?, JsonObject>)!;
        public static Func<string, JsonValue, string?, JsonArray> requireArray
        {
            get;
            private set;
        } = default(Func<string, JsonValue, string?, JsonArray>)!;
        public static Action<JsonObject, string, string?> assertUniqueFields
        {
            get;
            private set;
        } = default(Action<JsonObject, string, string?>)!;
        public static Func<string, JsonValue, string?, ParamValue> toParam
        {
            get;
            private set;
        } = default(Func<string, JsonValue, string?, ParamValue>)!;
        public static Action<LanguageConfigBuilder, string, JsonValue, string?> applyLanguageField
        {
            get;
            private set;
        } = default(Action<LanguageConfigBuilder, string, JsonValue, string?>)!;
        public static Action<MenuEntryBuilder, string, JsonValue, string?> applyMenuField
        {
            get;
            private set;
        } = default(Action<MenuEntryBuilder, string, JsonValue, string?>)!;
        public static Func<string, string?, SiteConfig> parseJsonConfig
        {
            get;
            private set;
        } = default(Func<string, string?, SiteConfig>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Models.__tsonic_module_init();
            Menus.__tsonic_module_init();
            Params.__tsonic_module_init();
            Utils_int32.__tsonic_module_init();
            Utils_json.__tsonic_module_init();
            Utils_text.__tsonic_module_init();
            Config_builders.__tsonic_module_init();
            Config_helpers.__tsonic_module_init();
            requireString = (string field, JsonValue value, string? sourcePath) =>
            {
                if (value is JsonString)
                {
                    return ((JsonString)value).value;
                }
                throw invalidField(field, "a string", value, sourcePath);
            };
            requireInt = (string field, JsonValue value, string? sourcePath) =>
            {
                if (value is JsonNumber)
                {
                    int? narrowed = Utils_int32.toInt32(((JsonNumber)value).value);
                    if (narrowed is not null)
                    {
                        return narrowed.Value;
                    }
                }
                throw invalidField(field, "a 32-bit integer", value, sourcePath);
            };
            requireObject = (string field, JsonValue value, string? sourcePath) =>
            {
                if (value is JsonObject)
                {
                    return (JsonObject)(JsonObject)value;
                }
                throw invalidField(field, "an object", value, sourcePath);
            };
            requireArray = (string field, JsonValue value, string? sourcePath) =>
            {
                if (value is JsonArray)
                {
                    return (JsonArray)(JsonArray)value;
                }
                throw invalidField(field, "an array", value, sourcePath);
            };
            assertUniqueFields = (JsonObject @object, string context, string? sourcePath) =>
            {
                Tsonic.CSharp.Js.Set<string> names = new Tsonic.CSharp.Js.Set<string>();
                for (int index = 0; index < @object.properties.length; index++)
                {
                    JsonProperty property = @object.properties[index];
                    string name = Tsonic.CSharp.Js.String.toLowerCase(property.key);
                    if (names.has(name))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_CONFIG_DUPLICATE_FIELD", $"{context} field '{property.key}' is declared more than once", sourcePath, property.line, property.column);
                    }
                    names.add(name);
                }
            };
            toParam = (string field, JsonValue value, string? sourcePath) =>
            {
                if (value is JsonString)
                {
                    return ParamValue.@string(((JsonString)value).value);
                }
                if (value is JsonBool)
                {
                    return ParamValue.@bool(((JsonBool)value).value);
                }
                if (value is JsonNumber)
                {
                    return ParamValue.number(requireInt(field, (JsonNumber)value, sourcePath));
                }
                throw invalidField(field, "a string, boolean, or 32-bit integer", value, sourcePath);
            };
            applyLanguageField = (LanguageConfigBuilder builder, string field, JsonValue value, string? sourcePath) =>
            {
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(field);
                if (normalized == "languagename")
                {
                    builder.languageName = requireString(field, value, sourcePath);
                }
                else
                {
                    if (normalized == "languagedirection")
                    {
                        builder.languageDirection = requireString(field, value, sourcePath);
                    }
                    else
                    {
                        if (normalized == "contentdir")
                        {
                            builder.contentDir = requireString(field, value, sourcePath);
                        }
                        else
                        {
                            if (normalized == "weight")
                            {
                                builder.weight = requireInt(field, value, sourcePath);
                            }
                            else
                            {
                                throw Diagnostics.createTsumoError("TSUMO_CONFIG_UNKNOWN_FIELD", $"Unknown language configuration field '{field}'", sourcePath, value.line, value.column);
                            }
                        }
                    }
                }
            };
            applyMenuField = (MenuEntryBuilder builder, string field, JsonValue value, string? sourcePath) =>
            {
                string normalized = Tsonic.CSharp.Js.String.toLowerCase(field);
                if (normalized == "name")
                {
                    builder.name = requireString(field, value, sourcePath);
                }
                else
                {
                    if (normalized == "url")
                    {
                        builder.url = requireString(field, value, sourcePath);
                    }
                    else
                    {
                        if (normalized == "pageref")
                        {
                            builder.pageRef = requireString(field, value, sourcePath);
                        }
                        else
                        {
                            if (normalized == "title")
                            {
                                builder.title = requireString(field, value, sourcePath);
                            }
                            else
                            {
                                if (normalized == "parent")
                                {
                                    builder.parent = requireString(field, value, sourcePath);
                                }
                                else
                                {
                                    if (normalized == "identifier")
                                    {
                                        builder.identifier = requireString(field, value, sourcePath);
                                    }
                                    else
                                    {
                                        if (normalized == "pre")
                                        {
                                            builder.pre = requireString(field, value, sourcePath);
                                        }
                                        else
                                        {
                                            if (normalized == "post")
                                            {
                                                builder.post = requireString(field, value, sourcePath);
                                            }
                                            else
                                            {
                                                if (normalized == "weight")
                                                {
                                                    builder.weight = requireInt(field, value, sourcePath);
                                                }
                                                else
                                                {
                                                    throw Diagnostics.createTsumoError("TSUMO_CONFIG_UNKNOWN_FIELD", $"Unknown menu configuration field '{field}'", sourcePath, value.line, value.column);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            parseJsonConfig = (string text, string? sourcePath) =>
            {
                JsonValue rootValue = Utils_json.parseJson(text, sourcePath);
                JsonObject root = requireObject("<root>", rootValue, sourcePath);
                assertUniqueFields(root, "Configuration", sourcePath);
                string title = "Tsumo Site";
                string baseURL = "";
                string languageCode = "en-us";
                string contentDir = "content";
                string? theme = null;
                string? copyright = null;
                bool hasLanguageCode = false;
                Tsonic.CSharp.Js.Map<string, ParamValue> @params = new Tsonic.CSharp.Js.Map<string, ParamValue>();
                Tsonic.CSharp.Js.JSArray<LanguageConfig> languages = new Tsonic.CSharp.Js.JSArray<LanguageConfig>(new LanguageConfig[] { });
                Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<MenuEntry>> menus = new Tsonic.CSharp.Js.Map<string, Tsonic.CSharp.Js.JSArray<MenuEntry>>();
                for (int index = 0; index < root.properties.length; index++)
                {
                    JsonProperty property = root.properties[index];
                    string key = Tsonic.CSharp.Js.String.toLowerCase(property.key);
                    JsonValue value = property.value;
                    if (key == "title")
                    {
                        title = requireString(property.key, value, sourcePath);
                    }
                    else
                    {
                        if (key == "baseurl")
                        {
                            baseURL = requireString(property.key, value, sourcePath);
                        }
                        else
                        {
                            if (key == "languagecode")
                            {
                                languageCode = requireString(property.key, value, sourcePath);
                                hasLanguageCode = true;
                            }
                            else
                            {
                                if (key == "contentdir")
                                {
                                    contentDir = requireString(property.key, value, sourcePath);
                                }
                                else
                                {
                                    if (key == "theme")
                                    {
                                        theme = requireString(property.key, value, sourcePath);
                                    }
                                    else
                                    {
                                        if (key == "copyright")
                                        {
                                            copyright = requireString(property.key, value, sourcePath);
                                        }
                                        else
                                        {
                                            if (key == "params")
                                            {
                                                JsonObject @object = requireObject(property.key, value, sourcePath);
                                                assertUniqueFields(@object, "Configuration params", sourcePath);
                                                for (int paramIndex = 0; paramIndex < @object.properties.length; paramIndex++)
                                                {
                                                    JsonProperty parameter = @object.properties[paramIndex];
                                                    @params.set(parameter.key, toParam(parameter.key, parameter.value, sourcePath));
                                                }
                                            }
                                            else
                                            {
                                                if (key == "languages")
                                                {
                                                    JsonObject object_1 = requireObject(property.key, value, sourcePath);
                                                    assertUniqueFields(object_1, "Configuration languages", sourcePath);
                                                    for (int languageIndex = 0; languageIndex < object_1.properties.length; languageIndex++)
                                                    {
                                                        JsonProperty language = object_1.properties[languageIndex];
                                                        JsonObject fields = requireObject(language.key, language.value, sourcePath);
                                                        assertUniqueFields(fields, $"Language '{language.key}'", sourcePath);
                                                        LanguageConfigBuilder builder = new LanguageConfigBuilder(language.key);
                                                        for (int fieldIndex = 0; fieldIndex < fields.properties.length; fieldIndex++)
                                                        {
                                                            JsonProperty field = fields.properties[fieldIndex];
                                                            applyLanguageField(builder, field.key, field.value, sourcePath);
                                                        }
                                                        languages.push(builder.toConfig());
                                                    }
                                                }
                                                else
                                                {
                                                    if (key == "menu")
                                                    {
                                                        JsonObject object_2 = requireObject(property.key, value, sourcePath);
                                                        assertUniqueFields(object_2, "Configuration menus", sourcePath);
                                                        for (int menuIndex = 0; menuIndex < object_2.properties.length; menuIndex++)
                                                        {
                                                            JsonProperty menu = object_2.properties[menuIndex];
                                                            JsonArray menuItems = requireArray(menu.key, menu.value, sourcePath);
                                                            Tsonic.CSharp.Js.JSArray<MenuEntry> entries = new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { });
                                                            for (int entryIndex = 0; entryIndex < menuItems.items.length; entryIndex++)
                                                            {
                                                                JsonValue entryValue = menuItems.items[entryIndex];
                                                                JsonObject fields_1 = requireObject($"{menu.key}[{entryIndex}]", entryValue, sourcePath);
                                                                assertUniqueFields(fields_1, $"Menu '{menu.key}' entry", sourcePath);
                                                                MenuEntryBuilder builder_1 = new MenuEntryBuilder(menu.key);
                                                                for (int fieldIndex_1 = 0; fieldIndex_1 < fields_1.properties.length; fieldIndex_1++)
                                                                {
                                                                    JsonProperty field_1 = fields_1.properties[fieldIndex_1];
                                                                    applyMenuField(builder_1, field_1.key, field_1.value, sourcePath);
                                                                }
                                                                entries.push(builder_1.toEntry());
                                                            }
                                                            menus.set(menu.key, Menus.buildMenuHierarchy(entries));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw Diagnostics.createTsumoError("TSUMO_CONFIG_UNKNOWN_FIELD", $"Unknown configuration field '{property.key}'", sourcePath, property.line, property.column);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                SiteConfig config = new SiteConfig(title, Utils_text.ensureTrailingSlash(baseURL), languageCode, theme, copyright);
                config.contentDir = contentDir;
                config.Params = @params;
                config.Menus = menus;
                if (languages.length > 0)
                {
                    config.languages = Config_helpers.sortLanguages(languages);
                    LanguageConfig selected = config.languages[0];
                    config.contentDir = selected.contentDir;
                    if (!hasLanguageCode)
                    {
                        config.languageCode = selected.lang;
                    }
                }
                return config;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
