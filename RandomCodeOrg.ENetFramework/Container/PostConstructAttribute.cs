using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework.Container {



    /// <summary>
    /// Methods of managed resources, that this attribute is applied to, are called right after the resource has been initialized.
    /// 
    /// Note that the method must be invokeable by the container in terms of visibility and the lack of formal parameters.
    /// <seealso cref="OnDisposeAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PostConstructAttribute : Attribute {



    }
}
