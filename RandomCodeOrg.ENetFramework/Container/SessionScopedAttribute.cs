using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {

    /// <summary>
    /// This attribute is used to request a session scope for the implementation that this attribute is applied to.
    /// </summary>
    public class SessionScopedAttribute : ScopeAttribute {


        public SessionScopedAttribute() : base(Lifetime.SessionScoped) {

        }

    }
}
