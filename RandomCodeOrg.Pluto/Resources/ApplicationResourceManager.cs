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


        /*
        private readonly string homePath;
        private readonly string resourcesPath;
        private readonly string includesPath;
        private readonly string viewsPath;
        private readonly string applicationName;*/

        private readonly SourceChangeMonitor sourceChangeMonitor = new SourceChangeMonitor();

        private readonly IDictionary<string, ViewSource> views = new Dictionary<string, ViewSource>();
        private readonly IDictionary<string, XmlDocument> includeDocuments = new Dictionary<string, XmlDocument>();
        private readonly IDictionary<string, MemoryStream> staticResources = new Dictionary<string, MemoryStream>();



        protected bool IsDebugging {
            get {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }

        /*
        public string HomePath {
            get {
                return homePath;
            }
        }

        public string ResourcesPath {
            get {
                return resourcesPath;
            }
        }*/

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(ApplicationResourceManager));
        private readonly SourceFileFinder sff = new SourceFileFinder();

        private IApplicationHandle appHandle;

        public ApplicationResourceManager(IApplicationHandle appHandle) {
            this.appHandle = appHandle;
        }


        public bool HasView(string path) {
            string localPath = appHandle.TranslateViewPath(path);
            return views.ContainsKey(localPath);
        }

        public ViewSource GetView(string path) {
            string localPath = appHandle.TranslateViewPath(path);
            if (views.ContainsKey(localPath))
                return views[localPath];
            return null;
        }

        public XmlDocument IncludeDocument(string path) {
            string localPath = appHandle.TranslateIncludePath(path);
            if (includeDocuments.ContainsKey(localPath))
                return (XmlDocument)includeDocuments[localPath].CloneNode(true);
            return null;
        }

        public bool StaticResource(string path, Stream target) {
            string localPath = appHandle.TranslateStaticPath(path);
            if (staticResources.ContainsKey(localPath)) {
                MemoryStream source = staticResources[localPath];
                lock (source) {
                    source.CopyTo(target);
                    source.Position = 0;
                }
                return true;
            }
            return false;
        }


        public Stream StaticResource(string path) {
            MemoryStream ms = new MemoryStream();
            if(StaticResource(path, ms)) {
                return ms;
            }
            ms.Dispose();
            return null;
        }

        public bool HasStaticResource(string path) {
            string localPath = appHandle.TranslateStaticPath(path);
            return staticResources.ContainsKey(localPath);
        }


        public void Load() {
            foreach (string viewResource in appHandle.ViewResources) {
                LoadView(viewResource);
                MonitorResource(viewResource, LoadView);
            }
            foreach (string includeResource in appHandle.IncludeResources) {
                LoadInclude(includeResource);
                MonitorResource(includeResource, LoadInclude);
            }
            foreach (string staticResource in appHandle.StaticResources) {
                LoadStaticResource(staticResource);
                MonitorResource(staticResource, LoadStaticResource);
            }
        }

        private void LoadStaticResource(string path) {
            MemoryStream ms = new MemoryStream();
            using (Stream input = appHandle.OpenResource(path)) {
                input.CopyTo(ms);
            }
            ms.Position = 0;
            staticResources[path] = ms;
        }
        
        private void LoadView(string path) {
            using (Stream input = appHandle.OpenResource(path)) {
                views[path] = new ViewSource(input);
            }
        }

        private void LoadInclude(string path) {
            using (Stream input = appHandle.OpenResource(path)) {
                XmlDocument document = new XmlDocument();
                document.Load(input);
                includeDocuments[path] = document;
            }
        }

        private void MonitorResource(string path, Action<string> updateAction) {
            if (!IsDebugging)
                return;
            appHandle.ObserveResource(path, (sender, rsc) => updateAction(rsc));
        }
        
        /*

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




        [Obsolete("", false)]
        protected string BuildResourcePath(string prefix, string resourceName) {
            string suffix = resourceName.Substring(prefix.Length);
            string result = "";
            string[] parts = suffix.Split('.');
            for (int i = 0; i < parts.Length; i++) {
                if (i == parts.Length - 2) {
                    result += String.Format("/{0}.{1}", parts[i], parts[i + 1]);
                    break;
                } else {
                    result += String.Format("/{0}", parts[i]);
                }
            }
            return result;
        }
        */


    }
}
