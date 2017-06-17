using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    public enum Lifetime {

        ApplicationScoped = 0,
        RequestScoped = 3,
        ViewScoped = 2,
        SessionScoped = 1

    }
}
