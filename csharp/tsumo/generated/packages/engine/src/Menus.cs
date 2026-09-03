using System;

namespace Tsumo.Engine
{
    public static class Menus
    {
        public static Func<MenuEntry, string> menuEntryIdentity
        {
            get;
            private set;
        } = default(Func<MenuEntry, string>)!;
        public static Func<MenuEntry, MenuEntry, double> compareMenuEntries
        {
            get;
            private set;
        } = default(Func<MenuEntry, MenuEntry, double>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>> sortHierarchy
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>>)!;
        public static Action<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.Map<string, MenuEntry>> assertAcyclicParents
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.Map<string, MenuEntry>>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>> buildMenuHierarchy
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>>)!;
        public static Action<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>> appendFlatMenuEntries
        {
            get;
            private set;
        } = default(Action<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>>)!;
        public static Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>> flattenMenuEntries
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<MenuEntry>, Tsonic.CSharp.Js.JSArray<MenuEntry>>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Models.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            menuEntryIdentity = (MenuEntry entry) => Tsonic.CSharp.Js.String.trim(entry.identifier) != "" ? Tsonic.CSharp.Js.String.trim(entry.identifier) : Tsonic.CSharp.Js.String.trim(entry.name);
            compareMenuEntries = (MenuEntry left, MenuEntry right) =>
            {
                if (left.weight != right.weight)
                {
                    return left.weight - right.weight;
                }
                int identity = Utils_strings.compareText(menuEntryIdentity(left), menuEntryIdentity(right));
                if (identity != 0)
                {
                    return identity;
                }
                int name = Utils_strings.compareText(left.name, right.name);
                return name != 0 ? name : Utils_strings.compareText(left.url, right.url);
            };
            sortHierarchy = (Tsonic.CSharp.Js.JSArray<MenuEntry> entries) =>
            {
                entries.sort((MenuEntry left, MenuEntry right) => compareMenuEntries(left, right));
                for (int index = 0; index < entries.length; index++)
                {
                    MenuEntry entry = entries[index];
                    entry.children = sortHierarchy(entry.children);
                }
                return entries;
            };
            assertAcyclicParents = (Tsonic.CSharp.Js.JSArray<MenuEntry> entries, Tsonic.CSharp.Js.Map<string, MenuEntry> byIdentity) =>
            {
                for (int index = 0; index < entries.length; index++)
                {
                    Tsonic.CSharp.Js.Map<string, bool> visited = new Tsonic.CSharp.Js.Map<string, bool>();
                    MenuEntry current = entries[index];
                    while (Tsonic.CSharp.Js.String.trim(current.parent) != "")
                    {
                        string identity = menuEntryIdentity(current);
                        if (visited.has(identity))
                        {
                            throw Diagnostics.createTsumoError("TSUMO_MENU_PARENT_CYCLE", $"Menu parent cycle includes '{identity}'");
                        }
                        visited.set(identity, true);
                        MenuEntry? parent = Tsonic.CSharp.Js.Map.getReference<string, MenuEntry>(byIdentity, Tsonic.CSharp.Js.String.trim(current.parent));
                        if (parent is null)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_MENU_PARENT_NOT_FOUND", $"Menu entry '{identity}' names missing parent '{current.parent}'");
                        }
                        current = parent;
                    }
                }
            };
            buildMenuHierarchy = (Tsonic.CSharp.Js.JSArray<MenuEntry> entries) =>
            {
                Tsonic.CSharp.Js.Map<string, MenuEntry> byIdentity = new Tsonic.CSharp.Js.Map<string, MenuEntry>();
                for (int index = 0; index < entries.length; index++)
                {
                    MenuEntry entry = entries[index];
                    entry.children = new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { });
                    string identity = menuEntryIdentity(entry);
                    if (identity == "")
                    {
                        throw Diagnostics.createTsumoError("TSUMO_MENU_IDENTITY_REQUIRED", "Every menu entry requires an identifier or name");
                    }
                    if (byIdentity.has(identity))
                    {
                        throw Diagnostics.createTsumoError("TSUMO_MENU_IDENTITY_DUPLICATE", $"Duplicate menu entry identity: {identity}");
                    }
                    byIdentity.set(identity, entry);
                }
                assertAcyclicParents(entries, byIdentity);
                Tsonic.CSharp.Js.JSArray<MenuEntry> topLevel = new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { });
                for (int index_1 = 0; index_1 < entries.length; index_1++)
                {
                    MenuEntry entry_1 = entries[index_1];
                    string parentName = Tsonic.CSharp.Js.String.trim(entry_1.parent);
                    if (parentName == "")
                    {
                        topLevel.push(entry_1);
                    }
                    else
                    {
                        MenuEntry? parent = Tsonic.CSharp.Js.Map.getReference<string, MenuEntry>(byIdentity, parentName);
                        if (parent is null)
                        {
                            throw Diagnostics.createTsumoError("TSUMO_MENU_PARENT_NOT_FOUND", $"Menu entry '{menuEntryIdentity(entry_1)}' names missing parent '{parentName}'");
                        }
                        parent.children.push(entry_1);
                    }
                }
                return sortHierarchy(topLevel);
            };
            appendFlatMenuEntries = (Tsonic.CSharp.Js.JSArray<MenuEntry> entries, Tsonic.CSharp.Js.JSArray<MenuEntry> result) =>
            {
                for (int index = 0; index < entries.length; index++)
                {
                    MenuEntry entry = entries[index];
                    MenuEntry clone = new MenuEntry(entry.name, entry.url, entry.pageRef, entry.title, entry.weight, entry.parent, entry.identifier, entry.pre, entry.post, entry.menu, entry.Params);
                    clone.page = entry.page;
                    result.push(clone);
                    appendFlatMenuEntries(entry.children, result);
                }
            };
            flattenMenuEntries = (Tsonic.CSharp.Js.JSArray<MenuEntry> entries) =>
            {
                Tsonic.CSharp.Js.JSArray<MenuEntry> result = new Tsonic.CSharp.Js.JSArray<MenuEntry>(new MenuEntry[] { });
                appendFlatMenuEntries(entries, result);
                return result;
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
