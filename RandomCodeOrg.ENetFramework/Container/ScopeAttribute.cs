using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ScopeAttribute : Attribute {


        private readonly Lifetime scope;
        
        public Lifetime Scope {
            get {
                return scope;
            }
        }

        public ScopeAttribute(Lifetime scope) {
            this.scope = scope;
        }

    }
}
