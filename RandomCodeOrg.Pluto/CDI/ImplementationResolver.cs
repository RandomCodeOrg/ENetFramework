using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.CDI {
    public class ImplementationResolver : IImplementationResolver {


        private readonly CDIContainer container;
        private readonly Lifetime lifetime;
        private readonly Type targetType;

        public ImplementationResolver(CDIContainer container, Lifetime lifetime, Type targetType) {
            this.container = container;
            this.lifetime = lifetime;
            this.targetType = targetType;
        }

        public object Resolve() {
            return container.GetInstance(targetType, lifetime);
        }
    }
}
