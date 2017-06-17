using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements {
    public static class ExtensionMethods {

        public static bool IsStatic(this PropertyInfo propertyInfo) {
            return ((propertyInfo.CanRead && propertyInfo.GetMethod.IsStatic) ||
                (propertyInfo.CanWrite && propertyInfo.SetMethod.IsStatic));
        }

        public static bool IsVararg(this ParameterInfo param) {
            return param.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
        }

        public static bool IsVarargOrDefault(this ParameterInfo param) {
            return param.HasDefaultValue || param.IsVararg();
        }
        

    }
}
