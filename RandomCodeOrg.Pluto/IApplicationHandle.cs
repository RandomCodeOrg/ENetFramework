using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto {
    public interface IApplicationHandle {



        /// <summary>
        /// Returns the applications namespace.
        /// </summary>
        string ApplicationNamespace { get; } //TODO: EntryPoint or topmost T:ENetFrameworkApplication or topmost T:object

        /// <summary>
        /// Provides the applications assembly.
        /// </summary>
        Assembly DefiningAssembly { get; }

        /// <summary>
        /// Provides the applications readable name.
        /// </summary>
        string FriendlyName { get; } //  assembly.GetName().Name;

        /// <summary>
        /// Provides the applications technical name.
        /// </summary>
        string TechnicalName { get; } // assembly.GetName().Name;

        /// <summary>
        /// Returns the applications version.
        /// </summary>
        string Version { get; } // assembly.GetName().Version

        /// <summary>
        /// Returns a path pointing to the location of this application.
        /// </summary>
        string Location { get; } // assembly.Location



    }
}
