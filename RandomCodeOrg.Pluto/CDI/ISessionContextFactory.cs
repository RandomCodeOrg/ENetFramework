using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public interface ISessionContextFactory {

        ManagedContext CreateSessionContext();

    }
}
