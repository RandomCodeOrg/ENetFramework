using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {

    /// <summary>
    /// Provides information about the current execution.
    /// </summary>
    public sealed class ENetFrameworkContext {

        private static readonly ENetFrameworkContext instance = new ENetFrameworkContext();
        private IApplicationContainer container;

        /// <summary>
        /// The current <see cref="ENetFrameworkContext"/>.
        /// </summary>
        public static ENetFrameworkContext Instance {
            get {
                return instance;
            }
        }

        /// <summary>
        /// Provides the currently executing application container.
        /// </summary>
        public IApplicationContainer ApplicationContainer {
            get {
                return container;
            }
        }

        private ENetFrameworkContext() {

        }


        /// <summary>
        /// Registers the given application container.
        /// This method must be called by the application container after it was created.
        /// </summary>
        /// <param name="container"></param>
        public void Register(IApplicationContainer container) {
            this.container = container;
        }

    }
}
