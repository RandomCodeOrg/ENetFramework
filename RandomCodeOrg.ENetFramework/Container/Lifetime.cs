using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {

    /// <summary>
    /// An enumeration of the available scope types.
    /// </summary>
    public enum Lifetime {

        ApplicationScoped = 0,
        RequestScoped = 3,
        SessionScoped = 1

    }
}
