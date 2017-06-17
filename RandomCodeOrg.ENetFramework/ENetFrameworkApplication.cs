using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;



namespace RandomCodeOrg.ENetFramework {
    public class ENetFrameworkApplication {



        protected void Start(string[] args) {
          
            string containerName = null;
            string containerPath = null;
            Action<string> nextAction = null;
            foreach (string arg in args) {
                System.Diagnostics.Debug.WriteLine(arg);
                if (nextAction != null) {
                    if (arg.StartsWith("-")) {
                        nextAction = null;
                    } else {
                        nextAction(arg);
                        nextAction = null;
                        continue;
                    }
                }

                if ("-Container".Equals(arg)) {
                    nextAction = (s) => containerName = s;
                } else if ("-ContainerPath".Equals(arg)) {
                    nextAction = (s) => containerPath = s;
                }
            }
            if (containerPath == null || containerName == null)
                return;
            Debug.WriteLine("Using container {0} at '{1}.'", containerName, containerPath);
            string filePath = Path.Combine(containerPath, containerName) + ".dll";
            if (!File.Exists(filePath))
                filePath = Path.Combine(containerPath, containerName)+ ".exe";
            if (!File.Exists(filePath))
                filePath = containerPath;
            if (!File.Exists(filePath)) {
                Debug.WriteLine("Container could not be found.");
                return;
            }
            filePath = Path.GetFullPath(filePath);
            Debug.WriteLine("Using assembly at: {0}", args = new string[] { filePath });
            LoadDepdendencies(filePath);
            var loaded = Assembly.LoadFile(filePath);
            
            Debug.WriteLine("Loaded assembly: {0}", new string[]{ loaded.FullName });

            Assembly toDeploy = Assembly.LoadFile(GetType().Assembly.Location);

            Start(loaded, toDeploy);
        }

        private void LoadDepdendencies(string file) {
            string dirPath = Path.GetDirectoryName(file);
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => {
                string basePath;
                if (e.Name.Contains(","))
                    basePath = Path.Combine(dirPath, e.Name.Substring(0, e.Name.IndexOf(",")));
                else
                    basePath = Path.Combine(dirPath, e.Name);

                string path;
                if (File.Exists(basePath)) {
                    path = basePath;
                }else if(File.Exists(basePath + ".dll")) {
                    path = basePath + ".dll";
                }else if(File.Exists(basePath + ".exe")) {
                    path = basePath + ".exe";
                } else {
                    throw new Exception("Not found!");
                }
                Debug.WriteLine("Assembly at: " + path);
                Assembly result = Assembly.LoadFile(path);
                return result;
            };
            foreach(string child in Directory.GetFiles(dirPath)) {
                if (!child.Equals(file) && child.EndsWith(".dll")) {
                    Assembly assembly = Assembly.LoadFile(child);
                    AppDomain.CurrentDomain.Load(assembly.GetName());
                }
            }
        }

        private void Start(Assembly assembly, Assembly toDeploy) {
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
            var container = ENetFrameworkContext.Instance.ApplicationContainer;
            if(container == null) {
                Debug.WriteLine("Startup failed.");
                return;
            }
            container.Deploy(toDeploy);
            Console.ReadKey();
            container.Shutdown();
            Environment.Exit(0);
        }


    }
}
