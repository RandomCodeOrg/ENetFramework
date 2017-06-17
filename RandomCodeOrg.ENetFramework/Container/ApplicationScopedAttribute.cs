using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    public class ApplicationScopedAttribute : ScopeAttribute {

        public ApplicationScopedAttribute() : base(Lifetime.ApplicationScoped) {

        }

    }
}
