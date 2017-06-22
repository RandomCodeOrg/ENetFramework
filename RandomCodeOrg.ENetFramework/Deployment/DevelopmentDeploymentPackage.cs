using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static RandomCodeOrg.ENetFramework.FrameworkConstants;

namespace RandomCodeOrg.ENetFramework.Deployment {
    internal class DevelopmentDeploymentPackage : IDeploymentPackage {


        private readonly ISet<IDependency> dependencies = new HashSet<IDependency>();

        public IEnumerable<IDependency> Dependencies => dependencies;

        private readonly string fileName;

        public string AssemblyFileName => fileName;

        private readonly Assembly currentAssembly;

        public DevelopmentDeploymentPackage(Assembly currentAssembly) {
            this.currentAssembly = currentAssembly;
            string location = currentAssembly.Location;
            fileName = new FileInfo(Path.GetFullPath(currentAssembly.Location)).Name;
            SearchDependencies(Directory.GetParent(location).FullName, 3);

        }

        private void SearchDependencies(string currentPath, int maxDepth) {
            if (maxDepth < 0)
                return;
            if (File.Exists(currentPath)) {
                if (Path.GetFullPath(currentPath).Equals(Path.GetFullPath(currentAssembly.Location)))
                    return;
                if (currentPath.EndsWith("RandomCodeOrg.ENetFramework.dll"))
                    return;
                if (currentPath.EndsWith(".exe") || currentPath.EndsWith(".dll")) {
                    dependencies.Add(new FSBinaryDependency(currentPath));
                }
            } else if (Directory.Exists(currentPath)) {
                DirectoryInfo directory = new DirectoryInfo(currentPath);
                foreach (FileInfo childFile in directory.GetFiles())
                    SearchDependencies(childFile.FullName, maxDepth - 1);
                foreach (DirectoryInfo childDirectory in directory.GetDirectories())
                    SearchDependencies(childDirectory.FullName, maxDepth - 1);
            }
        }


        public Stream GetAssemblyContent() {
            return new FileStream(Path.GetFullPath(currentAssembly.Location), FileMode.Open, FileAccess.Read);
        }

        public Stream GetDescriptorContent() {
            if (File.Exists(APP_DESCRIPTOR_FILE_NAME)) {
                return new FileStream(APP_DESCRIPTOR_FILE_NAME, FileMode.Open, FileAccess.Read);
            }
            Stream stream;
            foreach (string resourceName in currentAssembly.GetManifestResourceNames()) {
                if (resourceName.EndsWith(APP_DESCRIPTOR_FILE_NAME)) {
                    if ((stream = currentAssembly.GetManifestResourceStream(resourceName)) != null) {
                        return stream;
                    }
                }
            }
            return null;
        }
    }


}
