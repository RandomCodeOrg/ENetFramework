using slf4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Debugging {
    public class SourceChangeMonitor : IDisposable {

        public delegate void ResourceChangedDelegate(SourceChangeMonitor sender, string resourceKey, string location);


        private readonly SourceFileFinder sourceFileFinder;

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(SourceChangeMonitor));


        // Directory to observe <--> File system watcher
        private readonly IDictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();

        // Resource to observe <--> Callbacks
        private readonly IDictionary<string, ISet<ResourceChangedDelegate>> callbacks = new Dictionary<string, ISet<ResourceChangedDelegate>>();

        // File path <--> Resource to observe
        private readonly IDictionary<string, string> fileResourceMapping = new Dictionary<string, string>();

        public SourceChangeMonitor(SourceFileFinder sourceFileFinder) {
            this.sourceFileFinder = sourceFileFinder;
        }

        public SourceChangeMonitor() : this(new SourceFileFinder()) {

        }

        public bool Monitor(Assembly assembly, string resource, ResourceChangedDelegate callback) {
            if (callbacks.ContainsKey(resource)) {
                callbacks[resource].Add(callback);
            }
            string location = sourceFileFinder.GetSourceLocation(assembly, resource);
            if (location == null)
                return false;

            string directory = Directory.GetParent(location).FullName;
            if (!Directory.Exists(directory))
                return false;

            if (!watchers.ContainsKey(directory)) {
                FileSystemWatcher fsWatcher = new FileSystemWatcher(directory);
                fsWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size;
                fsWatcher.Changed += FsWatcher_Changed;
                watchers[directory] = fsWatcher;
                fsWatcher.EnableRaisingEvents = true;
            }

            AddFile(resource, location, callback);
            return true;
        }

        private void FsWatcher_Changed(object sender, FileSystemEventArgs e) {
            string filePath = e.FullPath;
            logger.Trace("A file system object changed: {0}", filePath);
            if (fileResourceMapping.ContainsKey(filePath)) {
                string resource = fileResourceMapping[filePath];
                if (callbacks.ContainsKey(resource)) {
                    foreach(ResourceChangedDelegate callback in callbacks[resource]) {
                        callback(this, resource, filePath);
                    }
                }
            }
        }

        protected void AddFile(string resource, string file, ResourceChangedDelegate callback) {
            fileResourceMapping[file] = resource;
            if (!callbacks.ContainsKey(resource)) {
                callbacks[resource] = new HashSet<ResourceChangedDelegate>();
            }
            callbacks[resource].Add(callback);
        }

        public void Dispose() {
            foreach(FileSystemWatcher fsw in watchers.Values) {
                fsw.EnableRaisingEvents = false;
                fsw.Dispose();
            }

        }
    }
}
