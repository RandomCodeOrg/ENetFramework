using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;



namespace RandomCodeOrg.ENetFramework {


    /// <summary>
    /// This class provides the logic that allows an ENetFramework application to be executed right out of the IDE.
    /// Let your main class inherit from this class and redirect the startup arguments to the protected instance method 'Start(string[])'.
    ///
    /// </summary>
    /// <example>
    /// <code>
    /// using RandomCodeOrg.ENetFramework;
    /// 
    /// namespace Foo.Bar {
    /// 
    /// class YourMainClass : ENetFrameworkApplication {
    /// 
    ///     static void Main(string[] args) {
    ///         new Program().Start(args);
    ///     }
    ///     
    /// }
    /// </code>
    /// </example>
    public abstract class ENetFrameworkApplication {

        /// <summary>
        /// This method starts the application server configured by the given command line arguments and deploys this application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        protected void Start(string[] args) {
            string containerName = null;
            string containerPath = null;

            ApplicationStartupConfig startupConfig = new ApplicationStartupConfig(args);
            if (!startupConfig.TryOption(StartupArgument.CONTAINER, out containerName) & !startupConfig.TryOption(StartupArgument.CONTAINER_PATH, out containerPath)) {
                Console.WriteLine("The container could not be found!");
                Console.WriteLine("You may use the command line arguments -Container and -ContainerPath to specify the application server to be used.");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            bool remoteLoading = startupConfig.HasFlag(StartupArgument.REMOTE_LOADING);

            Debug.WriteLine("Using container {0} at '{1}.'", containerName, containerPath);
            string filePath = Path.Combine(containerPath, containerName) + ".dll";
            if (!File.Exists(filePath))
                filePath = Path.Combine(containerPath, containerName) + ".exe";
            if (!File.Exists(filePath))
                filePath = containerPath;
            if (!File.Exists(filePath)) {
                Console.WriteLine("The container could not be found!");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
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


            Debug.WriteLine("Loaded assembly: {0}", new string[] { containerAssembly.FullName });

            Assembly toDeploy = GetType().Assembly;


            Start(containerAssembly, toDeploy, startupConfig.HasFlag(StartupArgument.CRATE_PACKAGE));
        }


        private void Start(Assembly containerAssembly, Assembly toDeploy, bool createPackage) {
            containerAssembly.EntryPoint.Invoke(null, new object[] { new string[] { } });
            var container = ENetFrameworkContext.Instance.ApplicationContainer;
            if (container == null) {
                Debug.WriteLine("Startup failed.");
                return;
            }
            var package = new Deployment.DevelopmentDeploymentPackage(toDeploy);
            if (createPackage) {
                Deployment.CompressedApplicationPackage.Create("package.capp", package);
            }

            container.Deploy(toDeploy);
            Console.ReadKey();
            container.Shutdown();
            Environment.Exit(0);
        }


    }
}
