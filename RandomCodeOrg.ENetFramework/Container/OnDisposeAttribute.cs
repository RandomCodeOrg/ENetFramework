using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Container {

    /// <summary>
    /// Methods of managed resources, that this attribute is applied to, are called right before the resource is discarded.
    /// 
    /// Note that the method must be invokeable by the container in terms of visibility and the lack of formal parameters.
    /// <seealso cref="PostConstructAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OnDisposeAttribute : Attribute {



    }
}
