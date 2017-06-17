using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RandomCodeOrg.ENetFramework.Container;
using slf4net;

namespace RandomCodeOrg.Pluto.CDI {
    public class CDIContainer {

        private IDictionary<Lifetime, ISet<Type>> managedTypes = new Dictionary<Lifetime, ISet<Type>>();
        private readonly IDictionary<Type, Lifetime> lifetimeMapping = new Dictionary<Type, Lifetime>();
        private readonly IDictionary<Type, Type> proxyTypes = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, ISet<Type>> implementationMapping = new Dictionary<Type, ISet<Type>>(); 
        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(CDIContainer));

        private readonly InjectorChain injectorChain = new InjectorChain();

        private IDictionary<Type, object> applicationInstances = new Dictionary<Type, object>();


        private ContextStateManager contextManager;

        public CDIContainer(ContextStateManager manager) {
            contextManager = manager;
        }


        public void Load(Assembly assembly) {
            ProxyImplementationBuilder proxyBuilder = new ProxyImplementationBuilder(assembly);
            
            ManagedAttribute managedAttr;
            ScopeAttribute scopeAttr;
            Lifetime lifetime;

            ISet<Type> toCreate = new HashSet<Type>();
            
            foreach(Type t in assembly.GetTypes()) {
                if ((managedAttr = t.GetCustomAttribute<ManagedAttribute>()) != null) {
                    scopeAttr = t.GetCustomAttribute<ScopeAttribute>();
                    lifetime = Lifetime.RequestScoped;
                    if (scopeAttr != null)
                        lifetime = scopeAttr.Scope;
                    foreach(Type interfaceType in t.GetInterfaces()) {
                        if (!proxyTypes.ContainsKey(interfaceType))
                            proxyTypes[interfaceType] = proxyBuilder.Build(interfaceType);
                    }
                    Register(t, lifetime);
                    if (lifetime == Lifetime.ApplicationScoped)
                        toCreate.Add(t);
                }
            }


            foreach (Type t in toCreate)
                Activate(null, t);

        }

        internal object GetInstance(Type type, Lifetime lifetime) {
            ManagedContext context = null;
            switch (lifetime) {
                case Lifetime.RequestScoped:
                    context = contextManager.GetRequestContext();
                    break;
                case Lifetime.SessionScoped:
                    context = contextManager.GetSessionContext();
                    break;
            }
            if (context == null)
                throw new InjectionException("Invalid invocation context.");

            object result = context.GetInstance(type);
            if (result != null)
                return result;
            result = ActivateRaw(type);
            context.Attach(type, result);
            return result;
        }

        protected object ActivateRaw(Type type) {
            object instance = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            DoInject(instance, true);
            return instance;
        }

        protected void AfterInject(object instance) {
            if (instance == null)
                return;
            Type type = instance.GetType();
            foreach(MethodInfo m in type.GetMethods()) {
                if (m.GetCustomAttribute<PostConstructAttribute>() == null)
                    continue;
                if (m.GetParameters().Length != 0) {
                    logger.Warn("Could not call [PostConstruct] method '{0}.{1}' because it expects at least one parameter.", m.DeclaringType.FullName, m.Name);
                    continue;
                }
                m.Invoke(instance, new object[0]);
            }
        }

        protected object Activate(object target, Type originalType) {
            Type type = originalType;
            if (type.IsInterface) {
                if (!implementationMapping.ContainsKey(type)) {
                    object result = null;
                    if (injectorChain.TryInject(target, type, out result))
                        return result;
                    throw new InjectionException(string.Format("No implementation found for '{0}'.", type.FullName));
                }
                type = implementationMapping[type].First();
            }
            Lifetime lifetime = lifetimeMapping[type];
            if (lifetime == Lifetime.ApplicationScoped && applicationInstances.ContainsKey(type))
                return applicationInstances[type];

            if(lifetime == Lifetime.ApplicationScoped) {
                object instance = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                applicationInstances[type] = instance;
                DoInject(instance, true);
                return instance;
            }else {
                ProxyImplementation instance = (ProxyImplementation) proxyTypes[originalType].GetConstructor(new Type[] { }).Invoke(new object[] { });
                instance.ImplementationType = type;
                instance.Resolver = new ImplementationResolver(this, lifetime, type);
                return instance;
            }
            
        }


        public void Inject(object instance) {
            DoInject(instance, false);
        }

        protected void DoInject(object instance, bool invokeConstructed) {
            Type t = instance.GetType();
            foreach(PropertyInfo propInfo in t.GetProperties()) {
                if(propInfo.GetCustomAttribute<InjectAttribute>() != null) {
                    object value = Activate(instance, propInfo.PropertyType);
                    propInfo.SetValue(instance, value);
                }
            }
           
            foreach (FieldInfo fieldInfo in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (fieldInfo.GetCustomAttribute<InjectAttribute>() != null) {
                    object value = Activate(instance, fieldInfo.FieldType);
                    fieldInfo.SetValue(instance, value);
                }
            }
            if (invokeConstructed)
                AfterInject(instance);
        }
        

        public CDIContainer Use(IInjector injector) {
            injectorChain.Add(injector);
            return this;
        }
        
        protected void Register(Type t, Lifetime lifetime) {
            logger.Debug("Discovered a managed type: {0} ({1})", t.Name, lifetime);
            foreach (Type interf in t.GetInterfaces()) {
                GetImplementations(interf).Add(t);
            }
            GetTypes(lifetime).Add(t);
            lifetimeMapping[t] = lifetime;
        }

        protected ISet<Type> GetImplementations(Type interfaceType) {
            if (implementationMapping.ContainsKey(interfaceType)) {
                return implementationMapping[interfaceType];
            }
            HashSet<Type> implementations = new HashSet<Type>();
            implementationMapping[interfaceType] = implementations;
            return implementations;
        }

        protected ISet<Type> GetTypes(Lifetime l) {
            if (!managedTypes.ContainsKey(l)) {
                ISet<Type> types = new HashSet<Type>();
                managedTypes[l] = types;
                return types;
            }
            return managedTypes[l];
        }

        public IEnumerable<Type> GetInjectableTypes() {
            ISet<Type> result = new HashSet<Type>();
            foreach(var pair in lifetimeMapping) {
                if(pair.Value == Lifetime.ApplicationScoped) {
                    result.Add(pair.Key);
                }
            }
            foreach(var intf in implementationMapping.Keys) {
                if (implementationMapping[intf].Count > 0)
                    result.Add(intf);
            }
            return result;
        }

        public string GetIdentifier(Type t) {
            string name = t.Name;
            if(t.IsInterface && name.StartsWith("I")) {
                name = name.Substring(1);
            }
            return name.ToLower()[0] + name.Substring(1);
        }

    }
}
