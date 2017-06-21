using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using slf4net;
using System.Xml;
using RandomCodeOrg.Pluto.Debugging;


namespace RandomCodeOrg.Pluto.Resources {
    public class ApplicationResourceManager {
        
        private readonly string homePath;
        private readonly string resourcesPath;
        private readonly string includesPath;
        private readonly string viewsPath;
        private readonly string applicationName;

        private readonly SourceChangeMonitor sourceChangeMonitor = new SourceChangeMonitor();

        private readonly IDictionary<string, ViewSource> views = new Dictionary<string, ViewSource>();
        private readonly IDictionary<string, XmlDocument> includeDocuments = new Dictionary<string, XmlDocument>();
        


        protected bool IsDebugging {
            get {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }

        public string HomePath {
            get {
                return homePath;
            }
        }

        public string ResourcesPath {
            get {
                return resourcesPath;
            }
        }

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(ApplicationResourceManager));
        private readonly SourceFileFinder sff = new SourceFileFinder();
        

        public ApplicationResourceManager(string serverPath, string applicationName) {
            this.applicationName = applicationName;
            this.homePath = Path.Combine(serverPath, applicationName);
            Directory.CreateDirectory(homePath);
            this.resourcesPath = Path.Combine(homePath, "Resources");
            this.viewsPath = Path.Combine(homePath, "Views");
            Directory.CreateDirectory(resourcesPath);
            Directory.CreateDirectory(viewsPath);
            this.includesPath = Path.Combine(homePath, "Includes");
            Directory.CreateDirectory(includesPath);
        }


        public bool HasView(string path) {
            return views.ContainsKey(path);
        }

        public ViewSource GetView(string path) {
            if (HasView(path))
                return views[path];
            return null;
        }
    
        public XmlDocument IncludeDocument(string path) {
            if (path.StartsWith("/")) path = path.Substring(1);
            string localPath = Path.Combine(includesPath, path);
            localPath = Path.GetFullPath(localPath);
            if (!includeDocuments.ContainsKey(localPath) && File.Exists(localPath) && localPath.EndsWith(".xml")) {
                using(FileStream fs = new FileStream(localPath, FileMode.Open)) {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fs);
                    includeDocuments[localPath] = doc;
                }
            }
            if (!includeDocuments.ContainsKey(localPath))
                return null;
            return (XmlDocument) includeDocuments[localPath].CloneNode(true);
        }


        public void Load(Assembly assembly) {
            string basePrefix = assembly.GetName().Name;
            string resourcePrefix = basePrefix + ".Resources.";
            string viewsPrefix = basePrefix + ".Views.";
            string includesPrefix = basePrefix + ".Includes.";
            foreach (string resource in assembly.GetManifestResourceNames()) {
                string targetPath;
                if (resource.StartsWith(resourcePrefix)) {
                    logger.Debug("Discovered resource: {0}", resource);
                    targetPath = LoadResource(assembly, resourcesPath, resourcePrefix, resource);
                    MonitorResource(assembly, resource, (sender, resKey, filePath) => OnResourceChanged(sender, resKey, filePath, targetPath));
                } else if (resource.StartsWith(viewsPrefix)) {
                    logger.Debug("Discovered view: {0}", resource);
                    targetPath = LoadResource(assembly, viewsPath, viewsPrefix, resource);
                    LoadView(assembly, viewsPrefix, resource);
                    MonitorResource(assembly, resource, (sender, resKey, filePath) => OnViewChanged(sender, resKey, filePath, viewsPrefix));
                    
                }
                if (resource.StartsWith(includesPrefix)) {
                    logger.Debug("Discovered include: {0}", resource);
                    targetPath = LoadResource(assembly, includesPath, includesPrefix, resource);
                    MonitorResource(assembly, resource, (sender, resKey, filePath) => OnResourceChanged(sender, resKey, filePath, targetPath));
                }
            }
        }

        private void MonitorResource(Assembly assembly, string source, SourceChangeMonitor.ResourceChangedDelegate callback) {
            if (!IsDebugging)
                return;
            sourceChangeMonitor.Monitor(assembly, source, callback);
        }

        private void OnViewChanged(object sender, string resourceKey, string filePath, string prefix) {
            logger.Info("Reloading modified view '{0}' from path: {1}", resourceKey, filePath);
            string resourcePath = BuildResourcePath(prefix, resourceKey);
            using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
                views[resourcePath] = new ViewSource(fs);
            }
        }

        private void OnResourceChanged(object sender, string resourceKey, string filePath, string targetPath) {
            logger.Info("Reloading modified resource '{0}' from path: {1}", resourceKey, filePath);
            logger.Debug("Overwriting {0}...", targetPath);
            using (FileStream input = new FileStream(filePath, FileMode.Open)) {
                using (FileStream output = new FileStream(targetPath, FileMode.Create, FileAccess.Write)) {
                    input.CopyTo(output);
                }
            }
        }


        protected void LoadView(Assembly assembly, string prefix, string resourceKey) {
            string resourcePath = BuildResourcePath(prefix, resourceKey);
            using(Stream input = assembly.GetManifestResourceStream(resourceKey)) {
                views[resourcePath] = new ViewSource(input);
            }
        }

        protected string LoadResource(Assembly assembly, string parentPath, string resourcePrefix, string resourceKey) {
            string targetPath = BuildResourcePath(parentPath, resourcePrefix, resourceKey);
            if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            using(Stream input = assembly.GetManifestResourceStream(resourceKey)) {
                using (Stream output = new FileStream(targetPath, FileMode.Create, FileAccess.Write)) {
                    input.CopyTo(output);
                }
            }
            return targetPath;
        }

        public void Unload() {
            DeleteRecursive(homePath);
        }


        protected string BuildResourcePath(string prefix, string resourceName) {
            string suffix = resourceName.Substring(prefix.Length);
            string result = "";
            string[] parts = suffix.Split('.');
            for (int i = 0; i < parts.Length; i++) {
                if (i == parts.Length - 2) {
                    result +=  String.Format("/{0}.{1}",  parts[i] , parts[i + 1]);
                    break;
                } else {
                    result += String.Format("/{0}", parts[i]);
                }
            }
            return result;
        }

        protected string BuildResourcePath(string basePath, string prefix, string resourceName) {
            string suffix = resourceName.Substring(prefix.Length);
            string result = basePath;
            string[] parts = suffix.Split('.');
            for (int i = 0; i < parts.Length; i++) {
                if (i == parts.Length - 2) {
                    result = Path.Combine(result, parts[i] + "." + parts[i + 1]);
                    break;
                } else {
                    result = Path.Combine(result, parts[i]);
                }
            }
            return result;
        }

        private void DeleteRecursive(string path) {
            if (!File.Exists(path) && !Directory.Exists(path))
                return;
            if (File.Exists(path)) {
                File.Delete(path);
                return;
            }
            if (Directory.Exists(path)) {
                foreach (string childFile in Directory.GetFiles(path)) {
                    DeleteRecursive(childFile);
                }
                foreach (string childDir in Directory.GetDirectories(path)) {
                    DeleteRecursive(childDir);
                }
                try {
                    Directory.Delete(path);
                } catch (IOException) {
                    Directory.Delete(path);
                } catch (UnauthorizedAccessException) {
                    Directory.Delete(path);
                }

            }
        }

    }
}
