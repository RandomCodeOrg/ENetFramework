using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class ConstantImplementationResolver : IImplementationResolver {

        private readonly object instance;

        public ConstantImplementationResolver(object instance) {
            this.instance = instance;
        }

        public object Resolve() {
            return instance;
        }
    }
}
