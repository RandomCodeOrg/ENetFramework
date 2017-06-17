using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    public class RequestScopedAttribute : ScopeAttribute {

        public RequestScopedAttribute() : base(Lifetime.RequestScoped) {

        }

    }
}
