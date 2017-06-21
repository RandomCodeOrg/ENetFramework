using System;
using System.Collections.Generic;
using System.Text;

namespace RandomCodeOrg.ENetFramework.Container {



    /// <summary>
    /// This attribute can be used on implementations that should be managed by the container. An implementation also needs to use one of the attributes listed below to define the target scope.
    /// <seealso cref="ApplicationScopedAttribute"/>
    /// <seealso cref="SessionScopedAttribute"/>
    /// <seealso cref="RequestScopedAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ManagedAttribute : Attribute {


    }
}
