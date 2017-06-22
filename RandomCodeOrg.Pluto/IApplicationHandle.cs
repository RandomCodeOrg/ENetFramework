using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto {

    public interface IApplicationHandle {

        /// <summary>
        /// Returns the applications namespace.
        /// </summary>
        string ApplicationNamespace { get; }

        /// <summary>
        /// Provides the applications assembly.
        /// </summary>
        Assembly DefiningAssembly { get; }

        /// <summary>
        /// Provides the applications readable name.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Provides the applications technical name.
        /// </summary>
        string TechnicalName { get; }

        /// <summary>
        /// Returns the applications version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Returns a path pointing to the location of this application.
        /// </summary>
        string Location { get; }

        IReadOnlyCollection<string> ViewResources { get; }

        IReadOnlyCollection<string> IncludeResources { get; }

        IReadOnlyCollection<string> StaticResources { get; }
        
        string TranslateViewPath(string source);

        string TranslateIncludePath(string resource);

        string TranslateStaticPath(string resource);

        Stream OpenResource(string path);

        bool ObserveResource(string path, ResourceModifiedCalback callback);

    }
}
