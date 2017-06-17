using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class ProxyImplementation {


        public Type ImplementationType { get; set; }
        public IImplementationResolver Resolver { get; set; }

        private readonly IDictionary<string, PropertyInfo> propertyMap = new Dictionary<string, PropertyInfo>();
        private readonly IDictionary<string, MethodInfo> methodMap = new Dictionary<string, MethodInfo>();

        private readonly ILogger logger;

        public ProxyImplementation() {
            logger = LoggerFactory.GetLogger(GetType());
        }

        private object GetInstance() {
            return Resolver.Resolve();
        }

        public object CallMethod(string identifier, object[] args) {
            if (!methodMap.ContainsKey(identifier)) {
                foreach(MethodInfo mi in ImplementationType.GetMethods()) {
                    methodMap[ProxyImplementationBuilder.BuildIdentifier(mi)] = mi;
                }
            }
            return methodMap[identifier].Invoke(GetInstance(), args);
        }

        public object GetProperty(string name) {
            if (!propertyMap.ContainsKey(name)) {
                propertyMap[name] = ImplementationType.GetProperty(name);
            }
            object result = propertyMap[name].GetValue(GetInstance());
            return result;
        }

        public void SetProperty(string name, object value) {
            if (!propertyMap.ContainsKey(name)) {
                propertyMap[name] = ImplementationType.GetProperty(name);
            }
            propertyMap[name].SetValue(GetInstance(), value);
        }
        

    }
}
