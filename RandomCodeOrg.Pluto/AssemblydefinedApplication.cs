using RandomCodeOrg.ENetFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto {
    public class AssemblydefinedApplication : IApplicationHandle {


        private readonly Assembly assembly;
        private readonly string applicationNamespace;
        private readonly AssemblyName assemblyName;
        private readonly string assemblyVersion;
        private readonly string assemblyLocation;

        public string ApplicationNamespace => applicationNamespace;

        public Assembly DefiningAssembly => assembly;

        public string FriendlyName => assemblyName.Name;

        public string TechnicalName => assemblyName.Name;

        public string Version => assemblyVersion;

        public string Location => assemblyLocation;


        public AssemblydefinedApplication(Assembly assembly) {
            this.assembly = assembly;
            applicationNamespace = GetApplicationNamespace(assembly);
            assemblyName = assembly.GetName();
            assemblyVersion = assemblyName.Version.ToString();
            assemblyLocation = assembly.Location;
        }


        
        private string GetApplicationNamespace(Assembly assembly) {
            var entryPoint = assembly.EntryPoint;
            if (entryPoint != null)
                return entryPoint.DeclaringType.Namespace;

            int currentCount = int.MaxValue;
            string currentVal = null;
            int current;

            foreach (Type t in assembly.GetTypes()) {
                if((current = CountNamespaceComponents(t.Namespace)) < currentCount) {
                    currentCount = current;
                    currentVal = t.Namespace;
                }
            }

            if (currentVal != null)
                return currentVal;

            string tmp;
            foreach(string resourceName in assembly.GetManifestResourceNames()) {
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


        

    }
}
