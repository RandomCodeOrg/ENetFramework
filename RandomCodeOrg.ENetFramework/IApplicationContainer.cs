using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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

        // Shuts down this application container.
        void Shutdown();

    }
}
