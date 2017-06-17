using RandomCodeOrg.ENetFramework.Container;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class ManagedContext {


        private readonly IDictionary<Type, object> instances = new Dictionary<Type, object>();

        private readonly IDictionary<object, ISet<MethodInfo>> completeMethods = new Dictionary<object, ISet<MethodInfo>>();

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(ManagedContext));

        public object GetInstance(Type t) {
            if (instances.ContainsKey(t)) {
                return instances[t];
            }
            return null;
        }

        public void Attach(Type t, object instance) {
            instances[t] = instance;
            ISet<MethodInfo> methods = new HashSet<MethodInfo>();
            foreach (MethodInfo method in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (method.GetCustomAttribute<OnDisposeAttribute>() == null)
                    continue;
                if (method.GetParameters().Length > 0) {
                    logger.Warn("The [OnDispose]-annotated method '{0}.{1}' will be ignored because it expects at least one parameter.", method.DeclaringType.FullName, method.Name);
                    continue;
                }
                methods.Add(method);
            }
            if (methods.Count > 0)
                completeMethods[instance] = methods;
        }

        public void Complete() {
            foreach (object instance in completeMethods.Keys) {
                foreach (MethodInfo method in completeMethods[instance]) {
                    method.Invoke(instance, new object[0]);
                }
            }
        }
        
    }
}
