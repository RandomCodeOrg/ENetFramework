using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {

   /// <summary>
   /// This attribute is used to request an application scope for the implementation that this attribute is applied to.
   /// </summary>
    public class ApplicationScopedAttribute : ScopeAttribute {

        public ApplicationScopedAttribute() : base(Lifetime.ApplicationScoped) {

        }

    }
}
