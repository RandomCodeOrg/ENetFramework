using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    public class SessionScopedAttribute : ScopeAttribute {


        public SessionScopedAttribute() : base(Lifetime.SessionScoped) {

        }

    }
}
