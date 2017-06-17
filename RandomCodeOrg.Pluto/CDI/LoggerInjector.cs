using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class LoggerInjector : IInjector {


        public bool TryInject(object instance, Type requiredType, out object result) {
            result = null;
            if (instance == null || !requiredType.IsAssignableFrom(typeof(ILogger)))
                return false;
            result = LoggerFactory.GetLogger(instance.GetType());
            return true;
        }


    }
}
