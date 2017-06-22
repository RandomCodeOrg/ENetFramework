using RandomCodeOrg.ENetFramework.Deployment;
using RandomCodeOrg.Pluto.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RandomCodeOrg.Pluto {
    public class DebugApplication : AssemblydefinedApplication {

        private readonly DevelopmentDeploymentPackage package;

        private readonly SourceChangeMonitor sourceChangeMonitor;

        private readonly IDictionary<string, ISet<ResourceModifiedCalback>> observers = new Dictionary<string, ISet<ResourceModifiedCalback>>();
        private readonly SourceFileFinder sourceFileFinder;

        public DebugApplication(DevelopmentDeploymentPackage package) : base(package.ApplicationAssembly) {
            this.package = package;
            sourceFileFinder = new SourceFileFinder();
            sourceChangeMonitor = new SourceChangeMonitor(sourceFileFinder);

        }



        public override bool ObserveResource(string path, ResourceModifiedCalback callback) {
            if (base.ObserveResource(path, callback))
                return true;

            string assemblyIdentifier = GetAssemblyIdentifier(path);
            if(sourceChangeMonitor.Monitor(DefiningAssembly, assemblyIdentifier, HandleResourceChanged)) {
                if (!observers.ContainsKey(path))
                    observers[path] = new HashSet<ResourceModifiedCalback>();
                observers[path].Add(callback);
                return true;
            }
            return false;
        }

        public override Stream OpenResource(string identifier) {
            string assemblyIdentifier = GetAssemblyIdentifier(identifier);
            string location = sourceFileFinder.GetSourceLocation(DefiningAssembly, assemblyIdentifier);
            if(location != null) {
                return new FileStream(location, FileMode.Open, FileAccess.Read);
            }
            return base.OpenResource(identifier);
        }


        private void HandleResourceChanged(object sender, string assemblyIdentifier, string filePath) {
            string path = GetResourcePath(assemblyIdentifier, true);
            if (observers.ContainsKey(path)) {
                foreach(ResourceModifiedCalback callback in observers[path]) {
                    callback(this, path);
                }
            }
        }

    }
}
