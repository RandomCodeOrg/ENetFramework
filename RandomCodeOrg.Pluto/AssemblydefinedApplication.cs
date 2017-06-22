using RandomCodeOrg.ENetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace RandomCodeOrg.Pluto {
    public class AssemblydefinedApplication : IApplicationHandle {


        private readonly Assembly assembly;
        private readonly string applicationNamespace;
        private readonly AssemblyName assemblyName;
        private readonly string assemblyVersion;
        private readonly string assemblyLocation;
        private readonly IReadOnlyCollection<string> viewResources;
        private readonly IReadOnlyCollection<string> includeResources;
        private readonly IReadOnlyCollection<string> staticResources;

        private readonly string viewRoot;
        private readonly string includeRoot;
        private readonly string staticRoot;

        public string ApplicationNamespace => applicationNamespace;

        public Assembly DefiningAssembly => assembly;

        public string FriendlyName => assemblyName.Name;

        public string TechnicalName => assemblyName.Name;

        public string Version => assemblyVersion;

        public string Location => assemblyLocation;

        public IReadOnlyCollection<string> ViewResources => viewResources;

        public IReadOnlyCollection<string> IncludeResources => includeResources;

        public IReadOnlyCollection<string> StaticResources => staticResources;

        public AssemblydefinedApplication(Assembly assembly) {
            this.assembly = assembly;
            applicationNamespace = GetApplicationNamespace(assembly);
            assemblyName = assembly.GetName();
            assemblyVersion = assemblyName.Version.ToString();
            assemblyLocation = assembly.Location;


            var resourceNames = assembly.GetManifestResourceNames();

            string viewsPrefix = string.Format("{0}.Views.", ApplicationNamespace);
            viewRoot = GetResourcePath(string.Format("{0}.Views", ApplicationNamespace), false);
            viewResources = new ReadOnlyCollection<string>(resourceNames.Where(e => e.StartsWith(viewsPrefix)).Select(e => GetResourcePath(e)).ToList());

            string includePrefix = string.Format("{0}.Includes.", ApplicationNamespace);
            includeRoot = GetResourcePath(string.Format("{0}.Includes", ApplicationNamespace), false);
            includeResources = new ReadOnlyCollection<string>(resourceNames.Where(e => e.StartsWith(includePrefix)).Select(e => GetResourcePath(e)).ToList());

            string staticPrefix = string.Format("{0}.Resources.", ApplicationNamespace);
            staticRoot = GetResourcePath(string.Format("{0}.Resources", ApplicationNamespace), false);
            staticResources = new ReadOnlyCollection<string>(resourceNames.Where(e => e.StartsWith(staticPrefix)).Select(e => GetResourcePath(e)).ToList());
        }


        protected string GetResourcePath(string assemblyIdentifier, bool treatLastComponentAsFileExtension = true) {
            StringBuilder sb = new StringBuilder();
            var components = assemblyIdentifier.Split('.');
            for (int i = 0; i < components.Length; i++) {
                if (treatLastComponentAsFileExtension && i == components.Length - 1)
                    sb.AppendFormat(".{0}", components[i]);
                else
                    sb.AppendFormat("/{0}", components[i]);
            }
            return sb.ToString();
        }

        protected string GetAssemblyIdentifier(string path) {
            if (path.StartsWith("/"))
                path = path.Substring(1);
            return path.Replace('/', '.');
        }

        private string GetApplicationNamespace(Assembly assembly) {
            var entryPoint = assembly.EntryPoint;
            if (entryPoint != null)
                return entryPoint.DeclaringType.Namespace;

            int currentCount = int.MaxValue;
            string currentVal = null;
            int current;

            foreach (Type t in assembly.GetTypes()) {
                if ((current = CountNamespaceComponents(t.Namespace)) < currentCount) {
                    currentCount = current;
                    currentVal = t.Namespace;
                }
            }

            if (currentVal != null)
                return currentVal;

            string tmp;
            foreach (string resourceName in assembly.GetManifestResourceNames()) {
                tmp = resourceName;
                if (tmp.Contains('.'))
                    tmp = tmp.Substring(0, tmp.LastIndexOf('.')); // Removes file extensions (if present)
                if ((current = CountNamespaceComponents(tmp)) < currentCount) {
                    currentCount = current;
                    currentVal = tmp;
                }
            }

            if (currentVal == null)
                throw new ApplicationValidationException("Can't deploy an empty application.");

            return currentVal;
        }

        private int CountNamespaceComponents(string namespaceName) {
            return namespaceName.Count(c => c == '.');
        }
        

        public Stream OpenResource(string identifier) {
            return assembly.GetManifestResourceStream(GetAssemblyIdentifier(identifier));
        }

        public string TranslateViewPath(string source) {
            return TranslatePath(viewRoot, source);
        }

        public string TranslateIncludePath(string resource) {
            return TranslatePath(includeRoot, resource);
        }

        public string TranslateStaticPath(string resource) {
            return TranslatePath(staticRoot, resource);
        }

        protected string TranslatePath(string basePath, string subPath) {
            if (subPath.StartsWith("/")) {
                return string.Format("{0}{1}", basePath, subPath);
            } else {
                return string.Format("{0}/{1}", basePath, subPath);
            }
        }

    }
}
