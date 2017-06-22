using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Deployment {

    /// <summary>
    /// Represents a dependency.
    /// </summary>
    public interface IDependency {

        /// <summary>
        /// The human readable name of a dependency.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Provides access to the dependencies data.
        /// </summary>
        /// <returns>A stream containing the dependencies data.</returns>
        Stream GetContent();

    }
}
