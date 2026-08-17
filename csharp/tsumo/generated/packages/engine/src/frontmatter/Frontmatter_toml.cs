using System;

namespace Tsumo.Engine
{
    public static class Frontmatter_toml
    {
        public static Action<FrontMatterMenu, string, string, string?, int> applyMenuProperty
        {
            get;
            private set;
        } = default(Action<FrontMatterMenu, string, string, string?, int>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<string>, string?, FrontMatter> parseTomlFrontMatter
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<string>, string?, FrontMatter>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            Utils_structuredScalars.__tsonic_module_init();
            Frontmatter_data.__tsonic_module_init();
            Frontmatter_menu.__tsonic_module_init();
            Frontmatter_scalars.__tsonic_module_init();
            applyMenuProperty = (FrontMatterMenu entry, string keyRaw, string valueRaw, string? sourcePath, int line) =>
            {
                string key = Tsonic.CSharp.Js.String.toLowerCase(keyRaw);
                if (key == "weight")
                {
                    entry.weight = Frontmatter_scalars.parseFrontMatterInt(valueRaw, keyRaw, "toml", sourcePath, line);
                }
                else
                {
                    if (key == "name")
                    {
                        entry.name = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                    }
                    else
                    {
                        if (key == "parent")
                        {
                            entry.parent = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                        }
                        else
                        {
                            if (key == "identifier")
                            {
                                entry.identifier = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                            }
                            else
                            {
                                if (key == "pre")
                                {
                                    entry.pre = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                                }
                                else
                                {
                                    if (key == "post")
                                    {
                                        entry.post = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                                    }
                                    else
                                    {
                                        if (key == "title")
                                        {
                                            entry.title = Frontmatter_scalars.parseFrontMatterString(valueRaw, keyRaw, "toml", sourcePath, line);
                                        }
                                        else
                                        {
                                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_MENU_FIELD_UNKNOWN", $"Unknown front matter menu field '{keyRaw}'", sourcePath, line, 1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            parseTomlFrontMatter = (Tsonic.CSharp.Js.JSArray<string> lines, string? sourcePath) =>
            {
                FrontMatter frontMatter = new FrontMatter();
                string table = "";
                FrontMatterMenu? menuEntry = null;
                Tsonic.CSharp.Js.Set<string> rootFields = new Tsonic.CSharp.Js.Set<string>();
                Tsonic.CSharp.Js.Set<string> declaredTables = new Tsonic.CSharp.Js.Set<string>();
                Tsonic.CSharp.Js.Set<string> menuNames = new Tsonic.CSharp.Js.Set<string>();
                Tsonic.CSharp.Js.Set<string> tableFields = new Tsonic.CSharp.Js.Set<string>();
                Tsonic.CSharp.Js.Set<string> menuFields = new Tsonic.CSharp.Js.Set<string>();
                for (int index = 0; index < lines.length; index++)
                {
                    int lineNumber = index + 2;
                    string line = Tsonic.CSharp.Js.String.trim(Utils_structuredScalars.stripStructuredComment(lines[index], "toml"));
                    if (line == "")
                    {
                        continue;
                    }
                    if (Tsonic.CSharp.Js.String.startsWith(line, "[["))
                    {
                        if (!Tsonic.CSharp.Js.String.endsWith(line, "]]"))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_SYNTAX_INVALID", "Malformed TOML array table", sourcePath, lineNumber, 1);
                        }
                        table = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(line, 2, line.Length - 4)));
                        if (!Tsonic.CSharp.Js.String.startsWith(table, "menu.") || table.Length == "menu.".Length)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_TABLE_UNSUPPORTED", $"Unsupported front matter TOML array table '{table}'", sourcePath, lineNumber, 1);
                        }
                        Frontmatter_scalars.recordFrontMatterField(menuNames, Utils_strings.substringFrom(table, "menu.".Length), "Front matter menu", sourcePath, lineNumber);
                        menuEntry = new FrontMatterMenu(Utils_strings.substringFrom(table, "menu.".Length));
                        menuFields = new Tsonic.CSharp.Js.Set<string>();
                        frontMatter.menus.push(menuEntry);
                        continue;
                    }
                    if (Tsonic.CSharp.Js.String.startsWith(line, "["))
                    {
                        if (!Tsonic.CSharp.Js.String.endsWith(line, "]"))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_SYNTAX_INVALID", "Malformed TOML table", sourcePath, lineNumber, 1);
                        }
                        table = Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(line, 1, line.Length - 2)));
                        if (table != "params")
                        {
                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_TABLE_UNSUPPORTED", $"Unsupported front matter TOML table '{table}'", sourcePath, lineNumber, 1);
                        }
                        if (declaredTables.has(table))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_FIELD_DUPLICATE", $"Front matter table '{table}' is declared more than once", sourcePath, lineNumber, 1);
                        }
                        declaredTables.add(table);
                        tableFields = new Tsonic.CSharp.Js.Set<string>();
                        menuEntry = null;
                        continue;
                    }
                    int separator = Tsonic.CSharp.Js.String.indexOf(line, "=");
                    if (separator <= 0)
                    {
                        throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_SYNTAX_INVALID", "TOML front matter entries require 'key = value' syntax", sourcePath, lineNumber, 1);
                    }
                    string key = Tsonic.CSharp.Js.String.trim(Utils_strings.substringCount(line, 0, separator));
                    string value = Tsonic.CSharp.Js.String.trim(Utils_strings.substringFrom(line, separator + 1));
                    if (value == "")
                    {
                        throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_SYNTAX_INVALID", $"Front matter field '{key}' requires a value", sourcePath, lineNumber, 1);
                    }
                    if (menuEntry is not null && Tsonic.CSharp.Js.String.startsWith(table, "menu."))
                    {
                        Frontmatter_scalars.recordFrontMatterField(menuFields, key, $"Front matter menu '{menuEntry.menu}'", sourcePath, lineNumber);
                        applyMenuProperty(menuEntry, key, value, sourcePath, lineNumber);
                    }
                    else
                    {
                        if (table == "params")
                        {
                            Frontmatter_scalars.recordFrontMatterField(tableFields, key, "Front matter params", sourcePath, lineNumber);
                            frontMatter.Params.set(key, Frontmatter_scalars.parseFrontMatterParam(value, "toml", sourcePath, lineNumber));
                        }
                        else
                        {
                            if (table == "")
                            {
                                Frontmatter_scalars.recordFrontMatterField(rootFields, key, "Front matter", sourcePath, lineNumber);
                                Frontmatter_scalars.applyFrontMatterScalar(frontMatter, key, value, "toml", sourcePath, lineNumber);
                            }
                            else
                            {
                                throw Diagnostics.createTsumoError("TSUMO_FRONTMATTER_TOML_TABLE_UNSUPPORTED", $"Unsupported front matter TOML table '{table}'", sourcePath, lineNumber, 1);
                            }
                        }
                    }
                }
                return frontMatter;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
