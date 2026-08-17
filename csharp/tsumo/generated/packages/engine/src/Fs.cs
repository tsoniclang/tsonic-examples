using System;

namespace Tsumo.Engine
{
    public static class Fs
    {
        public static Func<string, string, bool> matchesPattern
        {
            get;
            private set;
        } = default(Func<string, string, bool>)!;
        public static Func<string, bool> dirExists
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Func<string, bool> fileExists
        {
            get;
            private set;
        } = default(Func<string, bool>)!;
        public static Action<string> ensureDir
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Func<string, string> readTextFile
        {
            get;
            private set;
        } = default(Func<string, string>)!;
        public static Func<string, Tsonic.CSharp.Node.Buffer> readBinaryFile
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Node.Buffer>)!;
        public static Action<string, string> writeTextFile
        {
            get;
            private set;
        } = default(Action<string, string>)!;
        public static Action<string> deleteDirRecursive
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Action<string> rejectFilesystemLink
        {
            get;
            private set;
        } = default(Action<string>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry>> listManagedDirectoryEntries
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry>>)!;
        public static Func<string, string, Tsonic.CSharp.Js.JSArray<string>> listFilesTopDirectory
        {
            get;
            private set;
        } = default(Func<string, string, Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Func<string, Tsonic.CSharp.Js.JSArray<string>> listDirectoriesTopDirectory
        {
            get;
            private set;
        } = default(Func<string, Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Func<string, string, Tsonic.CSharp.Js.JSArray<string>> listFilesRecursive
        {
            get;
            private set;
        } = default(Func<string, string, Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Action<string, string> copyDirRecursive
        {
            get;
            private set;
        } = default(Action<string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Utils_strings.__tsonic_module_init();
            matchesPattern = (string filePath, string searchPattern) =>
            {
                if (searchPattern == "*" || searchPattern == "*.*")
                {
                    return true;
                }
                if (Tsonic.CSharp.Js.String.startsWith(searchPattern, "*."))
                {
                    return Tsonic.CSharp.Js.String.endsWith(Tsonic.CSharp.Js.String.toLowerCase(filePath), Tsonic.CSharp.Js.String.toLowerCase(Tsonic.CSharp.Js.String.substring(searchPattern, 1)));
                }
                return Tsonic.CSharp.Js.String.endsWith(filePath, searchPattern);
            };
            dirExists = (string path) =>
            {
                return Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsDirectory();
            };
            fileExists = (string path) =>
            {
                return Tsonic.CSharp.Node.fs.existsSync(path) && Tsonic.CSharp.Node.fs.statSync(path).IsFile();
            };
            ensureDir = (string path) =>
            {
                Tsonic.CSharp.Node.fs.mkdirSync(path, true);
            };
            readTextFile = (string path) =>
            {
                rejectFilesystemLink(path);
                return Tsonic.CSharp.Node.fs.readFileSync(path, "utf-8");
            };
            readBinaryFile = (string path) =>
            {
                rejectFilesystemLink(path);
                return Tsonic.CSharp.Node.fs.readFileSync(path);
            };
            writeTextFile = (string path, string content) =>
            {
                string dir = Tsonic.CSharp.Node.path.dirname(path);
                if (dir != "")
                {
                    Tsonic.CSharp.Node.fs.mkdirSync(dir, true);
                }
                Tsonic.CSharp.Node.fs.writeFileSync(path, content, "utf-8");
            };
            deleteDirRecursive = (string path) =>
            {
                if (!dirExists(path))
                {
                    return;
                }
                Tsonic.CSharp.Node.fs.rmSync(path, true);
            };
            rejectFilesystemLink = (string path) =>
            {
                System.IO.FileAttributes attributes = System.IO.File.GetAttributes(path);
                if ((attributes & System.IO.FileAttributes.ReparsePoint) != System.IO.FileAttributes.ReparsePoint)
                {
                    return;
                }
                throw Diagnostics.createTsumoError("TSUMO_FILESYSTEM_LINK_UNSUPPORTED", "Symbolic links and filesystem reparse points are not supported in Tsumo-managed filesystem trees", path);
            };
            listManagedDirectoryEntries = (string directory) =>
            {
                if (!dirExists(directory))
                {
                    return new Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry>(new ManagedDirectoryEntry[] { });
                }
                rejectFilesystemLink(directory);
                string[] names = Tsonic.CSharp.Node.fs.readdirSync(directory);
                Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry> entries = new Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry>(new ManagedDirectoryEntry[] { });
                for (int index = 0; index < names.Length; index++)
                {
                    string path = Tsonic.CSharp.Node.path.join(directory, names[index]);
                    rejectFilesystemLink(path);
                    entries.push(new ManagedDirectoryEntry(path, Tsonic.CSharp.Node.fs.statSync(path).IsDirectory()));
                }
                entries.sort((ManagedDirectoryEntry left, ManagedDirectoryEntry right) => Utils_strings.compareText(left.path, right.path));
                return entries;
            };
            listFilesTopDirectory = (string rootDir, string searchPattern) =>
            {
                Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry> entries = listManagedDirectoryEntries(rootDir);
                Tsonic.CSharp.Js.JSArray<string> files = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int index = 0; index < entries.length; index++)
                {
                    ManagedDirectoryEntry entry = entries[index];
                    if (!entry.directory && matchesPattern(entry.path, searchPattern))
                    {
                        files.push(entry.path);
                    }
                }
                return files;
            };
            listDirectoriesTopDirectory = (string rootDir) =>
            {
                Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry> entries = listManagedDirectoryEntries(rootDir);
                Tsonic.CSharp.Js.JSArray<string> directories = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                for (int index = 0; index < entries.length; index++)
                {
                    ManagedDirectoryEntry entry = entries[index];
                    if (entry.directory)
                    {
                        directories.push(entry.path);
                    }
                }
                return directories;
            };
            listFilesRecursive = (string rootDir, string searchPattern) =>
            {
                Tsonic.CSharp.Js.JSArray<string> files = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                Action<string> walk = default(Action<string>)!;
                walk = (string currentDir) =>
                {
                    Tsonic.CSharp.Js.JSArray<ManagedDirectoryEntry> entries = listManagedDirectoryEntries(currentDir);
                    for (int index = 0; index < entries.length; index++)
                    {
                        ManagedDirectoryEntry entry = entries[index];
                        if (entry.directory)
                        {
                            walk(entry.path);
                            continue;
                        }
                        if (matchesPattern(entry.path, searchPattern))
                        {
                            files.push(entry.path);
                        }
                    }
                };
                walk(rootDir);
                files.sort((string left, string right) => Utils_strings.compareText(left, right));
                return files;
            };
            copyDirRecursive = (string srcDir, string destDir) =>
            {
                if (!dirExists(srcDir))
                {
                    return;
                }
                ensureDir(destDir);
                Tsonic.CSharp.Js.JSArray<string> files = listFilesRecursive(srcDir, "*");
                for (int i = 0; i < files.length; i++)
                {
                    string srcFile = files[i];
                    string relPath = Tsonic.CSharp.Node.path.relative(srcDir, srcFile);
                    string destFile = Tsonic.CSharp.Node.path.join(destDir, relPath);
                    ensureDir(Tsonic.CSharp.Node.path.dirname(destFile));
                    Tsonic.CSharp.Node.fs.copyFileSync(srcFile, destFile);
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class ManagedDirectoryEntry
    {
        public string path;
        public bool directory;
        public ManagedDirectoryEntry(string path, bool directory)
        {
            this.path = path;
            this.directory = directory;
        }
    }
}
