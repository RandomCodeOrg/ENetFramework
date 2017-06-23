using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
                foreach (MethodInfo mi in ImplementationType.GetMethods()) {
                    methodMap[ProxyImplementationBuilder.BuildIdentifier(mi)] = mi;
                }
            }
            return DoCallMethod(GetInstance(), methodMap[identifier], args);
        }

        public object GetProperty(string name) {
            if (!propertyMap.ContainsKey(name)) {
                propertyMap[name] = ImplementationType.GetProperty(name);
            }
            return DoGetProperty(GetInstance(), propertyMap[name]);
        }

        public void SetProperty(string name, object value) {
            if (!propertyMap.ContainsKey(name)) {
                propertyMap[name] = ImplementationType.GetProperty(name);
            }
            DoSetProperty(GetInstance(), propertyMap[name], value);
        }

        protected virtual object DoCallMethod(object instance, MethodInfo method, object[] args) {
            try {
                return method.Invoke(instance, args);
            } catch (TargetInvocationException e) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        protected virtual void DoSetProperty(object instance, PropertyInfo property, object value) {
            try {
            property.SetValue(instance, value);
            } catch(TargetInvocationException e) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        protected virtual object DoGetProperty(object instance, PropertyInfo property) {
            try {
                object result = property.GetValue(instance);
                return result;
            } catch(TargetInvocationException e) {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
            
        }


    }
}
