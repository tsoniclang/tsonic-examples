using System;

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
        public static Func<Tsonic.CSharp.Js.JSArray<string>, Tsonic.CSharp.Js.Map<string, WatchEntryState>> createWatchSnapshot
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.JSArray<string>, Tsonic.CSharp.Js.Map<string, WatchEntryState>>)!;
        public static Func<Tsonic.CSharp.Js.Map<string, WatchEntryState>, Tsonic.CSharp.Js.Map<string, WatchEntryState>, bool> watchSnapshotsEqual
        {
            get;
            private set;
        } = default(Func<Tsonic.CSharp.Js.Map<string, WatchEntryState>, Tsonic.CSharp.Js.Map<string, WatchEntryState>, bool>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            Fs.__tsonic_module_init();
            createWatchSnapshot = (Tsonic.CSharp.Js.JSArray<string> targets) =>
            {
                Tsonic.CSharp.Js.Map<string, WatchEntryState> snapshot = new Tsonic.CSharp.Js.Map<string, WatchEntryState>();
                for (double i = 0; i < targets.length; i++)
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
                    for (double j = 0; j < files.length; j++)
                    {
                        addFileState(snapshot, files[j]);
                    }
                }
                return snapshot;
            };
            watchSnapshotsEqual = (Tsonic.CSharp.Js.Map<string, WatchEntryState> left, Tsonic.CSharp.Js.Map<string, WatchEntryState> right) =>
            {
                if (left.size != right.size)
                {
                    return false;
                }
                foreach (string filePath in left.keys())
                {
                    WatchEntryState? state = Tsonic.CSharp.Js.Map.getReference<string, WatchEntryState>(left, filePath);
                    WatchEntryState? other = Tsonic.CSharp.Js.Map.getReference<string, WatchEntryState>(right, filePath);
                    if (state is null || other is null || state.modifiedAt != other.modifiedAt || state.size != other.size)
                    {
                        return false;
                    }
                }
                return true;
            };
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
        public double size;
        public WatchEntryState(double modifiedAt, double size)
        {
            this.modifiedAt = modifiedAt;
            this.size = size;
        }
    }
}
