using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using RandomCodeOrg.ENetFramework.Deployment;

namespace RandomCodeOrg.ENetFramework {

    /// <summary>
    /// This is the interface defining an application container.
    /// </summary>
    public interface IApplicationContainer {

        /// <summary>
        /// Deploys the application given by its assembly.
        /// </summary>
        /// <param name="assembly">The assembly of the application to be deployed.</param>
        void Deploy(Assembly assembly);


        /// <summary>
        /// Deploys the application given by a deployment package.
        /// </summary>
        /// <param name="deploymentPackage">The deploymnet package to deploy.</param>
        void Deploy(IDeploymentPackage deploymentPackage);

        // Shuts down this application container.
        void Shutdown();

    }
}
