using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {
    public class AssemblyLoader {

        private IList<string> searchPaths;

        private IReadOnlyCollection<string> searchPathsReadOnly;

        public IReadOnlyCollection<string> SearchPaths {
            get {
                return searchPathsReadOnly;
            }
        }

        private readonly ResolveEventHandler resolveHandler;

        public bool AllowRemoteAssemblies { get; set; }


        public AssemblyLoader(IEnumerable<string> searchPaths) {
            AllowRemoteAssemblies = false;
            this.searchPaths = new List<string>();
            searchPathsReadOnly = new ReadOnlyCollection<string>(this.searchPaths);
            resolveHandler = new ResolveEventHandler(ResolveAssembly);
            string fullPath;
            foreach (string path in searchPaths) {
                fullPath = Path.GetDirectoryName(Path.GetFullPath(path));
                if (this.searchPaths.Contains(fullPath) || !Directory.Exists(fullPath))
                    continue;
                this.searchPaths.Add(fullPath);
            }
        }

        public AssemblyLoader(params string[] searchPaths) : this(searchPaths.ToList()) {

        }
        
        public void Handle(AppDomain domain) {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));
            domain.AssemblyResolve += this;
        }

        protected virtual Assembly ResolveAssembly(object sender, ResolveEventArgs e) {
            Assembly assembly;
            foreach(string path in searchPaths) {
                if (TryResolveAssembly(path, e, out assembly))
                    return assembly;
            }
            return null;
        }

        protected virtual bool TryResolveAssembly(string searchPath, ResolveEventArgs e, out Assembly assembly) {
            string basePath;
            if (e.Name.Contains(",")) {
                basePath = Path.Combine(searchPath, e.Name.Substring(0, e.Name.IndexOf(',')));
            } else {
                basePath = Path.Combine(searchPath, e.Name);
            }
            string path;
            if (File.Exists(basePath)) {
                path = basePath;
            }else if(File.Exists(basePath + ".dll")) {
                path = basePath + ".dll";
            }else if(File.Exists(basePath + ".exe")) {
                path = basePath + ".exe";
            } else {
                assembly = null;
                return false;
            }
            return TryLoadAssembly(path, out assembly);
        }

        protected virtual bool TryLoadAssembly(string path, out Assembly assembly) {
            if (AllowRemoteAssemblies) {
                assembly = Assembly.UnsafeLoadFrom(path);
            } else {
                assembly = Assembly.LoadFile(path);
            }
            return assembly != null;
        }

        public virtual Assembly LoadDirect(string filePath) {
            Assembly assembly;
            if(TryLoadAssembly(filePath, out assembly)) {
                return assembly;
            }
            return null;
        }

        public static implicit operator ResolveEventHandler(AssemblyLoader loader) {
            return loader.resolveHandler;
        }

    }
}
