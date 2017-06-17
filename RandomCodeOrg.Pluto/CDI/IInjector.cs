using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public interface IInjector {

        bool TryInject(object instance, Type requiredType, out object result);

    }


    public delegate bool InjectorDelegate(object instance, Type requiredType, out object result);

    public class CustomInjector : IInjector {


        private readonly InjectorDelegate injectorDelegate;

        public CustomInjector(InjectorDelegate injectorDelegate) {
            this.injectorDelegate = injectorDelegate;
        }

        public bool TryInject(object instance, Type requiredType, out object result) {
            return injectorDelegate(instance, requiredType, out result);
        }

        public static implicit operator CustomInjector(InjectorDelegate injectorDelegate) {
            return new CustomInjector(injectorDelegate);
        }

        public static implicit operator InjectorDelegate(CustomInjector customInjector) {
            return customInjector.injectorDelegate;
        }

    }

}
