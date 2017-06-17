using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using slf4net;

namespace RandomCodeOrg.Pluto.Resources {
    public class ApplicationResourceManager {
        
        private readonly string homePath;
        private readonly string resourcesPath;
        private readonly string viewsPath;
        private readonly string applicationName;

        private readonly IDictionary<string, ViewSource> views = new Dictionary<string, ViewSource>();

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

        public ApplicationResourceManager(string serverPath, string applicationName) {
            this.applicationName = applicationName;
            this.homePath = Path.Combine(serverPath, applicationName);
            Directory.CreateDirectory(homePath);
            this.resourcesPath = Path.Combine(homePath, "Resources");
            this.viewsPath = Path.Combine(homePath, "Views");
            Directory.CreateDirectory(resourcesPath);
        }


        public bool HasView(string path) {
            return views.ContainsKey(path);
        }

        public ViewSource GetView(string path) {
            if (HasView(path))
                return views[path];
            return null;
        }


        public void Load(Assembly assembly) {
            string basePrefix = assembly.GetName().Name;
            string resourcePrefix = basePrefix + ".Resources.";
            string viewsPrefix = basePrefix + ".Views.";
            string targetPath;
            foreach (string resource in assembly.GetManifestResourceNames()) {
                if (resource.StartsWith(resourcePrefix)) {
                    logger.Debug("Discovered resource: {0}", resource);
                    LoadResource(assembly, resourcesPath, resourcePrefix, resource);
                }
                if (resource.StartsWith(viewsPrefix)) {
                    logger.Debug("Discovered view: {0}", resource);
                    targetPath = LoadResource(assembly, viewsPath, viewsPrefix, resource);
                    LoadView(assembly, viewsPrefix, resource);
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
                using (Stream output = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.Write)) {
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
