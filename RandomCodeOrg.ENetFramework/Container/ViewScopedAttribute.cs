using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    public class ViewScopedAttribute : ScopeAttribute {

        public ViewScopedAttribute() : base(Lifetime.ViewScoped) {

        }

    }
}
