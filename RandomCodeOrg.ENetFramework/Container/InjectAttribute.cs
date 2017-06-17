using System;

namespace RandomCodeOrg.ENetFramework.Container {
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InjectAttribute : Attribute {

        public Type Implementation { get; set; }

    }


}
