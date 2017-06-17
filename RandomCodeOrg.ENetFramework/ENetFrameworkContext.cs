using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {
    public sealed class ENetFrameworkContext {

        private static readonly ENetFrameworkContext instance = new ENetFrameworkContext();
        private IApplicationContainer container;

        public static ENetFrameworkContext Instance {
            get {
                return instance;
            }
        }


        public IApplicationContainer ApplicationContainer {
            get {
                return container;
            }
        }

        private ENetFrameworkContext() {

        }


        public void Register(IApplicationContainer container) {
            this.container = container;
        }

    }
}
