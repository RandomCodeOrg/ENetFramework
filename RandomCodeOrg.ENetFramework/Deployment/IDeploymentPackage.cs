using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Deployment {

    /// <summary>
    /// A deployment package defines an application, its dependencies and meta information.
    /// </summary>
    public interface IDeploymentPackage {
       
        /// <summary>
        /// Provides the dependencies of the packaged application.
        /// </summary>
        IEnumerable<IDependency> Dependencies { get; }

        /// <summary>
        /// Provides the file name of the compiled application assembly.
        /// </summary>
        string AssemblyFileName { get; }

        /// <summary>
        /// Returns a stream that can be used to access the applications assembly.
        /// </summary>
        /// <returns>A stream that contains the applications assembly.</returns>
        Stream GetAssemblyContent();

        /// <summary>
        /// Returns a stream that can be used to access the application descriptor (application.xml) for this package.
        /// 
        /// If this method returns <code>null</code> the application container will retry retrieving the application descriptor from the applications assembly.
        /// </summary>
        /// <returns>A stream containing the application descriptor or <code>null</code> if the assembly embedded application descriptor should be used.</returns>
        Stream GetDescriptorContent();
        
    }
}
