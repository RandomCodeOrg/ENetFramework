using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto {
    public static class ExtensionMethods {

        public static T As<T>(this object instance) {
            if (instance is T)
                return (T)instance;
            return default(T);
        }


        public static StringBuilder AppendFL(this StringBuilder sb, string format, params object[] args) {
            sb.AppendLine(string.Format(format, args));
            return sb;
        }
        

    }
}
