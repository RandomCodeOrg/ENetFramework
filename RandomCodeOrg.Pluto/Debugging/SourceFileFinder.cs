using slf4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Debugging {
    public class SourceFileFinder {

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(SourceFileFinder));

        private int maxDepth = 3;
        
        public int MaxDepth {
            get {
                return maxDepth;
            }
        }


        private string projectFileEnding = ".csproj";

        public string ProjectFileEnding {
            get {
                return projectFileEnding;
            }
            set {
                projectFileEnding = value;
            }
        }


        public SourceFileFinder() {

        }

        public string GetSourceLocation(Type type) {
            string result = DoGetSourceLocation(type);
            if (result != null) {
                logger.Debug("Found source file for '{0}' at: {1}", type.FullName, result);
            } else {
                logger.Debug("Could not find the source file for '{0}'", type.FullName);
            }
            return result;
        }

        protected string DoGetSourceLocation(Type type) {
            return DoGetSourceLocation(type.Assembly, type.FullName);
        }
        

        public string GetSourceLocation(Assembly assembly, string resourceKey) {
            string result = DoGetSourceLocation(assembly, resourceKey);
            if(result != null) {
                logger.Debug("Found source file for '{0}' at: {1}", resourceKey, result);
            } else {
                logger.Debug("Could not find the source file for '{0}'", resourceKey);
            }
            return result;
        }

        protected string DoGetSourceLocation(Assembly assembly, string resourceKey) {
            string location = assembly.Location;
            string[] components = GetPathComponents(resourceKey);
            logger.Debug("Try to find the sources for '{0}'...", resourceKey);
            string result;
            if (File.Exists(location)) {
                location = Directory.GetParent(location).FullName;
            }
            for(int i=0; i<maxDepth; i++) {
                if (!Directory.Exists(location))
                    return null;
                if (TryGetResourceLocation(location, resourceKey, components, out result))
                    return result;
                location = Directory.GetParent(location).FullName;
            }
            return null;
        }

        protected string[] GetPathComponents(string resourceKey) {
            string[] rawComponents = resourceKey.Split('.');
            IList<string> pathComponents = new List<string>();
            for (int i = 0; i < rawComponents.Length - 1; i++) {
                if (i == rawComponents.Length - 2) {
                    pathComponents.Add(string.Format("{0}.{1}", rawComponents[i], rawComponents[i + 1]));
                } else {
                    pathComponents.Add(rawComponents[i]);
                }
            }
            return pathComponents.ToArray();
        }

        protected string BuildSubPath(string root, string[] subComponents, int offset) {
            string result = root;
            for(int i=offset; i<subComponents.Length; i++) {
                result = Path.Combine(result, subComponents[i]);
            }
            return result;
        }

        protected bool TryGetResourceLocation(string current, string resourceKey, string[] resourceComponents, out string result) {
            string path;
            for(int i=0; i<resourceComponents.Length; i++) {
                path = BuildSubPath(current, resourceComponents, i);
                if (File.Exists(path)) {
                    result = path;
                    return true;
                }
            }

            result = null;
            return false;
        }


    }
}
