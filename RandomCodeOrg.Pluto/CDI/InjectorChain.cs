using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class InjectorChain : List<IInjector>, IInjector {

        public InjectorChain() {

        }

        public bool TryInject(object instance, Type requiredType, out object result) {
            object tmpResult = null;
            foreach (IInjector injector in this) {
                if (injector.TryInject(instance, requiredType, out tmpResult)) {
                    result = tmpResult;
                    return true;
                }
            }
            result = tmpResult;
            return false;
        }



    }
}
