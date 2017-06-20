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
           
            ApplicationStartupConfig startupConfig = new ApplicationStartupConfig(args);
            if (!startupConfig.TryOption(StartupArgument.CONTAINER, out containerName) & !startupConfig.TryOption(StartupArgument.CONTAINER_PATH, out containerPath))
                return;
            bool remoteLoading = startupConfig.HasFlag(StartupArgument.REMOTE_LOADING);

            Debug.WriteLine("Using container {0} at '{1}.'", containerName, containerPath);
            string filePath = Path.Combine(containerPath, containerName) + ".dll";
            if (!File.Exists(filePath))
                filePath = Path.Combine(containerPath, containerName) + ".exe";
            if (!File.Exists(filePath))
                filePath = containerPath;
            if (!File.Exists(filePath)) {
                Debug.WriteLine("Container could not be found.");
                return;
            }
            filePath = Path.GetFullPath(filePath);
            Debug.WriteLine("Using assembly at: {0}", args = new string[] { filePath });
            AssemblyLoader assemblyLoader = new AssemblyLoader(filePath);
            if (remoteLoading) {
                Debug.WriteLine("WARN: Usage of remote assemblies is enabled. For more details refer to: https://msdn.microsoft.com/en-us/library/dd409252(vs.100).aspx#Anchor_1");
            }
            assemblyLoader.AllowRemoteAssemblies = remoteLoading;
            assemblyLoader.Handle(AppDomain.CurrentDomain);
            

            var containerAssembly = assemblyLoader.LoadDirect(filePath);
            

            Debug.WriteLine("Loaded assembly: {0}", new string[]{ containerAssembly.FullName });

            Assembly toDeploy = GetType().Assembly;
            

            Start(containerAssembly, toDeploy);
        }
        

        private void Start(Assembly containerAssembly, Assembly toDeploy) {
            containerAssembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
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
