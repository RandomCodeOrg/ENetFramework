using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {


    /// <summary>
    /// This attribute is used to request a rquest scope for the implementation that this attribute is applied to.
    /// </summary>
    public class RequestScopedAttribute : ScopeAttribute {

        public RequestScopedAttribute() : base(Lifetime.RequestScoped) {

        }

    }
}
