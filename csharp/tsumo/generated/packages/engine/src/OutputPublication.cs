using System;

namespace Tsumo.Engine
{
    public static class OutputPublicationModule
    {
        public static Func<string, string, bool, OutputPublication> beginOutputPublication
        {
            get;
            private set;
        } = default(Func<string, string, bool, OutputPublication>)!;
        public static Action<string, string, string, string> recoverOutputPublication
        {
            get;
            private set;
        } = default(Action<string, string, string, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Diagnostics.__tsonic_module_init();
            Fs.__tsonic_module_init();
            Utils_paths.__tsonic_module_init();
            beginOutputPublication = (string siteDir, string requestedDestinationDir, bool preserveExistingOutput) =>
            {
                string siteRoot = Tsonic.CSharp.Node.path.resolve(siteDir);
                string destinationDir = Tsonic.CSharp.Node.path.isAbsolute(requestedDestinationDir) ? Tsonic.CSharp.Node.path.resolve(requestedDestinationDir) : Tsonic.CSharp.Node.path.resolve(siteRoot, requestedDestinationDir);
                if (!Tsonic.CSharp.Node.path.isAbsolute(requestedDestinationDir) && !Utils_paths.pathContainsOrEquals(siteRoot, destinationDir))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_DESTINATION_ESCAPES_SITE", $"Relative output directory escapes the site root: {requestedDestinationDir}");
                }
                if (Utils_paths.pathContainsOrEquals(destinationDir, siteRoot))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_DESTINATION_CONTAINS_SITE", $"Output directory cannot contain the source site: {destinationDir}");
                }
                string parent = Tsonic.CSharp.Node.path.dirname(destinationDir);
                if (parent == destinationDir)
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_DESTINATION_IS_ROOT", $"Output directory cannot be a filesystem root: {destinationDir}");
                }
                if (Fs.fileExists(destinationDir))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_DESTINATION_IS_FILE", $"Output directory path names an existing file: {destinationDir}");
                }
                Fs.ensureDir(parent);
                string key = Tsonic.CSharp.Js.String.slice(Tsonic.CSharp.Node.crypto.createHash("sha256").update(destinationDir).digest("hex"), 0, 24);
                string scratchPrefix = $".tsumo-output-{key}";
                string backupDir = Tsonic.CSharp.Node.path.resolve(parent, $"{scratchPrefix}.backup");
                string stagePrefix = Tsonic.CSharp.Node.path.resolve(parent, $"{scratchPrefix}.stage-");
                recoverOutputPublication(destinationDir, backupDir, parent, $"{scratchPrefix}.stage-");
                string stagingDir = Tsonic.CSharp.Node.fs.mkdtempSync(stagePrefix);
                if (preserveExistingOutput && Fs.dirExists(destinationDir))
                {
                    Fs.copyDirRecursive(destinationDir, stagingDir);
                }
                return new OutputPublication(destinationDir, stagingDir, backupDir);
            };
            recoverOutputPublication = (string destinationDir, string backupDir, string parentDir, string stageNamePrefix) =>
            {
                if (Fs.fileExists(backupDir))
                {
                    throw Diagnostics.createTsumoError("TSUMO_OUTPUT_BACKUP_IS_FILE", $"Output publication backup path names an existing file: {backupDir}");
                }
                if (Fs.dirExists(backupDir))
                {
                    if (Fs.dirExists(destinationDir))
                    {
                        Tsonic.CSharp.Node.fs.rmSync(backupDir, true);
                    }
                    else
                    {
                        Tsonic.CSharp.Node.fs.renameSync(backupDir, destinationDir);
                    }
                }
                string[] entries = Tsonic.CSharp.Node.fs.readdirSync(parentDir);
                for (int index = 0; index < entries.Length; index++)
                {
                    string entry = entries[index];
                    if (Tsonic.CSharp.Js.String.startsWith(entry, stageNamePrefix))
                    {
                        Tsonic.CSharp.Node.fs.rmSync(Tsonic.CSharp.Node.path.resolve(parentDir, entry), true);
                    }
                }
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class OutputPublication
    {
        public string destinationDir;
        public string stagingDir;
        public string backupDir;
        public OutputPublication(string destinationDir, string stagingDir, string backupDir)
        {
            this.destinationDir = destinationDir;
            this.stagingDir = stagingDir;
            this.backupDir = backupDir;
        }
        public void publish()
        {
            bool previousOutputMoved = false;
            if (Fs.dirExists(this.destinationDir))
            {
                Tsonic.CSharp.Node.fs.renameSync(this.destinationDir, this.backupDir);
                previousOutputMoved = true;
            }
            try
            {
                Tsonic.CSharp.Node.fs.renameSync(this.stagingDir, this.destinationDir);
            }
            catch
            {
                if (previousOutputMoved && !Tsonic.CSharp.Node.fs.existsSync(this.destinationDir))
                {
                    Tsonic.CSharp.Node.fs.renameSync(this.backupDir, this.destinationDir);
                }
                throw;
            }
            if (previousOutputMoved && Fs.dirExists(this.backupDir))
            {
                Tsonic.CSharp.Node.fs.rmSync(this.backupDir, true);
            }
        }
        public void abort()
        {
            if (Fs.dirExists(this.stagingDir))
            {
                Tsonic.CSharp.Node.fs.rmSync(this.stagingDir, true);
            }
            if (!Fs.dirExists(this.backupDir))
            {
                return;
            }
            if (Fs.dirExists(this.destinationDir))
            {
                Tsonic.CSharp.Node.fs.rmSync(this.backupDir, true);
            }
            else
            {
                Tsonic.CSharp.Node.fs.renameSync(this.backupDir, this.destinationDir);
            }
        }
    }
}
