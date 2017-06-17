using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace RandomCodeOrg.Pluto.CDI {
    public class PropertyOverwriter {




        public void Overwrite(PropertyInfo propInfo) {
            var methodBody = propInfo.GetMethod.GetMethodBody();

            DynamicMethod dm = new DynamicMethod("", propInfo.PropertyType, new Type[] { });
            var body = dm.GetILGenerator();
            body.Emit(OpCodes.Ldarg_0);
            body.EmitCall(OpCodes.Call, typeof(CDIContainer).GetMethod("GetInstance"), new Type[] { });
            body.Emit(OpCodes.Ret);
         
        }


    }
}
