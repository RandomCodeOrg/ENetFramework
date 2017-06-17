using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RandomCodeOrg.ENetFramework {
    public interface IApplicationContainer {

        void Deploy(Assembly assembly);

        void Shutdown();

    }
}
