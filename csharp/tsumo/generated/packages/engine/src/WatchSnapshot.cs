namespace Tsumo.Engine
{
    public static class WatchSnapshot
    {
        internal static void addFileState(Tsonic.CSharp.Js.Map<string, WatchEntryState> snapshot, string path)
        {
            Fs.rejectFilesystemLink(path);
            Tsonic.CSharp.Node.Stats stats = Tsonic.CSharp.Node.fs.statSync(path);
            snapshot.set(path, new WatchEntryState(stats.mtimeMs, stats.size));
        }
        public static Tsonic.CSharp.Js.Map<string, WatchEntryState> createWatchSnapshot(Tsonic.CSharp.Js.JSArray<string> targets)
        {
            Tsonic.CSharp.Js.Map<string, WatchEntryState> snapshot = new Tsonic.CSharp.Js.Map<string, WatchEntryState>();
            for (int i = 0; i < targets.length; i++)
            {
                string target = targets[i];
                if (Fs.fileExists(target))
                {
                    addFileState(snapshot, target);
                    continue;
                }
                if (!Fs.dirExists(target))
                {
                    continue;
                }
                Tsonic.CSharp.Js.JSArray<string> files = Fs.listFilesRecursive(target, "*");
                for (int j = 0; j < files.length; j++)
                {
                    addFileState(snapshot, files[j]);
                }
            }
            return snapshot;
        }
        public static bool watchSnapshotsEqual(Tsonic.CSharp.Js.Map<string, WatchEntryState> left, Tsonic.CSharp.Js.Map<string, WatchEntryState> right)
        {
            if (left.size != right.size)
            {
                return false;
            }
            foreach (string filePath in left.keys())
            {
                WatchEntryState? state = Tsonic.CSharp.Js.Map.getOptional<string, WatchEntryState>(left, filePath);
                WatchEntryState? other = Tsonic.CSharp.Js.Map.getOptional<string, WatchEntryState>(right, filePath);
                if (state is null || other is null || state.modifiedAt != other.modifiedAt || state.size != other.size)
                {
                    return false;
                }
            }
            return true;
        }
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
    public class WatchEntryState
    {
        public double modifiedAt;
        public long size;
        public WatchEntryState(double modifiedAt, long size)
        {
            this.modifiedAt = modifiedAt;
            this.size = size;
        }
    }
}
